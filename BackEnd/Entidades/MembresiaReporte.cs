using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class MembresiaReporte
    {
        public string TipoMembresia { get; set; }
        public int NumeroMembresias { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal PorcentajeMembresías { get; set; }
    }
}
