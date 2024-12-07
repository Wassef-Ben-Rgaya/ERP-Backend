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
    [Route("api/personnel")]
    [EnableCors("CORSPolicy")]
    [ApiController]
    public class PersonnelController : ControllerBase
    {
        private readonly IPersonnelService _personnelService;
        private readonly ILogger<PersonnelController> _logger;

        public PersonnelController(IPersonnelService personnelService, ILogger<PersonnelController> logger)
        {
            _personnelService = personnelService;
            _logger = logger;
        }

        // Récupérer tous les employés
        [HttpGet]
        public IActionResult RecupererTousLesEmployes()
        {
            try
            {
                var employes = _personnelService.RecupererTousLesEmployes();
                return Ok(employes);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération des employés : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des employés.");
            }
        }

        // Récupérer un employé par matricule
        [HttpGet("{matricule}")]
        public async Task<IActionResult> RecupererEmployeParMatricule(int matricule)
        {
            var employe = await _personnelService.RecupererEmployeParMatricule(matricule);
            return employe != null ? Ok(employe) : NotFound(new { Message = "Employé non trouvé." });
        }

        // Ajouter un employé
        [HttpPost]
        public async Task<IActionResult> AjouterEmploye([FromBody] PersonnelDto personnelDto)
        {
            if (personnelDto == null)
            {
                return BadRequest("Les données de l'employé ne peuvent pas être vides.");
            }

            var success = await _personnelService.AjouterEmploye(personnelDto);
            return success ? Ok("Employé ajouté avec succès.") : StatusCode(500, "Une erreur est survenue lors de l'ajout de l'employé.");
        }

        // Mettre à jour un employé
        [HttpPut]
        public async Task<IActionResult> MettreAJourEmploye([FromBody] PersonnelDto personnelDto)
        {
            if (personnelDto == null)
            {
                return BadRequest("Les données de l'employé ne peuvent pas être vides.");
            }

            var success = await _personnelService.MettreAJourEmploye(personnelDto);
            return success ? Ok("Employé mis à jour avec succès.") : StatusCode(500, "Une erreur est survenue lors de la mise à jour de l'employé.");
        }

        // Supprimer un employé
        [HttpDelete("{matricule}")]
        public async Task<IActionResult> SupprimerEmploye(int matricule)
        {
            var success = await _personnelService.SupprimerEmploye(matricule);
            return success ? Ok("Employé supprimé avec succès.") : StatusCode(500, "Une erreur est survenue lors de la suppression de l'employé.");
        }

        // Authentifier un employé
        [HttpPost("authentifier")]
        public async Task<IActionResult> Authentifier([FromBody] LoginDto loginDto)
        {
            var employe = await _personnelService.Authentifier(loginDto.Matricule, loginDto.Mdp);
            return employe != null ? Ok(employe) : Unauthorized(new { Message = "Matricule ou mot de passe incorrect." });
        }

        // Consulter le profil d'un employé
        [HttpGet("profil/{matricule}")]
        public async Task<IActionResult> ConsulterProfil(int matricule)
        {
            var profil = await _personnelService.ConsulterProfil(matricule);
            return profil != null ? Ok(profil) : NotFound(new { Message = "Profil non trouvé." });
        }

        // Demander des congés
        [HttpPost("{matricule}/demander-conge")]
        public async Task<IActionResult> DemanderConge(int matricule, [FromBody] CongeDto congeDto)
        {
            var success = await _personnelService.DemanderConge(matricule, congeDto);
            return success ? Ok("Demande de congé soumise avec succès.") : StatusCode(500, "Une erreur est survenue lors de la demande de congé.");
        }

        // Consulter les paies
        [HttpGet("{matricule}/paies")]
        public async Task<IActionResult> ConsulterPaies(int matricule)
        {
            var paies = await _personnelService.ConsulterPaies(matricule);
            return paies != null ? Ok(paies) : NotFound(new { Message = "Aucune paie trouvée." });
        }

        // Récupérer les employés avec des retards
        [HttpGet("retards")]
        public async Task<IActionResult> RecupererEmployesAvecRetards()
        {
            var employes = await _personnelService.RecupererEmployesAvecRetards();
            return Ok(employes);
        }

        // Récupérer les employés avec des absences
        [HttpGet("absences")]
        public async Task<IActionResult> RecupererEmployesAvecAbsences()
        {
            var employes = await _personnelService.RecupererEmployesAvecAbsences();
            return Ok(employes);
        }

        // Récupérer les employés avec des permissions
        [HttpGet("permissions")]
        public async Task<IActionResult> RecupererEmployesAvecPermissions()
        {
            var employes = await _personnelService.RecupererEmployesAvecPermissions();
            return Ok(employes);
        }
    }
}
