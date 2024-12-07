using Service.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IAssiduiteService
    {
        Task<bool> AjouterHoraire(HoraireDto horaireDto);
        Task<bool> AjouterSupplementaire(SupplementaireDto supplementaireDto);
        Task<bool> AjouterPermission(PermissionDto permissionDto);
        Task<bool> AjouterRetard(RetardDto retardDto);
        Task<bool> AjouterAbsence(AbsenceDto absenceDto);

        Task<IEnumerable<HoraireDto>> RecupererHorairesParAssiduite(int assiduiteId);
        Task<IEnumerable<SupplementaireDto>> RecupererSupplementairesParAssiduite(int assiduiteId);
        Task<IEnumerable<PermissionDto>> RecupererPermissionsParAssiduite(int assiduiteId);
        Task<IEnumerable<RetardDto>> RecupererRetardsParAssiduite(int assiduiteId);
        Task<IEnumerable<AbsenceDto>> RecupererAbsencesParAssiduite(int assiduiteId);

        Task<AssiduiteDto> GetAssiduiteParMatricule(int matricule);
    }
}
