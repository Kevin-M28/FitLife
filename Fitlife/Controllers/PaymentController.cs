using BackEnd.Logic.Payment;
using BackEnd.ResAndReq.Req.Payment;
using BackEnd.ResAndReq.Res.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fitlife.Controllers
{
    [RoutePrefix("api/payment")]
    public class PaymentController : ApiController
    {
        public PaymentController() { }

        /// <summary>
        /// Register a new payment for membership
        /// </summary>
        /// <param name="req">Payment registration data</param>
        /// <param name="token">User session token (from header)</param>
        /// <returns>Payment registration result</returns>
        [HttpPost]
        [Route("register")]
        public ResRegisterPayment RegisterPayment(ReqRegisterPayment req)
        {
            string token = Request.Headers.GetValues("Authorization")?.FirstOrDefault()?.Replace("Bearer ", "");
            return new LogPayment().RegistrarPago(req, token);
        }

        /// <summary>
        /// Approve a pending payment (Admin only)
        /// </summary>
        /// <param name="req">Payment approval data</param>
        /// <param name="token">Admin session token (from header)</param>
        /// <returns>Payment approval result</returns>
        [HttpPost]
        [Route("approve")]
        public ResApprovePayment ApprovePayment(ReqApprovePayment req)
        {
            string token = Request.Headers.GetValues("Authorization")?.FirstOrDefault()?.Replace("Bearer ", "");
            return new LogPayment().AprobarPago(req, token);
        }

        /// <summary>
        /// Get payment history for the authenticated user
        /// </summary>
        /// <param name="token">User session token (from header)</param>
        /// <returns>User's payment history</returns>
        [HttpGet]
        [Route("history")]
        public ResGetPaymentHistory GetPaymentHistory()
        {
            string token = Request.Headers.GetValues("Authorization")?.FirstOrDefault()?.Replace("Bearer ", "");
            return new LogPayment().ObtenerHistorialPagos(token);
        }
    }
}