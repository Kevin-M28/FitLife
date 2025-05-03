using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Asistencia
{
    public class ReqEstadisticasAsistencia : ReqBase
    {
        public int GimnasioID { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string TipoEstadistica { get; set; } = "todo"; // todo, general, frecuencia, pico, duracion
    }
}
