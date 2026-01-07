using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations;

public class HourValue:Entity<int>
{
    public HourType HourTypeId { get; private set; }
    public int Value { get; private set; }
    public HourValue(HourType hourTypeId, int value)
    {
        HourTypeId = hourTypeId;
        Value = value;
    }

    public void Update(int value)
    {
        Value = value;
    }
}

