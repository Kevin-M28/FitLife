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
       public int Id_User { get; set; }
        public string Estado { get; set; }
        public DateTime FechaExpira { get; set; }
        public string Tipo { get; set; }
    }
}
