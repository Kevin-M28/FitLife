using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Progreso
    {
        public string Id { get; set; }
        public int idGym { get; set; }
        public string idUsuario { get; set; }
        public DateTime fecha { get; set; }
        public decimal peso { get; set; }
        public decimal porcentajeGrasaCorporal { get; set; }
        public decimal masaMuscular { get; set; }
    }
}
