using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Request;

namespace BackEnd.Request.Modulo_Membresia
{
    public class ReqInsertUsuarioMembresia : ReqBase
    {
        public int UsuarioID { get; set; }
        public int MembresiaID { get; set; }
        public DateTime? FechaInicio { get; set; }
        public int? AdminID { get; set; }
    }
}
