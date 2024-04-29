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
    public class JuegosController : Controller
    {
        private share_enjoyEntities db = new share_enjoyEntities();

        // GET: Juegos
        public ActionResult Index()
        {
            var juegos = db.Juegos.Include(j => j.Categorias);
            return View(juegos.ToList());
        }

        // GET: Juegos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Juegos juegos = db.Juegos.Find(id);
            if (juegos == null)
            {
                return HttpNotFound();
            }
            return View(juegos);
        }

        // GET: Juegos/Create
        public ActionResult Create()
        {
            ViewBag.categoria_id = new SelectList(db.Categorias, "categoria_id", "nombre_categoria");
            return View();
        }

        // POST: Juegos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "juego_id,nombre_juego,descripcion,visible,categoria_id")] Juegos juegos)
        {
            if (ModelState.IsValid)
            {
                db.Juegos.Add(juegos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.categoria_id = new SelectList(db.Categorias, "categoria_id", "nombre_categoria", juegos.categoria_id);
            return View(juegos);
        }

        // GET: Juegos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Juegos juegos = db.Juegos.Find(id);
            if (juegos == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoria_id = new SelectList(db.Categorias, "categoria_id", "nombre_categoria", juegos.categoria_id);
            return View(juegos);
        }

        // POST: Juegos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "juego_id,nombre_juego,descripcion,visible,categoria_id")] Juegos juegos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(juegos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categoria_id = new SelectList(db.Categorias, "categoria_id", "nombre_categoria", juegos.categoria_id);
            return View(juegos);
        }

        // GET: Juegos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Juegos juegos = db.Juegos.Find(id);
            if (juegos == null)
            {
                return HttpNotFound();
            }
            return View(juegos);
        }

        // POST: Juegos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Juegos juegos = db.Juegos.Find(id);
            db.Juegos.Remove(juegos);
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
