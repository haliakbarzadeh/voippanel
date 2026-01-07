using Voip.Framework.Domain;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.OperationSettings.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Entities;

public class QueueLimitation : BaseQueryEntity<int>
{
    public int QueueId { get; set; }
    public Queu Queue { get; set; }
    public IList<HourValue> HourValues { get; set; }

}
