using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace TfgDAW.Models
{
    public class ValidaUser
    {
        public String Role { get; set; }
        private share_enjoyEntities db = new share_enjoyEntities();
        public bool ExisteUsuario(String usuario, String password)
        {    
            Usuarios userObj = db.Usuarios.FirstOrDefault(u => u.email == usuario);

            if (userObj == null)
            {
                return false;
            }
            else
            {  
                if (userObj.rol == "admin")

                {

                    this.Role = "admin";
                    return true;

                }
                else if (userObj.rol == "usuario")

                {
                    this.Role = "usuario";
                    return true;
                }
              
            }
            return false;
        }
    }
}
