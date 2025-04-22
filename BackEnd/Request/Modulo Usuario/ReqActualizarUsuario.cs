using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Usuario
{
    public class ReqActualizarUsuario: ReqBase
    {
        public int usuarioID { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
    }
}
