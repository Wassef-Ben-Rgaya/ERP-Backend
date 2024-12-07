using Core.Entities;
using Service.Common.Mappings;
using AutoMapper;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class PayeDto : IMapFrom<Paye>
    {
        internal readonly object DatePaiement;

        public int Payeid { get; set; }
        public int Matricule { get; set; }
        public double Salairebrut { get; set; }
        public double Salairenet { get; set; }
        public DateOnly Datepaiement { get; set; }
        public double? Prime { get; set; }
        public DateOnly Periode { get; set; }
        public int Nombredejours { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Paye, PayeDto>().ReverseMap();

        }
    }
}
