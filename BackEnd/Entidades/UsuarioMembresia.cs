using AccesoDatos;
using BackEnd.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class UsuarioMembresia
    {
        public int UsuarioMembresiaID { get; set; }
        public int UsuarioID { get; set; }
        public int MembresiaID { get; set; }
        public string TipoMembresia { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public EnumEstadoMembresia Estado { get; set; }
        public Pago PagoAsociado { get; set; }
    }
}
