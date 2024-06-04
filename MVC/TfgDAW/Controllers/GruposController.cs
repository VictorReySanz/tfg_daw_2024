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

        public ActionResult Chat(int groupId)
        {
            int userId = (int)Session["userId"];
            var user = db.Usuarios.Find(userId);
            ViewBag.user = user.nombre;

            if (user.rol == "admin")
            {
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }

            if (groupId == 0)
            {
                ViewBag.NoChat = true;
            }
            else
            {

                ViewBag.NoChat = false;

                var group = db.Grupos.Find(groupId);
                if (group == null)
                {
                    return HttpNotFound();
                }
                ViewBag.GroupName = group.nombre_grupo;

                ViewBag.GroupId = groupId;
            }
            ViewBag.Groups = db.Grupos.ToList();

            return View();
        }

    }
}
 