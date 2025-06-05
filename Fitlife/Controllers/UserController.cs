using BackEnd.Logic;
using BackEnd.ResAndReq.Req;
using BackEnd.ResAndReq.Req.User;
using BackEnd.ResAndReq.Res;
using BackEnd.ResAndReq.Res.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fitlife.Controllers
{

    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        public UserController() { }

        [HttpPost]
        [Route("user_registrer")]
        public ResAddUser UserRegistrer(ReqAddUser req)
        {
            return new LogUser().UserRegistration(req);
        }

        [HttpPost]
        [Route("login")]
        public ResUserLogin UserLogin(ReqUserLogin req)
        {
            return new LogUser().Login(req);
        }

        [HttpPost]
        [Route("logout")]
        public ResBase LogoutUser(ReqBase req)
        {
            return new LogUser().LogOut(req);
        }

        [HttpPost]
        [Route("validate_session")]
        public ResBase ValidateSession(ReqBase req)
        {
            return new LogUser().ValidateSession(req);
        }

        [HttpPatch]
        [Route("change_password")]
        public ResBase ChangePassword(ReqChangePassword req)
        {
            return new LogUser().ChangePassword(req);
        }

        [HttpPost]
        [Route("profile")]
        public ResUserProfile GetUserProfile(ReqBase req)
        {
            return new LogUser().GetUserProfile(req); 
        }
        [HttpPost]
        [Route("profile_cedula")]
        public ResUserProfile GetUserProfileByCedula(ReqGetUserCedula req)
        {

            return new LogUser().GetUserProfileByCedula(req);
        }


        [HttpPatch]
        [Route("update_profile")]
        public ResBase UpdateUserProfile(ReqUpdateUser req)
        {
            return new LogUser().UpdateUserProfile(req);
        }

        [HttpPost]
        [Route("reset_password")]
        public ResBase ResetPassword(ReqForgotPassword req)
        {
            return new LogUser().ResetPassword(req);
        }
    }

}
