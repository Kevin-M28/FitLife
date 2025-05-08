using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class EjercicioRutina
    {
        public int EjercicioDiaRutinaID { get; set; }
        public int RutinaID { get; set; }
        public string NombreRutina { get; set; }
        public int NumeroDia { get; set; }
        public string NombreDia { get; set; }
        public int EjercicioID { get; set; }
        public string NombreEjercicio { get; set; }
        public string DescripcionEjercicio { get; set; }
        public string MusculoObjetivo { get; set; }
        public int Series { get; set; }
        public int Repeticiones { get; set; }
        public int DescansoSegundos { get; set; }
    }
}
