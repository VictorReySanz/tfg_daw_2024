using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers;

public partial class Favorito
{
    [Key]
    [Column("favorito_id")]
    public int FavoritoId { get; set; }

    [Column("usuario_id")]
    public int? UsuarioId { get; set; }

    [Column("opcion_id")]
    public int? OpcionId { get; set; }

    [Column("tipo_opcion")]
    [StringLength(20)]
    [Unicode(false)]
    public string? TipoOpcion { get; set; }

    [ForeignKey("OpcionId")]
    [InverseProperty("Favoritos")]
    public virtual Cv? Opcion { get; set; }

    [ForeignKey("OpcionId")]
    [InverseProperty("Favoritos")]
    public virtual Libro? Opcion1 { get; set; }

    [ForeignKey("OpcionId")]
    [InverseProperty("Favoritos")]
    public virtual Juego? OpcionNavigation { get; set; }

    [ForeignKey("UsuarioId")]
    [InverseProperty("Favoritos")]
    public virtual Usuario? Usuario { get; set; }
}
