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
    public class CongeService : ServiceAsync<Conge, CongeDto>, ICongeService
    {
        private readonly IRepositoryAsync<Conge> _congeRepository;
        private readonly IRepositoryAsync<Personnel> _personnelRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CongeService(IRepositoryAsync<Conge> congeRepository, IRepositoryAsync<Personnel> personnelRepository, IMapper mapper, ILogger logger)
            : base(congeRepository, mapper)
        {
            _congeRepository = congeRepository ?? throw new ArgumentNullException(nameof(congeRepository));
            _personnelRepository = personnelRepository ?? throw new ArgumentNullException(nameof(personnelRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Récupérer tous les congés
        public async Task<IEnumerable<CongeDto>> RecupererTousLesConges()
        {
            try
            {
                var conges = await _congeRepository.GetAll().ToListAsync();
                _logger.Information("Récupération de tous les congés réussie.");
                return _mapper.Map<IEnumerable<CongeDto>>(conges);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des congés : {ex.Message}", ex);
                return new List<CongeDto>();
            }
        }

        // Ajouter un congé
        public async Task<bool> AjouterConge(CongeDto congeDto)
        {
            if (congeDto == null)
            {
                _logger.Error("Tentative d'ajout d'un congé nul.");
                return false;
            }

            try
            {
                var entity = _mapper.Map<Conge>(congeDto);
                await _congeRepository.Add(entity);
                _logger.Information($"Congé ajouté avec succès pour le matricule {entity.Matricule}.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de l'ajout du congé : {ex.Message}", ex);
                return false;
            }
        }

        // Mettre à jour un congé
        public async Task<bool> MettreAJourConge(CongeDto congeDto)
        {
            if (congeDto == null)
            {
                _logger.Error("Tentative de mise à jour d'un congé nul.");
                return false;
            }

            try
            {
                var entity = _mapper.Map<Conge>(congeDto);
                await _congeRepository.Update(entity);
                _logger.Information($"Congé mis à jour avec succès pour le matricule {entity.Matricule}.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la mise à jour du congé : {ex.Message}", ex);
                return false;
            }
        }

        // Supprimer un congé
        public async Task<bool> SupprimerConge(int congeId)
        {
            try
            {
                await _congeRepository.Delete(congeId);
                _logger.Information($"Congé avec l'ID {congeId} supprimé avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la suppression du congé avec l'ID {congeId} : {ex.Message}", ex);
                return false;
            }
        }

        // Récupérer un congé par ID
        public async Task<CongeDto> RecupererCongeParId(int congeId)
        {
            try
            {
                var conge = await _congeRepository.GetById(congeId);
                if (conge == null)
                {
                    _logger.Warning($"Aucun congé trouvé avec l'ID {congeId}.");
                    return null;
                }

                _logger.Information($"Récupération du congé avec l'ID {congeId} réussie.");
                return _mapper.Map<CongeDto>(conge);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération du congé avec l'ID {congeId} : {ex.Message}", ex);
                return null;
            }
        }

        // Récupérer les congés par employé
        public async Task<IEnumerable<CongeDto>> RecupererCongesParEmploye(int matricule)
        {
            try
            {
                var conges = await _congeRepository.GetAll().Where(c => c.Matricule == matricule).ToListAsync();
                _logger.Information($"Récupération des congés pour l'employé avec le matricule {matricule} réussie.");
                return _mapper.Map<IEnumerable<CongeDto>>(conges);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des congés pour l'employé avec le matricule {matricule} : {ex.Message}", ex);
                return new List<CongeDto>();
            }
        }

        // Récupérer les congés en cours par employé
        public async Task<IEnumerable<CongeDto>> RecupererCongesEnCoursParEmploye(int matricule)
        {
            try
            {
                var now = DateTime.Now;
                var nowDateOnly = DateOnly.FromDateTime(now);
                var conges = await _congeRepository.GetAll().Where(c => c.Matricule == matricule && c.Datedebut <= nowDateOnly && c.Datefin >= nowDateOnly).ToListAsync();
                _logger.Information($"Récupération des congés en cours pour l'employé avec le matricule {matricule} réussie.");
                return _mapper.Map<IEnumerable<CongeDto>>(conges);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération des congés en cours pour l'employé avec le matricule {matricule} : {ex.Message}", ex);
                return new List<CongeDto>();
            }
        }

        // Récupérer l'historique des congés par employé
        public async Task<IEnumerable<CongeDto>> RecupererCongesHistoriqueParEmploye(int matricule)
        {
            try
            {
                var now = DateTime.Now;
                var nowDateOnly = DateOnly.FromDateTime(now);
                var conges = await _congeRepository.GetAll().Where(c => c.Matricule == matricule && c.Datefin < nowDateOnly).ToListAsync();
                _logger.Information($"Récupération de l'historique des congés pour l'employé avec le matricule {matricule} réussie.");
                return _mapper.Map<IEnumerable<CongeDto>>(conges);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération de l'historique des congés pour l'employé avec le matricule {matricule} : {ex.Message}", ex);
                return new List<CongeDto>();
            }
        }

        // Demander un congé
        public async Task<bool> DemanderConge(CongeDto congeDto)
        {
            if (congeDto == null)
            {
                _logger.Error("Tentative de demande d'un congé nul.");
                return false;
            }

            try
            {
                var employe = await _personnelRepository.GetById(congeDto.Matricule);
                if (employe == null)
                {
                    _logger.Warning($"Aucun employé trouvé avec le matricule {congeDto.Matricule} pour la demande de congé.");
                    return false;
                }

                var entity = _mapper.Map<Conge>(congeDto);
                await _congeRepository.Add(entity);
                _logger.Information($"Demande de congé soumise avec succès pour le matricule {congeDto.Matricule}.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la demande de congé pour le matricule {congeDto.Matricule} : {ex.Message}", ex);
                return false;
            }
        }

        // Annuler un congé
        public async Task<bool> AnnulerConge(int congeId)
        {
            try
            {
                var conge = await _congeRepository.GetById(congeId);
                if (conge == null)
                {
                    _logger.Warning($"Aucun congé trouvé avec l'ID {congeId} pour annulation.");
                    return false;
                }

                await _congeRepository.Delete(congeId);
                _logger.Information($"Congé avec l'ID {congeId} annulé avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de l'annulation du congé avec l'ID {congeId} : {ex.Message}", ex);
                return false;
            }
        }

        // Approuver un congé
        public async Task<bool> ApprouverConge(int congeId)
        {
            try
            {
                var conge = await _congeRepository.GetById(congeId);
                if (conge == null)
                {
                    _logger.Warning($"Aucun congé trouvé avec l'ID {congeId} pour approbation.");
                    return false;
                }

                conge.Status = "Approved";
                await _congeRepository.Update(conge);
                _logger.Information($"Congé avec l'ID {congeId} approuvé avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de l'approbation du congé avec l'ID {congeId} : {ex.Message}", ex);
                return false;
            }
        }

        // Rejeter un congé
        public async Task<bool> RejeterConge(int congeId)
        {
            try
            {
                var conge = await _congeRepository.GetById(congeId);
                if (conge == null)
                {
                    _logger.Warning($"Aucun congé trouvé avec l'ID {congeId} pour rejet.");
                    return false;
                }

                conge.Status = "Rejected";
                await _congeRepository.Update(conge);
                _logger.Information($"Congé avec l'ID {congeId} rejeté avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors du rejet du congé avec l'ID {congeId} : {ex.Message}", ex);
                return false;
            }
        }
    }
}
