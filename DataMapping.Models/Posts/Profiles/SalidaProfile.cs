using AutoMapper;

namespace DataMapping.Models.Posts.Profiles
{
    public class SalidaProfile : Profile
    {
        public SalidaProfile()
        {
            CreateMap<ServerPost, Salida>()
                .ForMember(s => s.Titulo, mo => mo.MapFrom(sp => sp.Title));
        }
    }
}
