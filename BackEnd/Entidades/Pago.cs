using BackEnd.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Pago
    {
        public int PagoID { get; set; }
        public int UsuarioID { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public EnumMetodoPago MetodoPago { get; set; }
        public string ComprobanteRuta { get; set; }
        public EnumEstadoPago Estado { get; set; }
        public string ConceptoPago { get; set; }
    }
}
