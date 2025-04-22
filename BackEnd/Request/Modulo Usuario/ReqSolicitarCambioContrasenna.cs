using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Usuario
{
    public class ReqSolicitarCambioContrasenna : ReqBase
    {
        public string Email { get; set; }
    }
}
