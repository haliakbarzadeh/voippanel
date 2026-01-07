using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Entities;

public class HourValue: BaseQueryEntity<int>
{
    public HourType HourTypeId { get; set; }
    public int Value { get; set; }
    public int QueueLimitationId {  get; set; }
    public QueueLimitation QueueLimitation { get; set; }
}

