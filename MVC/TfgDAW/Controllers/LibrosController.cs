using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TfgDAW.Models;

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

            return View(libros);
        }
        //Mis elementos
        public ActionResult MisElementos(string buscar)
        {
            var librosQuery = db.Libros.Include(l => l.Categorias).Include(l => l.Usuarios).Where(l => l.usuario_id == 1);

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


        //Crear elemento GET
        public ActionResult CrearElemento()
        {
            return View();
        }

        //Crear elemento POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearElemento([Bind(Include = "libro_id,titulo,autor,descripcion,visible,categoria_id,usuario_id")] Libros libros)
        {
            libros.libro_id = 8;
            libros.categoria_id = 1;
            libros.usuario_id = 1;

            if (ModelState.IsValid)
            {
                db.Libros.Add(libros);
                db.SaveChanges();
                return RedirectToAction("MisElementos");
            }

            return View(libros);
        }






        // GET: Libros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libros libros = db.Libros.Find(id);
            if (libros == null)
            {
                return HttpNotFound();
            }
            return View(libros);
        }

        // GET: Libros/Create
        public ActionResult Create()
        {
            ViewBag.categoria_id = new SelectList(db.Categorias, "categoria_id", "nombre_categoria");
            ViewBag.usuario_id = new SelectList(db.Usuarios, "usuario_id", "nombre");
            return View();
        }

        // POST: Libros/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "libro_id,titulo,autor,descripcion,visible,categoria_id,usuario_id")] Libros libros)
        {
            if (ModelState.IsValid)
            {
                db.Libros.Add(libros);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.categoria_id = new SelectList(db.Categorias, "categoria_id", "nombre_categoria", libros.categoria_id);
            ViewBag.usuario_id = new SelectList(db.Usuarios, "usuario_id", "nombre", libros.usuario_id);
            return View(libros);
        }

        // GET: Libros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libros libros = db.Libros.Find(id);
            if (libros == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoria_id = new SelectList(db.Categorias, "categoria_id", "nombre_categoria", libros.categoria_id);
            ViewBag.usuario_id = new SelectList(db.Usuarios, "usuario_id", "nombre", libros.usuario_id);
            return View(libros);
        }

        // POST: Libros/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "libro_id,titulo,autor,descripcion,visible,categoria_id,usuario_id")] Libros libros)
        {
            if (ModelState.IsValid)
            {
                db.Entry(libros).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categoria_id = new SelectList(db.Categorias, "categoria_id", "nombre_categoria", libros.categoria_id);
            ViewBag.usuario_id = new SelectList(db.Usuarios, "usuario_id", "nombre", libros.usuario_id);
            return View(libros);
        }

        // GET: Libros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libros libros = db.Libros.Find(id);
            if (libros == null)
            {
                return HttpNotFound();
            }
            return View(libros);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Libros libros = db.Libros.Find(id);
            db.Libros.Remove(libros);
            db.SaveChanges();
            return RedirectToAction("Index");
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
