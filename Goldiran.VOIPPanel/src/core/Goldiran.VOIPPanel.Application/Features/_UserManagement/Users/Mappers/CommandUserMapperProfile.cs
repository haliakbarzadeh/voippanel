using AutoMapper;
using Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.CreateUser;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Mappers
{
    public class CommandUserMapperProfile : Profile
    {
        public CommandUserMapperProfile()
        {
            CreateMap<CreateUserCommand, AppUser>(MemberList.Destination);
        }
    }
}
