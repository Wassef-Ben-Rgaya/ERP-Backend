using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Horaire
{
    public int Horaireid { get; set; }

    public int? Assiduiteid { get; set; }

    public DateTime Heuredebut { get; set; }

    public DateTime Heurefin { get; set; }

    public double Totalheures { get; set; }

    public virtual Assiduite Assiduite { get; set; }
}
