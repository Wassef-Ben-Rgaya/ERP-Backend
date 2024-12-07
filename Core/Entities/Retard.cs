using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Retard
{
    public int Retardid { get; set; }

    public int? Assiduiteid { get; set; }

    public DateOnly Date { get; set; }

    public TimeSpan Duree { get; set; }

    public double Totalheures { get; set; }

    public virtual Assiduite Assiduite { get; set; }
}
