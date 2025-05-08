using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class ProgresoDiaRutina
    {
        public int NumeroDia { get; set; }
        public string NombreDia { get; set; }
        public int TotalEjercicios { get; set; }
        public int EjerciciosCompletados { get; set; }
        public decimal PorcentajeCompletado { get; set; }
        public DateTime? UltimaFechaRealizacion { get; set; }
        public int DiasCompletados { get; set; }
    }

}
