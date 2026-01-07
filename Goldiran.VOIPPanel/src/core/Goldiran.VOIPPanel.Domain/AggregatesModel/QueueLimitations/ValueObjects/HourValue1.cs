using Goldiran.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.ValueObjects;

public class HourValue1 : ValueObject
{
    public HourType HourType { get;private set; }
    public int Value { get;private  set; }
    public HourValue1(HourType hourType, int value)
    {
        HourType = hourType;
        Value = value;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
