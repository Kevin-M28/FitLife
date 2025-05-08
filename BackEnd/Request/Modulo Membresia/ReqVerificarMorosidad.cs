using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Request.Modulo_Membresia
{
    public class ReqVerificarMorosidad : ReqBase
    {
        public int? GimnasioID { get; set; }
        public int DiasGracia { get; set; } = 5;
    }
}
