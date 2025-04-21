using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BackEnd.Entidades
{
    public class Usuario
    {
        public string Id { get; set; }
        public int idGym { get; set; }
        public string nombre { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public int idRol { get; set; }
        public string correoElectronico { get; set; }
        public string password { get; set; }¡
        public bool activo { get; set; }
        public DateTime fechaRegistro { get; set; }
        public bool emailVerificado { get; set; }
        public DateTime? ultimoAcceso { get; set; }
        public int intentosFallidos { get; set; }
        public DateTime? bloqueadoHasta { get; set; }

        // Propiedad calculada para obtener el nombre completo
        public string apellidos
        {
            get { return apellido1 + (string.IsNullOrEmpty(apellido2) ? "" : " " + apellido2); }
        }
    }
}
