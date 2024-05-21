using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TfgDAW.Models;

namespace TfgDAW.Controllers
{
    public class CvController : Controller
    {

        private share_enjoyEntities db = new share_enjoyEntities();
        // GET: CV
        public ActionResult Index()
        {
            return View(db.Cv.ToList());
        }

        // GET: CV
        public ActionResult VerCv()
        {
            var cvQuery = db.Cv.Where(c => c.usuario_id == 1);
            var cv = cvQuery.ToList();

            return View(cv);
        }

        public ActionResult EditarElementoPortafolio()
        {

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
