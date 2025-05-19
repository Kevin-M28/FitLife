
using BackEnd.Logic.Membership;
using BackEnd.ResAndReq.Req.Membership;
using BackEnd.ResAndReq.Res.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fitlife.Controllers
{
    [RoutePrefix("api/membership")]
    public class MembershipController : ApiController
    {
        public MembershipController() { }

        /// <summary>
        /// Get active membership for the authenticated user
        /// </summary>
        /// <param name="token">User session token (from header)</param>
        /// <returns>Active membership information</returns>
        [HttpGet]
        [Route("active")]
        public ResGetActiveMembership GetActiveMembership()
        {
            string token = Request.Headers.GetValues("Authorization")?.FirstOrDefault()?.Replace("Bearer ", "");
            return new LogMembership().ObtenerMembresiaActiva(token);
        }

        /// <summary>
        /// Check membership status including expiration warnings
        /// </summary>
        /// <param name="token">User session token (from header)</param>
        /// <returns>Membership status information</returns>
        [HttpGet]
        [Route("status")]
        public ResCheckMembershipStatus CheckMembershipStatus()
        {
            string token = Request.Headers.GetValues("Authorization")?.FirstOrDefault()?.Replace("Bearer ", "");
            return new LogMembership().VerificarEstadoMembresia(token);
        }

        /// <summary>
        /// Get all available membership types
        /// </summary>
        /// <returns>List of available membership types</returns>
        [HttpGet]
        [Route("types")]
        public ResGetMembershipTypes GetMembershipTypes()
        {
            return new LogMembership().ObtenerTiposMembresia();
        }
    }
}