using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entidades;

namespace BackEnd.Response.Modulo_Membresia
{
    public class ResActualizarMembresia
    {
        public bool resultado { get; set; }
        public List<Error> error { get; set; }
        public Entidades.UsuarioMembresia membresia { get; set; }
    }
}
