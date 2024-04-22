using System;
using System.Collections.Generic;

namespace MVC.Models;

public partial class Libro
{
    public int LibroId { get; set; }

    public string? Titulo { get; set; }

    public string? Autor { get; set; }

    public string? Descripcion { get; set; }

    public bool? Visible { get; set; }

    public int? CategoriaId { get; set; }

    public virtual Categoria? Categoria { get; set; }

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
}
