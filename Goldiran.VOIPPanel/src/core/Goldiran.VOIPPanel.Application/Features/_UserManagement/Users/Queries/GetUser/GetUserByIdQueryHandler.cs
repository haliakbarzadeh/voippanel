using AutoMapper;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Goldiran.VOIPPanel.ReadModel.QueryRequest;
using Goldiran.VOIPPanel.ReadModel.Dto;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Queries.GetUser;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly UserManager<AppUser> _UserManager;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(UserManager<AppUser> UserManager, IMapper mapper)
    {
        _UserManager = UserManager;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {

        var entity = await _UserManager.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return _mapper.Map<UserDto>(entity);
    }
}

