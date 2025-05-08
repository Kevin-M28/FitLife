using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Pago
{
    public class ReqUpdateEstadoPago :ReqBase
    {
        public int PagoID { get; set; }
        public string NuevoEstado { get; set; } // 'confirmado', 'rechazado'
        public int AdminID { get; set; }
        public string Comentario { get; set; }
    }
}
