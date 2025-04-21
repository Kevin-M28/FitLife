using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class RegistroSeguridad
    {
        public int Id { get; set; }
        public int? idGym { get; set; }
        public string idUsuario { get; set; }
        public string tipoEvento { get; set; }
        public string descripcion { get; set; }
        public DateTime fecha { get; set; }
        public string direccionIP { get; set; }
        public string resultado { get; set; }
        public string dispositivo { get; set; } // Nuevo campo que reemplaza activity_log
    }
}
