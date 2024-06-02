
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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

        // Mis datos GET
        public ActionResult MisDatos()
        {

            int userId = (int)Session["userId"];
            ViewBag.UserId = userId;
            Usuarios usuario = db.Usuarios.Find(userId);

            return View(usuario);
        }

        // Mis datos POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MisDatos(Usuarios usuarios, HttpPostedFileBase imageFile, string password, string passwordN, string boton)
        {
            if (boton == "Guardar")
            {
                var existingUsuario = db.Usuarios.Find(usuarios.usuario_id);
                if (existingUsuario != null)
                {
                    existingUsuario.nombre = usuarios.nombre;
                    existingUsuario.email = usuarios.email;

                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        using (var reader = new System.IO.BinaryReader(imageFile.InputStream))
                        {
                            existingUsuario.foto = reader.ReadBytes(imageFile.ContentLength);
                        }
                    }

                    if ((password != null) || (passwordN != null))
                    {
                        int userId = (int)Session["userId"];
                        Usuarios usuariosPassword = db.Usuarios.Find(userId);
                        bool valida = BCrypt.Net.BCrypt.Verify(password, usuariosPassword.password);
                        if (valida)
                        {
                            existingUsuario.password = BCrypt.Net.BCrypt.HashPassword(passwordN);
                        }
                    }

                }

                if (ModelState.IsValid)
                {
                    db.Entry(existingUsuario).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("MisDatos");
                }
            } else if (boton == "Eliminar")
            {

            }
            return View(usuarios);
        }

        //Mostrar imagen en mis datos
        public ActionResult GetImage(int id)
        {
            int userId = (int)Session["userId"];
            var usuario = db.Usuarios.Find(userId);
            if (usuario != null && usuario.foto != null)
            {
                return File(usuario.foto, "image/jpg");
            }
            else
            {
                return HttpNotFound();
            }
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

            List<Usuarios> useremail = db.Usuarios.Where(u => u.email == email).ToList();

            if (useremail.Count == 0 && Validapass(password, pass2))
            {
                password = password.Trim();

                    string filePath = Server.MapPath("~/Content/imgs/iconocuenta.png");
                    byte[] imgEjemplo = null;
                    if (System.IO.File.Exists(filePath))
                    {
                        using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                        {
                            using (var reader = new System.IO.BinaryReader(fileStream))
                            {
                                imgEjemplo = reader.ReadBytes((int)fileStream.Length);
                            }
                        }
                    }

                    Usuarios nuevo = new Usuarios
                    {

                        usuario_id = this.GetMaxID() + 1,
                        nombre = nombre,
                        password = BCrypt.Net.BCrypt.HashPassword(password),
                        email = email,
                        rol = "usuario",
                        foto = imgEjemplo
                    };

                    this.db.Usuarios.Add(nuevo);
                    this.db.SaveChanges();
                    Session["userId"] = nuevo.usuario_id;
                    return RedirectToAction("index", "Libros");

                }
            TempData["Message"] = "Ya existe ese correo o revisa tu password";
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

              
                    TempData["Message"] = "Revisa tus datos";
                    return RedirectToAction("Index");
                }
            }
            else
            {

                TempData["Message"] = "Revisa tus datos";
                return RedirectToAction("Index");
            }

        }

        public ActionResult EliminarUsuario()
        {
            if (Session["userId"] != null)
            {
                int userId = (int)Session["userId"];
                var usuario = db.Usuarios.Find(userId);
                if (usuario != null)
                {
                    // Eliminar los libros del usuario
                    var libros = db.Libros.Where(l => l.usuario_id == userId).ToList();
                    db.Libros.RemoveRange(libros);

                    // Eliminar los cv del usuario
                    var cv = db.Cv.Where(l => l.usuario_id == userId).ToList();
                    db.Cv.RemoveRange(cv);

                    // Eliminar el usuario
                    db.Usuarios.Remove(usuario);

                    // Guardar los cambios en la base de datos
                    db.SaveChanges();

                    // Eliminar la sesión
                    Session["userId"] = null;
                }
            }

            // Redirigir a la página de usuarios
            return RedirectToAction("Index", "Usuarios");
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
