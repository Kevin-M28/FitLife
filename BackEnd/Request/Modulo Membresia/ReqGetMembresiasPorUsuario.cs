using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Membresia
{
    public class ReqGetMembresiasPorUsuario :ReqBase
    {
        public int UsuarioID { get; set; }
        public bool SoloActivas { get; set; } = true;
    }
}
