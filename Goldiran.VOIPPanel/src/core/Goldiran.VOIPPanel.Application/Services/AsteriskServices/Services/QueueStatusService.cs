using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using Goldiran.VOIPPanel.ReadModel.Entities;
using System.Net.Sockets;
using System.Text;

namespace Goldiran.VOIPPanel.Application.Services.AsteriskServices.Services;

public class QueueStatusService : IQueueStatusService
{
    private readonly IReadModelContext _readModelContext;
    public QueueStatusService(IReadModelContext readModelContext)
    {
        _readModelContext = readModelContext;
    }

    public async Task<List<QueueStatusResponse>> GetQueueStatus(QueueStatusRequest request)
    {
        var queueCodeList = request.QueueCodeList.Select(c => Convert.ToInt32(c)).ToList();
        var queueList = _readModelContext.Set<Queu>().Where(c => queueCodeList.Contains(c.Code)).ToList();
        int port = 5038; // Default AMI port

        List<QueueStatusResponse> results = new List<QueueStatusResponse>();
        foreach (var queue in queueList)
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                // Connect to the Asterisk server
                await socket.ConnectAsync(queue.IPAddress, port);
                Console.WriteLine("Connected to Asterisk server.");

                // Prepare the login command
                string loginCommand = $"Action: Login\r\nUsername: {queue.User}\r\nSecret: {queue.Secret}\r\n\r\n";
                await SendDataAsync(socket, loginCommand);

                string queueStatusCommand = $"Action: QueueStatus\r\nQueue: {queue.Code}\r\n\r\n";
                //string queueStatusCommand = $"Queue Show {queueName}";
                await SendDataAsync(socket, queueStatusCommand);
                // Read the response
                string response = await ReceiveDataAsync(socket);

                results.Add(new QueueStatusResponse() { QueueCode = queue.Code.ToString(), PausedCount = response.Split("Paused: 1").Count() - 1, UnPausedCount = response.Split("Paused: 0").Count() - 1, ExtensionList = GetExtensions(response) });

                //Console.WriteLine("Response from Asterisk:");
                //Console.WriteLine(response);

                // Prepare the logoff command
                string logoffCommand = "Action: Logoff\r\n\r\n";
                await SendDataAsync(socket, logoffCommand);
            }

        }

        return results;
    }
    private async Task SendDataAsync(Socket socket, string data)
    {
        byte[] byteData = Encoding.ASCII.GetBytes(data);
        await socket.SendAsync(new ArraySegment<byte>(byteData), SocketFlags.None);
    }

    private async Task<string> ReceiveDataAsync(Socket socket)
    {
        StringBuilder responseBuilder = new StringBuilder();
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None)) > 0)
        {
            responseBuilder.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));

            if (responseBuilder.ToString().Contains("Event: QueueStatusComplete"))
            {
                break;
            }
        }

        return responseBuilder.ToString();
    }

    private IList<string> GetExtensions(string response)
    {
        IList<string> extensions = new List<string>();

        var responseSplit = response.Split("Event: QueueMember");

        if (responseSplit.Length <= 1)
            return extensions;

        for (int i = 1; i < responseSplit.Length; i++)
        {
            extensions.Add(responseSplit[i].Substring(responseSplit[i].IndexOf("Name: SIP/") + 10, responseSplit[i].IndexOf("\r\nLocation") - responseSplit[i].IndexOf("Name: SIP/") - 10));
        }

        return extensions;
    }

}
