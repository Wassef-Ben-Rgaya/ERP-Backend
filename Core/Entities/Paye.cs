using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Paye
{
    public int Payeid { get; set; }

    public int? Matricule { get; set; }

    public double Salairebrut { get; set; }

    public double Salairenet { get; set; }

    public DateOnly Datepaiement { get; set; }

    public double? Prime { get; set; }

    public DateOnly Periode { get; set; }

    public int Nombredejours { get; set; }

    public virtual Personnel MatriculeNavigation { get; set; }
}
