using Voip.Framework.Domain;
using Voip.Framework.Domain.Models.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;

namespace Goldiran.VOIPPanel.Domain.AggregatesModel.Files.Contracts;

public interface IFileQueryService : IBaseQueryService
{
    public Task<FileDto> GetFileById(Guid id);
    public Task<PaginatedList<FileRowDto>> GetFiles(GetFilesQuery filter);
}
