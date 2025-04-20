using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BackEnd.Request;
using BackEnd.Response;
using BackEnd.Entidades;
using BackEnd.Logica;


namespace Fitlife.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        public UserController() { }
   
        [HttpPost]
        [Route("insert")]
        public ResInsertarUsuario insertarUsuario(ReqInsertarUsuario req)
        {
            return new LogUsuario().insertar(req);
        }
    }
}
