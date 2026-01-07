using Voip.Framework.Domain.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.ValueObjects;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class HourValueDto
{
    public int Id { get; set; }
    public HourType HourTypeId { get; set; }
    public string HourType { get { return HourTypeId != 0 ? HourTypeId.Description() : string.Empty; } }
    public int Value { get; set; }
}
