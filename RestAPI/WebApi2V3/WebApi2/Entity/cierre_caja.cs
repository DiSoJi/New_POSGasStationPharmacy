//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class cierre_caja
    {
        public int id { get; set; }
        public string idapertura { get; set; }
        public Nullable<int> ventaefectivo { get; set; }
        public Nullable<int> ventatarjeta { get; set; }
        public Nullable<int> dinerototal { get; set; }
        public int activo { get; set; }
    
        public virtual apertura_caja apertura_caja { get; set; }
    }
}
