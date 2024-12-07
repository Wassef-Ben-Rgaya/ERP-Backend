using Core.Entities;
using Service.Common.Mappings;
using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class CombinedPatientDto
    {
        public int Utilisateurid { get; set; }

        [Required(ErrorMessage = "Le nom est requis.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est requis.")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "L'adresse est requise.")]
        public string Adresse { get; set; }

        [Required(ErrorMessage = "Le numéro de téléphone est requis.")]
        [Phone(ErrorMessage = "Le format du numéro de téléphone est incorrect.")]
        public string Numtelephone { get; set; }

        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "Le format de l'email est incorrect.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        public string Motdepasse { get; set; }  // Password field

        // Champs spécifiques à Patient
        [Required(ErrorMessage = "La date de naissance est requise.")]
        public DateOnly? Datenaissance { get; set; }

        [Required(ErrorMessage = "Le sexe est requis.")]
        public string Sexe { get; set; }

        [Required(ErrorMessage = "La situation familiale est requise.")]
        public string Situationfamiliale { get; set; }

    }
}
