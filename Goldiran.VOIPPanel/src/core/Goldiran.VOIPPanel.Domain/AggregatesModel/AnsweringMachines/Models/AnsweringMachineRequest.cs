using Goldiran.VOIPPanel.Domain.AggregatesModel.AnsweringMachines.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.AnsweringMachines.Models;

public class AnsweringMachineRequest
{
    public long Id { get; set; }
    public int Agent { get; set; }
    public AMStatusType Status { get; set; }
    public string Description { get; set; }=string.Empty;
    public DateTime EditDate { get; set; }
    public TimeSpan EditTime { get; set; }
    public DateTime UpdateTime { get; set; }
}
