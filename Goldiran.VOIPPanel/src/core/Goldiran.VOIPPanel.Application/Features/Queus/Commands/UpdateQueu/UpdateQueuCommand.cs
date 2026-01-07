using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Goldiran.VOIPPanel.Application.Features.Queus.Commands.CreateQueu.CreateQueuCommand;

namespace Goldiran.VOIPPanel.Application.Features.Queus.Commands.UpdateQueu;

public class UpdateQueuCommand : BaseCreateCommandRequest, IRequest<long>
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; }
    public string IPAddress { get; set; }
    public string User { get; set; }
    public string Secret { get; set; }
    public bool IsSLA { get; set; }
    public bool IsFCR { get; set; }


    public class Handler : IRequestHandler<UpdateQueuCommand,long>
    {
        private readonly IQueuRepository _QueuRepository;
        private readonly IReadModelContext _readModelContext;
        private readonly IAppUserManager _appUserManager;
        private IMapper _mapper;
        public Handler(IQueuRepository QueuRepository, IReadModelContext readModelContext, IAppUserManager appUserManager, IMapper mapper)
        {
            _QueuRepository = QueuRepository;
            _readModelContext = readModelContext;
            _appUserManager = appUserManager;
            _mapper = mapper;
        }

        public async Task<long> Handle(UpdateQueuCommand request, CancellationToken cancellationToken)
        {

            var entity = await _QueuRepository.FindByIdAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException();
            }

            entity.Update(request.Code,request.Name,request.IPAddress,request.User,request.Secret,request.IsSLA,request.IsFCR);

            _QueuRepository.Update(entity);

            await _QueuRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }    
    }
}
