using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class ActividadSemanal
    {
        public int Año { get; set; }
        public int NumeroSemana { get; set; }
        public DateTime InicioSemana { get; set; }
        public DateTime FinSemana { get; set; }
        public int DiasActivos { get; set; }
        public int EjerciciosCompletados { get; set; }
        public int DiasInactivos { get; set; }
    }
}
