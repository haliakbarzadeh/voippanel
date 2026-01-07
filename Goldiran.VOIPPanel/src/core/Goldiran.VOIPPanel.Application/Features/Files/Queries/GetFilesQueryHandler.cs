using AutoMapper;
using AutoMapper.QueryableExtensions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;

namespace Goldiran.VOIPPanel.Application.Features.Files.Queries;


public class GetFilesQueryHandler : IRequestHandler<GetFilesQuery, PaginatedList<FileRowDto>>
{
    private readonly IMapper _mapper;
    private readonly IFileQueryService _fileQueryService;
    public GetFilesQueryHandler(IMapper mapper, IFileQueryService fileQueryService)
    {
        _fileQueryService = fileQueryService;
        _mapper = mapper;
    }

    public async Task<PaginatedList<FileRowDto>> Handle(GetFilesQuery request, CancellationToken cancellationToken)
    {
        return await _fileQueryService.GetFiles(request);
    }
}
