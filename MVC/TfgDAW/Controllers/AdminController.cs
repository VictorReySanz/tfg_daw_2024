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
    public class AdminController : Controller
    {
        private share_enjoyEntities db = new share_enjoyEntities();

        // GET: Admin
        public ActionResult Index(string buscar)
        {
            var userQuery = db.Usuarios.Include(u => u.Libros);

            // Aplicamos el filtro si searchString no está vacío
            if (!string.IsNullOrEmpty(buscar))
            {
                userQuery = userQuery.Where(u => u.nombre.Contains(buscar) || u.email.Contains(buscar));
            }

            var users = userQuery.ToList();

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

            return View(users);
        }
        public ActionResult AdministrarLibros(string buscar)
        {
            
            var librosQuery = db.Libros.Include(l => l.Categorias).Include(l => l.Usuarios);

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
        public ActionResult AdministrarCVs(string buscar)
        {
            var cvQuery = db.Cv.Include(c => c.Categorias).Include(c => c.Usuarios);

            // Aplicamos el filtro si searchString no está vacío
            if (!string.IsNullOrEmpty(buscar))
            {
                cvQuery = cvQuery.Where(c => c.nombre_completo.Contains(buscar) || c.profesión.Contains(buscar));
            }

            var cv = cvQuery.ToList();

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

            return View(cv);
        }

        //Eliminar libro
        public ActionResult EliminarLibro(int id)
        {

            Libros librosEliminar = db.Libros.Find(id);
            db.Libros.Remove(librosEliminar);
            db.SaveChanges();
            return RedirectToAction("AdministrarLibros");
        }

        //Eliminar cv
        public ActionResult EliminarCV(int id)
        {

            Cv cvEliminar = db.Cv.Find(id);
            db.Cv.Remove(cvEliminar);
            db.SaveChanges();
            return RedirectToAction("AdministrarCVs");
        }

        //Eliminar usuario
        public ActionResult EliminarUsuario(int id)
        {
                var usuario = db.Usuarios.Find(id);
                if (usuario != null)
                {
                    // Eliminar los libros del usuario
                    var libros = db.Libros.Where(l => l.usuario_id == id).ToList();
                    db.Libros.RemoveRange(libros);

                    // Eliminar los cv del usuario
                    var cv = db.Cv.Where(l => l.usuario_id == id).ToList();
                    db.Cv.RemoveRange(cv);

                    // Eliminar el usuario
                    db.Usuarios.Remove(usuario);

                    // Guardar los cambios en la base de datos
                    db.SaveChanges();

                }

            return RedirectToAction("Index", "Admin");
        }



    }
}
