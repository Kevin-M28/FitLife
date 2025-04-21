using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class TokenDeRecuperacion
    {
        public string Id { get; set; }
        public int idGym { get; set; }
        public string idUsuario { get; set; }
        public string token { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaExpiracion { get; set; }
        public bool usado { get; set; }
    }
}
