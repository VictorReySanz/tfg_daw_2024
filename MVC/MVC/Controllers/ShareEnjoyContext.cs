using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers;

public partial class ShareEnjoyContext : DbContext
{
    public ShareEnjoyContext()
    {
    }

    public ShareEnjoyContext(DbContextOptions<ShareEnjoyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Cv> Cvs { get; set; }

    public virtual DbSet<Favorito> Favoritos { get; set; }

    public virtual DbSet<Grupo> Grupos { get; set; }

    public virtual DbSet<Juego> Juegos { get; set; }

    public virtual DbSet<Libro> Libros { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-MFNE5M6\\SQLEXPRESS;Initial Catalog=share_enjoy;Integrated Security=True;Trusted_Connection=SSPI;MultipleActiveResultSets=true;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PK__Categori__DB875A4F35A52752");

            entity.Property(e => e.CategoriaId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Cv>(entity =>
        {
            entity.HasKey(e => e.CvId).HasName("PK__CV__C36883E617102C62");

            entity.Property(e => e.CvId).ValueGeneratedNever();

            entity.HasOne(d => d.Categoria).WithMany(p => p.Cvs).HasConstraintName("FK__CV__categoria_id__30F848ED");
        });

        modelBuilder.Entity<Favorito>(entity =>
        {
            entity.HasKey(e => e.FavoritoId).HasName("PK__Favorito__B8BA20CADAD8D800");

            entity.Property(e => e.FavoritoId).ValueGeneratedNever();

            entity.HasOne(d => d.Opcion).WithMany(p => p.Favoritos)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Favoritos__opcio__3E52440B");

            entity.HasOne(d => d.OpcionNavigation).WithMany(p => p.Favoritos)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Favoritos__opcio__3F466844");

            entity.HasOne(d => d.Opcion1).WithMany(p => p.Favoritos)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Favoritos__opcio__3D5E1FD2");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Favoritos).HasConstraintName("FK__Favoritos__usuar__3C69FB99");
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.HasKey(e => e.GrupoId).HasName("PK__Grupos__90617DE1F4CF3635");

            entity.Property(e => e.GrupoId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Juego>(entity =>
        {
            entity.HasKey(e => e.JuegoId).HasName("PK__Juegos__9C2D8A86D3E938B1");

            entity.Property(e => e.JuegoId).ValueGeneratedNever();

            entity.HasOne(d => d.Categoria).WithMany(p => p.Juegos).HasConstraintName("FK__Juegos__categori__33D4B598");
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.LibroId).HasName("PK__Libros__5BB6A2FC510B7E1A");

            entity.Property(e => e.LibroId).ValueGeneratedNever();

            entity.HasOne(d => d.Categoria).WithMany(p => p.Libros).HasConstraintName("FK__Libros__categori__2E1BDC42");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2ED7D2AFF1522507");

            entity.Property(e => e.UsuarioId).ValueGeneratedNever();

            entity.HasMany(d => d.Categoria).WithMany(p => p.Usuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "UsuariosCategoria",
                    r => r.HasOne<Categoria>().WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Usuarios___categ__2B3F6F97"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Usuarios___usuar__2A4B4B5E"),
                    j =>
                    {
                        j.HasKey("UsuarioId", "CategoriaId").HasName("PK__Usuarios__C36FA70BCF1FCCF2");
                        j.ToTable("Usuarios_Categorias");
                        j.IndexerProperty<int>("UsuarioId").HasColumnName("usuario_id");
                        j.IndexerProperty<int>("CategoriaId").HasColumnName("categoria_id");
                    });

            entity.HasMany(d => d.Grupos).WithMany(p => p.Usuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "UsuariosGrupo",
                    r => r.HasOne<Grupo>().WithMany()
                        .HasForeignKey("GrupoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Usuarios___grupo__398D8EEE"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Usuarios___usuar__38996AB5"),
                    j =>
                    {
                        j.HasKey("UsuarioId", "GrupoId").HasName("PK__Usuarios__27D1C5719C641F6D");
                        j.ToTable("Usuarios_Grupos");
                        j.IndexerProperty<int>("UsuarioId").HasColumnName("usuario_id");
                        j.IndexerProperty<int>("GrupoId").HasColumnName("grupo_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
