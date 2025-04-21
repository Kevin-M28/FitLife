using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Facturacion
    {
        public string Id { get; set; }
        public int idGym { get; set; }
        public decimal precio { get; set; }
        public DateTime fecha { get; set; }
        public string idUsuario { get; set; }
    }
}
