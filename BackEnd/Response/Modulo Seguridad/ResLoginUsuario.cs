using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Entidades;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Response.Modulo_Usuario
{
    public class ResLoginUsuario: ResBase
    {
        public string Token { get; set; }
    }
}
