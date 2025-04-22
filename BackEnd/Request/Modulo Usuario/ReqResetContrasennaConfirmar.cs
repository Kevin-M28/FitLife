using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Usuario
{
    public class ReqResetContrasennaConfirmar : ReqBase
    {
        public int UsuarioID { get; set; }
        public string ResetToken { get; set; }
        public string NuevaPassword { get; set; }
    }
}
