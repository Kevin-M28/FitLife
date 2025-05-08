using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class EstadisticasNotificaciones
    {
        public int TotalNotificaciones { get; set; }
        public int NoLeidas { get; set; }
        public int Leidas { get; set; }
        public int NotificacionesPago { get; set; }
        public int NotificacionesLogro { get; set; }
        public int NotificacionesAnuncio { get; set; }
        public int NotificacionesUltimaSemana { get; set; }
    }
}
