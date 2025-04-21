using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Notificacion
    {
        public string Id { get; set; }
        public int idGym { get; set; }
        public string idUsuario { get; set; }
        public string mensaje { get; set; }
        public DateTime fecha { get; set; }
        public string estado { get; set; }
    }
}
