using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class ResumenNotificaciones
    {
        public int NotificacionesNoLeidas { get; set; }
        public int NotificacionesPago { get; set; }
        public int NotificacionesLogro { get; set; }
        public int NotificacionesAnuncio { get; set; }
    }
}
