using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Asistencia
{
        public class ReqObtenerHistorialAsistencia : ReqBase
        {
            public int UsuarioID { get; set; }
            public DateTime? FechaInicio { get; set; }
            public DateTime? FechaFin { get; set; }
            public bool SoloCompletadas { get; set; }
        }
}
