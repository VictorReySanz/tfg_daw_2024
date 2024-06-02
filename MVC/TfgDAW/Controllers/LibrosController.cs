using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TfgDAW.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TfgDAW.Controllers
{
    public class LibrosController : Controller
    {
        private share_enjoyEntities db = new share_enjoyEntities();


        // Principal
    
        public ActionResult Index(string buscar)
        {
            var librosQuery = db.Libros.Include(l => l.Categorias).Include(l => l.Usuarios).Where(l => l.visible == true);

            // Aplicamos el filtro si searchString no está vacío
            if (!string.IsNullOrEmpty(buscar))
            {
                librosQuery = librosQuery.Where(l => l.titulo.Contains(buscar) || l.autor.Contains(buscar));
            }

            var libros = librosQuery.ToList();

            //Mostrar nombre de usuario en el menu usuario
       
            if (Session["userId"] != null)
            {
                int userId = (int)Session["userId"];
                var user = db.Usuarios.Find(userId);
                ViewBag.user = user.nombre;
                
            //Verificar si es un admin
            if(user.rol == "admin")
            {
                ViewBag.IsAdmin = true;
            } else
            {
                ViewBag.IsAdmin = false;
            }

            }else {
                ViewBag.user = "invitado";
            }

            return View(libros);
        }

        // Favoritos
        [AutorizeAttribute]
        public ActionResult Favoritos(string buscar)
        {

            int userId = (int)Session["userId"];
            var librosQuery = (from fav in db.Favoritos
                         join libro in db.Libros on fav.favorito_ref_id equals libro.libro_id
                         where fav.usuario_id == userId
                   select libro);

            // Aplicamos el filtro si searchString no está vacío
            if (!string.IsNullOrEmpty(buscar))
            {
                librosQuery = librosQuery.Where(l => l.titulo.Contains(buscar) || l.autor.Contains(buscar));
            }

            var libros = librosQuery.ToList();

            //Mostrar nombre de usuario en el menu usuario
            var user = db.Usuarios.Find(userId);
            ViewBag.user = user.nombre;

            //Verificar si es un admin
            if (user.rol == "admin")
            {
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }

            return View(libros);
        }

        //Mis elementos
       [AutorizeAttribute]
        public ActionResult MisElementos(string buscar)
        {

            int userId = (int)Session["userId"];
            var librosQuery = db.Libros.Include(l => l.Categorias).Include(l => l.Usuarios).Where(l => l.usuario_id == userId);

            // Aplicamos el filtro si searchString no está vacío
            if (!string.IsNullOrEmpty(buscar))
            {
                librosQuery = librosQuery.Where(l => l.titulo.Contains(buscar) || l.autor.Contains(buscar));
            }

            var libros = librosQuery.ToList();

            //Mostrar nombre de usuario en el menu usuario
            var user = db.Usuarios.Find(userId);
            ViewBag.user = user.nombre;

            //Verificar si es un admin
            if (user.rol == "admin")
            {
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }

            return View(libros);
        }

        //Ver elemento
        public ActionResult VerElemento(int id)
        {
            var librosQuery = db.Libros.Include(l => l.Categorias).Include(l => l.Usuarios).Where(l => l.libro_id == id);

            var libros = librosQuery.ToList();

            //Mostrar nombre de usuario en el menu usuario
            if (Session["userId"] != null)
            {
                int userId = (int)Session["userId"];
                var user = db.Usuarios.Find(userId);
                ViewBag.user = user.nombre;

            //Verificar si es un admin
            if (user.rol == "admin")
            {
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }

            }
            else
            {
                ViewBag.user = "invitado";
            }

            //verificar si esta en favoritos
            /*var favoritos = db.Favoritos.Where(f => f.usuario_id == userId && f.favorito_ref_id == id).ToList();
            if(favoritos.Count != 0)
            {
                ViewBag.Favorito = true;
            } else
            {
                ViewBag.Favorito = false;
            }*/
 
            return View(libros);
        }

        //Descargar elemento
        [AutorizeAttribute]
        public FileResult DescargarArchivo(int id)
        {
            var libro = db.Libros.FirstOrDefault(l => l.libro_id == id);

            if (libro != null && libro.file_libros != null)
            {
                // archivo contiene el contenido del archivo que deseas descargar
                byte[] archivoContenido = libro.file_libros;
                string nombreArchivo = libro.titulo + ".pdf"; // Nombre del archivo a descargar

                // Devuelve el archivo para descargar
                return File(archivoContenido, "application/pdf", nombreArchivo);
            }

            return null;
        }

        //Crear elemento GET
        [AutorizeAttribute]
        public ActionResult CrearElemento()
        {
            //Mostrar nombre de usuario en el menu usuario
            int userId = (int)Session["userId"];
            var user = db.Usuarios.Find(userId);
            ViewBag.user = user.nombre;

            //Verificar si es un admin
            if (user.rol == "admin")
            {
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }
            
            return View();
        }


        //Crear elemento POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AutorizeAttribute]
        public ActionResult CrearElemento(Libros libros, HttpPostedFileBase imageFile, HttpPostedFileBase pdfFile)
        {

            int userId = (int)Session["userId"];

            libros.categoria_id = 1;
            libros.usuario_id = userId;

            if (ModelState.IsValid)
            {
                // Leer la imagen del archivo
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    using (var binaryReader = new BinaryReader(imageFile.InputStream))
                    {
                        libros.portada = binaryReader.ReadBytes(imageFile.ContentLength);
                    }
                }

                // Leer el archivo del archivo
                if (pdfFile != null && pdfFile.ContentLength > 0)
                {
                    using (var binaryReader = new BinaryReader(pdfFile.InputStream))
                    {
                        libros.file_libros = binaryReader.ReadBytes(pdfFile.ContentLength);
                    }
                }

                // Guardar el libro en la base de datos
                db.Libros.Add(libros);
                db.SaveChanges();
                return RedirectToAction("MisElementos");
            }

            return View(libros);
        }


        // Editar/eliminar elemento GET
        [AutorizeAttribute]
        public ActionResult EditarElemento(int id)
        {
            Libros libros = db.Libros.Find(id);

            //Mostrar nombre de usuario en el menu usuario
            int userId = (int)Session["userId"];
            var user = db.Usuarios.Find(userId);
            ViewBag.user = user.nombre;

            //Verificar si es un admin
            if (user.rol == "admin")
            {
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }

            return View(libros);
        }

        // Editar/eliminar elemento POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AutorizeAttribute]
        public ActionResult EditarElemento([Bind(Include = "libro_id,titulo,autor,descripcion,visible,portada")] Libros libros, string boton, HttpPostedFileBase imageFile, HttpPostedFileBase pdfFile)
        {

            int userId = (int)Session["userId"];
libros.categoria_id = 1;
libros.usuario_id = userId;


    var existingLibro = db.Libros.Find(libros.libro_id);
    if (existingLibro != null)
    {

        // Leer la imagen del archivo
        if (imageFile != null && imageFile.ContentLength > 0)
        {
            using (var binaryReader = new BinaryReader(imageFile.InputStream))
            {
                libros.portada = binaryReader.ReadBytes(imageFile.ContentLength);
                existingLibro.portada = libros.portada;
            }
        }

        // Actualizar solo las propiedades necesarias
        existingLibro.titulo = libros.titulo;
        existingLibro.autor = libros.autor;
        existingLibro.descripcion = libros.descripcion;
        existingLibro.visible = libros.visible;
        existingLibro.categoria_id = libros.categoria_id;
        existingLibro.usuario_id = libros.usuario_id;

        // Mantener el valor existente de file_libros si no se modifica
        if (imageFile == null || imageFile.ContentLength == 0)
        {
            // Conservar el valor existente de file_libros
            libros.file_libros = existingLibro.file_libros;
        }
    }

    if (ModelState.IsValid)
    {
        db.Entry(existingLibro).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("MisElementos");
    }


return View(libros);
        }

        //Mostrar imagen en editar elemento
        [AutorizeAttribute]
        public ActionResult GetImage(int id)
        {
            var libro = db.Libros.Find(id);
            if (libro != null && libro.portada != null)
            {
                return File(libro.portada, "image/jpg");
            }
            else
            {
                return HttpNotFound();
            }
        }

        //Mostrar archivo
        [AutorizeAttribute]
        public ActionResult VerArchivo(int id)
        {
            var librosArchivo = db.Libros.Find(id);
            if (librosArchivo == null)
            {
                return HttpNotFound();
            }

            return File(librosArchivo.file_libros, "application/pdf");
        }

        //Mostrar icono perfil
        public ActionResult GetIcono()
        {

           var icono = new Usuarios();
            if (Session["userId"] != null)
            {
                int userId = (int)Session["userId"];
                 icono = db.Usuarios.Find(userId);
            }

         
       
            if (icono != null && icono.foto != null)
            {
                return File(icono.foto, "image/jpg");
            }
            else
            {
                return File("∼/Content/imgs/iconocuenta.jpg", "image/jpg");
            }
        }

        //Cerrar sesion
        [AutorizeAttribute]
        public ActionResult EliminarSesion()
        {
            // Eliminar la sesión
            Session["userId"] = null;

            // Redirigir a la página de usuarios
            return RedirectToAction("Index", "Usuarios");
        }

        //Añadir a favoritos
        [HttpPost]
        public ActionResult AgregarFavorito(int libroId)
        {
            int userId = (int)Session["userId"];

            // Verificar si el libro ya está en favoritos para este usuario
            var favoritoExistente = db.Favoritos.FirstOrDefault(f => f.favorito_ref_id == libroId && f.usuario_id == userId);

            if (favoritoExistente != null)
            {
                // Si ya existe, lo eliminamos
                db.Favoritos.Remove(favoritoExistente);
            }
            else
            {
                // Si no existe, lo agregamos
                Favoritos favorito = new Favoritos
                {
                    categoria_id = 1,
                    usuario_id = userId,
                    favorito_ref_id = libroId
                };
                db.Favoritos.Add(favorito);
            }

            db.SaveChanges();

            return RedirectToAction("VerElemento", new { id = libroId });
        }


        //Eliminar libro
        public ActionResult EliminarElemento(int id)
        {

            Libros librosEliminar = db.Libros.Find(id);
            db.Libros.Remove(librosEliminar);
            db.SaveChanges();
            return RedirectToAction("MisElementos");
        }








        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
