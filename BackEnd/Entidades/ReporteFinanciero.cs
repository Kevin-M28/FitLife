using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class ReporteFinanciero
    {
        public int ReporteID { get; set; }
        public string NombreGimnasio { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string Periodo { get; set; }
        public string RangoFechas { get; set; }
        public decimal TotalIngresos { get; set; }
        public int PagosPendientes { get; set; }
        public decimal Morosidad { get; set; }
        public decimal BalanceNeto { get; set; }
    }
}
