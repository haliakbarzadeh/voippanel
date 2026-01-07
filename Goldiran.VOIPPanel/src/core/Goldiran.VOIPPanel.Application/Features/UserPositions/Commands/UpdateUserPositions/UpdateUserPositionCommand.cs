using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.CreateUserPosition;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Voip.Framework.Common.Extensions;
using System.Threading;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions;

namespace Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.UpdateUserPosition;

public class UpdateUserPositionCommand : BaseCreateCommandRequest, IRequest<long>
{
    public long Id { get; set; }
    public long PositionId { get; set; }
    public string Queues { get; set; }
    public string Extension { get; set; }
    public string? ServerIp { get; set; }
    public class Handler : IRequestHandler<UpdateUserPositionCommand, long>
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

        public async Task<long> Handle(UpdateUserPositionCommand request, CancellationToken cancellationToken)
        {

            var userPosition = await _userPositionRepository.FindByIdAsync(request.Id);
            if (userPosition == null)
            {
                throw new NotFoundException();
            }

            if (userPosition.PositionId == request.PositionId)
            {
                return await Update(request, userPosition, cancellationToken);
            }
            else
            {
                return await AddArchieve(request, userPosition, cancellationToken);
            }
        }

        private async Task<long> Update(UpdateUserPositionCommand request, UserPosition userPosition, CancellationToken cancellationToken)
        {
            await CheckBusinessRules(request);

            if (!string.IsNullOrEmpty(userPosition.Queues) && !string.IsNullOrEmpty(request.Queues) && userPosition.Queues != request.Queues)
            {
                var deletedList = userPosition.Queues.Split(',').Except(request.Queues.Split(','));
                var addedList = request.Queues.Split(',').Except(userPosition.Queues.Split(','));
                var decQueues = _queuRepository.GetAll(q => deletedList.Contains(q.Name));
                foreach (var queue in decQueues)
                {
                    queue.ChangeCount(queue.Count - 1);
                }
                _queuRepository.UpdateRange(decQueues.ToList());

                var addQueues = _queuRepository.GetAll(q => addedList.Contains(q.Name));
                foreach (var queue in addQueues)
                {
                    queue.ChangeCount(queue.Count + 1);
                }

                _queuRepository.UpdateRange(addQueues.ToList());
            }
            else if (!string.IsNullOrEmpty(userPosition.Queues) && string.IsNullOrEmpty(request.Queues))
            {
                var queueList = userPosition.Queues.Split(',');
                var queues = _queuRepository.GetAll(q => queueList.Contains(q.Name));
                foreach (var queue in queues)
                {
                    queue.ChangeCount(queue.Count - 1);
                }
                _queuRepository.UpdateRange(queues.ToList());
            }
            else if (string.IsNullOrEmpty(userPosition.Queues) && !string.IsNullOrEmpty(request.Queues))
            {
                var queueList = request.Queues.Split(',');
                var queues = _queuRepository.GetAll(q => queueList.Contains(q.Name));
                foreach (var queue in queues)
                {
                    queue.ChangeCount(queue.Count + 1);
                }
                _queuRepository.UpdateRange(queues.ToList());
            }

            userPosition.Update(request.Queues, request.Extension, request.ServerIp);


            _userPositionRepository.Update(userPosition);

            await _userPositionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return userPosition.Id;
        }

        private async Task CheckBusinessRules(UpdateUserPositionCommand request)
        {
            List<string> errors = new List<string>();

            if (!string.IsNullOrEmpty(request.Queues))
            {
                var userPosition = await _readModelContext.Set<Goldiran.VOIPPanel.ReadModel.Entities.UserPosition>().Include(c => c.Position).AsNoTracking().Where(c => c.Id == request.Id).FirstOrDefaultAsync();

                if (userPosition.Position.ParentPositionId != null)
                {
                    var parentUserPosition = await _readModelContext.Set<Goldiran.VOIPPanel.ReadModel.Entities.UserPosition>().Where(c => c.PositionId == userPosition.Position.ParentPositionId).FirstOrDefaultAsync();

                    if (parentUserPosition != null && (string.IsNullOrEmpty(parentUserPosition.Queues) || !parentUserPosition.Queues.Split(',').Any(c => request.Queues.Split(',').Contains(c))))
                    {
                        errors.Add($"شماره صف ها فقط می تواند شامل موارد ذیل باشد {parentUserPosition.Queues}");
                    }
                }
            }

            if (!errors.IsNullOrEmpty())
                throw new ValidationException(errors);

        }

        private async Task<long> AddArchieve(UpdateUserPositionCommand request, UserPosition userPosition, CancellationToken cancellationToken)
        {
            userPosition.SetDisactivePosition();
            _userPositionRepository.Update(userPosition);

            if (!string.IsNullOrEmpty(userPosition.Queues))
            {
                if (userPosition.Queues[0] == ',')
                {

                    userPosition.Queues.Remove(0, 1);
                }

                var queueList = userPosition.Queues.Split(',');
                var queues = _queuRepository.GetAll(q => queueList.Contains(q.Name));
                foreach (var queue in queues)
                {
                    queue.ChangeCount(queue.Count - 1);
                }
                _queuRepository.UpdateRange(queues.ToList());
            }
            //
            var entity = await _readModelContext.Set<Goldiran.VOIPPanel.ReadModel.Entities.UserPosition>().Where(c => c.UserId == userPosition.UserId && c.PositionId == request.PositionId && c.IsActive).FirstOrDefaultAsync();

            if (entity != null)
            {
                throw new ValidationException(new List<string>() { "این سمت برای کاربر پیش از این ثبت شده است" });
            }
            await CheckAddBusinessRules(request);

            var userPos = new UserPosition(userPosition.UserId, request.PositionId, DateTime.Now, null, null, true, request.Queues, request.Extension, request.ServerIp);

            _userPositionRepository.Add(userPos);
            if (!string.IsNullOrEmpty(request.Queues))
            {
                var queueList = request.Queues.Split(',');
                var queues = _queuRepository.GetAll(q => queueList.Contains(q.Name));
                foreach (var queue in queues)
                {
                    queue.ChangeCount(queue.Count + 1);
                }
                _queuRepository.UpdateRange(queues.ToList());
            }

            await _userPositionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return userPosition.Id;
        }

        private async Task CheckAddBusinessRules(UpdateUserPositionCommand request)
        {
            List<string> errors = new List<string>();

            if (!string.IsNullOrEmpty(request.Queues))
            {
                var position = await _readModelContext.Set<Goldiran.VOIPPanel.ReadModel.Entities.Position>().Where(c => c.Id == request.PositionId).FirstOrDefaultAsync();

                if (position.ParentPositionId != null)
                {
                    var parentUserPosition = await _readModelContext.Set<Goldiran.VOIPPanel.ReadModel.Entities.UserPosition>().Where(c => c.PositionId == position.ParentPositionId).FirstOrDefaultAsync();

                    if (parentUserPosition != null && (string.IsNullOrEmpty(parentUserPosition.Queues) || !parentUserPosition.Queues.Split(',').Any(c => request.Queues.Split(',').Contains(c))))
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
