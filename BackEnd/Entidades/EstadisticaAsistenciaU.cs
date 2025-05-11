using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class EstadisticasAsistenciaU
    {
        public int TotalAsistencias { get; set; }
        public DateTime? UltimaAsistencia { get; set; }
        public int AsistenciasUltimoMes { get; set; }
        public int AsistenciasUltimaSemana { get; set; }
        public double? DuracionPromedioMinutos { get; set; }  // Cambiado de double? para evitar problemas con decimal
        public string DiaMasFrecuente { get; set; }
    }
}
