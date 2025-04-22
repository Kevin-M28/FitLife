using AccesoDatos;
using BackEnd.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Sesion
    {
        public int Id { get; set; }
        public DateTime inicio { get; set; }
        public Usuario usuario { get; set; }
        public string origen { get; set; }
        public EnumRolUsuario rol { get; set; }
        public EnumEstadoSesion estado { get; set; }
        public DateTime ultimaActividad { get; set; }
    }
}
