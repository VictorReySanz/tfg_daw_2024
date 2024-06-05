using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        //chats
        [Autorize]
        public ActionResult Chat(int groupId)
        {
            int userId = (int)Session["userId"];
            var user = db.Usuarios.Include(u => u.Grupos).SingleOrDefault(u => u.usuario_id == userId);
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
                ViewBag.Members = new List<string>();
            }
            else
            {
                ViewBag.NoChat = false;
                var group = db.Grupos.Include(g => g.Usuarios).SingleOrDefault(g => g.grupo_id == groupId);
                if (group == null)
                {
                    return HttpNotFound();
                }
                ViewBag.GroupName = group.nombre_grupo;
                ViewBag.GroupId = groupId;
                ViewBag.Members = group.Usuarios.Select(u => u.nombre).ToList();
            }

            ViewBag.Groups = user.Grupos.ToList();

            return View();
        }

        //crear grupo get
        [HttpGet]
        [Autorize]
        public ActionResult CrearGrupo()
        {
            int userId = (int)Session["userId"];
            var usuarioActual = db.Usuarios.Include(u => u.Grupos).SingleOrDefault(u => u.usuario_id == userId);

            if (usuarioActual.rol == "admin")
            {
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }

            var usuarios = db.Usuarios.Where(u => u.usuario_id != userId).ToList();

            var currentUser = db.Usuarios.Find(userId);
            ViewBag.CurrentUser = currentUser;
            return View(usuarios);
        }

        //crear grupo post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearGrupo(string nombreGrupo, int[] usuarios)
        {
            int userId = (int)Session["userId"];
            if (ModelState.IsValid)
            {
                var nuevoGrupo = new Grupos
                {
                    nombre_grupo = nombreGrupo,
                    Usuarios = new List<Usuarios>()
                };

                var currentUser = db.Usuarios.Find(userId);
                if (currentUser != null)
                {
                    nuevoGrupo.Usuarios.Add(currentUser);
                }

                if (usuarios != null)
                {
                    foreach (var user in usuarios)
                    {
                        var usuario = db.Usuarios.Find(user);
                        if (usuario != null)
                        {
                            nuevoGrupo.Usuarios.Add(usuario);
                        }
                    }
                }

                db.Grupos.Add(nuevoGrupo);
                db.SaveChanges();

                return RedirectToAction("Chat", "Grupos", new { groupId = 0 });
            }

            var allUsuarios = db.Usuarios.Where(u => u.usuario_id != userId).ToList();
            return View(allUsuarios);
        }

        //Salirse del grupo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarUsuario(int groupId)
        {
            int userId = (int)Session["userId"]; // Obtener el ID del usuario actual de la sesión
            var grupo = db.Grupos.Include(g => g.Usuarios).FirstOrDefault(g => g.grupo_id == groupId);
            var usuario = grupo.Usuarios.FirstOrDefault(u => u.usuario_id == userId);

            if (usuario != null)
            {
                grupo.Usuarios.Remove(usuario);
                db.SaveChanges();
            }

            return RedirectToAction("Chat", "Grupos", new { groupId = 0 });
        }

        //Cerrar sesion
        //  [AutorizeAttribute]
        public ActionResult EliminarSesion()
        {
            // Eliminar la sesión
            Session["userId"] = null;

            // Redirigir a la página de usuarios
            return RedirectToAction("Index", "Usuarios");
        }



    }
}
 