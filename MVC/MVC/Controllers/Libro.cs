using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers;

public partial class Libro
{
    [Key]
    [Column("libro_id")]
    public int LibroId { get; set; }

    [Column("titulo")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Titulo { get; set; }

    [Column("autor")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Autor { get; set; }

    [Column("descripcion", TypeName = "text")]
    public string? Descripcion { get; set; }

    [Column("visible")]
    public bool? Visible { get; set; }

    [Column("categoria_id")]
    public int? CategoriaId { get; set; }

    [ForeignKey("CategoriaId")]
    [InverseProperty("Libros")]
    public virtual Categoria? Categoria { get; set; }

    [InverseProperty("Opcion1")]
    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
}
