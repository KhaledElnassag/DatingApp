using AutoMapper;
using DatingApp.Core.Entities;
using DatingApp.DTO;

namespace DatingApp.Profiles
{
    public class Profiler:Profile
    {
        public Profiler()
        {
            CreateMap<AppUser, MembersDto>().
                ForMember(d=>d.PhotoUrl,o=>o.MapFrom(s=>s.Photos.FirstOrDefault(p=>p.IsMain).Url));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>()
                .ForMember(d=>d.UserName,o=>o.MapFrom(s=>s.Username.ToLower()));
            CreateMap<AppUser, LikeDto>().ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsMain).Url));

            CreateMap<Messages, MessageDto>()
                .ForMember(d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.Photos
                .FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.RecipientPhotoUrl, o => o.MapFrom(s => s.Recipient.Photos
                .FirstOrDefault(x => x.IsMain).Url));

        }
    }
}
