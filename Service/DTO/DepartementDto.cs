using Core.Entities;
using Service.Common.Mappings;
using AutoMapper;
namespace Service.DTO

{
    public class DepartementDto : IMapFrom<Département>
    {
        public int DepartementID { get; set; }
        public string Nom { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Département, DepartementDto>().ReverseMap();

        }
    }
   
}
