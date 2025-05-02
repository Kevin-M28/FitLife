using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class ReporteGeneradoAsistencia
    {
        public int ReporteID { get; set; }
        public string NombreGimnasio { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string Periodo { get; set; }
        public string RangoFechas { get; set; }
        public int TotalAsistencias { get; set; }
        public int UsuariosUnicos { get; set; }
        public double PromedioAsistenciasDiarias { get; set; }
        public double DuracionPromedioMinutos { get; set; }
        public string DatosJSON { get; set; }
    }
}
