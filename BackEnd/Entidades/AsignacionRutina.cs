using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class AsignacionRutina
    {
        public int UsuarioRutinaID { get; set; }
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public int RutinaID { get; set; }
        public string NombreRutina { get; set; }
        public string DescripcionRutina { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public int AsignadoPorUsuarioID { get; set; }
        public string AsignadoPor { get; set; }
    }
}
