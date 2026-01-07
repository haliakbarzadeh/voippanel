using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetMasterAskCustomersQuery : BaseQueryRequest, IRequest<MasterAskCustomerDto>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Agents { get; set; }
    public int? Code { get; set; }
    public List<int> Scorces { get; set; } = new List<int>();

}
