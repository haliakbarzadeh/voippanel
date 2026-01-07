using AutoMapper;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus;
using MediatR;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Voip.Framework.Common.Exceptions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;

namespace Goldiran.VOIPPanel.Application.Features.Queus.Commands.CreateQueu;

public class CreateQueuCommand : BaseCreateCommandRequest, IRequest<int>
{
    public int Code { get; set; }
    public string Name { get; set; }
    public string IPAddress { get; set; }
    public string User { get; set; }
    public string Secret { get; set; }
    public bool IsSLA { get; set; }
    public bool IsFCR { get; set; }


    public class Handler : IRequestHandler<CreateQueuCommand, int>
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

        public async Task<int> Handle(CreateQueuCommand request, CancellationToken cancellationToken)
        {
            var entity = new Queu(request.Code,request.Name,request.IPAddress,request.User,request.Secret,request.IsSLA,request.IsFCR);
                       
            _QueuRepository.Add(entity);
            await _QueuRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
           
            return entity.Id;
        }

    }
}

