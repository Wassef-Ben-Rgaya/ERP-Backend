using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Personnel
{
    public int Matricule { get; set; }

    public string Nom { get; set; }

    public string Prenom { get; set; }

    public DateOnly Datenaissance { get; set; }

    public string Adresse { get; set; }

    public string Email { get; set; }

    public string Mdp { get; set; }

    public string Poste { get; set; }

    public DateOnly Dateembauche { get; set; }

    public string Statutfamiliale { get; set; }

    public string Typecontrat { get; set; }

    public long Numerotelephone { get; set; }

    public int? Departementid { get; set; }

    public virtual ICollection<Assiduite> Assiduites { get; set; } = new List<Assiduite>();

    public virtual ICollection<Conge> Conges { get; set; } = new List<Conge>();

    public virtual Département Departement { get; set; }

    public virtual ICollection<Paye> Payes { get; set; } = new List<Paye>();

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}
