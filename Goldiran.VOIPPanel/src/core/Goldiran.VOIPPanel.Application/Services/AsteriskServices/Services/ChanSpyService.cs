using AsterNET.NetStandard.Manager.Action;
using AsterNET.NetStandard.Manager;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.Application.Features.Rasads.Enums;

namespace Goldiran.VOIPPanel.Application.Services.AsteriskServices.Services;

public class ChanSpyService: IChanSpyService
{
    private readonly IConfiguration _configuration;
    public ChanSpyService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SetChanSpy(ChanSpyRequest request)
    {
        //var ipAddress = _configuration.GetValue<string>("IzabelConfigs:IPAddress");
        var ipAddress =request.IP;
        var username = _configuration.GetValue<string>("IzabelConfigs:UserName");
        var password = _configuration.GetValue<string>("IzabelConfigs:Pass");

        ManagerConnection _managerConnection = new ManagerConnection(ipAddress, 5038, username, password);

        _managerConnection.Login();


        var action = new OriginateAction { CallerId = request.Caller, Channel = $"SIP/{request.Extension}", Context=(request.SpyType == SpyType.Spy ? "chanspy" : "whisper") ,Exten= (request.SpyType== SpyType.Spy? "70": "72"), Priority = "1" };
        action.SetVariable("path", request.Caller);

        //var action = new OriginateAction { CallerId = "998", Channel = $"SIP/999", Context = (request.SpyType == SpyType.Spy ? "chanspy" : "whisper"), Exten = (request.SpyType == SpyType.Spy ? "70" : "72"), Priority = "1" };

        var response = _managerConnection.SendAction(action);

        _managerConnection.Logoff();

    }
}
