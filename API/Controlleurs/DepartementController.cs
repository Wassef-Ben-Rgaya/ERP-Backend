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
    [Route("api/departement")]
    [EnableCors("CORSPolicy")]
    [ApiController]
    public class DepartementController : ControllerBase
    {
        private readonly IDepartementService _departementService;
        private readonly ILogger<DepartementController> _logger;

        public DepartementController(IDepartementService departementService, ILogger<DepartementController> logger)
        {
            _departementService = departementService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetDepartements()
        {
            try
            {
                var departements = _departementService.GetDepartements();
                return Ok(departements);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération des départements : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des départements.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartementById(int id)
        {
            var departement = await _departementService.GetDepartementById(id);
            return departement != null ? Ok(departement) : NotFound(new { Message = "Département non trouvé." });
        }

        [HttpGet("search/{name}")]
        public async Task<IActionResult> GetDepartementsByName(string name)
        {
            var departements = await _departementService.GetDepartementsByName(name);
            return Ok(departements);
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartement([FromBody] DepartementDto departementDto)
        {
            if (departementDto == null)
            {
                return BadRequest("Les données du département ne peuvent pas être vides.");
            }

            var success = await _departementService.AddDepartement(departementDto);
            return success ? Ok("Département ajouté avec succès.") : StatusCode(500, "Une erreur est survenue lors de l'ajout du département.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDepartement([FromBody] DepartementDto departementDto)
        {
            if (departementDto == null)
            {
                return BadRequest("Les données du département ne peuvent pas être vides.");
            }

            var success = await _departementService.UpdateDepartement(departementDto);
            return success ? Ok("Département mis à jour avec succès.") : StatusCode(500, "Une erreur est survenue lors de la mise à jour du département.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartement(int id)
        {
            var success = await _departementService.DeleteDepartement(id);
            return success ? Ok("Département supprimé avec succès.") : StatusCode(500, "Une erreur est survenue lors de la suppression du département.");
        }

        [HttpGet("{id}/employes")]
        public async Task<IActionResult> GetEmployesParDepartement(int id)
        {
            try
            {
                var employes = await _departementService.GetEmployesParDepartement(id);
                return Ok(employes);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération des employés du département ID {id} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des employés.");
            }
        }

        [HttpGet("employe/{employeId}")]
        public async Task<IActionResult> GetDepartementByEmploye(int employeId)
        {
            try
            {
                var departement = await _departementService.GetDepartementByEmploye(employeId);
                return departement != null ? Ok(departement) : NotFound(new { Message = "Département non trouvé pour cet employé." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération du département pour l'employé ID {employeId} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération du département.");
            }
        }

        [HttpGet("{id}/conges")]
        public async Task<IActionResult> GetEmployesCongeParDepartement(int id)
        {
            try
            {
                var conges = await _departementService.GetEmployesCongeParDepartement(id);
                return Ok(conges);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération des congés des employés du département ID {id} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des congés.");
            }
        }

        [HttpGet("{id}/payes")]
        public async Task<IActionResult> GetEmployesPayeParDepartement(int id)
        {
            try
            {
                var payes = await _departementService.GetEmployesPayeParDepartement(id);
                return Ok(payes);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération des paies des employés du département ID {id} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des paies.");
            }
        }

        [HttpGet("{id}/assiduites")]
        public async Task<IActionResult> GetEmployesAssiduiteParDepartement(int id)
        {
            try
            {
                var assiduites = await _departementService.GetEmployesAssiduiteParDepartement(id);
                return Ok(assiduites);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération des assiduités des employés du département ID {id} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des assiduités.");
            }
        }

        [HttpGet("{id}/absences")]
        public async Task<IActionResult> GetEmployesAbsenceParDepartement(int id)
        {
            try
            {
                var absences = await _departementService.GetEmployesAbsenceParDepartement(id);
                return Ok(absences);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération des absences des employés du département ID {id} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des absences.");
            }
        }

        [HttpGet("{id}/retards")]
        public async Task<IActionResult> GetEmployesRetardParDepartement(int id)
        {
            try
            {
                var retards = await _departementService.GetEmployesRetardParDepartement(id);
                return Ok(retards);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération des retards des employés du département ID {id} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des retards.");
            }
        }

        [HttpGet("{id}/permissions")]
        public async Task<IActionResult> GetEmployesPermissionParDepartement(int id)
        {
            try
            {
                var permissions = await _departementService.GetEmployesPermissionParDepartement(id);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la récupération des permissions des employés du département ID {id} : {ex.Message}");
                return StatusCode(500, "Une erreur est survenue lors de la récupération des permissions.");
            }
        }
    }
}
