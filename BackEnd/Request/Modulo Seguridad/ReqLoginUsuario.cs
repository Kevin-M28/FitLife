using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackEnd.Entidades;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Usuario
{
    public class ReqLoginUsuario
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
