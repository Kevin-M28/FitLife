using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BackEnd.Logica.Modulo_Pago;
using BackEnd.Request.Modulo_Membresia;
using BackEnd.Response.Modulo_Membresia;
using System.Web.Http;

namespace Fitlife.Controllers
{
    [RoutePrefix("api/membresias")]
    public class MembresiasController : ApiController
    {
        public MembresiasController() { }

        [HttpPost]
        [Route("asignar")]
        public ResInsertUsuarioMembresia AsignarMembresia(ReqInsertUsuarioMembresia req)
        {
            return new LogMembresias().InsertUsuarioMembresia(req);
        }
    }
}