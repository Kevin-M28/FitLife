using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class MetodoPagoReporte
    {
        public string MetodoPago { get; set; }
        public int NumeroPagos { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal PorcentajePagos { get; set; }
    }
}
