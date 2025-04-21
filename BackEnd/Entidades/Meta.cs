using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Meta
    {
        public string Id { get; set; }
        public int idGym { get; set; }
        public string idUsuario { get; set; }
        public string descripcion { get; set; }
        public decimal pesoObjetivo { get; set; }
        public DateTime fechaObjetivo { get; set; }
        public string estado { get; set; }
    }
}
