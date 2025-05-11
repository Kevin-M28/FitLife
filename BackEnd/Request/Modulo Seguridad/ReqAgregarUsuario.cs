using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackEnd.Entidades;
using System.Threading.Tasks;
using BackEnd.Enum;

namespace BackEnd.Request.Modulo_Usuario
{
    public class ReqAgregarUsuario
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string correoElectronico { get; set; }
        public string telefono { get; set; }
        public string password { get; set; }
        public int GimnasioID { get; set; }
       

    }
}
