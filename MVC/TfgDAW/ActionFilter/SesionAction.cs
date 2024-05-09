using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TfgDAW.Models;

namespace TfgDAW.ActionFilter
{
    public class SesionAction : ActionFilterAttribute
    {
        
        private share_enjoyEntities db = new share_enjoyEntities();

        public bool Disable { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Disable) return;
            UpdateUserSession(filterContext);
        }

        private void UpdateUserSession(ActionExecutingContext filterContext)
        {
            var usuario = filterContext.HttpContext.User.Identity.Name.ToLower();
            filterContext.HttpContext.Session.Clear();
            filterContext.HttpContext.Session.Add("UserCorreo", usuario);
            List<Usuarios> userObj = db.Usuarios.Where(u => u.email == usuario).ToList();
            if (userObj.Count == 0)
            {
                filterContext.Result = new RedirectResult("~/Home/Forbidden");
            }
            else
            {
                Usuarios u = userObj.First();
                filterContext.HttpContext.Session.Add("nombre", u.nombre);
                filterContext.HttpContext.Session.Add("foto", u.foto);
                filterContext.HttpContext.Session.Add("email", u.email);
                filterContext.HttpContext.Session.Add("rol", u.rol_id);      
                //u.Fecha = DateTime.Now;
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}