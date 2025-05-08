using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class MetricaCorporal
    {
        public int MetricaID { get; set; }
        public int UsuarioID { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Peso { get; set; }
        public decimal Altura { get; set; }
        public decimal? IMC { get; set; }
        public decimal? PorcentajeGrasa { get; set; }
        public decimal? CambioPeso { get; set; }
        public decimal? CambioIMC { get; set; }
    }
}
