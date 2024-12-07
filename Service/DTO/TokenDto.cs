using Core.Entities;
using Service.Common.Mappings;
using AutoMapper;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class TokenDto : IMapFrom<Token>
    {
        public int TokenId { get; set; }
        public int Matricule { get; set; }
        public string TokenValue { get; set; }
        public DateTime Expiration { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Token, TokenDto>().ReverseMap();

        }
    }
}
