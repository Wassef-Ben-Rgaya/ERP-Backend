using AutoMapper;
using Core.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Service.DTO;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Service
{
    public class DepartementService : ServiceAsync<Département, DepartementDto>, IDepartementService
    {
        private readonly IRepositoryAsync<Département> _departementRepository;
        private readonly IRepositoryAsync<Personnel> _personnelRepository;
        private readonly IRepositoryAsync<Conge> _congeRepository;
        private readonly IRepositoryAsync<Paye> _payeRepository;
        private readonly IRepositoryAsync<Assiduite> _assiduiteRepository;
        private readonly IRepositoryAsync<Absence> _absenceRepository;
        private readonly IRepositoryAsync<Retard> _retardRepository;
        private readonly IRepositoryAsync<Permission> _permissionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DepartementService(
            IRepositoryAsync<Département> departementRepository,
            IRepositoryAsync<Personnel> personnelRepository,
            IRepositoryAsync<Conge> congeRepository,
            IRepositoryAsync<Paye> payeRepository,
            IRepositoryAsync<Assiduite> assiduiteRepository,
            IRepositoryAsync<Absence> absenceRepository,
            IRepositoryAsync<Retard> retardRepository,
            IRepositoryAsync<Permission> permissionRepository,
            IMapper mapper,
            ILogger logger)
            : base(departementRepository, mapper)
        {
            _departementRepository = departementRepository ?? throw new ArgumentNullException(nameof(departementRepository));
            _personnelRepository = personnelRepository ?? throw new ArgumentNullException(nameof(personnelRepository));
            _congeRepository = congeRepository ?? throw new ArgumentNullException(nameof(congeRepository));
            _payeRepository = payeRepository ?? throw new ArgumentNullException(nameof(payeRepository));
            _assiduiteRepository = assiduiteRepository ?? throw new ArgumentNullException(nameof(assiduiteRepository));
            _absenceRepository = absenceRepository ?? throw new ArgumentNullException(nameof(absenceRepository));
            _retardRepository = retardRepository ?? throw new ArgumentNullException(nameof(retardRepository));
            _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IQueryable<DepartementDto> GetDepartements()
        {
            try
            {
                var departements = _departementRepository.GetAll();
                _logger.Information("Récupération de tous les départements réussie.");
                return _mapper.ProjectTo<DepartementDto>(departements);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des départements : {ex.Message}", ex);
                return Enumerable.Empty<DepartementDto>().AsQueryable();
            }
        }

        public async Task<DepartementDto> GetDepartementById(int id)
        {
            try
            {
                var departement = await _departementRepository.GetById(id);
                if (departement == null)
                {
                    _logger.Warning($"Aucun département trouvé avec l'ID {id}.");
                    return null;
                }

                _logger.Information($"Récupération du département avec ID {id} réussie.");
                return _mapper.Map<DepartementDto>(departement);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération du département ID {id} : {ex.Message}", ex);
                return null;
            }
        }

        public async Task<IEnumerable<DepartementDto>> GetDepartementsByName(string name)
        {
            try
            {
                var departements = await _departementRepository.GetAll()
                    .Where(d => d.Nom.Contains(name, StringComparison.OrdinalIgnoreCase))
                    .ToListAsync();

                _logger.Information($"Récupération des départements contenant le nom {name} réussie.");
                return _mapper.Map<IEnumerable<DepartementDto>>(departements);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des départements contenant le nom {name} : {ex.Message}", ex);
                return new List<DepartementDto>();
            }
        }

        public async Task<bool> AddDepartement(DepartementDto departementDto)
        {
            if (departementDto == null)
            {
                _logger.Error("Tentative d'ajout d'un département nul.");
                return false;
            }

            try
            {
                var entity = _mapper.Map<Département>(departementDto);
                await _departementRepository.Add(entity);
                _logger.Information($"Département ajouté avec succès : {entity.Nom}.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de l'ajout du département : {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> UpdateDepartement(DepartementDto departementDto)
        {
            if (departementDto == null)
            {
                _logger.Error("Tentative de mise à jour d'un département nul.");
                return false;
            }

            try
            {
                var entity = _mapper.Map<Département>(departementDto);
                await _departementRepository.Update(entity);
                _logger.Information($"Département mis à jour avec succès : {entity.Nom}.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la mise à jour du département : {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> DeleteDepartement(int id)
        {
            try
            {
                await _departementRepository.Delete(id);
                _logger.Information($"Département avec ID {id} supprimé avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la suppression du département ID {id} : {ex.Message}", ex);
                return false;
            }
        }

        public async Task<IEnumerable<PersonnelDto>> GetEmployesParDepartement(int departementId)
        {
            try
            {
                var employes = await _personnelRepository.GetAll()
                    .Where(p => p.Departementid == departementId)
                    .ToListAsync();

                _logger.Information($"Récupération des employés du département ID {departementId} réussie.");
                return _mapper.Map<IEnumerable<PersonnelDto>>(employes);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des employés du département ID {departementId} : {ex.Message}", ex);
                return new List<PersonnelDto>();
            }
        }

        public async Task<DepartementDto> GetDepartementByEmploye(int employeId)
        {
            try
            {
                var employe = await _personnelRepository.GetById(employeId);
                if (employe == null)
                {
                    _logger.Warning($"Aucun employé trouvé avec l'ID {employeId}.");
                    return null;
                }

                var departement = await _departementRepository.GetById(employe.Departementid);
                if (departement == null)
                {
                    _logger.Warning($"Aucun département trouvé pour l'employé avec l'ID {employeId}.");
                    return null;
                }

                _logger.Information($"Récupération du département pour l'employé avec l'ID {employeId} réussie.");
                return _mapper.Map<DepartementDto>(departement);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération du département pour l'employé ID {employeId} : {ex.Message}", ex);
                return null;
            }
        }

        public async Task<IEnumerable<CongeDto>> GetEmployesCongeParDepartement(int departementId)
        {
            try
            {
                var conges = await _congeRepository.GetAll()
                    .Include(c => c.MatriculeNavigation)
                    .Where(c => c.MatriculeNavigation.Departementid == departementId)
                    .ToListAsync();

                _logger.Information($"Récupération des congés des employés du département ID {departementId} réussie.");
                return _mapper.Map<IEnumerable<CongeDto>>(conges);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des congés des employés du département ID {departementId} : {ex.Message}", ex);
                return new List<CongeDto>();
            }
        }

        public async Task<IEnumerable<PayeDto>> GetEmployesPayeParDepartement(int departementId)
        {
            try
            {
                var payes = await _payeRepository.GetAll()
                    .Include(p => p.MatriculeNavigation)
                    .Where(p => p.MatriculeNavigation.Departementid == departementId)
                    .ToListAsync();

                _logger.Information($"Récupération des paies des employés du département ID {departementId} réussie.");
                return _mapper.Map<IEnumerable<PayeDto>>(payes);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des paies des employés du département ID {departementId} : {ex.Message}", ex);
                return new List<PayeDto>();
            }
        }

        public async Task<IEnumerable<AssiduiteDto>> GetEmployesAssiduiteParDepartement(int departementId)
        {
            try
            {
                var assiduites = await _assiduiteRepository.GetAll()
                    .Include(a => a.MatriculeNavigation)
                    .Where(a => a.MatriculeNavigation.Departementid == departementId)
                    .ToListAsync();

                _logger.Information($"Récupération des assiduités des employés du département ID {departementId} réussie.");
                return _mapper.Map<IEnumerable<AssiduiteDto>>(assiduites);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des assiduités des employés du département ID {departementId} : {ex.Message}", ex);
                return new List<AssiduiteDto>();
            }
        }

        public async Task<IEnumerable<AbsenceDto>> GetEmployesAbsenceParDepartement(int departementId)
        {
            try
            {
                var absences = await _absenceRepository.GetAll()
                    .Include(a => a.Assiduite)
                    .ThenInclude(a => a.MatriculeNavigation)
                    .Where(a => a.Assiduite.MatriculeNavigation.Departementid == departementId)
                    .ToListAsync();

                _logger.Information($"Récupération des absences des employés du département ID {departementId} réussie.");
                return _mapper.Map<IEnumerable<AbsenceDto>>(absences);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des absences des employés du département ID {departementId} : {ex.Message}", ex);
                return new List<AbsenceDto>();
            }
        }

        public async Task<IEnumerable<RetardDto>> GetEmployesRetardParDepartement(int departementId)
        {
            try
            {
                var retards = await _retardRepository.GetAll()
                    .Include(r => r.Assiduite)
                    .ThenInclude(a => a.MatriculeNavigation)
                    .Where(r => r.Assiduite.MatriculeNavigation.Departementid == departementId)
                    .ToListAsync();

                _logger.Information($"Récupération des retards des employés du département ID {departementId} réussie.");
                return _mapper.Map<IEnumerable<RetardDto>>(retards);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des retards des employés du département ID {departementId} : {ex.Message}", ex);
                return new List<RetardDto>();
            }
        }

        public async Task<IEnumerable<PermissionDto>> GetEmployesPermissionParDepartement(int departementId)
        {
            try
            {
                var permissions = await _permissionRepository.GetAll()
                    .Include(p => p.Assiduite)
                    .ThenInclude(a => a.MatriculeNavigation)
                    .Where(p => p.Assiduite.MatriculeNavigation.Departementid == departementId)
                    .ToListAsync();

                _logger.Information($"Récupération des permissions des employés du département ID {departementId} réussie.");
                return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des permissions des employés du département ID {departementId} : {ex.Message}", ex);
                return new List<PermissionDto>();
            }
        }
    }
}
