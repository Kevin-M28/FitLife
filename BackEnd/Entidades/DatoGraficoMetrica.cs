using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class DatoGraficoMetrica
    {
        public DateTime PeriodoFecha { get; set; }
        public string Etiqueta { get; set; }
        public decimal ValorPromedio { get; set; }
        public decimal ValorMinimo { get; set; }
        public decimal ValorMaximo { get; set; }
        public int CantidadRegistros { get; set; }
    }
}
