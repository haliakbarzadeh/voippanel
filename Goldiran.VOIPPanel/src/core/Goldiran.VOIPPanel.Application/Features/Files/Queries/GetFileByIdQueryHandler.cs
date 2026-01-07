using AutoMapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files.Contracts;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Voip.Framework.Common.Exceptions;

namespace Goldiran.VOIPPanel.Application.Features.Files.Queries;


public class GetFileByIdQueryHandler : IRequestHandler<GetFileByIdQuery, FileDto>
{
    private readonly IMapper _mapper;
    private readonly IFileQueryService _fileQueryService;
    public GetFileByIdQueryHandler(IMapper mapper, IFileQueryService fileQueryService)
    {
        _fileQueryService = fileQueryService;
        _mapper = mapper;
    }

    public async Task<FileDto> Handle(GetFileByIdQuery request, CancellationToken cancellationToken)
    {
        var entity =await _fileQueryService.GetFileById(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(File), request.Id);
        }

        return entity;
    }
}

