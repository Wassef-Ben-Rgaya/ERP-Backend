using AutoMapper;
using Core.Entities;
using DAL.IRepository;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Serilog;
using Service.DTO;
using Service.IService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Drawing;
using Microsoft.EntityFrameworkCore; // Ajoutez ceci pour utiliser FontFamily

namespace Service.Service
{
    public class PayeService : ServiceAsync<Paye, PayeDto>, IPayeService
    {
        private readonly IRepositoryAsync<Paye> _payeRepository;
        private readonly IRepositoryAsync<Assiduite> _assiduiteRepository;
        private readonly IRepositoryAsync<Absence> _absenceRepository;
        private readonly IRepositoryAsync<Supplementaire> _supplementaireRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PayeService(
            IRepositoryAsync<Paye> payeRepository,
            IRepositoryAsync<Assiduite> assiduiteRepository,
            IRepositoryAsync<Absence> absenceRepository,
            IRepositoryAsync<Supplementaire> supplementaireRepository,
            IMapper mapper,
            ILogger logger)
            : base(payeRepository, mapper)
        {
            _payeRepository = payeRepository ?? throw new ArgumentNullException(nameof(payeRepository));
            _assiduiteRepository = assiduiteRepository ?? throw new ArgumentNullException(nameof(assiduiteRepository));
            _absenceRepository = absenceRepository ?? throw new ArgumentNullException(nameof(absenceRepository));
            _supplementaireRepository = supplementaireRepository ?? throw new ArgumentNullException(nameof(supplementaireRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> AjouterPaye(PayeDto payeDto)
        {
            if (payeDto == null)
            {
                _logger.Error("Tentative d'ajout d'une paie nulle.");
                return false;
            }

            try
            {
                var entity = _mapper.Map<Paye>(payeDto);
                await _payeRepository.Add(entity);
                _logger.Information($"Paie ajoutée avec succès pour le matricule {entity.Matricule}.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de l'ajout de la paie : {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> MettreAJourPaye(PayeDto payeDto)
        {
            if (payeDto == null)
            {
                _logger.Error("Tentative de mise à jour d'une paie nulle.");
                return false;
            }

            try
            {
                var entity = _mapper.Map<Paye>(payeDto);
                await _payeRepository.Update(entity);
                _logger.Information($"Paie mise à jour avec succès pour le matricule {entity.Matricule}.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la mise à jour de la paie : {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> SupprimerPaye(int payeId)
        {
            try
            {
                await _payeRepository.Delete(payeId);
                _logger.Information($"Paie avec l'ID {payeId} supprimée avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la suppression de la paie avec l'ID {payeId} : {ex.Message}", ex);
                return false;
            }
        }

        public async Task<PayeDto> RecupererPayeParId(int payeId)
        {
            try
            {
                var paye = await _payeRepository.GetById(payeId);
                if (paye == null)
                {
                    _logger.Warning($"Aucune paie trouvée avec l'ID {payeId}.");
                    return null;
                }

                var payeDto = _mapper.Map<PayeDto>(paye);

                _logger.Information($"Récupération de la paie avec l'ID {payeId} réussie.");
                return payeDto;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération de la paie avec l'ID {payeId} : {ex.Message}", ex);
                return null;
            }
        }

        public async Task<PayeDto> CalculePaye(int matricule, DateOnly periode, double salaireBase, double tauxHoraire, double tauxSupplementaire, double tauxAbsence)
        {
            try
            {
                var assiduite = await _assiduiteRepository.GetFirstOrDefault(a => a.Matricule == matricule);
                if (assiduite == null)
                {
                    _logger.Warning($"Aucune assiduité trouvée pour le matricule {matricule}.");
                    return null;
                }

                DateTime periodeDateTime = periode.ToDateTime(TimeOnly.MinValue); // Conversion de DateOnly en DateTime

                var absences = await _absenceRepository.GetMuliple(a => a.Assiduiteid == assiduite.Assiduiteid && a.Date.Month == periodeDateTime.Month && a.Date.Year == periodeDateTime.Year);
                var supplementaires = await _supplementaireRepository.GetMuliple(s => s.Assiduiteid == assiduite.Assiduiteid && s.Heuredebut.Month == periodeDateTime.Month && s.Heuredebut.Year == periodeDateTime.Year);

                var totalAbsenceHeures = absences.Sum(a => a.Totalheures);
                var totalSupplementaireHeures = supplementaires.Sum(s => s.Totalheures);

                var salaireBrut = salaireBase + (totalSupplementaireHeures * tauxSupplementaire);
                var deductions = totalAbsenceHeures * tauxAbsence;
                var salaireNet = salaireBrut - deductions;

                var paye = new PayeDto
                {
                    Matricule = matricule,
                    Salairebrut = salaireBrut,
                    Salairenet = salaireNet, // Correction du nom de la propriété
                    Datepaiement = DateOnly.FromDateTime(DateTime.Now), // Using DateOnly for Datepaiement
                    Periode = periode, // Directly assigning DateOnly to Periode
                    Nombredejours = DateTime.DaysInMonth(periode.Year, periode.Month) // Calculating days in month using DateOnly properties
                };

                return paye;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors du calcul de la paie pour le matricule {matricule} et la période {periode} : {ex.Message}", ex);
                return null;
            }
        }

        public async Task<IEnumerable<PayeDto>> GetAllPaye()
        {
            try
            {
                var payes = await _payeRepository.GetAll().ToListAsync();
                var payeDtos = _mapper.Map<IEnumerable<PayeDto>>(payes);
                return payeDtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la récupération de toutes les paies : {ex.Message}", ex);
                return null;
            }
        }

        public async Task<byte[]> ImprimePaye(PayeDto payeDto)
        {
            try
            {
                var document = new PdfDocument();
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Verdana", 20, XFontStyle.Bold);

                gfx.DrawString("Fiche de Paie", font, XBrushes.Black,
                    new XRect(0, 0, page.Width, page.Height),
                    XStringFormats.TopCenter);

                font = new XFont("Verdana", 12, XFontStyle.Regular);
                gfx.DrawString($"Matricule: {payeDto.Matricule}", font, XBrushes.Black, new XRect(50, 50, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString($"Salaire Brut: {payeDto.Salairebrut}", font, XBrushes.Black, new XRect(50, 80, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString($"Salaire Net: {payeDto.Salairenet}", font, XBrushes.Black, new XRect(50, 110, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString($"Date de Paiement: {payeDto.Datepaiement}", font, XBrushes.Black, new XRect(50, 140, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString($"Période: {payeDto.Periode}", font, XBrushes.Black, new XRect(50, 170, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString($"Nombre de Jours: {payeDto.Nombredejours}", font, XBrushes.Black, new XRect(50, 200, page.Width, page.Height), XStringFormats.TopLeft);

                using (var stream = new MemoryStream())
                {
                    document.Save(stream, false);
                    _logger.Information("Fiche de paie générée avec succès.");
                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Erreur lors de la génération de la fiche de paie : {ex.Message}", ex);
                return null;
            }
        }
    }

}
