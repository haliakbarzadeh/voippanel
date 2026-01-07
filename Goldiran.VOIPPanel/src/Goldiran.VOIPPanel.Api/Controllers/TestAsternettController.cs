using AsterNET.NetStandard.Manager;
using AsterNET.NetStandard.Manager.Action;
using AsterNET.NetStandard.Manager.Event;
using Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePosition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Goldiran.VOIPPanel.Api.Controllers;


//[Authorize(Policy = "position")]
public class TestAsternettController : ApiControllerBase
{
    int i = 0;
    private bool _evetraised = false;
    //[AllowAnonymous]
    ////[Authorize(Policy = "positioncreate")]
    //[Route("create")]
    //[HttpPost]
    ////[HasPermission("Position:create")]
    //public async Task<ActionResult<long>> Create(CreatePositionCommand command)
    //{
    //    //    //var client = new RestClient("http://10.121.21.25:8088/ari/agents/101");
    //    //    //var request = new RestRequest("http://10.121.21.25:8088/ari/agents/101",Method.Put);
    //    //    //var username = "hamid";
    //    //    //var password = "1234";
    //    //    //var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");

    //    //    //request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{username}:{password}"))}");
    //    //    ////request.AddHeader("Postman-Token", "1fe5ffbc-89bc-4536-91b7-9a96a99d54de");
    //    //    //request.AddHeader("cache-control", "no-cache");
    //    //    //request.AddHeader("Content-Type", "application/json");
    //    //    //request.AddParameter("undefined", "{\n\t\"state\":\"available\"\n}", ParameterType.RequestBody);
    //    //    //var response = client.Execute(request);
    //    var que = "Install";
    //var member = "SIP/996";
    //    //var client = new RestClient($"http://10.121.21.25:8088/ari/queues/{que}/members/{member}");
    //    //var request = new RestRequest("http://10.121.21.25:8088/ari/queues/11/members/996", Method.Put);
    //    var client = new RestClient("http://10.121.21.25:8088/ari/asterisk/ping");
    //    var request = new RestRequest("http://10.121.21.25:8088/ari/asterisk/ping", Method.Get);
    //    var username = "hamid";
    //var password = "1234";
    //var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");

    //request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{username}:{password}"))}");
    //    //request.AddHeader("Postman-Token", "1fe5ffbc-89bc-4536-91b7-9a96a99d54de");
    //    request.AddHeader("cache-control", "no-cache");
    //    //request.AddHeader("Content-Type", "application/json");
    //    //request.AddParameter("undefined", "{\n\t\"status\":\"paused\"\n}", ParameterType.RequestBody);
    //    var response = client.Execute(request);

    //    return 1;
    //}

    [AllowAnonymous]
    //[Authorize(Policy = "positioncreate")]
    [Route("create2")]
    [HttpPost]
    //[HasPermission("Position:create")]
    public async Task<ActionResult<long>> Create2(CreatePositionCommand commands)
    {
        //
        //try
        //{
        //    using (var client3 = new TcpClient("10.121.21.25", 5038))
        //    using (var stream = client3.GetStream())
        //    using (var reader = new StreamReader(stream, Encoding.UTF8))
        //    using (var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
        //    {
        //        await ReadResponseAsync(reader);
        //        // Read login response // Send QueueStatus command await writer.WriteLineAsync("Action: QueueStatus\r\n\r\n");
        //        var response3 = await ReadResponseAsync(reader); // Parse response to find member status
        //        var status = ParseQueueStatus(response3, "11", "SIP/996"); // Logoff from AMI await writer.WriteLineAsync("Action: Logoff\r\n\r\n");
        //        await ReadResponseAsync(reader);
        //    }
        //}
        //catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        //
        //ManagerConnection _managerConnection = new ManagerConnection("10.121.21.25", 5038, "hamid", "1234");

        //_managerConnection.Login();

        ////var originateAction = new SIPShowPeerAction() { Peer = "996" };



        //_managerConnection.QueueMember += _managerConnection_QueueMember;
        ////_managerConnection.QueueStatusComplete += _managerConnection_QueueStatusComplete;
        ////_managerConnection.QueueStatusComplete += _managerConnection_QueueStatusComplete;
        ////_managerConnection.Agents += _managerConnection_Agents;
        ////_managerConnection.QueueMemberStatus += _managerConnection_QueueMemberStatus;
        ////_managerConnection.Status += _managerConnection_Status;
        ////_managerConnection.ZapShowChannels += _managerConnection_ZapShowChannels;
        ////_managerConnection.QueueEntry += _managerConnection_QueueEntry;
        ////_managerConnection.NewState += _managerConnection_NewState;
        ////_managerConnection.UseASyncEvents = true;
        ////var action1 = new QueueLogAction() { Queue = "11", Interface = "SIP/996",Event="" };
        //// action1 = new QueueStatusAction { Queue = "11", Member = "SIP/996" };
        ////var action1 = new QueueStatusAction() { Member = "SIP/996" };
        //var action1 = new QueueStatusAction() { Queue = "11" };
        ////var action1 = new CommandAction() { Command= $"queue show\r\n 11" };
        ////var action1 = new SIPShowPeerAction() {  };

        ////var action2 = new ExtensionStateAction() { Exten = "996" };
        ////var action3=new QueueStatusAction() { Member}
        ////ManagerAction action1 = new QueuePauseAction() { Interface = $"SIP/996", Queue = "11", Paused = true };
        //var response2 = _managerConnection.SendAction(action1);

        var t = 1;
        //await Task.Delay(20000);
        //while (!_evetraised)
        //{
        //    Thread.Sleep(10);
        //}

        //if (response2 is ManagerResponse managerResponse)
        //{
        //    if (managerResponse.IsSuccess())
        //    {
        //        // Process the response to get the member status


        //        foreach (var member in managerResponse.Members)
        //        {
        //            Console.WriteLine($"Member: {member.Name}, Status: {member.Status}, Interface: {member.Interface}");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Failed to get queue status: {managerResponse.Message}");
        //    }
        //}
        //if (response2 is QueueStatusResponse queueStatusResponse)
        //{
        //}
        //ManagerAction action = new QueuePauseAction($"SIP/996", "11", true);
        // var action = new QueueAddAction("11", "SIP/996", "996");
        //{
        //    Queue = queueName,
        //    Interface = memberId, // The interface should be the channel ID (e.g., SIP/1001)
        //    Penalty = 0, // Optional: set the penalty for the member
        //    MemberName = memberId // Optional: set a name for the member
        //};

        // Send the action
        //var response = await _managerConnection.SendActionAsync(action);

        //var response1 = _managerConnection.SendAction(action);
        // Send the action
        //for (int I = 0; I < 1000; I++)
        //{
        //    ManagerAction action = new QueuePauseAction($"SIP/{I}", "INSTALL", true);
        //    var response = _managerConnection.SendAction(action);
        //    if (response.IsSuccess())
        //    {
        //        var t = 1;
        //    }
        //}

        //if (response is ManagerResponse managerResponse)
        //{
        //    if (managerResponse.IsSuccess)
        //    {
        //        Console.WriteLine($"Successfully changed status of member {memberId} in queue {queueName} to {status}.");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Failed to change member status: {managerResponse.Message}");
        //    }
        //}
        //else
        //{
        //    Console.WriteLine("Unexpected response type.");
        //var client = new RestClient("http://10.121.21.25:8088/ari/applications");
        //var request = new RestRequest("http://10.121.21.25:8088/ari/applications", Method.Get);
        //var username = "hamid";
        //var password = "1234";
        //var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");

        //request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{username}:{password}"))}");
        ////request.AddHeader("Postman-Token", "1fe5ffbc-89bc-4536-91b7-9a96a99d54de");
        ////request.AddHeader("cache-control", "no-cache");
        ////request.AddHeader("Content-Type", "application/json");
        ////request.AddParameter("undefined", "{\n\t\"state\":\"available\"\n}", ParameterType.RequestBody);
        //var response = await client.ExecuteAsync(request);
        //}
        await test();
        return i;
    }
    private async Task test()
    {
        await Task.Delay(1);
        ManagerConnection _managerConnection = new ManagerConnection("10.121.21.25", 5038, "hamid", "1234");

        _managerConnection.Login();

        //var originateAction = new SIPShowPeerAction() { Peer = "996" };



        _managerConnection.QueueMember += _managerConnection_QueueMember;
        //_managerConnection.QueueStatusComplete += _managerConnection_QueueStatusComplete;
        //_managerConnection.QueueStatusComplete += _managerConnection_QueueStatusComplete;
        //_managerConnection.Agents += _managerConnection_Agents;
        //_managerConnection.QueueMemberStatus += _managerConnection_QueueMemberStatus;
        //_managerConnection.Status += _managerConnection_Status;
        //_managerConnection.ZapShowChannels += _managerConnection_ZapShowChannels;
        //_managerConnection.QueueEntry += _managerConnection_QueueEntry;
        //_managerConnection.NewState += _managerConnection_NewState;
        //_managerConnection.UseASyncEvents = true;
        //var action1 = new QueueLogAction() { Queue = "11", Interface = "SIP/996",Event="" };
        // action1 = new QueueStatusAction { Queue = "11", Member = "SIP/996" };
        //var action1 = new QueueStatusAction() { Member = "SIP/996" };
        var action1 = new QueueStatusAction() { Queue = "11" };

        //var action1 = new CommandAction() { Command= $"queue show\r\n 11" };
        //var action1 = new SIPShowPeerAction() {  };

        //var action2 = new ExtensionStateAction() { Exten = "996" };
        //var action3=new QueueStatusAction() { Member}
        //ManagerAction action1 = new QueuePauseAction() { Interface = $"SIP/996", Queue = "11", Paused = true };
        var response2 = _managerConnection.SendAction(action1);
    }

    private void _managerConnection_NewState(object sender, NewStateEvent e)
    {
        var t = e;
        //throw new NotImplementedException();
    }

    private void _managerConnection_QueueEntry(object sender, QueueEntryEvent e)
    {
        var t = e;
        //throw new NotImplementedException();
    }

    private void _managerConnection_ZapShowChannels(object sender, ZapShowChannelsEvent e)
    {
        var t = e;
       // throw new NotImplementedException();
    }

    private void _managerConnection_Status(object sender, StatusEvent e)
    {
        var t = e;
       // throw new NotImplementedException();
    }

    private void _managerConnection_QueueMemberStatus(object sender, QueueMemberStatusEvent e)
    {
        i++;
        var t = e;
       // throw new NotImplementedException();
    }

    private void _managerConnection_Agents(object sender, AgentsEvent e)
    {
        var t = e;
        //throw new NotImplementedException();
    }

    [AllowAnonymous]
    //[Authorize(Policy = "positioncreate")]
    [Route("create")]
    [HttpPost]
    //[HasPermission("Position:create")]
    public async Task<ActionResult<long>> Create(CreatePositionCommand commands)
    {
        //
        //try
        //{
        //    using (var client3 = new TcpClient("10.121.21.25", 5038))
        //    using (var stream = client3.GetStream())
        //    using (var reader = new StreamReader(stream, Encoding.UTF8))
        //    using (var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
        //    {
        //        await ReadResponseAsync(reader);
        //        // Read login response // Send QueueStatus command await writer.WriteLineAsync("Action: QueueStatus\r\n\r\n");
        //        var response3 = await ReadResponseAsync(reader); // Parse response to find member status
        //        var status = ParseQueueStatus(response3, "11", "SIP/996"); // Logoff from AMI await writer.WriteLineAsync("Action: Logoff\r\n\r\n");
        //        await ReadResponseAsync(reader);
        //    }
        //}
        //catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        //
        ManagerConnection _managerConnection = new ManagerConnection("10.121.21.25", 5038, "hamid", "1234");

        _managerConnection.Login();

        //var originateAction = new SIPShowPeerAction() { Peer = "996" };


        //_managerConnection.QueueMember += _managerConnection_QueueMember;
        //_managerConnection.QueueParams += _managerConnection_QueueParams1;
        //_managerConnection.QueueStatusComplete += _managerConnection_QueueStatusComplete;
        //_managerConnection.QueueStatusComplete += _managerConnection_QueueStatusComplete;
        //var action1 = new QueueLogAction() { Queue = "11", Interface = "SIP/996",Event="" };
        var action1 = new QueueStatusAction { Queue = "5", Member = "SIP/999" };
        //var action1 = new QueueStatusAction() {  };
        //var action2 = new ExtensionStateAction() { Exten = "996" };
        //var action3=new QueueStatusAction() { Member}
        //ManagerAction action1 = new QueuePauseAction() {Interface= $"SIP/996",Queue="11" };
        var response2 = _managerConnection.SendAction(action1);
        //_managerConnection.QueueMember += _managerConnection_QueueMember;
        //while (!_evetraised) 
        //{
        //    Thread.Sleep(10);
        //}
        //if (response2 is ManagerResponse managerResponse)
        //{
        //    if (managerResponse.IsSuccess())
        //    {
        //        // Process the response to get the member status


        //        foreach (var member in managerResponse.Members)
        //        {
        //            Console.WriteLine($"Member: {member.Name}, Status: {member.Status}, Interface: {member.Interface}");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Failed to get queue status: {managerResponse.Message}");
        //    }
        //}
        //if (response2 is QueueStatusResponse queueStatusResponse)
        //{
        //}
        ManagerAction action = new QueuePauseAction($"SIP/996", "11", true);
        // var action = new QueueAddAction("11", "SIP/996", "996");
        //{
        //    Queue = queueName,
        //    Interface = memberId, // The interface should be the channel ID (e.g., SIP/1001)
        //    Penalty = 0, // Optional: set the penalty for the member
        //    MemberName = memberId // Optional: set a name for the member
        //};

        // Send the action
        //var response = await _managerConnection.SendActionAsync(action);

        //var response1 = _managerConnection.SendAction(action);
        //action = new QueueRemoveAction() { Queue = "11", Interface = "SIP/996" };
        //response1 = _managerConnection.SendAction(action);
        //action = new QueueAddAction() { Queue = "11", Interface = "SIP/996" };
        //response1 = _managerConnection.SendAction(action);
        //action = new QueueAddAction() { Queue = "11", Interface = "SIP/999" };
        //response1 = _managerConnection.SendAction(action);
        var response1 = _managerConnection.SendAction(action);
        action = new QueuePauseAction($"SIP/991", "11", true);
        response1 = _managerConnection.SendAction(action);
        //action = new QueueRemoveAction() { Queue = "11", Interface = "SIP/996" };
        //response1 = _managerConnection.SendAction(action);
        action = new QueueAddAction() { Queue = "11", Interface = "SIP/991" };
        response1 = _managerConnection.SendAction(action);
        action = new QueueAddAction() { Queue = "11", Interface = "SIP/992" };
        response1 = _managerConnection.SendAction(action);
        action = new QueueAddAction() { Queue = "11", Interface = "SIP/993" };
        response1 = _managerConnection.SendAction(action);
        action = new QueueAddAction() { Queue = "11", Interface = "SIP/994" };
        response1 = _managerConnection.SendAction(action);
        action = new QueueAddAction() { Queue = "11", Interface = "SIP/995" };
        response1 = _managerConnection.SendAction(action);
        action = new QueueAddAction() { Queue = "11", Interface = "SIP/997" };
        response1 = _managerConnection.SendAction(action);
        action = new QueueAddAction() { Queue = "11", Interface = "SIP/998" };
        response1 = _managerConnection.SendAction(action);
        
        // Send the action
        //for (int I = 0; I < 1000; I++)
        //{
        //    ManagerAction action = new QueuePauseAction($"SIP/{I}", "INSTALL", true);
        //    var response = _managerConnection.SendAction(action);
        //    if (response.IsSuccess())
        //    {
        //        var t = 1;
        //    }
        //}

        //if (response is ManagerResponse managerResponse)
        //{
        //    if (managerResponse.IsSuccess)
        //    {
        //        Console.WriteLine($"Successfully changed status of member {memberId} in queue {queueName} to {status}.");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Failed to change member status: {managerResponse.Message}");
        //    }
        //}
        //else
        //{
        //    Console.WriteLine("Unexpected response type.");
        var client = new RestClient("http://10.121.21.25:8088/ari/applications");
        var request = new RestRequest("http://10.121.21.25:8088/ari/applications", Method.Get);
        var username = "hamid";
        var password = "1234";
        var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");

        request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{username}:{password}"))}");
        //request.AddHeader("Postman-Token", "1fe5ffbc-89bc-4536-91b7-9a96a99d54de");
        //request.AddHeader("cache-control", "no-cache");
        //request.AddHeader("Content-Type", "application/json");
        //request.AddParameter("undefined", "{\n\t\"state\":\"available\"\n}", ParameterType.RequestBody);
        var response = await client.ExecuteAsync(request);
        //}
        return 1;
    }

    private void _managerConnection_QueueParams1(object sender, QueueParamsEvent e)
    {
        var t = e;
        //throw new NotImplementedException();
    }

    private void _managerConnection_QueueParams(object sender, QueueParamsEvent e)
    {
        var t = e;
        //throw new NotImplementedException();
    }

    private void _managerConnection_QueueStatusComplete(object sender, QueueStatusCompleteEvent e)
    {
        var t = e;

        //throw new NotImplementedException();
    }

    //public void _managerConnection_QueueMember(object sender, QueueMemberEvent e)
    //{
    //    var t = e;
    //}
    private void _managerConnection_QueueMember(object sender, QueueMemberEvent e)
    {
        i++;
        _evetraised = true;
        var t = e;
        //throw new NotImplementedException();
    }

    [AllowAnonymous]
    //[Authorize(Policy = "positioncreate")]
    [Route("create1")]
    [HttpPost]
    //[HasPermission("Position:create")]
    public async Task<ActionResult<long>> Create1(CreatePositionCommand commands)
    {
        //
        try
        {
            using (var client3 = new TcpClient("10.121.21.25", 5038))
            using (var stream = client3.GetStream())
            using (var reader = new StreamReader(stream, Encoding.Default))
            using (var writer = new StreamWriter(stream, Encoding.Default) { AutoFlush = true })
            {
                string username = "hamid"; // AMI username
                string password = "1234";
                var queueName = "11"; // Queue name (if needed)
                string memberName = "SIP/996";

                // Login command
                string loginCommand = $"Action: Login\r\nUsername: {username}\r\nSecret: {password}\r\n\r\n";
                await writer.WriteAsync(loginCommand);

                // Read login response
                string response = await reader.ReadLineAsync();
                Console.WriteLine(response);

                // Request queue status
                string queueStatusCommand = $"Action: QueueStatus\r\nQueue: {queueName}\r\n\r\n";
                await writer.WriteAsync(queueStatusCommand);
                //var t = await reader.ReadToEndAsync();
                // Read responses until we reach the end
                while ((response = await reader.ReadLineAsync()) != null )
                {
                    Console.WriteLine(response);
                    if (response.Contains($"Paused:1") || response.Contains($"Paused:0"))
                    {
                        Console.WriteLine($"Status of {memberName}: {response}");
                        break; // Stop reading when we find the member
                    }
                    //if (response.Contains(memberName))
                    //{
                    //    Console.WriteLine($"Status of {memberName}: {response}");
                    //    break; // Stop reading when we find the member
                    //}

                    //if (!response.Contains(memberName) )
                    //{
                    //    continue;     
                    //}

                    // You can add additional checks here to determine when to stop reading
                }

                // Log out from AMI
                string logoutCommand = "Action: Logoff\r\n\r\n"; // Ensure proper formatting
                await writer.WriteAsync(logoutCommand);
                //response = await reader.ReadLineAsync();
                Console.WriteLine(response); // Read logout response
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        //

        return 1;
    }

    [AllowAnonymous]
    //[Authorize(Policy = "positioncreate")]
    [Route("create3")]
    [HttpPost]
    //[HasPermission("Position:create")]
    public async Task<ActionResult<long>> Create3(string queueName,string member)
    {
        string asteriskIp = "10.14.14.34"; // Replace with your Asterisk server IP
        int port = 5038; // Default AMI port
        string username = "gold-panel"; // Replace with your AMI username
        string password = "Voip@123#"; // Replace with your AMI password
        //string queueName = "24"; // Replace with your queue name

        try
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                // Connect to the Asterisk server
                await socket.ConnectAsync(asteriskIp, port);
                Console.WriteLine("Connected to Asterisk server.");

                // Prepare the login command
                string loginCommand = $"Action: Login\r\nUsername: {username}\r\nSecret: {password}\r\n\r\n";
                await SendDataAsync(socket, loginCommand);

                // Prepare the queue status request
                string queueStatusCommand = $"Action: QueueStatus\r\nQueue: {queueName}\r\n\r\n";
                ////string queueStatusCommand = $"Action: QueueSummary\r\nQueue: {queueName}\r\n\r\n";
                ////string queueStatusCommand = $"Action: QueueShow\r\nQueue: 24\r\n\r\n";
                ////string queueStatusCommand = $"Action: QueueLog\r\nQueue: {queueName}\r\n\r\n";
                ////string queueStatusCommand = $"Action: Command:\r\nqueue show {queueName}\r\n\r\n";
                ////string queueStatusCommand = $"Action: Queue Show
                ////string queueStatusCommand = $"Queue Show {queueName}";
                //string queueStatusCommand = $"Action: QueueStatus\r\nQueue: {queueName}\r\nMember: {member}\r\n\r\n";

                await SendDataAsync(socket, queueStatusCommand);

                // Read the response
                string response = await ReceiveDataAsync(socket);
                Console.WriteLine("Response from Asterisk:");
                Console.WriteLine(response);
                //
                //
                IList<string> extensions = new List<string>();

                //var responseSplit = response.Split("Event: QueueMember");
                var responseSplit = response.Split("Event: QueueMember");

                for (int i = 1; i < responseSplit.Length; i++)
                {
                    extensions.Add(responseSplit[i].Substring(responseSplit[i].IndexOf("Name: SIP/")+10, responseSplit[i].IndexOf("\r\nLocation") - responseSplit[i].IndexOf("Name: SIP/")-10));
                }
                //
                //
                // Prepare the logoff command
                string logoffCommand = "Action: Logoff\r\n\r\n";
                await SendDataAsync(socket, logoffCommand);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return 1;
    }

    [AllowAnonymous]
    //[Authorize(Policy = "positioncreate")]
    [Route("create4")]
    [HttpPost]
    //[HasPermission("Position:create")]
    public async Task<ActionResult<long>> Create4(CreatePositionCommand commands)
    {
        string asteriskIp = "10.96.1.104"; // Replace with your Asterisk server IP
        int port = 5038; // Default AMI port
        string username = "gold-panel"; // Replace with your AMI username
        string password = "Voip@123#"; // Replace with your AMI password
        string queueName = "5"; // Replace with your queue name

        try
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                // Connect to the Asterisk server
                await socket.ConnectAsync(asteriskIp, port);
                Console.WriteLine("Connected to Asterisk server.");

                // Prepare the login command
                string loginCommand = $"Action: Login\r\nUsername: {username}\r\nSecret: {password}\r\n\r\n";
                await SendDataAsync(socket, loginCommand);

                // Prepare the queue status request
                //string queueStatusCommand = $"Action: CoreShowChannels\r\n";
                string queueStatusCommand = $"Action: CoreShowChannels\r\n\r\n";
                await SendDataAsync(socket, queueStatusCommand);

                // Read the response
                string response = await ReceiveDataAsync(socket);
                Console.WriteLine("Response from Asterisk:");
                Console.WriteLine(response);
                //
                //
                IList<string> extensions = new List<string>();

                var responseSplit = response.Split("Event: CoreShowChannel");

                for (int i = 1; i < responseSplit.Length; i++)
                {
                    extensions.Add(responseSplit[i].Substring(responseSplit[i].IndexOf("Name: SIP/") + 10, responseSplit[i].IndexOf("\r\nLocation") - responseSplit[i].IndexOf("Name: SIP/") - 10));
                }
                //
                //
                // Prepare the logoff command
                string logoffCommand = "Action: Logoff\r\n\r\n";
                await SendDataAsync(socket, logoffCommand);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return 1;
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

            // Check for end of response (Asterisk sends a blank line)
            //if (responseBuilder.ToString().EndsWith("\r\n\r\n"))
            //{
            //    break;
            //}
            if (responseBuilder.ToString().StartsWith("Event: CoreShowAction"))
            {
                // Process CoreShowAction event
                Console.WriteLine("CoreShowAction Event Received:");
                //QueueStatusComplete
            }
            //if (responseBuilder.ToString().Contains("Event: CoreShowChannelsComplete"))
            //{
            //    break;
            //}
            if (responseBuilder.ToString().Contains("Event: CoreShowChannelsComplete"))
            {
                break;
            }
            var t = responseBuilder.ToString();
        }

        return responseBuilder.ToString();
    }
    async Task<string> ReadResponseAsync(StreamReader reader)
    {
        var response = new StringBuilder();
        string line;
        while (!string.IsNullOrEmpty(line = await reader.ReadLineAsync()))
        {
            response.AppendLine(line);
            if (line == "--END COMMAND--") break; // AMI response end marker
        }
        return response.ToString();
    }

    private string ParseQueueStatus(string response, string queueName, string memberName)
    {
        var lines = response.Split("\n");
        bool isTargetQueue = false;
        foreach (var line in lines)
        {
            if (line.StartsWith("Queue:") && line.Contains(queueName)) isTargetQueue = true;
            if (isTargetQueue && line.StartsWith("Member:") && line.Contains(memberName))
            {
                // Look for the Status field in the next few lines
                foreach (var statusLine in lines)
                { if (statusLine.StartsWith("Status:")) return statusLine.Split(":")[1].Trim(); }
            }
        }
        return "Not Found";
    }

    [AllowAnonymous]
    //[Authorize(Policy = "positioncreate")]
    [Route("create5")]
    [HttpPost]
    public async Task<ActionResult<long>> Create5(CreatePositionCommand commands)
    {
        //
        //try
        //{
        //    using (var client3 = new TcpClient("10.121.21.25", 5038))
        //    using (var stream = client3.GetStream())
        //    using (var reader = new StreamReader(stream, Encoding.UTF8))
        //    using (var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
        //    {
        //        await ReadResponseAsync(reader);
        //        // Read login response // Send QueueStatus command await writer.WriteLineAsync("Action: QueueStatus\r\n\r\n");
        //        var response3 = await ReadResponseAsync(reader); // Parse response to find member status
        //        var status = ParseQueueStatus(response3, "11", "SIP/996"); // Logoff from AMI await writer.WriteLineAsync("Action: Logoff\r\n\r\n");
        //        await ReadResponseAsync(reader);
        //    }
        //}
        //catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        //
        ManagerConnection _managerConnection = new ManagerConnection("10.121.21.25", 5038, "hamid", "1234");

        _managerConnection.Login();

        //var originateAction = new SIPShowPeerAction() { Peer = "996" };


        _managerConnection.NewChannel += _managerConnection_NewChannel;
        //_managerConnection.ZapShowChannels
        _managerConnection.QueueParams += _managerConnection_QueueParams1;
        _managerConnection.QueueMember += _managerConnection_QueueMember;
        //_managerConnection.QueueStatusComplete += _managerConnection_QueueStatusComplete;
        //var action1 = new QueueLogAction() { Queue = "11", Interface = "SIP/996",Event="" };
        var action1 = new CoreShowChannelsAction {};
        //var action1 = new QueueStatusAction() {  };
        //var action2 = new ExtensionStateAction() { Exten = "996" };
        //var action3=new QueueStatusAction() { Member}
        //ManagerAction action1 = new QueuePauseAction() {Interface= $"SIP/996",Queue="11" };
        var response2 = _managerConnection.SendAction(action1);
        //_managerConnection.QueueMember += _managerConnection_QueueMember;
        //while (!_evetraised) 
        //{
        //    Thread.Sleep(10);
        //}
        //if (response2 is ManagerResponse managerResponse)
        //{
        //    if (managerResponse.IsSuccess())
        //    {
        //        // Process the response to get the member status


        //        foreach (var member in managerResponse.Members)
        //        {
        //            Console.WriteLine($"Member: {member.Name}, Status: {member.Status}, Interface: {member.Interface}");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Failed to get queue status: {managerResponse.Message}");
        //    }
        //}
        //if (response2 is QueueStatusResponse queueStatusResponse)
        //{
        //}
        ManagerAction action = new QueuePauseAction($"SIP/996", "11", true);
        // var action = new QueueAddAction("11", "SIP/996", "996");
        //{
        //    Queue = queueName,
        //    Interface = memberId, // The interface should be the channel ID (e.g., SIP/1001)
        //    Penalty = 0, // Optional: set the penalty for the member
        //    MemberName = memberId // Optional: set a name for the member
        //};

        // Send the action
        //var response = await _managerConnection.SendActionAsync(action);

        //var response1 = _managerConnection.SendAction(action);
        //action = new QueueRemoveAction() { Queue = "11", Interface = "SIP/996" };
        //response1 = _managerConnection.SendAction(action);
        //action = new QueueAddAction() { Queue = "11", Interface = "SIP/996" };
        //response1 = _managerConnection.SendAction(action);
        //action = new QueueAddAction() { Queue = "11", Interface = "SIP/999" };
        //response1 = _managerConnection.SendAction(action);
        var response1 = _managerConnection.SendAction(action);
        action = new QueuePauseAction($"SIP/991", "11", true);
        response1 = _managerConnection.SendAction(action);
        //action = new QueueRemoveAction() { Queue = "11", Interface = "SIP/996" };
        //response1 = _managerConnection.SendAction(action);
        action = new QueueAddAction() { Queue = "11", Interface = "SIP/991" };
        response1 = _managerConnection.SendAction(action);
        action = new QueueAddAction() { Queue = "11", Interface = "SIP/992" };
        response1 = _managerConnection.SendAction(action);
        action = new QueueAddAction() { Queue = "11", Interface = "SIP/993" };
        response1 = _managerConnection.SendAction(action);
        action = new QueueAddAction() { Queue = "11", Interface = "SIP/994" };
        response1 = _managerConnection.SendAction(action);
        action = new QueueAddAction() { Queue = "11", Interface = "SIP/995" };
        response1 = _managerConnection.SendAction(action);
        action = new QueueAddAction() { Queue = "11", Interface = "SIP/997" };
        response1 = _managerConnection.SendAction(action);
        action = new QueueAddAction() { Queue = "11", Interface = "SIP/998" };
        response1 = _managerConnection.SendAction(action);
        // Send the action
        //for (int I = 0; I < 1000; I++)
        //{
        //    ManagerAction action = new QueuePauseAction($"SIP/{I}", "INSTALL", true);
        //    var response = _managerConnection.SendAction(action);
        //    if (response.IsSuccess())
        //    {
        //        var t = 1;
        //    }
        //}

        //if (response is ManagerResponse managerResponse)
        //{
        //    if (managerResponse.IsSuccess)
        //    {
        //        Console.WriteLine($"Successfully changed status of member {memberId} in queue {queueName} to {status}.");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Failed to change member status: {managerResponse.Message}");
        //    }
        //}
        //else
        //{
        //    Console.WriteLine("Unexpected response type.");
        var client = new RestClient("http://10.121.21.25:8088/ari/applications");
        var request = new RestRequest("http://10.121.21.25:8088/ari/applications", Method.Get);
        var username = "hamid";
        var password = "1234";
        var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");

        request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{username}:{password}"))}");
        //request.AddHeader("Postman-Token", "1fe5ffbc-89bc-4536-91b7-9a96a99d54de");
        //request.AddHeader("cache-control", "no-cache");
        //request.AddHeader("Content-Type", "application/json");
        //request.AddParameter("undefined", "{\n\t\"state\":\"available\"\n}", ParameterType.RequestBody);
        var response = await client.ExecuteAsync(request);
        //}
        return 1;
    }

    [AllowAnonymous]
    //[Authorize(Policy = "positioncreate")]
    [Route("create6")]
    [HttpPost]
    //[HasPermission("Position:create")]
    public async Task<ActionResult<long>> Create6(CreatePositionCommand commands)
    {
        string asteriskIp = "10.121.21.25"; // Replace with your Asterisk server IP
        int port = 5038; // Default AMI port
        string username = "hamid"; // Replace with your AMI username
        string password = "1234"; // Replace with your AMI password
        string queueName = "11"; // Replace with your queue name

        try
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                // Connect to the Asterisk server
                await socket.ConnectAsync(asteriskIp, port);
                Console.WriteLine("Connected to Asterisk server.");

                // Prepare the login command
                string loginCommand = $"Action: Login\r\nUsername: {username}\r\nSecret: {password}\r\n\r\n";
                await SendDataAsync(socket, loginCommand);

                // Prepare the queue status request
                //string queueStatusCommand = $"Action: CoreShowChannels\r\n";
                //string queueStatusCommand = $"Action: Queue Show\r\nQueue: 11\r\n\r\n";
                //string queueStatusCommand = $"Action: QueueLog\r\nQueue: {queueName}\r\n\r\n";
                //string queueStatusCommand = $"Action: Command:\r\nqueue show {queueName}\r\n\r\n";
                //string queueStatusCommand = $"Action: Queue Show 11";
                string queueStatusCommand = $"Action: QueueShow Queue:11";
                //string queueStatusCommand = $"Action: CoreShowChannels\r\n\r\n";
                await SendDataAsync(socket, queueStatusCommand);

                // Read the response
                string response = await ReceiveDataAsync(socket);
                Console.WriteLine("Response from Asterisk:");
                Console.WriteLine(response);
                //
                //
                IList<string> extensions = new List<string>();

                var responseSplit = response.Split("Event: QueueMember");

                for (int i = 1; i < responseSplit.Length; i++)
                {
                    extensions.Add(responseSplit[i].Substring(responseSplit[i].IndexOf("Name: SIP/") + 10, responseSplit[i].IndexOf("\r\nLocation") - responseSplit[i].IndexOf("Name: SIP/") - 10));
                }
                //
                //
                // Prepare the logoff command
                string logoffCommand = "Action: Logoff\r\n\r\n";
                await SendDataAsync(socket, logoffCommand);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return 1;
    }

    private void _managerConnection_NewChannel(object sender, NewChannelEvent e)
    {
        var t = e;
        //throw new NotImplementedException();
    }

    [AllowAnonymous]
    //[Authorize(Policy = "positioncreate")]
    [Route("create7")]
    [HttpPost]
    //[HasPermission("Position:create")]
    public async Task<ActionResult<long>> Create7(CreatePositionCommand commands)
    {
        string asteriskIp = "10.121.21.25"; // Replace with your Asterisk server IP
        int port = 5038; // Default AMI port
        string username = "hamid"; // Replace with your AMI username
        string password = "1234"; // Replace with your AMI password
        string queueName = "11"; // Replace with your queue name

        try
        {
            using (var client = new ClientWebSocket())
            {
                // Connect to your Asterisk WebSocket server
                await client.ConnectAsync(new Uri("ws://10.121.21.25:8088/"), CancellationToken.None);

                // Prepare the login action
                var loginAction = new
                {
                    Action = "Login",
                    Username =username,
                    Secret = password
                };

                // Serialize to JSON
                var loginJson = JsonSerializer.Serialize(loginAction);
                var loginBytes = Encoding.UTF8.GetBytes(loginJson);
                var loginBuffer = new ArraySegment<byte>(loginBytes);

                // Send the login action
                await client.SendAsync(loginBuffer, WebSocketMessageType.Text, true, CancellationToken.None);

                // Receive the login response
                var responseBuffer = new byte[1024];
                var result = await client.ReceiveAsync(new ArraySegment<byte>(responseBuffer), CancellationToken.None);
                var loginResponse = Encoding.UTF8.GetString(responseBuffer, 0, result.Count);
                Console.WriteLine("Login Response: " + loginResponse);

                // Check if login was successful
                if (loginResponse.Contains("Success"))
                {
                    // Prepare the QueueShow action
                    var queueShowAction = new
                    {
                        Action = "QueueShow",
                        Queue = "11"
                    };

                    // Serialize to JSON
                    var queueShowJson = JsonSerializer.Serialize(queueShowAction);
                    var queueShowBytes = Encoding.UTF8.GetBytes(queueShowJson);
                    var queueShowBuffer = new ArraySegment<byte>(queueShowBytes);

                    // Send the QueueShow action
                    await client.SendAsync(queueShowBuffer, WebSocketMessageType.Text, true, CancellationToken.None);

                    // Receive the QueueShow response
                    result = await client.ReceiveAsync(new ArraySegment<byte>(responseBuffer), CancellationToken.None);
                    var queueShowResponse = Encoding.UTF8.GetString(responseBuffer, 0, result.Count);

                    // Output the QueueShow response
                    Console.WriteLine("QueueShow Response: " + queueShowResponse);
                }
                else
                {
                    Console.WriteLine("Login failed.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return 1;
    }

    [AllowAnonymous]
    //[Authorize(Policy = "positioncreate")]
    [Route("create8")]
    [HttpPost]
    //[HasPermission("Position:create")]
    public async Task<ActionResult<long>> Create8(CreatePositionCommand commands)
    {
        string asteriskIp = "10.121.21.25"; // Replace with your Asterisk server IP
        int port = 5038; // Default AMI port
        string username = "hamid"; // Replace with your AMI username
        string password = "1234"; // Replace with your AMI password
        string queueName = "11"; // Replace with your queue name

        try
        {
            using (var client = new TcpClient(asteriskIp, port))
            using (var stream = client.GetStream())
            using (var reader = new StreamReader(stream, Encoding.ASCII))
            using (var writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true })
            {
                // Login to Asterisk
                string loginCommand = $"Action: Login\r\nUsername: {username}\r\nSecret: {password}\r\n\r\n";
                await writer.WriteAsync(loginCommand);

                // Read login response
                string responsee = await reader.ReadLineAsync();
                Console.WriteLine(responsee);

                // Originate a call to ChanSpy
                await OriginateCall(writer, "SIP/998", "SIP/996"); // Replace with the appropriate channels

                // Read response
                string response;
                while ((response = await reader.ReadLineAsync()) != null)
                {
                    Console.WriteLine(response);
                    if (response.Contains("Response: Success"))
                    {
                        break; // Exit loop when success response is received
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return 1;
    }

    private  async Task SendCommand(StreamWriter writer, string action, string channel = null)
    {
        var command = new StringBuilder();
        command.AppendLine($"Action: {action}");
        if (channel != null)
        {
            command.AppendLine($"Channel: {channel}");
        }
        command.AppendLine("Events: off"); // Disable event notifications
        command.AppendLine(); // Blank line to end command

        await writer.WriteAsync(command.ToString());
    }

    private  async Task OriginateCall(StreamWriter writer, string channel, string context)
    {
        var command = new StringBuilder();
        command.AppendLine("Action: Originate");
        command.AppendLine($"Channel: SIP/996");
        command.AppendLine($"Context: ChanSpy"); // Use the context where ChanSpy is defined
        command.AppendLine($"Exten: 998"); // The extension to call
        command.AppendLine("Priority: 1");
        command.AppendLine($"CallerId: 999");
        command.AppendLine(); // Blank line to end command

        await writer.WriteAsync(command.ToString());
    }

    [AllowAnonymous]
    //[Authorize(Policy = "positioncreate")]
    [Route("create9")]
    [HttpPost]
    //[HasPermission("Position:create")]
    public async Task<ActionResult<long>> Create9(CreatePositionCommand commands)
    {
        //
        //try
        //{
        //    using (var client3 = new TcpClient("10.121.21.25", 5038))
        //    using (var stream = client3.GetStream())
        //    using (var reader = new StreamReader(stream, Encoding.UTF8))
        //    using (var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
        //    {
        //        await ReadResponseAsync(reader);
        //        // Read login response // Send QueueStatus command await writer.WriteLineAsync("Action: QueueStatus\r\n\r\n");
        //        var response3 = await ReadResponseAsync(reader); // Parse response to find member status
        //        var status = ParseQueueStatus(response3, "11", "SIP/996"); // Logoff from AMI await writer.WriteLineAsync("Action: Logoff\r\n\r\n");
        //        await ReadResponseAsync(reader);
        //    }
        //}
        //catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        //
        string asteriskIp = "10.96.1.104"; // Replace with your Asterisk server IP
        int port = 5038; // Default AMI port
        string username = "gold-panel"; // Replace with your AMI username
        string password = "Voip@123#";
        ManagerConnection _managerConnection = new ManagerConnection(asteriskIp, 5038, username, password);

        _managerConnection.Login();


        //var action1 = new OriginateAction { CallerId="101",Channel="SIP/996",Context= "chanspy", Exten="70",Priority="1" };
        //var action1 = new OriginateAction { CallerId = "998", Channel = "SIP/999", Context = "chanspy", Exten = "70", Priority = "1" };
        //action1.SetVariable("path", "998");
        var action1 = new OriginateAction { CallerId = "998", Channel = "SIP/999", Context = "whisper", Exten = "72", Priority = "1" };
        action1.SetVariable("path", "998");


        var response2 = _managerConnection.SendAction(action1);


        //}
        return 1;
    }

    [AllowAnonymous]
    //[Authorize(Policy = "positioncreate")]
    [Route("create10")]
    [HttpPost]
    public async Task<ActionResult<long>> Create10(CreatePositionCommand commands)
    {
        string asteriskIp = "10.96.1.104"; // Replace with your Asterisk server IP
        int port = 5038; // Default AMI port
        string username = "gold-panel"; // Replace with your AMI username
        string password = "Voip@123#"; // Replace with your AMI password
        string queueName = "5"; // Replace with your queue name

        try
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                // Connect to the Asterisk server
                await socket.ConnectAsync(asteriskIp, port);
                Console.WriteLine("Connected to Asterisk server.");

                // Prepare the login command
                string loginCommand = $"Action: Login\r\nUsername: {username}\r\nSecret: {password}\r\n\r\n";
                await SendDataAsync(socket, loginCommand);

                // Prepare the queue status request
                string queueStatusCommand = $"Action: QueueAdd\r\n" +
                 $"Interface: SIP/998\r\n" +
                 $"Queue: 5\r\n" +
                 $"MemberName: SIP/998\r\n" +
                $"Penalty: 0\r\n\r\n";
                //string queueStatusCommand = $"Action: QueueRemove\r\n" +
                //         $"Interface: SIP/999\r\n" +
                //         $"Queue: 5\r\n" +
                //         $"MemberName: SIP/999\r\n\r\n";
                //$"Penalty: 0\r\n\r\n";
                await SendDataAsync(socket, queueStatusCommand);
                queueStatusCommand = $"Action: QueuePause\r\n" +
                       $"Interface: SIP/998\r\n" +
                       $"Queue: 5\r\n" +
                       $"Paused: 0\r\n\r\n";

                await SendDataAsync(socket, queueStatusCommand);

                // Read the response
                string response = await ReceiveDataAsync(socket);
                Console.WriteLine("Response from Asterisk:");
                //
                //
                IList<string> extensions = new List<string>();

                //var responseSplit = response.Split("Event: QueueMember");
                
                // Prepare the logoff command
                string logoffCommand = "Action: Logoff\r\n\r\n";
                await SendDataAsync(socket, logoffCommand);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return 1;
    }

    [AllowAnonymous]
    //[Authorize(Policy = "positioncreate")]
    [Route("create11")]
    [HttpPost]
    //[HasPermission("Position:create")]
    public async Task<ActionResult<long>> Create11(CreatePositionCommand commands)
    {

        ManagerConnection _managerConnection = new ManagerConnection("10.96.1.104", 5038, "gold-panel", "Voip@123#");

        _managerConnection.Login();


        var action = new QueueAddAction() { Queue = "5", Interface = "SIP/999" };
        var response1 = _managerConnection.SendAction(action);
        var action1 = new QueuePauseAction($"SIP/999", "5", true);
        response1 = _managerConnection.SendAction(action1);

        return 1;
    }
}
// Usa
