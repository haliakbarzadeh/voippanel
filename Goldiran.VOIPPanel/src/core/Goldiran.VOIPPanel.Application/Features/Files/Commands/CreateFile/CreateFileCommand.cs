using MediatR;
using AutoMapper;
using Microsoft.Extensions.Options;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using File=Goldiran.VOIPPanel.Domain.AggregatesModel.Files.File;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files.Contracts;
using Goldiran.VOIPPanel.ReadModel.Mappers;
using Goldiran.VOIPPanel.ReadModel.Dto;
using Voip.Framework.Common.AppSettings;
using Voip.Framework.Common.Exceptions;

namespace Goldiran.VOIPPanel.Application.Features.Files.Commands.CreateFile
{
    public class CreateFileCommand : IRequest<FileDisplayDto>
    {
        public FileOwnerType FileOwnerTypeId { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public long Length { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public Guid? FileOwnerId { get; set; }


        public class Handler : IRequestHandler<CreateFileCommand, FileDisplayDto>
        {
            private readonly IMapper _mapper;
            private readonly IFileRepository _fileRepository;
            private readonly IOptionsSnapshot<SiteSettings> _siteOptions;

            public Handler(IMapper mapper, IFileRepository fileRepository, IOptionsSnapshot<SiteSettings> siteOptions)
            {
                _mapper = mapper;
                _siteOptions = siteOptions;
                _fileRepository = fileRepository;
            }

            public async Task<FileDisplayDto> Handle(CreateFileCommand request, CancellationToken cancellationToken)
            {
                CheckExtraValidation(request.FileName);

                var file = new File(request.FileOwnerTypeId, request.FileOwnerId, request.FileName, request.Name, request.Length,request.ContentType);
                file.SetFileContent(new FileContent(request.Content));

                _fileRepository.Add(file);

                await _fileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return _mapper.Map<FileDisplayDto>(file);
            }

            private void CheckExtraValidation(string fileName)
            {
                var prefixes=_siteOptions.Value.FilePrefix.Split(',').Select(c=>c.ToLower());
                if (!prefixes.Contains( fileName.Split('.')[1].ToLower()) && !prefixes.Contains(fileName.Split('.')[0].ToLower()))
                {
                    throw new ValidationException(new List<string>() { "امکان ارسال این نوع فایل وجود ندارد" });
                }
            }
        }
            
    }
}
