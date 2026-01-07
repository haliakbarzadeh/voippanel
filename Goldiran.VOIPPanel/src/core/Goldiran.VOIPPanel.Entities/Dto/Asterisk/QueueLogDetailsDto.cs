using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;


public class QueueLogDetailsDto
{
    public decimal TotalCount { get; set; }
    public decimal AnsweredCount { get; set; }
    public decimal AnsweredPercent { get; set; }
    public decimal CustomAnsweredCount { get; set; }
    public decimal CustomAnsweredPercent { get; set; }
    public decimal AbondonCount { get; set; }
    public decimal AbondonPercent { get; set; }
    public decimal WaitingAvgTime { get; set; }
    public decimal AnsweringAvgTime { get; set; }
    public IList<QueueLogSLDto> GroupContactCounts { get; set; }=new List<QueueLogSLDto>();
    public IList<QueueLogSLDto> GroupSLA { get; set; } = new List<QueueLogSLDto>();

}

public class QueueLogSLDto
{
    public string Number { get; set; }
    public int Count { get; set; }
}

