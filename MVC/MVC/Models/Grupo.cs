using System;
using System.Collections.Generic;

namespace MVC.Models;

public partial class Grupo
{
    public int GrupoId { get; set; }

    public string? NombreGrupo { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
