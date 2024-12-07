using Core.Entities;
using Service.Common.Mappings;
using AutoMapper;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class RetardDto : IMapFrom<Retard>
    {
        public int RetardID { get; set; }
        public int AssiduiteID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duree { get; set; }
        public float TotalHeures { get; set; } // If needed for detailed reporting
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Retard, RetardDto>().ReverseMap();

        }
    }
}
