using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TfgDAW.Models;

namespace TfgDAW.Controllers
{
    public class ValidacionController : Controller
    {
        // GET: Validacion
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()

        {
            return View();

        }
        [HttpPost]
        public ActionResult Login(String usuario, String password)
        {

            ValidaUser v = new ValidaUser();

            if (v.ExisteUsuario(usuario, password))

            {
                //EL USUARIO EXISTE Y NOS CREAMOS UN TICKET 
                //PARA LA AUTORIZACION 
                FormsAuthenticationTicket ticket =
                new FormsAuthenticationTicket(1, usuario
                , DateTime.Now, DateTime.Now.AddMinutes(3)
                , true, v.Role, FormsAuthentication.FormsCookiePath);
                string datoscifrados = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie("cookieseguridad", datoscifrados);

                this.Response.Cookies.Add(cookie);

                //LE PERMITIMOS EL ACCESO A LA ZONA SEGURA 

                return RedirectToAction("Index", "Seguridad");

            }
            else
            {
                //EL USUARIO O PASSWORD SON INCORRECTOS 
                ViewBag.Mensaje = "Usuario/Password incorrectos";
                return View();
            }

        }
        public ActionResult ErrorAcceso()
        {

            return View();

        }
    }
}