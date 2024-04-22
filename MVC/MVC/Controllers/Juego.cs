using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers;

public partial class Juego
{
    [Key]
    [Column("juego_id")]
    public int JuegoId { get; set; }

    [Column("nombre_juego")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NombreJuego { get; set; }

    [Column("descripcion")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [Column("visible")]
    public bool? Visible { get; set; }

    [Column("categoria_id")]
    public int? CategoriaId { get; set; }

    [ForeignKey("CategoriaId")]
    [InverseProperty("Juegos")]
    public virtual Categoria? Categoria { get; set; }

    [InverseProperty("OpcionNavigation")]
    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
}
