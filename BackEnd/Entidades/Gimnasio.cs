using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Gimnasio
    {
        public int GimnasioID { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public TimeSpan HorarioApertura { get; set; }
        public TimeSpan HorarioCierre { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
