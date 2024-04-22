using System;
using System.Collections.Generic;

namespace MVC.Models;

public partial class Favorito
{
    public int FavoritoId { get; set; }

    public int? UsuarioId { get; set; }

    public int? OpcionId { get; set; }

    public string? TipoOpcion { get; set; }

    public virtual Cv? Opcion { get; set; }

    public virtual Libro? Opcion1 { get; set; }

    public virtual Juego? OpcionNavigation { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
