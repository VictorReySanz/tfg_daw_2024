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
            var cvRedesQuery = db.Cv.Where(l => l.visible == true).Select(c => c.redes_sociales).FirstOrDefault();

            if (cvRedesQuery != null)
            {
                string[] redes = cvRedesQuery.Split(';');

                ViewBag.Redes = redes;
            }
            else
            {
                ViewBag.Redes = null;
            }

            return View(cv);
        }

        // Ver CV
        public ActionResult VerCv(int id)
        {

            int userId = (int)Session["userId"];
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

            //Listar tecnologias                      /*cambiar por id del usuario correcto*/
            var cvTecnologiasQuery = db.Cv.Where(c => c.usuario_id == userId).Select(c => c.tecnología).FirstOrDefault();

            if (cvTecnologiasQuery != null)
            {
                string[] tecnologias = cvTecnologiasQuery.Split(';');

                ViewBag.Tecnologias = tecnologias;
            }
            else
            {
                string[] tecnologiasVacio = { "No se han añadido aptitudes" };

                ViewBag.Tecnologias = tecnologiasVacio;
            }

            return View(cv);
        }

        // Editar CV
        public ActionResult EditarCv()
        {

           int userId = (int)Session["userId"];
            var cvQuery = db.Cv.Where(c => c.usuario_id == userId);/*cambiar por id del usuario correcto*/
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
                    file_cv = documentoEjemplo
                };

                db.Cv.Add(nuevoCv);
                db.SaveChanges();

                // Actualizar la variable cv después de agregar el nuevo CV
                cv = new List<Cv> { nuevoCv };
            }

            //Listar portafolio                      /*cambiar por id del usuario correcto*/
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

            //Listar tecnologias                      /*cambiar por id del usuario correcto*/
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

            return View(cv);
        }

        //Mostrar archivo
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
        public ActionResult CrearElementoPortafolio()
        {
            return View();
        }
        //Crear un elemento del portafolio: put
        [HttpPost]
        public ActionResult CrearElementoPortafolio(int id, FormCollection form)
        {
            string titulo = form["titulo"];
            string enlace = form["enlace"];

            var existingCv = db.Cv.Find(id);
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

        //Eliminar un elemento del portafolio
        public ActionResult EliminarPortafolio(int id, int ideliminar)
        {

            int userId = (int)Session["userId"];
            var cvQuery = db.Cv.Where(c => c.usuario_id == userId);/*cambiar por id del usuario correcto*/
            var cv = cvQuery.ToList();

            //Listar portafolio                      /*cambiar por id del usuario correcto*/
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
        public ActionResult EditarElementoPortafolio(string nombre, string enlace)
        {

            ViewBag.NombrePortafolioEditar = nombre;
            ViewBag.EnlacePortafolioEditar = enlace;
            return View();
        }
        //Editar un elemento del portafolio: put
        [HttpPost]
        public ActionResult EditarElementoPortafolio(int id, string nombre, string enlace, int ideditar)
        {

            int userId = (int)Session["userId"];
            var cvQuery = db.Cv.Where(c => c.usuario_id == userId);/*cambiar por id del usuario correcto*/
            var cv = cvQuery.ToList();

            //Listar portafolio                      /*cambiar por id del usuario correcto*/
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


                var existingCv = db.Cv.Find(id);
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
        public ActionResult EditarCV2(int id)
        {
            Cv cv = db.Cv.Find(id);

            return View(cv);
        }

        // Editar cv2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCV2(Cv cv, HttpPostedFileBase file)
        {

            int userId = (int)Session["userId"];
            cv.categoria_id = 2;
            cv.usuario_id = userId;


                var existingCv = db.Cv.Find(cv.cv_id);
                if (existingCv != null)
                {
                existingCv.visible = cv.visible;
                existingCv.profesión = cv.profesión;

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

        //Crear un elemento del portafolio: get
        public ActionResult CrearElementoTecnologia()
        {
            return View();
        }
        //Crear un elemento del portafolio: put
        [HttpPost]
        public ActionResult CrearElementoTecnologia(int id, FormCollection form)
        {
            string nombre = form["nombre"];

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
