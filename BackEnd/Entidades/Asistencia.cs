using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Asistencia
    {
        public long IdAsistencia { get; set; }
        public string Mensaje { get; set; }
        public long UsuarioID { get; set; }
        public DateTime? FechaHoraEntrada { get; set; }
        public DateTime? FechaHoraSalida { get; set; }
        public int? DuracionMinutos { get; set; }
        public string MetodoRegistro { get; set; }
        public int? TotalAsistencias { get; set; }
    }
}
