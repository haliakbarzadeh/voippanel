using AutoMapper;
using Goldiran.VOIPPanel.ReadModel.Dto.Voip;
using Goldiran.VOIPPanel.ReadModel.Entities.Voip;
using Voip.Framework.Common.Extensions;

namespace Goldiran.VOIPPanel.ReadModel.Mappers;

public class SoftPhoneEventMapper : Profile
{
    public SoftPhoneEventMapper()
    {
        CreateMap<SoftPhoneEvent,SoftPhoneEventDto>()
            .ForMember(d => d.Finished,mo => mo.MapFrom(s => s.FinishedAt))
            .ForMember(d => d.Started,mo => mo.MapFrom(s => s.StartedAt))
            .ForMember(d => d.Username,mo => mo.MapFrom(s => s.User.PersianFullName))
            .ForMember(d => d.Extension, mo => mo.MapFrom(s => s.UserPosition.Extension))
            .ForMember(d => d.EventTypeTitle, mo => mo.MapFrom(s => s.EventType.Description()));
    }
}
