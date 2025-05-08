using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class EstadisticasGenerales
    {
        public int UsuariosUnicos { get; set; }
        public int TotalAsistencias { get; set; }
        public double DuracionPromedioMinutos { get; set; }
        public double PromedioAsistenciasDiarias { get; set; }
        public int MaximoAsistenciasPorUsuario { get; set; }
    }

    public class UsuarioFrecuente
    {
        public long UsuarioID { get; set; }
        public string NombreCompleto { get; set; }
        public int CantidadAsistencias { get; set; }
        public DateTime UltimaAsistencia { get; set; }
        public double DuracionPromedioMinutos { get; set; }
    }

    public class AsistenciaPorDia
    {
        public int DiaSemana { get; set; }
        public string NombreDia { get; set; }
        public int CantidadAsistencias { get; set; }
        public int UsuariosUnicos { get; set; }
    }

    public class AsistenciaPorHoraE
    {
        public int Hora { get; set; }
        public string FranjaHoraria { get; set; }
        public int CantidadAsistencias { get; set; }
        public int UsuariosUnicos { get; set; }
    }

    public class AsistenciaPorFecha
    {
        public DateTime Fecha { get; set; }
        public int CantidadAsistencias { get; set; }
        public int UsuariosUnicos { get; set; }
    }

    public class AsistenciaPorDuracion
    {
        public string RangoDuracion { get; set; }
        public int CantidadAsistencias { get; set; }
        public double DuracionPromedioMinutos { get; set; }
    }

    public class UsuarioPorTiempo
    {
        public long UsuarioID { get; set; }
        public string NombreCompleto { get; set; }
        public int CantidadAsistencias { get; set; }
        public int TiempoTotalMinutos { get; set; }
        public double DuracionPromedioMinutos { get; set; }
        public int DuracionMaximaMinutos { get; set; }
    }

    public class EstadisticasAsistencia
    {
        public EstadisticasGenerales Generales { get; set; }
        public List<UsuarioFrecuente> UsuariosFrecuentes { get; set; }
        public List<AsistenciaPorDia> AsistenciasPorDia { get; set; }
        public List<AsistenciaPorHoraE> AsistenciasPorHora { get; set; }
        public List<AsistenciaPorFecha> AsistenciasPorFecha { get; set; }
        public List<AsistenciaPorDuracion> AsistenciasPorDuracion { get; set; }
        public List<UsuarioPorTiempo> UsuariosPorTiempo { get; set; }

        public EstadisticasAsistencia()
        {
            UsuariosFrecuentes = new List<UsuarioFrecuente>();
            AsistenciasPorDia = new List<AsistenciaPorDia>();
            AsistenciasPorHora = new List<AsistenciaPorHoraE>();
            AsistenciasPorFecha = new List<AsistenciaPorFecha>();
            AsistenciasPorDuracion = new List<AsistenciaPorDuracion>();
            UsuariosPorTiempo = new List<UsuarioPorTiempo>();
        }
    }
    public class ReporteGeneradoAsistencia
    {
        public int ReporteID { get; set; }
        public string NombreGimnasio { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string Periodo { get; set; }
        public string RangoFechas { get; set; }
        public int TotalAsistencias { get; set; }
        public int UsuariosUnicos { get; set; }
        public double PromedioAsistenciasDiarias { get; set; }
        public double DuracionPromedioMinutos { get; set; }
        public string DatosJSON { get; set; }
    }
}