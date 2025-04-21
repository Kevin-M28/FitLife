using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class Estadistica
    {
        public string Id { get; set; }
        public int idGym { get; set; }
        public string metrica { get; set; } // DAILY_ATTENDANCE, MEMBERSHIP_GROWTH, CLASS_ATTENDANCE, etc.
        public decimal valor { get; set; } // Valor numérico de la métrica
        public DateTime fecha { get; set; } // Fecha en que se registró la métrica
    }
}
