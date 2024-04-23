using System;
using System.Collections.Generic;

namespace MVC.Models;

public partial class Juego
{
    public int JuegoId { get; set; }

    public string? NombreJuego { get; set; }

    public string? Descripcion { get; set; }

    public bool? Visible { get; set; }




    public int? CategoriaId { get; set; }

    public virtual Categoria? Categoria { get; set; }

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
}
