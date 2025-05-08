using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class MetaUsuario
    {
        public int MetaID { get; set; }
        public int UsuarioID { get; set; }
        public string TipoMeta { get; set; }
        public decimal ValorObjetivo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaObjetivo { get; set; }
        public string Estado { get; set; }
        public int DiasRestantes { get; set; }
        public int DuracionTotalDias { get; set; }
        public int PorcentajeTiempoTranscurrido { get; set; }
        public decimal? ValorActual { get; set; }
    }
}
