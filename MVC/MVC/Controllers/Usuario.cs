using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers;

public partial class Usuario
{
    [Key]
    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [Column("email")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [InverseProperty("Usuario")]
    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    [ForeignKey("UsuarioId")]
    [InverseProperty("Usuarios")]
    public virtual ICollection<Categoria> Categoria { get; set; } = new List<Categoria>();

    [ForeignKey("UsuarioId")]
    [InverseProperty("Usuarios")]
    public virtual ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
}
