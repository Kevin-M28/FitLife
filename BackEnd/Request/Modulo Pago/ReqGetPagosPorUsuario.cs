using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Pago
{
    public class ReqGetPagosPorUsuario : ReqBase
    {
        public int UsuarioID { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; } // 'confirmado', 'pendiente', 'rechazado' o null para todos
    }
}
