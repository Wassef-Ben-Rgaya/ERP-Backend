using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.DTO;
using Service.IService;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/paye")]
    [EnableCors("CORSPolicy")]
    [ApiController]
    public class PayeController : ControllerBase
    {
        private readonly IPayeService _payeService;
        private readonly ILogger<PayeController> _logger;

        public PayeController(IPayeService payeService, ILogger<PayeController> logger)
        {
            _payeService = payeService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AjouterPaye([FromBody] PayeDto payeDto)
        {
            if (payeDto == null)
            {
                return BadRequest("Les données de la paie ne peuvent pas être vides.");
            }

            var success = await _payeService.AjouterPaye(payeDto);
            return success ? Ok("Paie ajoutée avec succès.") : StatusCode(500, "Une erreur est survenue lors de l'ajout de la paie.");
        }

        [HttpPut]
        public async Task<IActionResult> MettreAJourPaye([FromBody] PayeDto payeDto)
        {
            if (payeDto == null)
            {
                return BadRequest("Les données de la paie ne peuvent pas être vides.");
            }

            var success = await _payeService.MettreAJourPaye(payeDto);
            return success ? Ok("Paie mise à jour avec succès.") : StatusCode(500, "Une erreur est survenue lors de la mise à jour de la paie.");
        }

        [HttpDelete("{payeId}")]
        public async Task<IActionResult> SupprimerPaye(int payeId)
        {
            var success = await _payeService.SupprimerPaye(payeId);
            return success ? Ok("Paie supprimée avec succès.") : StatusCode(500, "Une erreur est survenue lors de la suppression de la paie.");
        }

        [HttpGet("{payeId}")]
        public async Task<IActionResult> RecupererPayeParId(int payeId)
        {
            try
            {
                var paye = await _payeService.RecupererPayeParId(payeId);
                return paye != null ? Ok(paye) : NotFound(new { Message = "Paie non trouvée." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération de la paie avec l'ID {payeId} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération de la paie.");
            }
        }

        [HttpGet("calcule/{matricule}/{periode}/{salaireBase}/{tauxHoraire}/{tauxSupplementaire}/{tauxAbsence}")]
        public async Task<IActionResult> CalculePaye(int matricule, string periode, double salaireBase, double tauxHoraire, double tauxSupplementaire, double tauxAbsence)
        {
            try
            {
                var datePeriode = DateOnly.Parse(periode);
                var paye = await _payeService.CalculePaye(matricule, datePeriode, salaireBase, tauxHoraire, tauxSupplementaire, tauxAbsence);
                return paye != null ? Ok(paye) : NotFound(new { Message = "Erreur lors du calcul de la paie." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors du calcul de la paie pour le matricule {matricule} et la période {periode} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors du calcul de la paie.");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPaye()
        {
            try
            {
                var payes = await _payeService.GetAllPaye();
                return payes != null ? Ok(payes) : NotFound(new { Message = "Aucune paie trouvée." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération de toutes les paies : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération de toutes les paies.");
            }
        }

        [HttpGet("imprime/{payeId}")]
        public async Task<IActionResult> ImprimePaye(int payeId)
        {
            try
            {
                var payeDto = await _payeService.RecupererPayeParId(payeId);
                if (payeDto == null)
                    return NotFound(new { Message = "Paie non trouvée." });

                var pdf = await _payeService.ImprimePaye(payeDto);
                if (pdf == null)
                    return StatusCode(500, "Erreur lors de la génération de la fiche de paie.");

                return File(pdf, "application/pdf", $"FicheDePaie_{payeId}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la génération de la fiche de paie pour l'ID {payeId} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la génération de la fiche de paie.");
            }
        }
    }
}
