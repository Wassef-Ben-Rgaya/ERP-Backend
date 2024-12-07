using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Assiduite
{
    public int Assiduiteid { get; set; }

    public int? Matricule { get; set; }

    public double Totalheurespresence { get; set; }

    public double Totalheuressupplementaires { get; set; }

    public double Totalheuresretard { get; set; }

    public double Totalheuresabsence { get; set; }

    public double Totalheurespermission { get; set; }

    public virtual ICollection<Absence> Absences { get; set; } = new List<Absence>();

    public virtual ICollection<Horaire> Horaires { get; set; } = new List<Horaire>();

    public virtual Personnel MatriculeNavigation { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public virtual ICollection<Retard> Retards { get; set; } = new List<Retard>();

    public virtual ICollection<Supplementaire> Supplementaires { get; set; } = new List<Supplementaire>();
}
