using System.Data.Entity;

namespace MVC.Models
{
    public class Context : DbContext
    {
        public Context()
        : base("Data Source=DESKTOP-MFNE5M6\\SQLEXPRESS;Initial Catalog=share_enjoy;Integrated Security=True;TrustServerCertificate=True;")
        {
      /*  public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<CV> CVs { get; set; }
        public DbSet<Juego> Juegos { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<UsuarioGrupo> UsuariosGrupos { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }*/
    }
    }
}
