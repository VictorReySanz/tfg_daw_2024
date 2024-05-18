using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TfgDAW.Models;

namespace TfgDAW.Controllers
{
    public class UsuariosController : Controller
    {

        private share_enjoyEntities db = new share_enjoyEntities();

        // Login
        public ActionResult Index()
            {
                return View();
            }

            // Vista rgistro
            public ActionResult Registro()
            {
                return View();
            }

        // Mis datos
        public ActionResult MisDatos()
        {
            return View();
        }


        /*public Usuarios GetUser(string nombre)
        {
            return this.db.Usuarios.Where(user => user.nombre == nombre).FirstOrDefault();
        }*/


        public void CrearUser(string nombre, string password, string email) {

            Usuarios nuevo = new Usuarios
            {

                usuario_id = this.GetMaxID() + 1,
                nombre = nombre,
                password = BCrypt.Net.BCrypt.HashPassword(password),
                email=email,
                rol="usuario"
            };

            this.db.Usuarios.Add(nuevo);
            this.db.SaveChanges();
        
        }

        private int GetMaxID() {
            if (this.db.Usuarios.Count() != 0) {
                int maxid = this.db.Usuarios.Max(user => user.usuario_id);

                if (maxid > 0)
                {
                    return maxid;
                }
                else {
                    return 0;
                }

            }
            else
            {
                return 0;
            }


        }



        // GET: Usuarios/Details/5
        public ActionResult Details(int id)
            {
                return View();
            }

            // GET: Usuarios/Create
            public ActionResult Create()
            {
                return View();
            }

            // POST: Usuarios/Create
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

            // GET: Usuarios/Edit/5
            public ActionResult Edit(int id)
            {
                return View();
            }

            // POST: Usuarios/Edit/5
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

            // GET: Usuarios/Delete/5
            public ActionResult Delete(int id)
            {
                return View();
            }

            // POST: Usuarios/Delete/5
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
