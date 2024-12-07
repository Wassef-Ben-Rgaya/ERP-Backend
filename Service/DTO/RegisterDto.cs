namespace Service.DTO
{
    public class RegisterDto
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime DateNaissance { get; set; }
        public string Adresse { get; set; }
        public string Email { get; set; }
        public string Mdp { get; set; }
        public string Poste { get; set; }
        public DateTime DateEmbauche { get; set; }
        public string StatutFamiliale { get; set; }
        public string TypeContrat { get; set; }
        public long NumeroTelephone { get; set; }
        public int DepartementId { get; set; }
        public int Matricule { get; set; } 
    }
}
