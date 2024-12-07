using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Conge
{
    public int Congeid { get; set; }

    public int? Matricule { get; set; }

    public DateOnly Datedebut { get; set; }

    public DateOnly Datefin { get; set; }

    public string Status { get; set; }

    public virtual Personnel MatriculeNavigation { get; set; }
}
