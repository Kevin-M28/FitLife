using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Asistencia
{
    public class ReqReporteAsistencia
    {
        public int GimnasioID { get; set; }
        public string Periodo { get; set; } // diario, semanal, mensual
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
