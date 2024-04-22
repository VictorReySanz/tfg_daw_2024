using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers;

[Table("CV")]
public partial class Cv
{
    [Key]
    [Column("cv_id")]
    public int CvId { get; set; }

    [Column("nombre_completo")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NombreCompleto { get; set; }

    [Column("visible")]
    public bool? Visible { get; set; }

    [Column("categoria_id")]
    public int? CategoriaId { get; set; }

    [ForeignKey("CategoriaId")]
    [InverseProperty("Cvs")]
    public virtual Categoria? Categoria { get; set; }

    [InverseProperty("Opcion")]
    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
}
