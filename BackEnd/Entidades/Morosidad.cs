using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Morosidad
    {
        public int MorosidadID { get; set; }
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public int DiasRetraso { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string TipoMembresia { get; set; }
        public DateTime FechaBloqueo { get; set; }
    }
}
