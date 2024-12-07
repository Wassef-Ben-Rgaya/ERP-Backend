using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Token
{
    public int Tokenid { get; set; }

    public int? Matricule { get; set; }

    public string Tokenvalue { get; set; }

    public DateTime Expiration { get; set; }

    public virtual Personnel MatriculeNavigation { get; set; }
}
