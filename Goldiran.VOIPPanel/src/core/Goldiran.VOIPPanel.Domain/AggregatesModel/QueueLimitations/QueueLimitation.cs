using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations;

public class QueueLimitation : AggregateRoot<int>
{
    public int QueueId { get;private set; }
    private readonly List<HourValue> _hourValues = new();
    public IReadOnlyCollection<HourValue> HourValues => _hourValues;
    public QueueLimitation(int queueId)
    {
        QueueId = queueId;
    }
    public QueueLimitation(int queueId, IEnumerable<HourValue>? hourvalues)
    {
        QueueId = queueId;
        if (hourvalues != null)
            AddHourValue(hourvalues);
    }

    public void Update(IEnumerable<HourValue>? hourvalues)
    {
        if (hourvalues != null)
            UpdateHourValues(hourvalues);
    }

    public void AddHourValue(IEnumerable<HourValue>? hourvalues)
    {
        _hourValues.AddRange(hourvalues);
    }

    public void UpdateHourValues(IEnumerable<HourValue>? hourvalues)
    {
        _hourValues.Clear();
        _hourValues.AddRange(hourvalues);
    }

    public void UpdateHourValue(int id,int value)
    {
        var entity = HourValues.FirstOrDefault(e => e.Id == id);
        entity.Update(value);
    }

    public void DeleteHourValue(IList<HourValue> hourValues)
    {
        _hourValues.AddRange(hourValues);

    }
}
