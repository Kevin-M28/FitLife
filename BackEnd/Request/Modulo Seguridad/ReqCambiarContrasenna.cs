using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Usuario
{
    public class ReqCambiarContrasenna : ReqBase
    {
        public string passwordActual { get; set; }
        public string nuevaPassword { get; set; }
    }
}
