using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TfgDAW.Models
{
    public class AutorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //PREGUNTAMOS SI EL USUARIO YA SE HA VALIDADO 
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                //RECUPERAMOS EL USUARIO QUE HEMOS ALMACENADO 
                //EN EL GLOBAL.ASAX 

                System.Security.Principal.GenericPrincipal usuario = (System.Security.Principal.GenericPrincipal)
                 HttpContext.Current.User;

                //PREGUNTAMOS POR EL ROLE DEL USUARIO 

                if (!usuario.IsInRole("usuario"))

                {
                    //SI EL USUARIO NO ESTÁ EN NUESTRO ROLE 
                    //LO LLEVAMOS A LA VISTA DE ERROR DE ACCESO 
                    filterContext.Result = new RedirectToRouteResult(new

                    RouteValueDictionary(new { controller = "Validacion", action = "ErrorAcceso" }));
                }
            }
            else
            {
                //EL USUARIO NO SE HA VALIDADO TODAVIA 
                //Y LLAMAMOS A LA PAGINA DE LOGIN DE user
               
                filterContext.Result = new RedirectToRouteResult(new
                  RouteValueDictionary(new { controller = "Usuarios", action = "Index" }));

            }

        }
    }
}