using Core.Entities;
using Service.Common.Mappings;
using AutoMapper;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class HoraireDto : IMapFrom<Horaire>
    {
        public int HoraireID { get; set; }
        public int AssiduiteID { get; set; }
        public DateTime HeureDebut { get; set; }
        public DateTime HeureFin { get; set; }
        public float TotalHeures { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Horaire, HoraireDto>().ReverseMap();

        }
    }
}
