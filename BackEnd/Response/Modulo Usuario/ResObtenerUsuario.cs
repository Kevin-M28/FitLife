using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackEnd.Entidades;
using System.Threading.Tasks;

namespace BackEnd.Response.Modulo_Usuario
{
    public class ResObtenerUsuario : ResBase
    {
        public Usuario usuario { get; set; }
        public UsuarioMembresia membresia { get; set; }
        public MetricaCorporal metricaCorporal { get; set; }
        public EstadisticasAsistenciaU estadisticasAsistencia { get; set; }
        public List<Pago> pagosRecientes { get; set; }
        public ResumenNotificaciones resumenNotificaciones { get; set; }
        public EstadoMorosidad estadoMorosidad { get; set; }
        public RutinaActiva rutinaActiva { get; set; }
        public List<MetaUsuario> metasActivas { get; set; }
        public List<Logro> logrosObtenidos { get; set; }

    }
}
