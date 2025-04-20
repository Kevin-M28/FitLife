using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request
{
    public class ReqInsertarMembresia
    {
        public string IdMembresia { get; set; }
        public int IdGym { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int DuracionDias { get; set; }
    }
}
