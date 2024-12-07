using AutoMapper;
using Core.Entities;
using DAL.IRepository;
using Serilog;
using Service.DTO;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Service.Service
{
    public class PersonnelService : ServiceAsync<Personnel, PersonnelDto>, IPersonnelService
    {
        private readonly IRepositoryAsync<Personnel> _personnelRepository;
        private readonly IRepositoryAsync<Conge> _congeRepository;
        private readonly IRepositoryAsync<Paye> _payeRepository;
        private readonly IRepositoryAsync<Retard> _retardRepository;
        private readonly IRepositoryAsync<Absence> _absenceRepository;
        private readonly IRepositoryAsync<Permission> _permissionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PersonnelService(
            IRepositoryAsync<Personnel> personnelRepository,
            IRepositoryAsync<Conge> congeRepository,
            IRepositoryAsync<Paye> payeRepository,
            IRepositoryAsync<Retard> retardRepository,
            IRepositoryAsync<Absence> absenceRepository,
            IRepositoryAsync<Permission> permissionRepository,
            IMapper mapper,
            ILogger logger)
            : base(personnelRepository, mapper)
        {
            _personnelRepository = personnelRepository ?? throw new ArgumentNullException(nameof(personnelRepository));
            _congeRepository = congeRepository ?? throw new ArgumentNullException(nameof(congeRepository));
            _payeRepository = payeRepository ?? throw new ArgumentNullException(nameof(payeRepository));
            _retardRepository = retardRepository ?? throw new ArgumentNullException(nameof(retardRepository));
            _absenceRepository = absenceRepository ?? throw new ArgumentNullException(nameof(absenceRepository));
            _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Récupérer tous les employés
        public IQueryable<PersonnelDto> RecupererTousLesEmployes()
        {
            try
            {
                var personnels = _personnelRepository.GetAll();
                _logger.Information("Récupération de tous les employés réussie.");
                return _mapper.ProjectTo<PersonnelDto>(personnels);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des employés : {ex.Message}", ex);
                return Enumerable.Empty<PersonnelDto>().AsQueryable();
            }
        }

        // Récupérer un employé par matricule
        public async Task<PersonnelDto> RecupererEmployeParMatricule(int matricule)
        {
            try
            {
                var personnel = await _personnelRepository.GetById(matricule);
                if (personnel == null)
                {
                    _logger.Warning($"Aucun employé trouvé avec le matricule {matricule}.");
                    return null;
                }

                _logger.Information($"Récupération de l'employé avec le matricule {matricule} réussie.");
                return _mapper.Map<PersonnelDto>(personnel);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération de l'employé avec le matricule {matricule} : {ex.Message}", ex);
                return null;
            }
        }

        // Ajouter un employé
        public async Task<bool> AjouterEmploye(PersonnelDto personnelDto)
        {
            if (personnelDto == null)
            {
                _logger.Error("Tentative d'ajout d'un employé nul.");
                return false;
            }

            try
            {
                var entity = _mapper.Map<Personnel>(personnelDto);
                await _personnelRepository.Add(entity);
                _logger.Information($"Employé ajouté avec succès : {entity.Nom} {entity.Prenom}.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de l'ajout de l'employé : {ex.Message}", ex);
                return false;
            }
        }

        // Mettre à jour un employé
        public async Task<bool> MettreAJourEmploye(PersonnelDto personnelDto)
        {
            if (personnelDto == null)
            {
                _logger.Error("Tentative de mise à jour d'un employé nul.");
                return false;
            }

            try
            {
                var entity = _mapper.Map<Personnel>(personnelDto);
                await _personnelRepository.Update(entity);
                _logger.Information($"Employé mis à jour avec succès : {entity.Nom} {entity.Prenom}.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la mise à jour de l'employé : {ex.Message}", ex);
                return false;
            }
        }

        // Supprimer un employé
        public async Task<bool> SupprimerEmploye(int matricule)
        {
            try
            {
                await _personnelRepository.Delete(matricule);
                _logger.Information($"Employé avec le matricule {matricule} supprimé avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la suppression de l'employé avec le matricule {matricule} : {ex.Message}", ex);
                return false;
            }
        }

        // Récupérer les employés par nom
        public async Task<IEnumerable<PersonnelDto>> RecupererEmployesParNom(string name)
        {
            try
            {
                var personnels = await _personnelRepository.GetAll()
                    .Where(p => p.Nom.Contains(name, StringComparison.OrdinalIgnoreCase) || p.Prenom.Contains(name, StringComparison.OrdinalIgnoreCase))
                    .ToListAsync();

                _logger.Information($"Récupération des employés contenant le nom {name} réussie.");
                return _mapper.Map<IEnumerable<PersonnelDto>>(personnels);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des employés contenant le nom {name} : {ex.Message}", ex);
                return new List<PersonnelDto>();
            }
        }

        // Récupérer les employés par département
        public async Task<IEnumerable<PersonnelDto>> RecupererEmployesParDepartement(int departmentId)
        {
            try
            {
                var personnels = await _personnelRepository.GetAll()
                    .Where(p => p.Departementid == departmentId)
                    .ToListAsync();

                _logger.Information($"Récupération des employés du département ID {departmentId} réussie.");
                return _mapper.Map<IEnumerable<PersonnelDto>>(personnels);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des employés du département ID {departmentId} : {ex.Message}", ex);
                return new List<PersonnelDto>();
            }
        }

        // Authentification d'un employé
        public async Task<PersonnelDto> Authentifier(int matricule, string mdp)
        {
            try
            {
                var personnel = await _personnelRepository.GetFirstOrDefault(p => p.Matricule == matricule && p.Mdp == mdp);
                if (personnel == null)
                {
                    _logger.Warning($"Aucun employé trouvé avec le matricule {matricule}.");
                    return null;
                }

                _logger.Information($"Authentification réussie pour le matricule {matricule}.");
                return _mapper.Map<PersonnelDto>(personnel);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de l'authentification du matricule {matricule} : {ex.Message}", ex);
                return null;
            }
        }

        // Consulter le profil d'un employé
        public async Task<PersonnelDto> ConsulterProfil(int matricule)
        {
            return await RecupererEmployeParMatricule(matricule);
        }

        // Demander des congés
        public async Task<bool> DemanderConge(int personnelId, CongeDto congeDto)
        {
            try
            {
                var personnel = await _personnelRepository.GetById(personnelId);
                if (personnel == null)
                {
                    _logger.Warning($"Aucun employé trouvé avec le matricule {personnelId}.");
                    return false;
                }

                var conge = _mapper.Map<Conge>(congeDto);
                personnel.Conges.Add(conge);
                await _personnelRepository.Update(personnel);

                _logger.Information($"Demande de congé ajoutée avec succès pour l'employé avec le matricule {personnelId}.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la demande de congé pour l'employé avec le matricule {personnelId} : {ex.Message}", ex);
                return false;
            }
        }

        // Consulter les paies d'un employé
        public async Task<IEnumerable<PayeDto>> ConsulterPaies(int personnelId)
        {
            try
            {
                var paies = await _payeRepository.GetAll()
                    .Where(p => p.Matricule == personnelId)
                    .ToListAsync();

                _logger.Information($"Récupération des paies réussie pour l'employé avec le matricule {personnelId}.");
                return _mapper.Map<IEnumerable<PayeDto>>(paies);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des paies pour l'employé avec le matricule {personnelId} : {ex.Message}", ex);
                return null;
            }
        }

        // Récupérer les employés avec des retards
        public async Task<IEnumerable<PersonnelDto>> RecupererEmployesAvecRetards()
        {
            try
            {
                var retards = await _retardRepository.GetAll()
                    .Include(r => r.Assiduite)
                    .ThenInclude(a => a.MatriculeNavigation)
                    .ToListAsync();

                var employes = retards.Select(r => r.Assiduite.MatriculeNavigation).Distinct();
                _logger.Information("Récupération des employés avec des retards réussie.");
                return _mapper.Map<IEnumerable<PersonnelDto>>(employes);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des employés avec des retards : {ex.Message}", ex);
                return new List<PersonnelDto>();
            }
        }

        // Récupérer les employés avec des absences
        public async Task<IEnumerable<PersonnelDto>> RecupererEmployesAvecAbsences()
        {
            try
            {
                var absences = await _absenceRepository.GetAll()
                    .Include(a => a.Assiduite)
                    .ThenInclude(a => a.MatriculeNavigation)
                    .ToListAsync();

                var employes = absences.Select(a => a.Assiduite.MatriculeNavigation).Distinct();
                _logger.Information("Récupération des employés avec des absences réussie.");
                return _mapper.Map<IEnumerable<PersonnelDto>>(employes);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des employés avec des absences : {ex.Message}", ex);
                return new List<PersonnelDto>();
            }
        }

        // Récupérer les employés avec des permissions
        public async Task<IEnumerable<PersonnelDto>> RecupererEmployesAvecPermissions()
        {
            try
            {
                var permissions = await _permissionRepository.GetAll()
                    .Include(p => p.Assiduite)
                    .ThenInclude(a => a.MatriculeNavigation)
                    .ToListAsync();

                var employes = permissions.Select(p => p.Assiduite.MatriculeNavigation).Distinct();
                _logger.Information("Récupération des employés avec des permissions réussie.");
                return _mapper.Map<IEnumerable<PersonnelDto>>(employes);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des employés avec des permissions : {ex.Message}", ex);
                return new List<PersonnelDto>();
            }
        }
    }
}
