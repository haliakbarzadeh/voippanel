
using MediatR;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.IServices;
using Goldiran.VOIPPanel.Application.Services.AsteriskServices.Models;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.ReadModel.Entities;
using Goldiran.VOIPPanel.Application.Features.Rasads.Enums;

namespace Goldiran.VOIPPanel.Application.Features.Rasads.Commands.ChanSpy;

public class ChanSpyCommand : BaseCreateCommandRequest, IRequest<bool>
{
    //public string? Extension { get; set; }
    public string? Caller { get; set; }
    public SpyType? SpyType { get; set; }

    public class Handler : IRequestHandler<ChanSpyCommand, bool>
    {
        private readonly IChanSpyService _chanSpyService;
        private readonly IReadModelContext _readModelContext;
        public Handler(IChanSpyService chanSpyService, IReadModelContext readModelContext)
        {
            _chanSpyService = chanSpyService;
            _readModelContext = readModelContext;
        }

        public async Task<bool> Handle(ChanSpyCommand request, CancellationToken cancellationToken)
        {
            var userPosition=_readModelContext.Set<UserPosition>().Where(c=>c.UserId==request.UserId && c.PositionId==request.PositionId && c.IsActive).FirstOrDefault();
            //await _chanSpyService.SetChanSpy(new ChanSpyRequest() { Caller = request.Caller, Extension = userPosition.Extension,SpyType=(SpyType)request.SpyType });
            try
            {
                if(userPosition.Queues.Contains("22") || userPosition.Queues.Contains("23") || userPosition.Queues.Contains("25") || userPosition.Queues.Contains("30"))
                    await _chanSpyService.SetChanSpy(new ChanSpyRequest() { Caller = request.Caller, Extension = userPosition.Extension, SpyType = (SpyType)request.SpyType,IP= "10.14.14.11" });
                else
                    await _chanSpyService.SetChanSpy(new ChanSpyRequest() { Caller = request.Caller, Extension = userPosition.Extension, SpyType = (SpyType)request.SpyType, IP = "10.14.14.34" });

            }
            catch (Exception ex)
            {

            }

           

            return true;
        }

    }
}

