using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class ResumenMetricas
    {
        public decimal PesoInicial { get; set; }
        public decimal IMCInicial { get; set; }
        public decimal? GrasaInicial { get; set; }
        public DateTime FechaInicial { get; set; }
        public decimal PesoActual { get; set; }
        public decimal IMCActual { get; set; }
        public decimal? GrasaActual { get; set; }
        public DateTime FechaActual { get; set; }
        public decimal CambioPeso { get; set; }
        public decimal CambioIMC { get; set; }
        public decimal? CambioGrasa { get; set; }
        public int DiasTranscurridos { get; set; }
        public string TendenciaPeso { get; set; }
    }
}
