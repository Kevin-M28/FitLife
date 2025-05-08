using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Pago
{
    public class ReqInsertPago : ReqBase
    {
        public int UsuarioID { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } // 'efectivo', 'tarjeta', 'transferencia', 'pendiente'
        public string ComprobanteRuta { get; set; }
    }
}
