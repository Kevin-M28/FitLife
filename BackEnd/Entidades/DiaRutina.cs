using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class DiaRutina
    {
        public int DiaRutinaID { get; set; }
        public int RutinaID { get; set; }
        public int NumeroDia { get; set; }
        public string NombreDia { get; set; }
    }
}
