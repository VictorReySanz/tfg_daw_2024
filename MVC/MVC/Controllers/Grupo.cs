using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers;

public partial class Grupo
{
    [Key]
    [Column("grupo_id")]
    public int GrupoId { get; set; }

    [Column("nombre_grupo")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NombreGrupo { get; set; }

    [ForeignKey("GrupoId")]
    [InverseProperty("Grupos")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
