using AutoMapper;

namespace CrmIntegrationApp.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CrmTicket, CxoneTicket>()
                .ForMember(dest => dest.TicketId, e => e.MapFrom(src => src.Id))
                .ForMember(dest => dest.Subject, e => e.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, e => e.MapFrom(src => src.Body))
                .ForMember(dest => dest.Created, e => e.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Priority, e => e.MapFrom(src => GetCxonePriority(src.Priority)))
                .ForMember(dest => dest.Tags, e => e.MapFrom(src => src.Tags));
        }

        private string GetCxonePriority(CrmPriority priorityLevel)
        {
            return priorityLevel switch
            {
                CrmPriority.Low => CxonePriority.Low,
                CrmPriority.Normal => CxonePriority.Normal,
                CrmPriority.High => CxonePriority.High
            };
        }

    }
}