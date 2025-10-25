using AutoMapper;
using Entities.Model;
using Entities.StaticValues;
using Shared.DataTransferObjects;

namespace VisitorManagementSystem.Mapper;

public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<CreateVisitorDto, Visitor>()
            .ForMember(o => o.VisitorPhoneNumber, src => src.MapFrom(x => x.PhoneNumber))
            .ForMember(o => o.VisitorEmailAddress, src => src.MapFrom(x => x.EmailAdddress))
            .ForMember(o => o.VisitorName, src => src.MapFrom(x => x.VisitorName));
        CreateMap<Visitor, VisitorDto>()
            .ForMember(x => x.PhoneNumber, src => src.MapFrom(y => y.VisitorPhoneNumber))
            .ForMember(x => x.EmailAddress, src => src.MapFrom(y => y.VisitorEmailAddress))
            .ForMember(x => x.VisitorName, src => src.MapFrom(y => y.VisitorName))
            .ForMember(x => x.CreatedDate, src => src.MapFrom(y => y.CreatedDate))
            .ForMember(x => x.IsActive, src => src.MapFrom(y => y.Status));
        CreateMap<CreateVisitDetailDto, VisitDetail>()
            .ForMember(dest => dest.VisitorPhoneNumber, src => src.MapFrom(x => x.VisitorPhoneNumber))
            .ForMember(dest => dest.VisitorEmailAddress, src => src.MapFrom(x => x.VisitorEmailAddress))
            .ForMember(dest => dest.HostName, src => src.MapFrom(x => x.HostName))
            .ForMember(dest => dest.VisitorName, src => src.MapFrom(x => x.VisitorName))
            .ForMember(dest => dest.VisitationDate, src => src.MapFrom(x => x.VisitationDate))
            .ForMember(dest => dest.PurposeOfVisit, src => src.MapFrom(x => x.PurposeOfVisit))
            .ForMember(dest => dest.VisitorGender, src => src.MapFrom(x => Enum.Parse(typeof(Gender), x.VisitorGender, true)))
            .ForMember(dest => dest.VisitorRegistrationType, src => src.MapFrom(x => Enum.Parse(typeof(VisitorRegistrationTypes), x.VisitorRegistrationType, true)))
            .ForMember(dest => dest.VisitType, src => src.MapFrom(x => Enum.Parse(typeof(VisitType), x.VisitType, true)));
        CreateMap<ScheduleVisitDetailDto, VisitDetail>()
            .ForMember(dest => dest.VisitType, src => src.MapFrom(x => x.VisitType))
            .ForMember(dest => dest.VisitorRegistrationType, src => src.MapFrom(x => x.VisitorRegistrationType))
            .ForMember(dest => dest.VisitorGender, src => src.MapFrom(x => x.VisitorGender));
        CreateMap<VisitDetail, VisitDetailDto>()
            .ForMember(dest => dest.VisitDate, src => src.MapFrom(x => x.VisitationDate))
            .ForMember(dest => dest.VisitStatus, src => src.MapFrom(x => x.VisitStatus.ToString()));
        CreateMap<RoleForRegistrationDto, Role>();
        CreateMap<Role, RoleDto>();
        CreateMap<UserForCreationDto, User>()
            .ForMember(o => o.UserName, src => src.MapFrom(x => string.Concat(x.FirstName, ".", x.LastName)))
            .ForMember(x => x.CreatedBy, src => src.MapFrom(x => x.CreatedBy ?? "" ));
        CreateMap<VisitDetail, SuccessfulCheckInDetailsDto>()
            .ForMember(dest => dest.CardNumber, src => src.MapFrom(x => x.AssignedCardNumber))
            .ForMember(dest => dest.VisitorIdentificationNumber, src => src.MapFrom(x => x.VisitorIdentificationNumber))
            .ForMember(dest => dest.VisitorName, src => src.MapFrom(x => x.VisitorName))
            .ForMember(dest => dest.ReceptionistName, src => src.MapFrom(x => x.ReceptionistName))
            .ForMember(dest => dest.CheckInTime, src => src.MapFrom(x => x.CheckTime))
            .ForMember(dest => dest.CheckOutTime, src => src.MapFrom(x => x.CheckOutTime));
    }
}
