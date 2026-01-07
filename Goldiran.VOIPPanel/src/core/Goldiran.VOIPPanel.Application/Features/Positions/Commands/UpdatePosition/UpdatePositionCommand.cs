using AutoMapper;
using Voip.Framework.Common.Exceptions;
using Voip.Framework.Domain.Models.CQRS;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Contracts;
using Goldiran.VOIPPanel.Domain.AggregatesModel.Positions.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace Goldiran.VOIPPanel.Application.Features.Positions.Commands.UpdatePosition;

public class UpdatePositionCommand : BaseCreateCommandRequest, IRequest<long>
{
    public long Id { get; set; }
    public PositionType PositionTypeId { get; set; }
    public long? ParentPositionId { get; set; }
    public string Title { get; set; }
    public bool IsContentAccess { get; set; }
    public ShiftType? ShiftType { get; set; }
    public bool? HasShifts { get; set; }

    public class Handler : IRequestHandler<UpdatePositionCommand,long>
    {
        private readonly IMapper _mapper;
        private readonly IPositionRepository _positionRepository;
        public Handler(IMapper mapper, IPositionRepository positionRepository)
        {
            _mapper = mapper;
            _positionRepository = positionRepository;
        }

        public async Task<long> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
        {

            var position = await _positionRepository.FindByIdAsync(request.Id);
            if (position == null)
            {
                throw new NotFoundException();
            }
            Position parentPosition = null;
            if (request.ParentPositionId != null)
            {
                parentPosition = await _positionRepository.FindByIdAsync((long)request.ParentPositionId, new List<System.Linq.Expressions.Expression<Func<Position, object>>>() { c => c.ParentPosition }); 
            }

            position.Update(request.Title, request.PositionTypeId, request.ParentPositionId, (request.ParentPositionId != null) ? (parentPosition.ContactedParentPositionId + ">'" + request.ParentPositionId + "'") : null, (request.ParentPositionId != null) ? (parentPosition.ContactedParentPositionName + ">" + request.Title) : request.Title, request.IsContentAccess,request.ShiftType,request.HasShifts);

            var childPositionList = _positionRepository.GetAll(c => c.ContactedParentPositionId.Contains(">'" + position.Id + "'")).ToList();

            if (childPositionList != null && childPositionList.Count() > 0)
            {
                childPositionList.ForEach(c => 
                c.UpdateParentPosition(c.ContactedParentPositionId.Replace(c.ContactedParentPositionId.Substring(0, c.ContactedParentPositionId.IndexOf(">'" + position.Id + "'") + (">'" + position.Id + "'").Length), position.ContactedParentPositionId + ">'" + position.Id + "'"),
                    c.ContactedParentPositionName.Replace(c.ContactedParentPositionName.Substring(0, c.ContactedParentPositionName.IndexOf(">" +position.Title) + (">" + position.Title).Length), position.ContactedParentPositionName + ">" + position.Title + ">" + request.Title)));
            }
            _positionRepository.UpdateRange(childPositionList);
                   
            await _positionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return position.Id;
        }

       
    }
}
