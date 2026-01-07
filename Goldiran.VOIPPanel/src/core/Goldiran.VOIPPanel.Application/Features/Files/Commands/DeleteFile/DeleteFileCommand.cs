using AutoMapper;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files.Contracts;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Features.Files.Commands.DeleteFile
{
    public class DeleteFileCommand : IRequest<bool>
    {
        public Guid Id { get; set; }


        public class Handler : IRequestHandler<DeleteFileCommand,bool>
        {
            private readonly IFileRepository _fileRepository;


            public Handler( IFileRepository fileRepository)
            {
                _fileRepository = fileRepository;
            }

            public async Task<bool> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
            {
                var file =await _fileRepository.FindByIdAsync(request.Id);

                _fileRepository.Delete(file); 
                await _fileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
