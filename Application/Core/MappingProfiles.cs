using Application.Attendances;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Attendance, Attendance>();
            CreateMap<AppUser, AppUser>();
            CreateMap<Attendance, AttendanceDto>()
                .ForMember(d => d.Profile, o => o.MapFrom(s => s.AppUser));
            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.UserName))
                .ForMember(d => d.DisplayName, o =>
                o.MapFrom(s => $"{s.FirstName} {s.MiddleInitial} {s.LastName}"));
        }
    }
}
