using System;
using System.Collections.Generic;

namespace MVC.Models;

public partial class Cv
{
    public int CvId { get; set; }

    public string? NombreCompleto { get; set; }

    public bool? Visible { get; set; }

    public int? CategoriaId { get; set; }

    public virtual Categoria? Categoria { get; set; }

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
}
