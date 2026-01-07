using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Entities;

public class OperationSetting : BaseQueryEntity<int>
{
    public OperationType OperationTypeId { get;  set; }
    public string Name { get;  set; }
    public string Description { get;  set; }
    public string Color { get;  set; }
    public string Icon { get;  set; }
    public string Label { get;  set; }
    public int Order { get;  set; }
    public bool InQueue { get;  set; }
    public bool IsActive { get;  set; }
    public bool Pasuse { get;  set; }
    public bool ShowToUser { get;  set; }
    public int? HitLimit { get;  set; }
    public int? Duration { get;  set; }
    public int? Penalty { get;  set; }
    public ActiveTime? ActiveTime { get;  set; }
}
