//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TfgDAW.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cv
    {
        public int cv_id { get; set; }
        public string nombre_completo { get; set; }
        public Nullable<bool> visible { get; set; }
        public Nullable<int> categoria_id { get; set; }
        public Nullable<int> usuario_id { get; set; }
    
        public virtual Categorias Categorias { get; set; }
        public virtual Usuarios Usuarios { get; set; }
    }
}
