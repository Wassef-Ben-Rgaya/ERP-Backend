using Core.Entities;
using Service.Common.Mappings;
using AutoMapper;

namespace Service.DTO
{
    public class AssiduiteDto : IMapFrom<Assiduite>
    {
        public int AssiduiteId { get; set; }
        public int Matricule { get; set; }
        public float TotalHeuresPresence { get; set; }
        public float TotalHeuresSupplementaires { get; set; }
        public float TotalHeuresRetard { get; set; }
        public float TotalHeuresAbsence { get; set; }
        public float TotalHeuresPermission { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Assiduite, AssiduiteDto>().ReverseMap();

        }
    }
}
