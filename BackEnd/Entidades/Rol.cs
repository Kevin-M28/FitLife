using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Rol
    {
        public int Id { get; set; }
        public int idGym { get; set; }
        public string tipo { get; set; }
        public string descripcion { get; set; }
        public bool puedeGestionarUsuarios { get; set; }
        public bool puedeGestionarMembresias { get; set; }
        public bool puedeGestionarClases { get; set; }
        public bool puedeVerReportes { get; set; }
        public bool puedeEditarPerfil { get; set; }
    }
}
