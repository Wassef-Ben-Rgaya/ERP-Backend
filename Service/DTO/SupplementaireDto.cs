using Core.Entities;
using Service.Common.Mappings;
using AutoMapper;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class SupplementaireDto : IMapFrom<Supplementaire>
    {
        public int SupplementaireId { get; set; }
        public int AssiduiteId { get; set; }
        public DateTime HeureDebut { get; set; }
        public DateTime HeureFin { get; set; }
        public float TotalHeures { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Supplementaire, SupplementaireDto>().ReverseMap();

        }
    }
}
