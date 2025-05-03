using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entidades;

namespace BackEnd.Response.Modulo_Asistencia
{
    public class ResEstadisticasAsistencia : ResBase
    {
        public EstadisticasAsistencia Estadisticas { get; set; }

        public ResEstadisticasAsistencia()
        {
            Estadisticas = new EstadisticasAsistencia();
        }
    }
}
