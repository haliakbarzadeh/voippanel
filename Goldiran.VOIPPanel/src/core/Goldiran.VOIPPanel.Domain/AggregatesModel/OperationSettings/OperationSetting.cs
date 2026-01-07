using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings;

public class OperationSetting : AggregateRoot<int>
{
    public OperationType OperationTypeId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Color { get; private set; }
    public string Icon { get; private set; }
    public string Label { get; private set; }
    public int Order { get; private set; }
    public bool InQueue { get; private set; }
    public bool IsActive { get; private set; }
    public bool Pasuse { get; private set; }
    public bool ShowToUser { get; private set; }
    public int? HitLimit { get; private set; }
    public int? Duration { get; private set; }
    public int? Penalty { get; private set; }
    public ActiveTime? ActiveTime {get;private set;}

    //public OperationSetting(OperationType operationTypeId, string name, string description, string color, string icon, string label, int order, bool inQueue, bool isActive, bool pasuse, bool showToUser, int hitLimit, int duration, int penalty, ActiveTime? activeTime)
    //{
    //    OperationTypeId = operationTypeId;
    //    Name = name;
    //    Description = description;
    //    Color = color;
    //    Icon = icon;
    //    Label = label;
    //    Order = order;
    //    InQueue = inQueue;
    //    IsActive = isActive;
    //    Pasuse = pasuse;
    //    ShowToUser = showToUser;
    //    HitLimit = hitLimit;
    //    Duration = duration;
    //    Penalty = penalty;
    //    ActiveTime = activeTime;
    //}
    public OperationSetting(OperationType operationTypeId, string name, string description, string color, string icon, string label, int order, bool inQueue, bool isActive, bool pasuse, bool showToUser, int? hitLimit, int? duration, int? penalty)
    {
        OperationTypeId = operationTypeId;
        Name = name;
        Description = description;
        Color = color;
        Icon = icon;
        Label = label;
        Order = order;
        InQueue = inQueue;
        IsActive = isActive;
        Pasuse = pasuse;
        ShowToUser = showToUser;
        HitLimit = hitLimit;
        Duration = duration;
        Penalty = penalty;
    }
    public void Update(OperationType operationTypeId, string name, string description, string color, string icon, string label, int order, bool inQueue, bool isActive, bool pasuse, bool showToUser, int? hitLimit, int? duration, int? penalty)
    {
        OperationTypeId = operationTypeId;
        Name = name;
        Description = description;
        Color = color;
        Icon = icon;
        Label = label;
        Order = order;
        InQueue = inQueue;
        IsActive = isActive;
        Pasuse = pasuse;
        ShowToUser = showToUser;
        HitLimit = hitLimit;
        Duration = duration;
        Penalty = penalty;
    }

    public void SetActiveTime(ActiveTime? activeTime)
    {
        if(activeTime != null) { ActiveTime = activeTime; }
    }

}
