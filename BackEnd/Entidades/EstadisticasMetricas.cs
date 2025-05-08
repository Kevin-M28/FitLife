using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class EstadisticasMetricas
    {
        public int TotalRegistros { get; set; }
        public decimal PromediosPeso { get; set; }
        public decimal PesoMinimo { get; set; }
        public decimal PesoMaximo { get; set; }
        public decimal PromedioIMC { get; set; }
        public decimal? PromedioGrasa { get; set; }
        public decimal RangoPeso { get; set; }
    }
}
