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
    [Route("api/assiduite")]
    [EnableCors("CORSPolicy")]
    [ApiController]
    public class AssiduiteController : ControllerBase
    {
        private readonly IAssiduiteService _assiduiteService;
        private readonly ILogger<AssiduiteController> _logger;

        public AssiduiteController(IAssiduiteService assiduiteService, ILogger<AssiduiteController> logger)
        {
            _assiduiteService = assiduiteService;
            _logger = logger;
        }

        [HttpGet("{assiduiteId}/horaires")]
        public async Task<IActionResult> RecupererHorairesParAssiduite(int assiduiteId)
        {
            try
            {
                var horaires = await _assiduiteService.RecupererHorairesParAssiduite(assiduiteId);
                return Ok(horaires);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération des horaires pour l'assiduite ID {assiduiteId} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des horaires.");
            }
        }

        [HttpGet("{assiduiteId}/supplementaires")]
        public async Task<IActionResult> RecupererSupplementairesParAssiduite(int assiduiteId)
        {
            try
            {
                var supplementaires = await _assiduiteService.RecupererSupplementairesParAssiduite(assiduiteId);
                return Ok(supplementaires);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération des heures supplémentaires pour l'assiduite ID {assiduiteId} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des heures supplémentaires.");
            }
        }

        [HttpGet("{assiduiteId}/permissions")]
        public async Task<IActionResult> RecupererPermissionsParAssiduite(int assiduiteId)
        {
            try
            {
                var permissions = await _assiduiteService.RecupererPermissionsParAssiduite(assiduiteId);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération des permissions pour l'assiduite ID {assiduiteId} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des permissions.");
            }
        }

        [HttpGet("{assiduiteId}/retards")]
        public async Task<IActionResult> RecupererRetardsParAssiduite(int assiduiteId)
        {
            try
            {
                var retards = await _assiduiteService.RecupererRetardsParAssiduite(assiduiteId);
                return Ok(retards);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération des retards pour l'assiduite ID {assiduiteId} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des retards.");
            }
        }

        [HttpGet("{assiduiteId}/absences")]
        public async Task<IActionResult> RecupererAbsencesParAssiduite(int assiduiteId)
        {
            try
            {
                var absences = await _assiduiteService.RecupererAbsencesParAssiduite(assiduiteId);
                return Ok(absences);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération des absences pour l'assiduite ID {assiduiteId} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des absences.");
            }
        }

        [HttpPost("horaire")]
        public async Task<IActionResult> AjouterHoraire([FromBody] HoraireDto horaireDto)
        {
            if (horaireDto == null)
            {
                return BadRequest("Les données de l'horaire ne peuvent pas être vides.");
            }

            var success = await _assiduiteService.AjouterHoraire(horaireDto);
            return success ? Ok("Horaire ajouté avec succès.") : StatusCode(500, "Une erreur est survenue lors de l'ajout de l'horaire.");
        }

        [HttpPost("supplementaire")]
        public async Task<IActionResult> AjouterSupplementaire([FromBody] SupplementaireDto supplementaireDto)
        {
            if (supplementaireDto == null)
            {
                return BadRequest("Les données des heures supplémentaires ne peuvent pas être vides.");
            }

            var success = await _assiduiteService.AjouterSupplementaire(supplementaireDto);
            return success ? Ok("Heure supplémentaire ajoutée avec succès.") : StatusCode(500, "Une erreur est survenue lors de l'ajout de l'heure supplémentaire.");
        }

        [HttpPost("permission")]
        public async Task<IActionResult> AjouterPermission([FromBody] PermissionDto permissionDto)
        {
            if (permissionDto == null)
            {
                return BadRequest("Les données de la permission ne peuvent pas être vides.");
            }

            var success = await _assiduiteService.AjouterPermission(permissionDto);
            return success ? Ok("Permission ajoutée avec succès.") : StatusCode(500, "Une erreur est survenue lors de l'ajout de la permission.");
        }

        [HttpPost("retard")]
        public async Task<IActionResult> AjouterRetard([FromBody] RetardDto retardDto)
        {
            if (retardDto == null)
            {
                return BadRequest("Les données du retard ne peuvent pas être vides.");
            }

            var success = await _assiduiteService.AjouterRetard(retardDto);
            return success ? Ok("Retard ajouté avec succès.") : StatusCode(500, "Une erreur est survenue lors de l'ajout du retard.");
        }

        [HttpPost("absence")]
        public async Task<IActionResult> AjouterAbsence([FromBody] AbsenceDto absenceDto)
        {
            if (absenceDto == null)
            {
                return BadRequest("Les données de l'absence ne peuvent pas être vides.");
            }

            var success = await _assiduiteService.AjouterAbsence(absenceDto);
            return success ? Ok("Absence ajoutée avec succès.") : StatusCode(500, "Une erreur est survenue lors de l'ajout de l'absence.");
        }

        [HttpGet("matricule/{matricule}")]
        public async Task<IActionResult> GetAssiduiteParMatricule(int matricule)
        {
            try
            {
                var assiduite = await _assiduiteService.GetAssiduiteParMatricule(matricule);
                return assiduite != null ? Ok(assiduite) : NotFound(new { Message = "Assiduite non trouvée pour ce matricule." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération de l'assiduite pour le matricule {matricule} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération de l'assiduite.");
            }
        }
    }
}
