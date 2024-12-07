using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Permission
{
    public int Permissionid { get; set; }

    public int? Assiduiteid { get; set; }

    public DateOnly Date { get; set; }

    public TimeSpan Duree { get; set; }

    public string Status { get; set; }

    public virtual Assiduite Assiduite { get; set; }
}
