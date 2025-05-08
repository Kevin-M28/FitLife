using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entidades;

namespace BackEnd.Response.Modulo_Membresia
{
    public class ResVerificarMorosidad
    {
        public bool resultado { get; set; }
        public List<Error> error { get; set; }
        public List<Entidades.Morosidad> usuariosMorosos { get; set; }
        public Entidades.ResumenMorosidad resumen { get; set; }
    }
}
