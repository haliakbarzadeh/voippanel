using AutoMapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Entities.UserManagement;
using Goldiran.VOIPPanel.Domain.Common.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using BJsonIgnore = System.Text.Json.Serialization;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.CreateUser;

public class CreateUserCommand : BaseCreateCommandRequest, IRequest<bool>
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    [BJsonIgnore.JsonIgnore]
    public bool EmailConfirmed { get { return true; } }
    public string? PhoneNumber { get; set; }
    //public int AccessFailedCount { get; set; }
    public string? NationalCode { get; set; }
    public string? PersonalCode { get; set; }
    public string? PersianFullName { get; set; }
    public string? LatinFullName { get; set; }
    [BJsonIgnore.JsonIgnore]
    public int? OrganizationTypeId { get; set; }
    //public int? DepartmentTypeId { get; set; }
    public string? NativeDepartmentName { get; set; }
    public string? NativePsitionName { get; set; }
    public string? Fax { get; set; }
    public string? Mobile { get; set; }
    public string? PostalCode { get; set; }
    public string? ReportPath { get; set; }
    public string? Address { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public bool IsActive { get { return true; } }
    [BJsonIgnore.JsonIgnore]
    public bool IsTemperory { get { return true; } }
    public Guid? AttachmentFile { get; set; }
    public UserType? UserType { get; set; }

    public class Handler : IRequestHandler<CreateUserCommand, bool>
    {
        private IMapper _mapper;
        private IAppUserManager _userManager;
        private readonly IFileRepository _fileRepository;
        public Handler(IMapper mapper, IAppUserManager userManager, IFileRepository fileRepository)
        {

            _mapper = mapper;
            _userManager = userManager;
            _fileRepository = fileRepository;
        }

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<AppUser>(request);
            user.Guid = Guid.NewGuid();
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(c => c.Description).ToList());
            }

            if (request.AttachmentFile != null)
            {
                var file =await _fileRepository.FindByIdAsync((Guid)request.AttachmentFile);
                file.SetFileOwnerId(user.Guid);

                _fileRepository.Update(file);
                await _fileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);    
            }

            return true;
        }

    }
}

