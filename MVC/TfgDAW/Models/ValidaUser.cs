using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TfgDAW.Models
{
    public class ValidaUser
    {
        public String Role { get; set; }
        private share_enjoyEntities db = new share_enjoyEntities();
        public bool ExisteUsuario(String usuario, String password)
        {

            List<Usuarios> userObj = db.Usuarios.Where(u => u.nombre == usuario).ToList();
            if (userObj.Count == 0)
            {
                return false;
            }
            else
            {
                this.Role = "usuario";
                return true;

            }
        }
    }
}
