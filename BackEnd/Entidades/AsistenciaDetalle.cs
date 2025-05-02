using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class AsistenciaDetalle
    {
        public int AsistenciaID { get; set; }
        public DateTime FechaHoraEntrada { get; set; }
        public DateTime? FechaHoraSalida { get; set; }
        public int? DuracionMinutos { get; set; }
        public string MetodoRegistro { get; set; }
        public string Estado { get; set; }
        public int DuracionActual { get; set; }
        public string DuracionFormateada { get; set; }
    }

    public class AsistenciaResumen
    {
        public int TotalAsistencias { get; set; }
        public int AsistenciasCompletadas { get; set; }
        public int AsistenciasEnProgreso { get; set; }
        public int AsistenciasFinDeSemana { get; set; }
        public double DuracionPromedioMinutos { get; set; }
        public int DuracionMaximaMinutos { get; set; }
    }

    public class AsistenciaPorDiaSemana
    {
        public int DiaSemana { get; set; }
        public string NombreDia { get; set; }
        public int CantidadAsistencias { get; set; }
    }

    public class AsistenciaPorHora
    {
        public int Hora { get; set; }
        public int CantidadAsistencias { get; set; }
    }

    public class HistorialAsistencia
    {
        public List<AsistenciaDetalle> Detalles { get; set; }
        public AsistenciaResumen Resumen { get; set; }
        public List<AsistenciaPorDiaSemana> EstadisticasPorDia { get; set; }
        public List<AsistenciaPorHora> EstadisticasPorHora { get; set; }
    }
}
