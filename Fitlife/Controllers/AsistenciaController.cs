using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BackEnd;
using BackEnd.Request;
using BackEnd.Response;
using BackEnd.Entidades;
using BackEnd.Logica;
using BackEnd.Response.Modulo_Asistencia;
using BackEnd.Request.Modulo_Asistencia;
using BackEnd.Logica.Modulo_Logica;

namespace Fitlife.Controllers
{
    [RoutePrefix("api/asistencia")]
    public class AsistenciaController : ApiController
    {
        public AsistenciaController() { }

        [HttpPost]
        [Route("registrar")]
        public ResRegistrarAsistencia RegistrarAsistencia(ReqRegistrarAsistencia req)
        {
            return new LogAsistencia().RegistrarAsistencia(req);
        }

        [HttpPost]
        [Route("historial")]
        public ResObtenerHistorialAsistencia CambiarContrasenna(ReqObtenerHistorialAsistencia req)
        {
            return new LogAsistencia().ObtenerHistorialAsistencia(req);
        }

        [HttpPost]
        [Route("reporte")]
        public ResReporteAsistencia ObtenerReporteAsistencia(ReqReporteAsistencia req)
        {
            return new LogAsistencia().GenerarReporteAsistencia(req);
        }

        [HttpPost]
        [Route("estadisticas")]
        public ResEstadisticasAsistencia GetEstadisticasAsistencia(ReqEstadisticasAsistencia req)
        {
            return new LogAsistencia().GetEstadisticasAsistencia(req);
        }
    }
}
