using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Asistencia
{
    public class ReqRegistrarAsistencia : ReqBase
    {
        public int UsuarioID { get; set; }
        public string MetodoRegistro { get; set; } // QR, manual, etc.
        public int AdminID { get; set; }           // Usuario que registra la asistencia
        public DateTime FechaHoraEntrada { get; set; }
    }
}
