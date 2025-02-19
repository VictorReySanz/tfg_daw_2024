﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Models;

public partial class ShareEnjoyContext : DbContext
{
    public ShareEnjoyContext()
    {
    }
    public ShareEnjoyContext(DbContextOptions<ShareEnjoyContext> options)
     : base(options)
    {

    }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Libro> Libros { get; set; }
    public DbSet<Cv> CVs { get; set; }
    public DbSet<Juego> Juegos { get; set; }
    public DbSet<Grupo> Grupos { get; set; }
    public DbSet<Favorito> Favoritos { get; set; }
   
 }

    /*
    public ShareEnjoyContext(DbContextOptions<ShareEnjoyContext> options)
        : base(options)
    {

    }

    public virtual DbSet<Favorito> Favoritos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlServer("Data Source=DESKTOP-MFNE5M6\\SQLEXPRESS;Initial Catalog=share_enjoy;Integrated Security=True;Trusted_Connection=SSPI;MultipleActiveResultSets=true;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Favorito>(entity =>
        {
            entity.HasKey(e => e.FavoritoId).HasName("PK__Favorito__B8BA20CA802BE645");

            entity.Property(e => e.FavoritoId)
                .ValueGeneratedNever()
                .HasColumnName("favorito_id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.FavoritoRefId).HasColumnName("favorito_ref_id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<MVC.Models.Grupo>? Grupo { get; set; }

    public DbSet<MVC.Models.Usuario>? Usuario { get; set; }

    public DbSet<MVC.Models.Cv>? Cv { get; set; }

    */

