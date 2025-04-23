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
using BackEnd.Request.Modulo_Usuario;
using BackEnd.Response.Modulo_Usuario;
using BackEnd.Logica.Modulo_Usuario;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


namespace Fitlife.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        public UserController() { }
   
        [HttpPost]
        [Route("insert")]
        public ResAgregarUsuario insertarUsuario(ReqAgregarUsuario req)
        {
            return new LogicaUsuario().Registrar(req);
        }

        [HttpPost]
        [Route("login")]

        public ResLoginUsuario LoginUsuario(ReqLoginUsuario req)
        {
            return new LogicaUsuario().LoginUsuario(req);
        }


    }
}
