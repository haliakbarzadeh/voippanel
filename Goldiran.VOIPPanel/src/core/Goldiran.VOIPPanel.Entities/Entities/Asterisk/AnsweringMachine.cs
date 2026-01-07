using Goldiran.VOIPPanel.Domain.AggregatesModel.AnsweringMachines.Enums;
using Goldiran.VOIPPanel.ReadModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.Entities.Asterisk;


public class AnsweringMachine
{
    public long Id {  get; set; }
    public DateTime Date { get; set; }
    public int? Agent { get; set; }
    public TimeSpan Time {  get; set; }
    public string? CallerId { get; set; } = string.Empty;
    public string? CallerName { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public DateTime? EditDate { get; set; }
    public TimeSpan? EditTime { get; set; }
    public int Flag {  get; set; }
    public string? Mob { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string? Queue { get; set; } = string.Empty;
    public AMStatusType Status { get; set; }
    public DateTime? UpdateTime { get; set; }

}
