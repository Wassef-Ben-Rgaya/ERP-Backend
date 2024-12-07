using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Département
{
    public int Departementid { get; set; }

    public string Nom { get; set; }

    public virtual ICollection<Personnel> Personnel { get; set; } = new List<Personnel>();
}
