using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class IMC
    {
        public string Id { get; set; }
        public int idGym { get; set; }
        public string idUsuario { get; set; }
        public decimal altura { get; set; }
        public decimal peso { get; set; }
        public DateTime fecha { get; set; }
    }
}
