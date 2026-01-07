using Goldiran.VOIPPanel.ReadModel.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetOperationSettingByIdQuery : IRequest<OperationSettingDto>
{
    public int Id { get; set; }
}
