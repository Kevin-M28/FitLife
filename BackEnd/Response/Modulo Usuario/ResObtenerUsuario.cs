using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackEnd.Entidades;
using BackEnd.Entidades;
using System.Threading.Tasks;

namespace BackEnd.Response.Modulo_Usuario
{
    public class ResObtenerUsuario : ResBase
    {
        public Usuario usuario { get; set; }
    }
}
