using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class MetodoPago
    {
        public string Id { get; set; }
        public int idGym { get; set; }
        public string idUsuario { get; set; }
        public string tipo { get; set; }
        public string detalles { get; set; }
    }
}
