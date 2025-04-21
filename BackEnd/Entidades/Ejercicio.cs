using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
   public class Ejercicio
    {
        public string Id { get; set; }
        public int idGym { get; set; }
        public string nombre { get; set; }
        public string tipo { get; set; }
        public string musculo { get; set; }
        public string equipo { get; set; }
        public string dificultad { get; set; }
        public string instrucciones { get; set; }
    }
}
