
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


        public Usuarios GetUserbyEmail(string email)
        {
            return this.db.Usuarios.Where(user => user.email == email).FirstOrDefault();
        }


        public Boolean Validapass(string password, string passwordRepeat)
        {
            if (password != passwordRepeat)
            {
                //ViewData["ERROR"] = "las dos Contraseñas debe ser iguales";
                return false;
            }
            else
            {
                return true;

            }


        }

        public ActionResult CrearUser(string nombre, string password, string email, string pass2)
        {
            if (Validapass(password, pass2))
            {
                password = password.Trim();

                Usuarios nuevo = new Usuarios
                {

                    usuario_id = this.GetMaxID() + 1,
                    nombre = nombre,
                    password = BCrypt.Net.BCrypt.HashPassword(password),
                    email = email,
                    rol = "usuario"
                };

                this.db.Usuarios.Add(nuevo);
                this.db.SaveChanges();
                Session["userId"] = nuevo.usuario_id;
                return RedirectToAction("index", "Libros");

            }
            return RedirectToAction("Registro");

        }

        private int GetMaxID()
        {
            if (this.db.Usuarios.Count() != 0)
            {
                int maxid = this.db.Usuarios.Max(user => user.usuario_id);

                if (maxid > 0)
                {
                    return maxid;
                }
                else
                {
                    return 0;
                }

            }
            else
            {
                return 0;
            }


        }

        [HttpPost]
        public ActionResult EnviarFormulario(string email, string password)
        {

            /*Prueba del bcrypt
             * 
                    string originalPassword = "luis";
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(originalPassword);

                    bool isMatch = BCrypt.Net.BCrypt.Verify(originalPassword, hashedPassword);
                    Console.WriteLine($"Password matches: {isMatch}"); // Debería imprimir "Password matches: True"
              */


            Usuarios usermail = this.GetUserbyEmail(email);
            if (usermail != null)
            {
                bool valida = BCrypt.Net.BCrypt.Verify(password, usermail.password);
                if (valida)
                {
                    Session["userId"] = usermail.usuario_id;
                    int userId = (int)Session["userId"];
                    return RedirectToAction("index", "Libros");
                }
                else
                {

                    ViewData["ERROR"] = "Revisa tus datos";
                    return RedirectToAction("Index");
                }
            }
            else
            {

                ViewData["ERROR"] = "Revisa tus datos";
                return RedirectToAction("Index");
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
