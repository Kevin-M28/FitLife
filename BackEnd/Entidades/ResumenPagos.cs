using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class ResumenPagos
    {
        public int TotalPagos { get; set; }
        public int PagosConfirmados { get; set; }
        public int PagosPendientes { get; set; }
        public int PagosRechazados { get; set; }
        public decimal TotalMontoConfirmado { get; set; }
        public decimal TotalMontoPendiente { get; set; }
    }

}
