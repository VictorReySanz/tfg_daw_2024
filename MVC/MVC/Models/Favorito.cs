using System;
using System.Collections.Generic;

namespace MVC.Models;

public partial class Favorito
{
    public int FavoritoId { get; set; }

    public int? UsuarioId { get; set; }

    public int? CategoriaId { get; set; }

    public int? FavoritoRefId { get; set; }
}
