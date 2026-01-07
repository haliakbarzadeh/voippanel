using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Voip.Framework.EFCore;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Queus.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.UserPositions.Contracts;
using Goldiran.VOIPPanel.Domain.Common.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldiran.VOIPPanel.Application.Features.UserPositions.Commands.DeleteUserPosition;

public class DeleteUserPositionCommand : BaseCreateCommandRequest, IRequest<bool>
{
    public long Id { get; set; }

    public class Handler : IRequestHandler<DeleteUserPositionCommand,bool>
    {
        private readonly IUserPositionRepository _userPositionRepository;
        private readonly IQueuRepository _queuRepository;

        public Handler(IUserPositionRepository userPositionRepository, IQueuRepository queuRepository)
        {
            _userPositionRepository = userPositionRepository;
            _queuRepository = queuRepository;
        }

        public async Task<bool> Handle(DeleteUserPositionCommand request, CancellationToken cancellationToken)
        {

            var userPosition = await _userPositionRepository.FindByIdAsync(request.Id);
            if (userPosition == null)
            {
                throw new NotFoundException();
            }
            userPosition.SetDisactivePosition();
            _userPositionRepository.Update(userPosition);

            if (!string.IsNullOrEmpty(userPosition.Queues))
            {
                var queueList = userPosition.Queues.Split(',');
                var queues = _queuRepository.GetAll(q => queueList.Contains(q.Name));
                foreach (var queue in queues)
                {
                    queue.ChangeCount(queue.Count -1);
                }
                _queuRepository.UpdateRange(queues.ToList());
            }

            await _userPositionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }     
    }
}



