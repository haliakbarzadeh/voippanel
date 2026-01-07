using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Roles.Commands.ChangeRoleUser;

    public class ChangeRoleUserCommand : BaseUpdateCommandRequest, IRequest<bool>
    {
    public long Id { get; set; }
    public List<long> EntityList { get; set; }

    public class Handler : IRequestHandler<ChangeRoleUserCommand, bool>
        {
            private IMapper _mapper;
            private IAppRoleManager _roleManager;
            public Handler(IMapper mapper, IAppRoleManager roleManager)
            {
                _mapper = mapper;
                _roleManager = roleManager;
            }

            public async Task<bool> Handle(ChangeRoleUserCommand request, CancellationToken cancellationToken)
            {
            var result = await _roleManager.AddOrUpdateUserRolesAsync(request.Id, request.EntityList);

            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(c => c.Description).ToList());
            }

            return true;
            }
        }
    }

