using Goldiran.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.ValueObjects;

[Owned]
public class HourValue
{
    //public int Id { get; set; }
    public HourType HourType { get; set; }
    public int Value { get; set; }
}

