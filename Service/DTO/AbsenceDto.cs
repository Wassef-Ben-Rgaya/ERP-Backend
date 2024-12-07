using Core.Entities;
using Service.Common.Mappings;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class AbsenceDto : IMapFrom<Absence>
    {
        public int AbsenceId { get; set; }
        public int AssiduiteId { get; set; }
        public DateTime Date { get; set; }
        public bool Justifiee { get; set; }
        public float TotalHeures { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Absence, AbsenceDto>().ReverseMap();

        }
    }
}
