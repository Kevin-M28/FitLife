using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class ProgresoRutina
    {
        public int ProgresoID { get; set; }
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreEjercicio { get; set; }
        public string MusculoObjetivo { get; set; }
        public string NombreDia { get; set; }
        public int Series { get; set; }
        public int Repeticiones { get; set; }
        public DateTime FechaRealizacion { get; set; }
        public bool Completado { get; set; }
        public string ProgresoDia { get; set; }
    }
}
