using Core.Entities;
using Service.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IPersonnelService : IServiceAsync<Personnel, PersonnelDto>
    {
        IQueryable<PersonnelDto> RecupererTousLesEmployes();
        Task<PersonnelDto> RecupererEmployeParMatricule(int matricule);
        Task<bool> AjouterEmploye(PersonnelDto personnelDto);
        Task<bool> MettreAJourEmploye(PersonnelDto personnelDto);
        Task<bool> SupprimerEmploye(int matricule);
        Task<IEnumerable<PersonnelDto>> RecupererEmployesParNom(string name);
        Task<IEnumerable<PersonnelDto>> RecupererEmployesParDepartement(int departmentId);
        Task<PersonnelDto> Authentifier(int matricule, string mdp);
        Task<PersonnelDto> ConsulterProfil(int matricule);
        Task<bool> DemanderConge(int personnelId, CongeDto congeDto);
        Task<IEnumerable<PayeDto>> ConsulterPaies(int personnelId);
        Task<IEnumerable<PersonnelDto>> RecupererEmployesAvecRetards();
        Task<IEnumerable<PersonnelDto>> RecupererEmployesAvecAbsences();
        Task<IEnumerable<PersonnelDto>> RecupererEmployesAvecPermissions();
    }
}
