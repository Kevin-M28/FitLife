using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Registro
    {
        public string Id { get; set; }
        public int idGym { get; set; }
        public string idUsuario { get; set; }
        public string idEjercicio { get; set; }
        public DateTime fecha { get; set; }
        public decimal peso { get; set; }
        public int repeticiones { get; set; }
        public int series { get; set; }
    }
}
