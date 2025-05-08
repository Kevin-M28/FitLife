using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Membresia
{
    public class ReqActualizarMembresia : ReqBase
    {
        public int UsuarioMembresiaID { get; set; }
        public string NuevoEstado { get; set; } // 'activa', 'vencida', 'cancelada'
        public int AdminID { get; set; }
        public string Motivo { get; set; }
    }
}
