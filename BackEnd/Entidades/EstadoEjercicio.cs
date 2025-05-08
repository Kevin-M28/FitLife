using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class EstadoEjercicio
    {
        public string NombreEjercicio { get; set; }
        public string MusculoObjetivo { get; set; }
        public string NombreDia { get; set; }
        public int Series { get; set; }
        public int Repeticiones { get; set; }
        public DateTime? UltimaRealizacion { get; set; }
        public int VecesCompletado { get; set; }
        public string Estado { get; set; }
    }
}
