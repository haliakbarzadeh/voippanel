using AutoMapper;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions;
using MediatR;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Voip.Framework.Common.Exceptions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;
using Voip.Framework.Common.Exceptions;
using Goldiran.VOIPPanel.Application.Features.Positions.Commands.CreatePosition;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Files;
using Goldiran.VOIPPanel.Domain.AggregatesModel.QueueLimitations.Enums;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.CreateUserPosition;

public class CreateUserPositionCommand : BaseCreateCommandRequest, IRequest<long>
{
    public long UserId { get; set; }
    public long PositionId { get; set; }
    public string Queues { get; set; }
    public string Extension { get; set; }
    public string? ServerIp { get; set; }

    public class Handler : IRequestHandler<CreateUserPositionCommand, long>
    {
        private readonly IUserPositionRepository _userPositionRepository;
        private readonly IQueuRepository _queuRepository;
        private readonly IReadModelContext _readModelContext;
        private readonly IAppUserManager _appUserManager;
        private IMapper _mapper;
        public Handler(IUserPositionRepository userPositionRepository, IQueuRepository queuRepository, IReadModelContext readModelContext, IAppUserManager appUserManager, IMapper mapper)
        {
            _userPositionRepository = userPositionRepository;
            _queuRepository = queuRepository;
            _readModelContext = readModelContext;
            _appUserManager = appUserManager;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreateUserPositionCommand request, CancellationToken cancellationToken)
        {
            var entity=await _readModelContext.Set<Goldiran.VOIPPanel.ReadModel.Entities.UserPosition>().Where(c=>c.UserId==request.UserId && c.PositionId==request.PositionId && c.IsActive).FirstOrDefaultAsync();

            if (entity != null) 
            {
                throw new ValidationException(new List<string>() { "این سمت برای کاربر پیش از این ثبت شده است"});
            }
            await CheckBusinessRules(request);

            var userPosition = new UserPosition(request.UserId, request.PositionId, DateTime.Now, null, null, true, request.Queues, request.Extension, request.ServerIp);

            _userPositionRepository.Add(userPosition);
            if (!string.IsNullOrEmpty(request.Queues))
            {
                if (request.Queues[0]==',')
                {

                    request.Queues.Remove(0, 1);
                }
                var queueList = request.Queues.Split(',');
                var queues = _queuRepository.GetAll(q => queueList.Contains(q.Name));
                foreach (var queue in queues)
                {
                    queue.ChangeCount(queue.Count+1);
                }
                _queuRepository.UpdateRange(queues.ToList());
            }

            await _userPositionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return userPosition.Id;
        }

        private async Task CheckBusinessRules(CreateUserPositionCommand request)
        {
            List<string> errors = new List<string>();

            if (!string.IsNullOrEmpty(request.Queues))
            {
                var position = await _readModelContext.Set<Goldiran.VOIPPanel.ReadModel.Entities.Position>().Where(c => c.Id == request.PositionId).FirstOrDefaultAsync();

                if (position.ParentPositionId != null)
                {
                    var parentUserPosition = await _readModelContext.Set<Goldiran.VOIPPanel.ReadModel.Entities.UserPosition>().Where(c => c.PositionId == position.ParentPositionId).FirstOrDefaultAsync();

                    if (parentUserPosition!=null && (string.IsNullOrEmpty(parentUserPosition.Queues) || !parentUserPosition.Queues.Split(',').Any(c => request.Queues.Split(',').Contains(c))))
                    {
                        errors.Add($"شماره صف ها فقط می تواند شامل موارد ذیل باشد {parentUserPosition.Queues}");
                    }
                }
            }

            if (!errors.IsNullOrEmpty())
                throw new ValidationException(errors);

        }

    }
}

