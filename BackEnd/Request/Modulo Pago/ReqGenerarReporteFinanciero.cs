using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Pago
{
    public class ReqGenerarReporteFinanciero : ReqBase
    {
        public int GimnasioID { get; set; }
        public string Periodo { get; set; } // 'diario', 'semanal', 'mensual', 'trimestral', 'anual'
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
