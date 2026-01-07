using Goldiran.VOIPPanel.ReadModel.Dto.Asterisk;
using Goldiran.VOIPPanel.ReadModel.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetGeneralSecureCallQuery : IRequest<GeneralSecureCallDto>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Number { get; set; }
    public SecureReportType? Type { get; set; }
}
