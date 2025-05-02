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
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string correoElectronico { get; set; }
        public string telefono { get; set; }
        public string password { get; set; }
        public int GimnasioID { get; set; }
        public string nombreGimnasio { get; set; }
        public EnumRolUsuario rol { get; set; }
        public EnumEstadoUsuario estado { get; set; }
        public UsuarioMembresia membresiaActiva { get; set; }
    }
}
