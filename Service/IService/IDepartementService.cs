using Service.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IDepartementService
    {
        IQueryable<DepartementDto> GetDepartements();
        Task<DepartementDto> GetDepartementById(int id);
        Task<bool> AddDepartement(DepartementDto departementDto);
        Task<bool> UpdateDepartement(DepartementDto departementDto);
        Task<bool> DeleteDepartement(int id);
        Task<IEnumerable<DepartementDto>> GetDepartementsByName(string name);
        Task<IEnumerable<PersonnelDto>> GetEmployesParDepartement(int departementId);
        Task<DepartementDto> GetDepartementByEmploye(int employeId);
        Task<IEnumerable<CongeDto>> GetEmployesCongeParDepartement(int departementId);
        Task<IEnumerable<PayeDto>> GetEmployesPayeParDepartement(int departementId);
        Task<IEnumerable<AssiduiteDto>> GetEmployesAssiduiteParDepartement(int departementId);
        Task<IEnumerable<AbsenceDto>> GetEmployesAbsenceParDepartement(int departementId);
        Task<IEnumerable<RetardDto>> GetEmployesRetardParDepartement(int departementId);
        Task<IEnumerable<PermissionDto>> GetEmployesPermissionParDepartement(int departementId);
    }
}
