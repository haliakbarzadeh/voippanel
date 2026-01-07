using AutoMapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Enums;
using MediatR;
using Newtonsoft.Json;
using System;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Common.Extensions;
using Voip.Framework.Domain.Models.CQRS;

namespace Goldiran.VOIPPanel.Application.Features.UserManagement.Users.Commands.UpdateUser;

public class UpdateUserCommand : BaseUpdateCommandRequest, IRequest<bool>
{
    public long Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? NationalCode { get; set; }
    public string? PersonalCode { get; set; }
    public string? PersianFullName { get; set; }
    public string? LatinFullName { get; set; }
    //public int? OrganizationTypeId { get; set; }
    //public int? DepartmentTypeId { get; set; }
    //public string? NativeDepartmentName { get; set; }
    //public string? NativePsitionName { get; set; }
    public string? Fax { get; set; }
    public string? Mobile { get; set; }
    public string? PostalCode { get; set; }
    public string? Address { get; set; }
    public string? ReportPath { get; set; }
    public bool? IsActive { get; set; }
    public UserType? UserType { get; set; }

    public Guid? AttachmentFile { get; set; }
    public class Handler : IRequestHandler<UpdateUserCommand, bool>
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

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            user.ConvertModelToOtherModel(request);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(c => c.Description).ToList());
            }

            if (request.AttachmentFile != null)
            {
                var removedFile= await _fileRepository.FindByAsync(c=>c.FileOwnerId==user.Guid);
                if(removedFile!=null)
                    _fileRepository.Delete(removedFile);

                var file = await _fileRepository.FindByIdAsync((Guid)request.AttachmentFile);
                file.SetFileOwnerId(user.Guid);

                _fileRepository.Update(file);
                await _fileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }

            return true;
        }

       
    }
}

