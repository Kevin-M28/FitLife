using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Membresia
{
    public class ReqGetMembresiasPorGimnasio : ReqBase
    {
        public int GimnasioID { get; set; }
        public string Estado { get; set; } // 'activa', 'vencida', 'cancelada' o null para todas
        public DateTime? FechaVencimientoDesde { get; set; }
        public DateTime? FechaVencimientoHasta { get; set; }
    }
}
