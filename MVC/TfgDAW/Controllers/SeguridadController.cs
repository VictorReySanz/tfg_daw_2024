using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TfgDAW.Models;

namespace TfgDAW.Controllers
{
    public class SeguridadController : Controller
    {
        // GET: Seguridad

        [AutorizeAttribute]
        public ActionResult Index()
        {
            return View();
        }
    }
}