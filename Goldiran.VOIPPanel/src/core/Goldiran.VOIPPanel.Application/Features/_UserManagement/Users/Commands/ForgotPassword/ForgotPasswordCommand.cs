using AutoMapper;
using Framework.Common.SMS;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Saramad.Core.ApplicationService.Common.Exceptions;
using Saramad.Core.ApplicationService.Common.Interfaces;
using Saramad.Core.ApplicationService.Common.Models.CQRS.Command;
using Saramad.Core.ApplicationService.Common.Services.CodeProvider;
using Saramad.Core.ApplicationService.Common.Services.IdentityService.IServices;
using Saramad.Core.Domain.Entities;
using Saramad.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saramad.Core.ApplicationService.Features.Users.Commands.ForgotPassword;

    public class ForgotPasswordCommand :  IRequest<string>
    {
        public string Username { get; set; }


    public class Handler : IRequestHandler<ForgotPasswordCommand, string>
    {
            private readonly IApplicationDbContext _db;
            private IMapper _mapper;
            private IApplicationUserManager _userManager;
            private ISMSSender _smsSender;
            private ICodeProvider _codeProvider;
            public Handler(IApplicationDbContext db, IMapper mapper, IApplicationUserManager userManager, ISMSSender smsSender, ICodeProvider codeProvider)
            {
                _db = db;
                _mapper = mapper;
                _userManager = userManager;
                _smsSender = smsSender;
                _codeProvider = codeProvider;
            }

            public async Task<string> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user == null)
                    throw new NotFoundException("کاربر مورد نظر یافت نشد");

                var code = _codeProvider.GetVerificationCode(request.Username);
                await _smsSender.SendSms(user.Mobile,$"کد ورود:{code}" ,null);

                return string.Empty;
            }
        }
    }

