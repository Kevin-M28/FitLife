using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class NotificacionMasiva
    {
        public int NotificacionMasivaID { get; set; }
        public int GimnasioID { get; set; }
        public string NombreGimnasio { get; set; }
        public string Tipo { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaEnvio { get; set; }
        public int UsuariosNotificados { get; set; }
    }
}
