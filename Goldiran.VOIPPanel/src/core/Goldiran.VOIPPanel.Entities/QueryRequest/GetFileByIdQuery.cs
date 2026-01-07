using Goldiran.VOIPPanel.ReadModel.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.ReadModel.QueryRequest;

public class GetFileByIdQuery : IRequest<FileDto>
{
    public Guid Id { get; set; }
}
