using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;


public class MasterAskCustomerDto
{
    public decimal? CSAT { get; set; }
    public decimal ContributorPercent {  get; set; }
    public IList<NumberAskDto> CSATNumbers { get; set; }=new List<NumberAskDto>();
    public IList<GroupAskDto> GroupContributors { get; set; }=new List<GroupAskDto>();
    public IList<GroupAskDto> GroupCSATNumbers { get; set; } = new List<GroupAskDto>();

}

public class NumberAskDto
{
    public string Number { get; set; }
    public int Count { get; set; }
}
public class GroupAskDto
{
    public string RowValue {  get; set; }
    public decimal ColumnValue { get; set; }
}
