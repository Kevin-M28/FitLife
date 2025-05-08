using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entidades;

namespace BackEnd.Response.Modulo_Membresia
{
    public class ResGetMembresiasPorGimnasio
    {
        public bool resultado { get; set; }
        public List<Error> error { get; set; }
        public List<Entidades.UsuarioMembresia> membresias { get; set; }
        public int TotalMembresias { get; set; }
        public int MembresiasActivas { get; set; }
        public int MembresiasVencidas { get; set; }
        public int MembresiasProximasAVencer { get; set; }
        public decimal IngresosTotales { get; set; }
    }
}
