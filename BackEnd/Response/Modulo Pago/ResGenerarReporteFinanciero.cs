using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entidades;

namespace BackEnd.Response.Modulo_Pago
{
    public class ResGenerarReporteFinanciero
    {
        public bool resultado { get; set; }
        public List<Error> error { get; set; }
        public Entidades.ReporteFinanciero reporte { get; set; }
        public List<Entidades.MetodoPagoReporte> pagosPorMetodo { get; set; }
        public List<Entidades.MembresiaReporte> membresiasPorTipo { get; set; }
    }
}
