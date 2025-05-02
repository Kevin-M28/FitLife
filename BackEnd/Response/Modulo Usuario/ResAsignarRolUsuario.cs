using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entidades;

namespace BackEnd.Response.Modulo_Usuario
{
    public class ResAsignarRolUsuario : ResBase
    {
        public string Mensaje { get; set; }
        public Usuario usuario { get; set; }
    }
}
