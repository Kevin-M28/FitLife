using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request
{
    public class ReqAsignarMembresia
    {
        public string IdMembresiaUsuario { get; set; }
        public int IdGym { get; set; }
        public string IdUsuario { get; set; }
        public string IdMembresia { get; set; }
        public DateTime? FechaInicio { get; set; }
    }
}
