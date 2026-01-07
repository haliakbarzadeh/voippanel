using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Microsoft.Extensions.Configuration;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Goldiran.VOIPPanel.Application.Services.AsteriskServices.Services;

public class OperationRasadService : IOperationRasadService
{
    private readonly IConfiguration _configuration;
    public OperationRasadService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<OperationRasadResponse>> GetOperationRasad(OperationRasadRequest request)
    {
        List<OperationRasadResponse> operationRasadResponseList = new List<OperationRasadResponse>();

        //var ipAddress = _configuration.GetValue<string>("IzabelConfigs:IPAddress");
        var ipAddress = request.IP;
        var username = _configuration.GetValue<string>("IzabelConfigs:UserName");
        var password = _configuration.GetValue<string>("IzabelConfigs:Pass");
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            // Connect to the Asterisk server
            await socket.ConnectAsync(ipAddress, 5038);

            // Prepare the login command
            string loginCommand = $"Action: Login\r\nUsername: {username}\r\nSecret: {password}\r\n\r\n";
            await SendDataAsync(socket, loginCommand);

            // Prepare the queue status request
            //string queueStatusCommand = $"Action: CoreShowChannels\r\n";
            string queueStatusCommand = $"Action: CoreShowChannels\r\n\r\n";
            await SendDataAsync(socket, queueStatusCommand);

            // Read the response
            string response = await ReceiveDataAsync(socket);

            var responseSplit = response.Split("Event: CoreShowChannel");

            for (int i = 1; i < responseSplit.Length; i++)
            {
                var operationRasadResponse = new OperationRasadResponse();
                try
                {
                    if (responseSplit[i].Contains("ConnectedLineNum: <unknown>") && !responseSplit[i].Contains("ConnectedLineNum:"))
                        continue;

                    if(responseSplit[i].Contains("Context: macro-dialout-trunk") || responseSplit[i].Contains("ApplicationData: (Outgoing Line)"))
                        operationRasadResponse.QueueCode = "تماس خروجی";
                    else if (responseSplit[i].Contains("Context: macro-dial-one"))
                        operationRasadResponse.QueueCode = "تماس ورودی مستقیم";                  
                    else
                        operationRasadResponse.QueueCode = responseSplit[i].Substring(responseSplit[i].IndexOf("Exten:") + 7, responseSplit[i].IndexOf("Priority:") - (responseSplit[i].IndexOf("Exten:") + 7)).Trim();

                    if (responseSplit[i].Contains("ApplicationData: (Outgoing Line)") && responseSplit[i].Contains("Application: AppDial"))
                    {
                        operationRasadResponse.Caller = responseSplit[i].Substring(responseSplit[i].IndexOf("CallerIDNum:") + 13, responseSplit[i].IndexOf("CallerIDName:") - (responseSplit[i].IndexOf("CallerIDNum:") + 13)).Trim();
                        operationRasadResponse.Extension = responseSplit[i].Substring(responseSplit[i].IndexOf("ConnectedLineNum:") + 18, responseSplit[i].IndexOf("ConnectedLineName:") - (responseSplit[i].IndexOf("ConnectedLineNum:") + 18)).Trim();

                    }
                    else if (responseSplit[i].Contains("ApplicationData: (Outgoing Line)") && responseSplit[i].Contains("Application: AppQueue"))
                    {
                        operationRasadResponse.Caller = responseSplit[i].Substring(responseSplit[i].IndexOf("ConnectedLineNum:") + 18, responseSplit[i].IndexOf("ConnectedLineName:") - (responseSplit[i].IndexOf("ConnectedLineNum:") + 18)).Trim();
                        operationRasadResponse.Extension = responseSplit[i].Substring(responseSplit[i].IndexOf("Channel: SIP/") + 14, responseSplit[i].IndexOf("-") - (responseSplit[i].IndexOf("Channel: SIP/") + 14)).Trim();

                    }
                    else if (responseSplit[i].Contains("Context: ext-queues") || responseSplit[i].Contains("Context: from-trunk"))
                    {
                        operationRasadResponse.Caller = responseSplit[i].Substring(responseSplit[i].IndexOf("CallerIDNum:") + 13, responseSplit[i].IndexOf("CallerIDName:") - (responseSplit[i].IndexOf("CallerIDNum:") + 13)).Trim();
                        operationRasadResponse.Extension = responseSplit[i].Substring(responseSplit[i].IndexOf("ConnectedLineNum:") + 18, responseSplit[i].IndexOf("ConnectedLineName:") - (responseSplit[i].IndexOf("ConnectedLineNum:") + 18)).Trim();
                    }
                    else
                    {
                        operationRasadResponse.Extension = responseSplit[i].Substring(responseSplit[i].IndexOf("CallerIDNum:") + 13, responseSplit[i].IndexOf("CallerIDName:") - (responseSplit[i].IndexOf("CallerIDNum:") + 13)).Trim();
                        operationRasadResponse.Caller = responseSplit[i].Substring(responseSplit[i].IndexOf("ConnectedLineNum:") + 18, responseSplit[i].IndexOf("ConnectedLineName:") - (responseSplit[i].IndexOf("ConnectedLineNum:") + 18)).Trim();
                    }


                    operationRasadResponse.Duration = responseSplit[i].Substring(responseSplit[i].IndexOf("Duration:") + 10, responseSplit[i].IndexOf("BridgeId:") - (responseSplit[i].IndexOf("Duration:") + 10)).Trim();
                    operationRasadResponseList.Add(operationRasadResponse);
                }
                catch (Exception ex)
                {
                    
                }
                
            }
            //
            //
            // Prepare the logoff command
            string logoffCommand = "Action: Logoff\r\n\r\n";
            await SendDataAsync(socket, logoffCommand);
        }

        return operationRasadResponseList;
    }
    async Task SendDataAsync(Socket socket, string data)
    {
        byte[] byteData = Encoding.ASCII.GetBytes(data);
        await socket.SendAsync(new ArraySegment<byte>(byteData), SocketFlags.None);
    }

    async Task<string> ReceiveDataAsync(Socket socket)
    {
        StringBuilder responseBuilder = new StringBuilder();
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None)) > 0)
        {
            responseBuilder.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));

            if (responseBuilder.ToString().Contains("Event: CoreShowChannelsComplete"))
            {
                break;
            }
            //var t = responseBuilder.ToString();
        }

        return responseBuilder.ToString();
    }

    //private IList<string> GetExtensions(string response)
    //{
    //    IList<string> extensions = new List<string>();

    //    var responseSplit = response.Split("Event: QueueMember");

    //    if (responseSplit.Length <= 1)
    //        return extensions;

    //    for (int i = 1; i < responseSplit.Length; i++)
    //    {
    //        extensions.Add(responseSplit[i].Substring(responseSplit[i].IndexOf("Name: SIP/") + 10, responseSplit[i].IndexOf("\r\nLocation") - responseSplit[i].IndexOf("Name: SIP/") - 10));
    //    }

    //    return extensions;
    //}

}
