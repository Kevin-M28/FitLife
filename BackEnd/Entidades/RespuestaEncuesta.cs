using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class RespuestaEncuesta
    {
        public string Id { get; set; }
        public int idGym { get; set; }
        public string idEncuesta { get; set; }
        public string idUsuario { get; set; }
        public string respuesta { get; set; }
        public DateTime fecha { get; set; }
        public bool anonima { get; set; }
    }
}
