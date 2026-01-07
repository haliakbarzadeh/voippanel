using Voip.Framework.Domain.Extensions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.ValueObjects;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Dto;

public class QueueLimitationDto
{
    public int Id { get; set; }
    public int QueueId { get; set; }
    public string QueueName { get; set; }
    public IList<HourValueDto> HourValues { get; set; }=new List<HourValueDto>();
}
