using AccesoDatos;
using BackEnd.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Usuario
    {
        public long Id { get; set; }
        public int GimnasioID { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string correoElectronico { get; set; }
        public string telefono { get; set; }
        public string password { get; set; }
        public enumRolUsuario rol { get; set; }
        public enumEstadoUsuario estado { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string nombreGimnasio { get; set; }
        public UsuarioMembresia membresiaActiva { get; set; }
    }
}
