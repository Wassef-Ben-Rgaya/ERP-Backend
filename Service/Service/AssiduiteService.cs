using AutoMapper;
using Core.Entities;
using DAL.IRepository;
using Serilog;
using Service.DTO;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Service
{
    public class AssiduiteService : IAssiduiteService
    {
        private readonly IRepositoryAsync<Assiduite> _assiduiteRepository;
        private readonly IRepositoryAsync<Horaire> _horaireRepository;
        private readonly IRepositoryAsync<Supplementaire> _supplementaireRepository;
        private readonly IRepositoryAsync<Permission> _permissionRepository;
        private readonly IRepositoryAsync<Retard> _retardRepository;
        private readonly IRepositoryAsync<Absence> _absenceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AssiduiteService(
            IRepositoryAsync<Assiduite> assiduiteRepository,
            IRepositoryAsync<Horaire> horaireRepository,
            IRepositoryAsync<Supplementaire> supplementaireRepository,
            IRepositoryAsync<Permission> permissionRepository,
            IRepositoryAsync<Retard> retardRepository,
            IRepositoryAsync<Absence> absenceRepository,
            IMapper mapper,
            ILogger logger)
        {
            _assiduiteRepository = assiduiteRepository;
            _horaireRepository = horaireRepository;
            _supplementaireRepository = supplementaireRepository;
            _permissionRepository = permissionRepository;
            _retardRepository = retardRepository;
            _absenceRepository = absenceRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> AjouterHoraire(HoraireDto horaireDto)
        {
            if (horaireDto == null)
            {
                _logger.Error("Tentative d'ajout d'un horaire nul.");
                return false;
            }

            try
            {
                var entity = _mapper.Map<Horaire>(horaireDto);
                await _horaireRepository.Add(entity);
                _logger.Information("Horaire ajouté avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de l'ajout de l'horaire : {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> AjouterSupplementaire(SupplementaireDto supplementaireDto)
        {
            if (supplementaireDto == null)
            {
                _logger.Error("Tentative d'ajout d'une heure supplémentaire nulle.");
                return false;
            }

            try
            {
                var entity = _mapper.Map<Supplementaire>(supplementaireDto);
                await _supplementaireRepository.Add(entity);
                _logger.Information("Heure supplémentaire ajoutée avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de l'ajout de l'heure supplémentaire : {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> AjouterPermission(PermissionDto permissionDto)
        {
            if (permissionDto == null)
            {
                _logger.Error("Tentative d'ajout d'une permission nulle.");
                return false;
            }

            try
            {
                var entity = _mapper.Map<Permission>(permissionDto);
                await _permissionRepository.Add(entity);
                _logger.Information("Permission ajoutée avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de l'ajout de la permission : {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> AjouterRetard(RetardDto retardDto)
        {
            if (retardDto == null)
            {
                _logger.Error("Tentative d'ajout d'un retard nul.");
                return false;
            }

            try
            {
                var entity = _mapper.Map<Retard>(retardDto);
                await _retardRepository.Add(entity);
                _logger.Information("Retard ajouté avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de l'ajout du retard : {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> AjouterAbsence(AbsenceDto absenceDto)
        {
            if (absenceDto == null)
            {
                _logger.Error("Tentative d'ajout d'une absence nulle.");
                return false;
            }

            try
            {
                var entity = _mapper.Map<Absence>(absenceDto);
                await _absenceRepository.Add(entity);
                _logger.Information("Absence ajoutée avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de l'ajout de l'absence : {ex.Message}", ex);
                return false;
            }
        }

        public async Task<IEnumerable<HoraireDto>> RecupererHorairesParAssiduite(int assiduiteId)
        {
            var horaires = await _horaireRepository.GetMuliple(h => h.Assiduiteid == assiduiteId);
            return _mapper.Map<IEnumerable<HoraireDto>>(horaires);
        }

        public async Task<IEnumerable<SupplementaireDto>> RecupererSupplementairesParAssiduite(int assiduiteId)
        {
            var supplementaires = await _supplementaireRepository.GetMuliple(s => s.Assiduiteid == assiduiteId);
            return _mapper.Map<IEnumerable<SupplementaireDto>>(supplementaires);
        }

        public async Task<IEnumerable<PermissionDto>> RecupererPermissionsParAssiduite(int assiduiteId)
        {
            var permissions = await _permissionRepository.GetMuliple(p => p.Assiduiteid == assiduiteId);
            return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
        }

        public async Task<IEnumerable<RetardDto>> RecupererRetardsParAssiduite(int assiduiteId)
        {
            var retards = await _retardRepository.GetMuliple(r => r.Assiduiteid == assiduiteId);
            return _mapper.Map<IEnumerable<RetardDto>>(retards);
        }

        public async Task<IEnumerable<AbsenceDto>> RecupererAbsencesParAssiduite(int assiduiteId)
        {
            var absences = await _absenceRepository.GetMuliple(a => a.Assiduiteid == assiduiteId);
            return _mapper.Map<IEnumerable<AbsenceDto>>(absences);
        }

        public async Task<AssiduiteDto> GetAssiduiteParMatricule(int matricule)
        {
            var assiduite = await _assiduiteRepository.GetFirstOrDefault(a => a.Matricule == matricule);
            return _mapper.Map<AssiduiteDto>(assiduite);
        }
    }
}
