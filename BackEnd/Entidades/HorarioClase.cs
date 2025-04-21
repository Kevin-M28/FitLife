using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class HorarioClase
    {
        public string Id { get; set; }
        public int idGym { get; set; }
        public string idClase { get; set; }
        public DateTime horaInicio { get; set; }
        public DateTime horaFin { get; set; }
        public int capacidad { get; set; }
    }
}
