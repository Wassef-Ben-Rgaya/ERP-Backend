using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Absence
{
    public int Absenceid { get; set; }

    public int? Assiduiteid { get; set; }

    public DateOnly Date { get; set; }

    public bool Justifiee { get; set; }

    public double Totalheures { get; set; }

    public virtual Assiduite Assiduite { get; set; }
}
