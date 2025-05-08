using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class ResumenMorosidad
    {
        public int TotalUsuariosMorosos { get; set; }
        public double PromedioRetraso { get; set; }
        public int MaximoRetraso { get; set; }
    }
}
