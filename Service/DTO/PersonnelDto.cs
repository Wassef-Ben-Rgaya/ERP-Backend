using Core.Entities;
using Service.Common.Mappings;
using AutoMapper;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class PersonnelDto : IMapFrom<Personnel>
    {
        public int Matricule { get; set; }

        public string Nom { get; set; }

        public string Prenom { get; set; }

        public DateOnly Datenaissance { get; set; }

        public string Adresse { get; set; }

        public string Email { get; set; }

        public string Mdp { get; set; }

        public string Poste { get; set; }

        public DateOnly Dateembauche { get; set; }

        public string Statutfamiliale { get; set; }

        public string Typecontrat { get; set; }

        public long Numerotelephone { get; set; }

        public int? Departementid { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Personnel, PersonnelDto>().ReverseMap();

        }
    }
}
