using Service.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface ICongeService
    {
        Task<IEnumerable<CongeDto>> RecupererTousLesConges();
        Task<bool> AjouterConge(CongeDto congeDto);
        Task<bool> MettreAJourConge(CongeDto congeDto);
        Task<bool> SupprimerConge(int congeId);
        Task<CongeDto> RecupererCongeParId(int congeId);
        Task<IEnumerable<CongeDto>> RecupererCongesParEmploye(int matricule);
        Task<IEnumerable<CongeDto>> RecupererCongesEnCoursParEmploye(int matricule);
        Task<IEnumerable<CongeDto>> RecupererCongesHistoriqueParEmploye(int matricule);
        Task<bool> DemanderConge(CongeDto congeDto);
        Task<bool> AnnulerConge(int congeId);
        Task<bool> ApprouverConge(int congeId);
        Task<bool> RejeterConge(int congeId);
    }
}
