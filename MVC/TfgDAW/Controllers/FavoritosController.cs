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
    public class FavoritosController : Controller
    {
        private share_enjoyEntities db = new share_enjoyEntities();

        // GET: Favoritos
        public ActionResult Index()
        {
            var favoritos = db.Favoritos.Include(f => f.Categorias).Include(f => f.Usuarios);
            return View(favoritos.ToList());
        }

        // GET: Favoritos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favoritos favoritos = db.Favoritos.Find(id);
            if (favoritos == null)
            {
                return HttpNotFound();
            }
            return View(favoritos);
        }

        // GET: Favoritos/Create
        public ActionResult Create()
        {
            ViewBag.categoria_id = new SelectList(db.Categorias, "categoria_id", "nombre_categoria");
            ViewBag.usuario_id = new SelectList(db.Usuarios, "usuario_id", "nombre");
            return View();
        }

        // POST: Favoritos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "favorito_id,usuario_id,categoria_id,favorito_ref_id")] Favoritos favoritos)
        {
            if (ModelState.IsValid)
            {
                db.Favoritos.Add(favoritos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.categoria_id = new SelectList(db.Categorias, "categoria_id", "nombre_categoria", favoritos.categoria_id);
            ViewBag.usuario_id = new SelectList(db.Usuarios, "usuario_id", "nombre", favoritos.usuario_id);
            return View(favoritos);
        }

        // GET: Favoritos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favoritos favoritos = db.Favoritos.Find(id);
            if (favoritos == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoria_id = new SelectList(db.Categorias, "categoria_id", "nombre_categoria", favoritos.categoria_id);
            ViewBag.usuario_id = new SelectList(db.Usuarios, "usuario_id", "nombre", favoritos.usuario_id);
            return View(favoritos);
        }

        // POST: Favoritos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "favorito_id,usuario_id,categoria_id,favorito_ref_id")] Favoritos favoritos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(favoritos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categoria_id = new SelectList(db.Categorias, "categoria_id", "nombre_categoria", favoritos.categoria_id);
            ViewBag.usuario_id = new SelectList(db.Usuarios, "usuario_id", "nombre", favoritos.usuario_id);
            return View(favoritos);
        }

        // GET: Favoritos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favoritos favoritos = db.Favoritos.Find(id);
            if (favoritos == null)
            {
                return HttpNotFound();
            }
            return View(favoritos);
        }

        // POST: Favoritos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Favoritos favoritos = db.Favoritos.Find(id);
            db.Favoritos.Remove(favoritos);
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
