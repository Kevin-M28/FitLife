using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class RutinaEjercicio
    {
        public string idRutina { get; set; }
        public string idEjercicio { get; set; }
        public int idGym { get; set; }
        public int series { get; set; }
        public int repeticiones { get; set; }
        public int tiempoDescanso { get; set; }
    }
}
