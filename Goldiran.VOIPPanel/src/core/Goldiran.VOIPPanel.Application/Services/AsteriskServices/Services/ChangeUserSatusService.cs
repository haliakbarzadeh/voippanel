using AsterNET.NetStandard.Manager;
using AsterNET.NetStandard.Manager.Action;
using AsterNET.NetStandard.Manager.Response;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using System.Net.Sockets;
using System.Text;

namespace Goldiran.VOIPPanel.Application.Services.AsteriskServices.Services;

public class ChangeUserSatusService : IChangeUserSatusService
{
    private readonly IReadModelContext _readModelContext;
    private int _retry = 0;
    public ChangeUserSatusService(IReadModelContext readModelContext)
    {
        _readModelContext = readModelContext;
    }

    public async Task ChangeUserStatus(ChangeUserSattusRequest request)
    {

        ManagerConnection _managerConnection = null;
        string ipAddress = string.Empty;
        bool flag = false;


        foreach (var item in request.QueueList)
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                await socket.ConnectAsync(item.IPAddress, 5038);
                string loginCommand = $"Action: Login\r\nUsername: {item.User}\r\nSecret: {item.Secret}\r\n\r\n";
                await SendDataAsync(socket, loginCommand);
                //if (!flag)
                //{
                //    await socket.ConnectAsync(item.IPAddress, 5038);
                //    string loginCommand = $"Action: Login\r\nUsername: {item.User}\r\nSecret: {item.Secret}\r\n\r\n";
                //    await SendDataAsync(socket, loginCommand);

                //    flag = true;
                //}
                //else if (flag && item.IPAddress != ipAddress)
                //{
                //    string logoffCommand = "Action: Logoff\r\n\r\n";
                //    await SendDataAsync(socket, logoffCommand);
                //    await socket.DisconnectAsync(false);

                //    await socket.ConnectAsync(item.IPAddress, 5038);
                //    string loginCommand = $"Action: Login\r\nUsername: {item.User}\r\nSecret: {item.Secret}\r\n\r\n";
                //    await SendDataAsync(socket, loginCommand);
                //}

                ipAddress = item.IPAddress;
                string queueStatusCommand = string.Empty;
                bool flagRecieve = true;
                ManagerAction action = null;
                ManagerResponse response = null;
                if (request.OperationType != OperationType.Exit)
                {
                    queueStatusCommand = $"Action: QueueAdd\r\n" +
                                 $"Interface: SIP/{request.Exten}\r\n" +
                                 $"Queue: {item.Code}\r\n" +
                                 $"MemberName: SIP/{request.Exten}\r\n" +
                                $"Penalty: 0\r\n\r\n";
                    await SendDataAsync(socket, queueStatusCommand);
                    Thread.Sleep(10);
                    // Read the response
                    //flagRecieve = await ReceiveDataAsync(socket);
                }
                //else if (operationType == OperationType.Exit)
                else
                {
                    //queueStatusCommand = $"Action: QueuePause\r\n" +
                    //   $"Interface: SIP/{request.Exten}\r\n" +
                    //   $"Queue: {item.Code}\r\n" +
                    //   $"Paused: 1\r\n\r\n";

                    //await SendDataAsync(socket, queueStatusCommand);
                    queueStatusCommand = $"Action: QueueRemove\r\n" +
                                                     $"Interface: SIP/{request.Exten}\r\n" +
                                                     $"Queue: {item.Code}\r\n" +
                                                     $"MemberName: {request.Exten}\r\n\r\n";

                    await SendDataAsync(socket, queueStatusCommand);

                    flagRecieve = await ReceiveDataAsync(socket);

                    continue;
                }
                
                string pause = request.OperationType == OperationType.Answering ? "0" : "1";
                //queueStatusCommand = $"Action: QueuePause\r\n" +
                //       $"Interface: SIP/{request.Exten}\r\n" +
                //       $"Queue: {item.Code}\r\n" +
                //       $"Paused: {pause}\r\n\r\n";

                //await SendDataAsync(socket, queueStatusCommand);

                //// Read the response
                //flagRecieve = await ReceivePauseDataAsync(socket);

                await SetPuaseAction(socket, pause, request.Exten, item.Code);
                _retry = 0;
            }
        }
    }

    private async Task SetPuaseAction(Socket socket, string pause, string exten, int queue)
    {
        _retry++;
        var queueStatusCommand = $"Action: QueuePause\r\n" +
               $"Interface: SIP/{exten}\r\n" +
               $"Queue: {queue}\r\n" +
               $"Paused: {pause}\r\n\r\n";

        await SendDataAsync(socket, queueStatusCommand);
        Thread.Sleep(6);
        // Read the response
        var flagRecieve = await ReceivePauseDataAsync(socket);
        if (flagRecieve)
        {
            return;
        }
        if (!flagRecieve && _retry<4)
        {
            Thread.Sleep(3);
            await SetPuaseAction(socket, pause, exten, queue);
        }
        else if (!flagRecieve && _retry >3)
        {
            throw new ValidationException(new List<string>() { "مجددا امتحان کنید" });

        }
    }

    private async Task SendDataAsync(Socket socket, string data)
    {
        byte[] byteData = Encoding.ASCII.GetBytes(data);
        await socket.SendAsync(new ArraySegment<byte>(byteData), SocketFlags.None);
    }

    private async Task<bool> ReceiveDataAsync(Socket socket)
    {
        StringBuilder responseBuilder = new StringBuilder();
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None)) > 0)
        {
            responseBuilder.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));

            if (responseBuilder.ToString().Contains("Response: Success") || responseBuilder.ToString().Contains("Message: Unable to add interface: Already there") || responseBuilder.ToString().Contains("Message: Unable to remove interface: Not there"))
            {
                var tt = responseBuilder.ToString();

                return true;
            }
            else if (responseBuilder.ToString().Contains("Event: RTCPReceived") || responseBuilder.ToString().Contains("Event: SuccessfulAuth"))
            {
                var t = responseBuilder.ToString();
                throw new ValidationException(new List<string>() { "سافت فون خود را متصل کنید" });
            }
        }

        throw new ValidationException(new List<string>() { "سافت فون خود را متصل کنید" });

    }

    private async Task<bool> ReceivePauseDataAsync(Socket socket)
    {
        StringBuilder responseBuilder = new StringBuilder();
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None)) > 0)
        {
            responseBuilder.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));

            //if (responseBuilder.ToString().Contains("Response: Success") && (responseBuilder.ToString().Contains("Interface paused successfully") || responseBuilder.ToString().Contains("Interface unpaused successfully")))
            if (responseBuilder.ToString().Contains("Interface paused successfully") || responseBuilder.ToString().Contains("Interface unpaused successfully"))
            {
                var tt = responseBuilder.ToString();

                return true;
            }
            else if (responseBuilder.ToString().Contains("Event: SuccessfulAuth") || responseBuilder.ToString().Contains("Event: RTCPReceived"))
            {
                var t = responseBuilder.ToString();
                return false;
            }
        }

        return false;
    }

}
