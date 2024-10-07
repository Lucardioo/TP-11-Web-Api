using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Role
{
    public Guid Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
