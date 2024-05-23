using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using TfgDAW.Models;

namespace TfgDAW.Controllers
{
    public class CvController : Controller
    {

        private share_enjoyEntities db = new share_enjoyEntities();

        //Buscar CVs
        public ActionResult Index()
        {
            var cvQuery = db.Cv.Where(l => l.visible == true);
            var cv = cvQuery.ToList();
            return View(cv);
        }

        // Ver CV
        public ActionResult VerCv()
        {
            var cvQuery = db.Cv.Where(c => c.usuario_id == 1);
            var cv = cvQuery.ToList();

            //Listar portafolio
            var cvPortafolioQuery = db.Cv.Where(c => c.cv_id == id).Select(c => c.Portafolio).FirstOrDefault();

            if (cvPortafolioQuery != null)
            {
                string[] elementos = cvPortafolioQuery.Split('|');
                List<string> nombres = new List<string>();
                List<string> enlaces = new List<string>();

                foreach (var elemento in elementos)
                {
                    string[] elementosSeparados = elemento.Split(';');
                    if (elementosSeparados.Length >= 2)
                    {
                        nombres.Add(elementosSeparados[0]);
                        enlaces.Add(elementosSeparados[1]);
                    }
                }

                ViewBag.Nombres = nombres;
                ViewBag.Enlaces = enlaces;
            }
            else
            {
                ViewBag.Nombres = null;
                ViewBag.Enlaces = null;
            }

            return View(cv);
        }

        // Editar CV
        public ActionResult EditarCv()
        {
            var cvQuery = db.Cv.Where(c => c.usuario_id == 1004);/*cambiar por id del usuario correcto*/
            var cv = cvQuery.ToList();

            //Listar portafolio                      /*cambiar por id del usuario correcto*/
            var cvPortafolioQuery = db.Cv.Where(c => c.usuario_id == 1004).Select(c => c.Portafolio).FirstOrDefault();

            if (cvPortafolioQuery != null)
            {
                string[] elementos = cvPortafolioQuery.Split('|');
                List<string> nombres = new List<string>();
                List<string> enlaces = new List<string>();

                foreach (var elemento in elementos)
                {
                    string[] elementosSeparados = elemento.Split(';');
                    if (elementosSeparados.Length >= 2)
                    {
                        nombres.Add(elementosSeparados[0]);
                        enlaces.Add(elementosSeparados[1]);
                    }
                }

                ViewBag.Nombres = nombres;
                ViewBag.Enlaces = enlaces;
            }
            else
            {
                ViewBag.Nombres = null;
                ViewBag.Enlaces = null;
            }

            return View(cv);
        }

        //Mostrar archivo
        public ActionResult VerArchivo(int id)
        {
            var librosArchivo = db.Libros.Find(2013);/*cambiar por id, como en libros*/
            if (librosArchivo == null)
            {
                return HttpNotFound();
            }

            return File(librosArchivo.file_libros, "application/pdf");
        }


        //Editar un elemento del portafolio: get
        public ActionResult CrearElementoPortafolio()
        {
            return View();
        }
            //Editar un elemento del portafolio: put
            [HttpPost]
        public ActionResult CrearElementoPortafolio(FormCollection form)
        {
            string titulo = form["titulo"];
            string enlace = form["enlace"];








            var existingCv = db.Cv.Find(1002);
            if (existingCv != null)
            {

                // Actualizar solo las propiedades necesarias
                existingCv.Portafolio = existingCv.Portafolio + titulo + ";" + enlace + "|";
            }

            if (ModelState.IsValid)
            {
                db.Entry(existingCv).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EditarCv");
            }
        

            return View();
        }






















        // GET: CV/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CV/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CV/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CV/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CV/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CV/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CV/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
