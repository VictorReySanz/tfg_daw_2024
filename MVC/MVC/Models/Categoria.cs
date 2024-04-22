using System;
using System.Collections.Generic;

namespace MVC.Models;

public partial class Categoria
{
    public int CategoriaId { get; set; }

    public string? NombreCategoria { get; set; }

    public virtual ICollection<Cv> Cvs { get; set; } = new List<Cv>();

    public virtual ICollection<Juego> Juegos { get; set; } = new List<Juego>();

    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
