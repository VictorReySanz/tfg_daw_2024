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
    public class GruposController : Controller
    {
        private share_enjoyEntities db = new share_enjoyEntities();

        public ActionResult Chat()
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
    }
}
 