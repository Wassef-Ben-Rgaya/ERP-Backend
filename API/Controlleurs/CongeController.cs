using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.DTO;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/conge")]
    [EnableCors("CORSPolicy")]
    [ApiController]
    public class CongeController : ControllerBase
    {
        private readonly ICongeService _congeService;
        private readonly ILogger<CongeController> _logger;

        public CongeController(ICongeService congeService, ILogger<CongeController> logger)
        {
            _congeService = congeService;
            _logger = logger;
        }

        // Récupérer tous les congés
        [HttpGet]
        public async Task<IActionResult> RecupererTousLesConges()
        {
            var conges = await _congeService.RecupererTousLesConges();
            return Ok(conges);
        }

        // Ajouter un congé
        [HttpPost]
        public async Task<IActionResult> AjouterConge([FromBody] CongeDto congeDto)
        {
            if (congeDto == null)
            {
                return BadRequest("Les données du congé ne peuvent pas être vides.");
            }

            var success = await _congeService.AjouterConge(congeDto);
            return success ? Ok("Congé ajouté avec succès.") : StatusCode(500, "Une erreur est survenue lors de l'ajout du congé.");
        }

        // Mettre à jour un congé
        [HttpPut]
        public async Task<IActionResult> MettreAJourConge([FromBody] CongeDto congeDto)
        {
            if (congeDto == null)
            {
                return BadRequest("Les données du congé ne peuvent pas être vides.");
            }

            var success = await _congeService.MettreAJourConge(congeDto);
            return success ? Ok("Congé mis à jour avec succès.") : StatusCode(500, "Une erreur est survenue lors de la mise à jour du congé.");
        }

        // Supprimer un congé
        [HttpDelete("{congeId}")]
        public async Task<IActionResult> SupprimerConge(int congeId)
        {
            var success = await _congeService.SupprimerConge(congeId);
            return success ? Ok("Congé supprimé avec succès.") : StatusCode(500, "Une erreur est survenue lors de la suppression du congé.");
        }

        // Récupérer un congé par ID
        [HttpGet("{congeId}")]
        public async Task<IActionResult> RecupererCongeParId(int congeId)
        {
            var conge = await _congeService.RecupererCongeParId(congeId);
            return conge != null ? Ok(conge) : NotFound(new { Message = "Congé non trouvé." });
        }

        // Récupérer les congés par employé
        [HttpGet("employe/{matricule}")]
        public async Task<IActionResult> RecupererCongesParEmploye(int matricule)
        {
            var conges = await _congeService.RecupererCongesParEmploye(matricule);
            return conges != null ? Ok(conges) : NotFound(new { Message = "Aucun congé trouvé pour cet employé." });
        }

        // Récupérer les congés en cours par employé
        [HttpGet("employe/{matricule}/encours")]
        public async Task<IActionResult> RecupererCongesEnCoursParEmploye(int matricule)
        {
            var conges = await _congeService.RecupererCongesEnCoursParEmploye(matricule);
            return conges != null ? Ok(conges) : NotFound(new { Message = "Aucun congé en cours trouvé pour cet employé." });
        }

        // Récupérer l'historique des congés par employé
        [HttpGet("employe/{matricule}/historique")]
        public async Task<IActionResult> RecupererCongesHistoriqueParEmploye(int matricule)
        {
            var conges = await _congeService.RecupererCongesHistoriqueParEmploye(matricule);
            return conges != null ? Ok(conges) : NotFound(new { Message = "Aucun congé historique trouvé pour cet employé." });
        }

        // Demander un congé
        [HttpPost("demander")]
        public async Task<IActionResult> DemanderConge([FromBody] CongeDto congeDto)
        {
            if (congeDto == null)
            {
                return BadRequest("Les données de la demande de congé ne peuvent pas être vides.");
            }

            var success = await _congeService.DemanderConge(congeDto);
            return success ? Ok("Demande de congé soumise avec succès.") : StatusCode(500, "Une erreur est survenue lors de la demande de congé.");
        }

        // Annuler un congé
        [HttpDelete("annuler/{congeId}")]
        public async Task<IActionResult> AnnulerConge(int congeId)
        {
            var success = await _congeService.AnnulerConge(congeId);
            return success ? Ok("Congé annulé avec succès.") : StatusCode(500, "Une erreur est survenue lors de l'annulation du congé.");
        }

        // Approuver un congé
        [HttpPut("approuver/{congeId}")]
        public async Task<IActionResult> ApprouverConge(int congeId)
        {
            var success = await _congeService.ApprouverConge(congeId);
            return success ? Ok("Congé approuvé avec succès.") : StatusCode(500, "Une erreur est survenue lors de l'approbation du congé.");
        }

        // Rejeter un congé
        [HttpPut("rejeter/{congeId}")]
        public async Task<IActionResult> RejeterConge(int congeId)
        {
            var success = await _congeService.RejeterConge(congeId);
            return success ? Ok("Congé rejeté avec succès.") : StatusCode(500, "Une erreur est survenue lors du rejet du congé.");
        }
    }
}
