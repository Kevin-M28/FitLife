using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class InfoRutina
    {
        public int RutinaID { get; set; }
        public string NombreRutina { get; set; }
        public string Descripcion { get; set; }
        public int TotalDias { get; set; }
        public int TotalEjercicios { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public int DiasDesdeAsignacion { get; set; }
    }
}
