using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Membresia
    {
        public int MembresiaID { get; set; }
        public string Tipo { get; set; }
        public decimal Precio { get; set; }
        public int DuracionDias { get; set; }
        public string Descripcion { get; set; }
    }
}
