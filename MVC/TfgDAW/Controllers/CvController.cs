using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TfgDAW.Models;

namespace TfgDAW.Controllers
{
    public class CvController : Controller
    {
        private share_enjoyEntities db = new share_enjoyEntities();

        //Buscar CVs
        public ActionResult Index(string buscar)
        {
            var cvQuery = db.Cv.Where(l => l.visible == true);

            // Aplicamos el filtro si searchString no está vacío
            if (!string.IsNullOrEmpty(buscar))
            {
                cvQuery = cvQuery.Where(c => c.tecnología.Contains(buscar) || c.profesión.Contains(buscar));
            }

            var cv = cvQuery.ToList();

            //Listar redes
            var cvRedesQuery = db.Cv.Where(l => l.visible == true).Select(c => c.redes_sociales);

            if (cvRedesQuery.Any())
            {
                List<string> redesList = new List<string>();

                foreach (var redes in cvRedesQuery)
                {
                    if (!string.IsNullOrEmpty(redes))
                    {
                        string[] redesArray = redes.Split(';');
                        redesArray = redesArray.Where(r => !string.IsNullOrEmpty(r)).ToArray();
                        redesList.AddRange(redesArray);
                    }
                }

                ViewBag.RedesIndex = redesList.ToArray();
            }
            else
            {
                ViewBag.RedesIndex = null;
            }

            //Mostrar nombre de usuario en el menu usuario
            if (Session["userId"] != null)
   {
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
             }else {
             ViewBag.user = "invitado";
             ViewBag.IsAdmin = false;
            }
            return View(cv);
        }

        // Ver CV
      
        public ActionResult VerCv(int id)
        {

            var cvQuery = db.Cv.Where(c => c.cv_id == id);
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
                List<string> nombresVacio = new List<string>();
                List<string> enlacesVacio = new List<string>();

                nombresVacio.Add("Sin elementos en el portafolio");
                enlacesVacio.Add("#");
                ViewBag.Nombres = nombresVacio;
                ViewBag.Enlaces = enlacesVacio;
            }

            //Listar tecnologias
            var cvTecnologiasQuery = db.Cv.Where(c => c.cv_id == id).Select(c => c.tecnología).FirstOrDefault();


            if (cvTecnologiasQuery != null)
            {
                string[] tecnologias = cvTecnologiasQuery.Split(';');

                ViewBag.Tecnologias = tecnologias;
            }
            else
            {
                string[] tecnologiasVacio = { "Añade aqui tus tecnologias" };

                ViewBag.Tecnologias = tecnologiasVacio;
            }


            //Mostrar nombre de usuario en el menu usuario
            if (Session["userId"] != null)
   {
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
                }else {
       ViewBag.user = "invitado";
                ViewBag.IsAdmin = false;
            }

            return View(cv);
        }

        // Editar CV
        // [AutorizeAttribute]
        [Autorize]
        public ActionResult EditarCv()
        {

           int userId = (int)Session["userId"];
            var cvQuery = db.Cv.Where(c => c.usuario_id == userId);
            var cv = cvQuery.ToList();

            if (!cv.Any())
            {

                var user = db.Usuarios.Find(userId);

                string filePath = Server.MapPath("~/Content/archivos/cvejemplo.pdf");
                byte[] documentoEjemplo = null;
                if (System.IO.File.Exists(filePath))
                {
                    using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        using (var reader = new System.IO.BinaryReader(fileStream))
                        {
                            documentoEjemplo = reader.ReadBytes((int)fileStream.Length);
                        }
                    }
                }

                Cv nuevoCv = new Cv
                {
                    nombre_completo = user.nombre,
                    visible = false,
                    categoria_id = 2,
                    usuario_id = user.usuario_id,
                    profesión = "Sin asignar",
                    redes_sociales = "#;#;#;#;",
                    file_cv = documentoEjemplo
                };

                db.Cv.Add(nuevoCv);
                db.SaveChanges();

                cv = new List<Cv> { nuevoCv };
            }

            //Listar portafolio
            var cvPortafolioQuery = db.Cv.Where(c => c.usuario_id == userId).Select(c => c.Portafolio).FirstOrDefault();

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
                List<string> nombresVacio = new List<string>();
                List<string> enlacesVacio = new List<string>();

                nombresVacio.Add("Añade aqui tu portafolio");
                enlacesVacio.Add("#");
                ViewBag.Nombres = nombresVacio;
                ViewBag.Enlaces = enlacesVacio;
            }

            //Listar tecnologias
            var cvTecnologiasQuery = db.Cv.Where(c => c.usuario_id == userId).Select(c => c.tecnología).FirstOrDefault();

            if (cvTecnologiasQuery != null)
            {
                string[] tecnologias = cvTecnologiasQuery.Split(';');

                ViewBag.Tecnologias = tecnologias;
            }
            else
            {
                string[] tecnologiasVacio = { "Añade aqui tus tecnologias" };

                ViewBag.Tecnologias = tecnologiasVacio;
            }

            //Listar redes
            var cvRedesQuery = db.Cv.Where(c => c.usuario_id == userId).Select(c => c.redes_sociales).FirstOrDefault();

            if (cvRedesQuery != null)
            {
                string[] redes = cvRedesQuery.Split(';');

                ViewBag.RedesPersonal = redes;
            }
            else
            {
                ViewBag.RedesPersonal = null;
            }

            //Mostrar nombre de usuario en el menu usuario
            var idUser = db.Usuarios.Find(userId);
            ViewBag.user = idUser.nombre;

            //Verificar si es un admin
            if (idUser.rol == "admin")
            {
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }

            return View(cv);
        }

        //Mostrar archivo
        //[AutorizeAttribute]
        public ActionResult VerArchivo(int id)
        {
            var cvArchivo = db.Cv.Find(id);
            if (cvArchivo == null)
            {
                return HttpNotFound();
            }

            return File(cvArchivo.file_cv, "application/pdf");
        }


        //Crear un elemento del portafolio: get
        // [AutorizeAttribute]
        [Autorize]
        public ActionResult CrearElementoPortafolio()
        {
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

            return View();
        }
        //Crear un elemento del portafolio: put
        [HttpPost]
       // [AutorizeAttribute]
        public ActionResult CrearElementoPortafolio(int id, FormCollection form)
        {
            string titulo = form["titulo"];
            string enlace = form["enlace"];

            var existingCv = db.Cv.Find(id);
            if (existingCv != null)
            {

                if ((titulo == "") || (enlace == ""))
                {
                    TempData["Message"] = "No puede haber campos vacios";
                    return RedirectToAction("CrearElementoPortafolio");
                }

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

        //Eliminar un elemento del portafolio
       // [AutorizeAttribute]
        public ActionResult EliminarPortafolio(int id, int ideliminar)
        {

            int userId = (int)Session["userId"];
            var cvQuery = db.Cv.Where(c => c.usuario_id == userId);
            var cv = cvQuery.ToList();

            //Listar portafolio
            var cvPortafolioQuery = db.Cv.Where(c => c.usuario_id == userId).Select(c => c.Portafolio).FirstOrDefault();

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

                List<string> elementosCombinados = new List<string>();

                for (int i = 0; i < nombres.Count; i++)
                {
                    if (i != id) {
                    elementosCombinados.Add($"{nombres[i]};{enlaces[i]}");
                }
                }

                // Volver a juntar todo en una sola cadena con el separador '|'
                string resultado = String.Join("|", elementosCombinados);


                var existingCv = db.Cv.Find(ideliminar);
                existingCv.Portafolio = resultado;
                if (ModelState.IsValid)
                {
                    db.Entry(existingCv).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("EditarCv");
                }
            }

            return RedirectToAction("EditarCv");
        }

        //Editar un elemento del portafolio: get
        //  [AutorizeAttribute]
        [Autorize]
        public ActionResult EditarElementoPortafolio(string nombre, string enlace, int ideditar)
        {

            ViewBag.NombrePortafolioEditar = nombre;
            ViewBag.EnlacePortafolioEditar = enlace;
            ViewBag.Ideditar = ideditar;

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

            return View();
        }
        //Editar un elemento del portafolio: put
        [HttpPost]
        public ActionResult EditarElementoPortafolio(int id, string nombre, string enlace, int ideditar)
        {

            if ((nombre == "") || (enlace == ""))
            {
                TempData["Message"] = "No puede haber campos vacios";
                return RedirectToAction("EditarElementoPortafolio", new { nombre = nombre, enlace = enlace, ideditar = ideditar });
            }

            int userId = (int)Session["userId"];
            var cvQuery = db.Cv.Where(c => c.usuario_id == userId);
            var cv = cvQuery.ToList();

            //Listar portafolio
            var cvPortafolioQuery = db.Cv.Where(c => c.usuario_id == userId).Select(c => c.Portafolio).FirstOrDefault();

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

                List<string> elementosCombinados = new List<string>();

                for (int i = 0; i < nombres.Count; i++)
                {
                    if (i != id)
                    {
                        elementosCombinados.Add($"{nombres[i]};{enlaces[i]}");
                    } else
                    {
                        elementosCombinados.Add($"{nombre};{enlace}");
                    }
                }

                // Volver a juntar todo en una sola cadena con el separador '|'
                string resultado = String.Join("|", elementosCombinados);


                var existingCv = db.Cv.Find(ideditar);
                existingCv.Portafolio = resultado;
                if (ModelState.IsValid)
                {
                    db.Entry(existingCv).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("EditarCv");
                }
            }

            return View("EditarElementoPortafolio");
        }

        // Editar cv2
        //[AutorizeAttribute]
        [Autorize]
        public ActionResult EditarCV2(int id)
        {
            Cv cv = db.Cv.Find(id);

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

        // Editar cv2
        [HttpPost]
        [ValidateAntiForgeryToken]
       // [AutorizeAttribute]
        public ActionResult EditarCV2(Cv cv, HttpPostedFileBase file)
        {

            int userId = (int)Session["userId"];
            cv.categoria_id = 2;
            cv.usuario_id = userId;


                var existingCv = db.Cv.Find(cv.cv_id);
                if (existingCv != null)
                {
                existingCv.visible = cv.visible;

                if (cv.profesión != null)
                {
                    existingCv.profesión = cv.profesión;
                }
                else
                {
                    TempData["Message"] = "La profesion no puede estar vacia";
                    return RedirectToAction("EditarCV2");
                }

                if (file != null && file.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        existingCv.file_cv = reader.ReadBytes(file.ContentLength);
                    }
                }

            }

                if (ModelState.IsValid)
                {
                    db.Entry(existingCv).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("EditarCv");
                }

            return View(cv);
        }

        //Eliminar un elemento de tecnologia
       // [AutorizeAttribute]
        public ActionResult EliminarTecnologia(int id, int ideliminarT)
        {

            int userId = (int)Session["userId"];

            //Listar tecnologias                      /*cambiar por id del usuario correcto*/
            var cvTecnologiasQuery = db.Cv.Where(c => c.usuario_id == userId).Select(c => c.tecnología).FirstOrDefault();

            string[] tecnologias = cvTecnologiasQuery.Split(';');

            List<string> elementosCombinados = new List<string>();

                for (int i = 0; i < tecnologias.Length; i++)
                {
                    if (i != id)
                    {
                        elementosCombinados.Add($"{tecnologias[i]}");
                    }
                }

                string resultado = String.Join(";", elementosCombinados);


                var existingCv = db.Cv.Find(ideliminarT);
                existingCv.tecnología = resultado;
                if (ModelState.IsValid)
                {
                    db.Entry(existingCv).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("EditarCv");
                }
            return RedirectToAction("EditarCv");
        }

        //Crear un elemento de tecnologia: get
        //  [AutorizeAttribute]
        [Autorize]
        public ActionResult CrearElementoTecnologia()
        {

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

            return View();
        }
        //Crear un elemento de tecnologia: put
        [HttpPost]
      //  [AutorizeAttribute]
        public ActionResult CrearElementoTecnologia(int id, FormCollection form)
        {
            string nombre = form["nombre"];

            if (nombre == "")
            {
                TempData["Message"] = "No puede haber campos vacios";
                return RedirectToAction("CrearElementoTecnologia");
            }

            var existingCv = db.Cv.Find(id);
            if (existingCv != null)
            {
                existingCv.tecnología = existingCv.tecnología + nombre + ";";
            }

            if (ModelState.IsValid)
            {
                db.Entry(existingCv).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EditarCv");
            }


            return View();
        }

        //Eliminar un elemento de redes sociales
        public ActionResult EliminarRed(int id, int ideliminarR)
        {

            int userId = (int)Session["userId"];

            var cvRedesQuery = db.Cv.Where(c => c.usuario_id == userId).Select(c => c.redes_sociales).FirstOrDefault();

            string[] redes = cvRedesQuery.Split(';');

            List<string> elementosCombinados = new List<string>();

            for (int i = 0; i < redes.Length; i++)
            {
                if (i != id)
                {
                    elementosCombinados.Add($"{redes[i]}");
                } else
                {
                    elementosCombinados.Add($"#");
                }
            }

            string resultado = String.Join(";", elementosCombinados);


            var existingCv = db.Cv.Find(ideliminarR);
            existingCv.redes_sociales = resultado;
            if (ModelState.IsValid)
            {
                db.Entry(existingCv).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EditarCv");
            }
            return RedirectToAction("EditarCv");
        }

        //Editar un elemento de rede social: get
        //  [AutorizeAttribute]
        [Autorize]
        public ActionResult EditarRed(string enlace, int ideditarR)
        {
            if (enlace == "#")
            {
                ViewBag.EnlaceRPortafolioEditar = "";
            } else
            {
                ViewBag.EnlaceRPortafolioEditar = enlace;
            }
            ViewBag.IdeditarR = ideditarR;

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

            return View();
        }
        //Editar un elemento de red social: put
        [HttpPost]
      //  [AutorizeAttribute]
        public ActionResult EditarRed(int id, string enlace, int ideditarR)
        {

            if (enlace == "")
            {
                TempData["Message"] = "No puede haber campos vacios";
                return RedirectToAction("EditarRed", new { enlace = enlace, ideditarR = ideditarR });
            }

            int userId = (int)Session["userId"];
            var cvQuery = db.Cv.Where(c => c.usuario_id == userId);
            var cv = cvQuery.ToList();

            //Listar portafolio
            var cvRedQuery = db.Cv.Where(c => c.usuario_id == userId).Select(c => c.redes_sociales).FirstOrDefault();

            if (cvRedQuery != null)
            {
                string[] redes = cvRedQuery.Split(';');

                List<string> elementosCombinados = new List<string>();

                for (int i = 0; i < redes.Length; i++)
                {
                    if (i != id)
                    {
                        elementosCombinados.Add($"{redes[i]}");
                    }
                    else
                    {
                        elementosCombinados.Add($"{enlace}");
                    }
                }

                // Volver a juntar todo en una sola cadena con el separador '|'
                string resultado = String.Join(";", elementosCombinados);


                var existingCv = db.Cv.Find(ideditarR);
                existingCv.redes_sociales = resultado;
                if (ModelState.IsValid)
                {
                    db.Entry(existingCv).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("EditarCv");
                }
            }

            return View("EditarRed");
        }
     
        public ActionResult GetIcono(int id)
        {
            var usuario = db.Usuarios.FirstOrDefault(u => u.usuario_id == id);
            if (usuario != null && usuario.foto != null)
            {
                return File(usuario.foto, "image/jpg"); 
            }
            return HttpNotFound();
        }

        //Cerrar sesion
        public ActionResult EliminarSesion()
        {
            // Eliminar la sesión
            Session["userId"] = null;

            // Redirigir a la página de usuarios
            return RedirectToAction("Index", "Usuarios");
        }

    }

}
