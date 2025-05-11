using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class RutinaActiva
    {
        public int RutinaID { get; set; }
        public string NombreRutina { get; set; }
        public string DescripcionRutina { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public int DiasDesdeAsignacion { get; set; }
        public string AsignadaPor { get; set; }
    }
}
