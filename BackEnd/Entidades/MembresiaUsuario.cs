using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class MembresiaUsuario
    {
        public string Id { get; set; }
        public int idGym { get; set; }
        public string idUsuario { get; set; }
        public string idMembresia { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public bool activo { get; set; }
    }
}
