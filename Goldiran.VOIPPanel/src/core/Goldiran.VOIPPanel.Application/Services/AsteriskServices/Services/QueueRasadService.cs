using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using Goldiran.VOIPPanel.ReadModel.Entities;
using System.Net.Sockets;
using System.Text;

namespace Goldiran.VOIPPanel.Application.Services.AsteriskServices.Services;

public class QueueRasadService : IQueueRasadService
{
    private readonly IReadModelContext _readModelContext;
    public QueueRasadService(IReadModelContext readModelContext)
    {
        _readModelContext = readModelContext;
    }

    public async Task<QueueRasadResponse> GetQueueRasad(QueueRasadRequest request)
    {
        int port = 5038;

        List<QueueRasadResponseInfo> queueRasadResponseInfoList = new List<QueueRasadResponseInfo>();
        foreach (var queue in request.QueueRasadInfoList)
        {
            try
            {
                var queueRasadResponseInfo = new QueueRasadResponseInfo();
                queueRasadResponseInfo.QueueCode = queue.Code;
                queueRasadResponseInfo.QueueName = queue.Name;

                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    // Connect to the Asterisk server
                    await socket.ConnectAsync(queue.IPAddress, port);

                    // Prepare the login command
                    string loginCommand = $"Action: Login\r\nUsername: {queue.User}\r\nSecret: {queue.Secret}\r\n\r\n";
                    await SendDataAsync(socket, loginCommand);

                    string queueStatusCommand = $"Action: QueueStatus\r\nQueue: {queue.Code}\r\n\r\n";
                    //string queueStatusCommand = $"Queue Show {queueName}";
                    await SendDataAsync(socket, queueStatusCommand);
                    // Read the response
                    string response = await ReceiveDataAsync(socket);

                    string queueParam = response;
                    if (response.Contains("Event: QueueMember"))
                        queueParam = response.Substring(response.IndexOf("Event: QueueParams"), response.IndexOf("Event: QueueMember") - response.IndexOf("Event: QueueParams"));

                    queueRasadResponseInfo.Ans = Convert.ToInt32(queueParam.Substring(queueParam.IndexOf("Completed:") + 11, queueParam.IndexOf("Abandoned:") - (queueParam.IndexOf("Completed:") + 11)).Trim());
                    queueRasadResponseInfo.SL = Convert.ToDecimal(queueParam.Substring(queueParam.IndexOf("ServicelevelPerf:") + 18, queueParam.IndexOf("ServicelevelPerf2:") - (queueParam.IndexOf("ServicelevelPerf:") + 18)).Trim());
                    queueRasadResponseInfo.AC = Convert.ToInt32(queueParam.Substring(queueParam.IndexOf("Abandoned:") + 11, queueParam.IndexOf("ServiceLevel:") - (queueParam.IndexOf("Abandoned:") + 11)).Trim());
                    queueRasadResponseInfo.Waiting = Convert.ToInt32(queueParam.Substring(queueParam.IndexOf("Calls:") + 7, queueParam.IndexOf("Holdtime:") - (queueParam.IndexOf("Calls:") + 7)).Trim());

                    var responseSplit = response.Split("Event: QueueMember");

                    for (int i = 1; i < responseSplit.Length; i++)
                    {
                        if (responseSplit[i].Substring(responseSplit[i].IndexOf("Paused:") + 8, responseSplit[i].IndexOf("PausedReason:") - (responseSplit[i].IndexOf("Paused:") + 8)).Trim() == "1")
                            queueRasadResponseInfo.Break++;

                        if (responseSplit[i].Substring(responseSplit[i].IndexOf("Status:") + 8, responseSplit[i].IndexOf("Paused:") - (responseSplit[i].IndexOf("Status:") + 8)).Trim() == "1" 
                            && responseSplit[i].Substring(responseSplit[i].IndexOf("Paused:") + 8, responseSplit[i].IndexOf("PausedReason:") - (responseSplit[i].IndexOf("Paused:") + 8)).Trim() == "0")
                            queueRasadResponseInfo.Free++;

                        if (responseSplit[i].Substring(responseSplit[i].IndexOf("Status:") + 8, responseSplit[i].IndexOf("Paused:") - (responseSplit[i].IndexOf("Status:") + 8)).Trim() == "2"
                            || responseSplit[i].Substring(responseSplit[i].IndexOf("Status:") + 8, responseSplit[i].IndexOf("Paused:") - (responseSplit[i].IndexOf("Status:") + 8)).Trim() == "3")
                            queueRasadResponseInfo.Busy++;
                    }
                    // Prepare the logoff command
                    string logoffCommand = "Action: Logoff\r\n\r\n";
                    await SendDataAsync(socket, logoffCommand);

                    queueRasadResponseInfoList.Add(queueRasadResponseInfo);
                }

            }
            catch (Exception)
            {


            }

        }

        return new QueueRasadResponse() { QueueRasadResponseInfoList= queueRasadResponseInfoList };
    }
    private async Task SendDataAsync(Socket socket, string data)
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

            if (responseBuilder.ToString().Contains("Event: QueueStatusComplete"))
            {
                break;
            }
        }

        return responseBuilder.ToString();
    }

}
