using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entidades;

namespace BackEnd.Entidades
{
    public class Sesion
    {
        public string idSesion { get; set; }
        public int idGym { get; set; }
        public string token { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaExpiracion { get; set; }
        public string direccionIP { get; set; }
        public bool activa { get; set; }
        public DateTime ultimoAcceso { get; set; }
        public Usuario usuario { get; set; }
    }
}
