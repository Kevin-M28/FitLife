using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Reporte
    {
        public string Id { get; set; }
        public int idGym { get; set; }
        public string tipo { get; set; } // MEMBERSHIP_STATS, CLASS_ATTENDANCE, REVENUE, USER_PROGRESS, EQUIPMENT_USAGE
        public DateTime fechaGeneracion { get; set; }
        public string detalles { get; set; } // Criterios o parámetros usados para generar el reporte
        public string idUsuario { get; set; } // Usuario que generó el reporte
    }
}
