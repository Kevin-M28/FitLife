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
        public string NombreCompleto { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public string MetodoPago { get; set; } // 'efectivo', 'tarjeta', 'transferencia', 'pendiente'
        public string ComprobanteRuta { get; set; }
        public string Estado { get; set; } // 'confirmado', 'pendiente', 'rechazado'
        public string EstadoDescripcion { get; set; }
        public string MembresiaAsociada { get; set; }
    }
}
