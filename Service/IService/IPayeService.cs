using Service.DTO;
using System.Collections.Generic;

namespace Service.IService
{
    public interface IPayeService
    {
        Task<bool> AjouterPaye(PayeDto payeDto);
        Task<bool> MettreAJourPaye(PayeDto payeDto);
        Task<bool> SupprimerPaye(int payeId);
        Task<PayeDto> RecupererPayeParId(int payeId);
        Task<PayeDto> CalculePaye(int matricule, DateOnly periode, double salaireBase, double tauxHoraire, double tauxSupplementaire, double tauxAbsence);
        Task<byte[]> ImprimePaye(PayeDto payeDto);
        Task<IEnumerable<PayeDto>> GetAllPaye();
    }
}
