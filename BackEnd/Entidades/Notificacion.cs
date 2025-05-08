using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Notificacion
    {
        public int NotificacionID { get; set; }
        public string Tipo { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaEnvio { get; set; }
        public bool EstadoLeido { get; set; }
        public int MinutosTranscurridos { get; set; }
        public string TiempoTranscurrido { get; set; }
    }
}
