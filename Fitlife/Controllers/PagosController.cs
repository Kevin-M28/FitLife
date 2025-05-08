using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BackEnd.Logica.Modulo_Pago;
using BackEnd.Request.Modulo_Pago;
using BackEnd.Response.Modulo_Pago;
using System.Web.Http;

namespace Fitlife.Controllers
{
    [RoutePrefix("api/pagos")]
    public class PagosController : ApiController
    {
        public PagosController() { }

        [HttpPost]
        [Route("insertar")]
        public ResInsertPago InsertarPago(ReqInsertPago req)
        {
            return new LogPagos().InsertPago(req);
        }

  
    }
}