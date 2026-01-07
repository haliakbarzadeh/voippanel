using Voip.Framework.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.ValueObjects;

[Owned]
public class ActiveTime : ValueObject
{
    public TimeSpan StartTime { get;private set; }
    public TimeSpan EndTime { get; private set; }
    public ActiveTime(TimeSpan startTime, TimeSpan endTime)
    {
        StartTime=startTime;
        EndTime=endTime;    
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
