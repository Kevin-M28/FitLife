using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Rutina
    {
        public string Id { get; set; }
        public int idGym { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string idUsuario { get; set; }
    }
}
