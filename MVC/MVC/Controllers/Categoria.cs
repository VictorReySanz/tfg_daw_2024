using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers;

public partial class Categoria
{
    [Key]
    [Column("categoria_id")]
    public int CategoriaId { get; set; }

    [Column("nombre_categoria")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NombreCategoria { get; set; }

    [InverseProperty("Categoria")]
    public virtual ICollection<Cv> Cvs { get; set; } = new List<Cv>();

    [InverseProperty("Categoria")]
    public virtual ICollection<Juego> Juegos { get; set; } = new List<Juego>();

    [InverseProperty("Categoria")]
    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();

    [ForeignKey("CategoriaId")]
    [InverseProperty("Categoria")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
