using Core.Entities;
using Service.Common.Mappings;
using AutoMapper;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class PermissionDto : IMapFrom<Permission>
    {
        public int PermissionId { get; set; }
        public int AssiduiteId { get; set; }
        public DateTime Date { get; set; }
        public float Duree { get; set; }
        public string Status { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Permission, PermissionDto>().ReverseMap();

        }
    }
}
