using System;
using Core.Entities;
using Service.Common.Mappings;
using AutoMapper;
namespace Service.DTO
{
    public class CongeDto : IMapFrom<Conge>
    {
        public int CongeID { get; set; }
        public int Matricule { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string Status { get; set; }  // New field to match the entity
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Conge, CongeDto>().ReverseMap();

        }
    }
   
}
