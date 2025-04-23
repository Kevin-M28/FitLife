using System;
using BackEnd.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Usuario
{
    public class ReqVerificarUsuario
    {
        public string correo { get; set; }
        public string codigo { get; set; }
    }
}
