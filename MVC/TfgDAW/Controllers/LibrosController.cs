using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
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
            int userId = (int)Session["userId"];

            var user = db.Usuarios.Find(userId);
            ViewBag.user = user.nombre;

            return View(libros);
        }

        // Favoritos
        public ActionResult Favoritos(string buscar)
        {

            int userId = (int)Session["userId"];
            var librosQuery = db.Libros.Where(a => db.Favoritos.Any(b => b.favorito_id == a.libro_id)).Where(l => l.usuario_id == userId);

            // Aplicamos el filtro si searchString no está vacío
            if (!string.IsNullOrEmpty(buscar))
            {
                librosQuery = librosQuery.Where(l => l.titulo.Contains(buscar) || l.autor.Contains(buscar));
            }

            var libros = librosQuery.ToList();

            return View(libros);
        }

        //Mis elementos
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

            return View(libros);
        }

        //Ver elemento
        public ActionResult VerElemento(int id)
        {
            var librosQuery = db.Libros.Include(l => l.Categorias).Include(l => l.Usuarios).Where(l => l.libro_id == id);

            var libros = librosQuery.ToList();

            return View(libros);
        }

        //Descargar elemento
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
        public ActionResult CrearElemento()
        {
            return View();
        }


        //Crear elemento POST
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public ActionResult EditarElemento(int id)
        {
            Libros libros = db.Libros.Find(id);

            return View(libros);
        }

        // Editar/eliminar elemento POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarElemento([Bind(Include = "libro_id,titulo,autor,descripcion,visible,portada")] Libros libros, string boton, HttpPostedFileBase imageFile, HttpPostedFileBase pdfFile)
        {

            int userId = (int)Session["userId"];
            libros.categoria_id = 1;
            libros.usuario_id = userId;

            if (boton == "Guardar")
            {



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
            }
            else if (boton == "Eliminar")
            {
                Libros librosEliminar = db.Libros.Find(libros.libro_id);
                db.Libros.Remove(librosEliminar);
                db.SaveChanges();
                return RedirectToAction("MisElementos");
            }

            return View(libros);
        }

        //Mostrar imagen en editar elemento
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
        public ActionResult VerArchivo(int id)
        {
            var librosArchivo = db.Libros.Find(id);
            if (librosArchivo == null)
            {
                return HttpNotFound();
            }

            return File(librosArchivo.file_libros, "application/pdf");
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
