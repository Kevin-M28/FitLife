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
        public ResBase ValidateSession( string token)
        {
            return new LogUser().ValidateSession(token);
        }

        [HttpPost]
        [Route("change_password")]
        public ResBase ChangePassword(string token,string oldPassword , string newPassword)
        {
            return new LogUser().ChangePassword(token, oldPassword, newPassword);
        }

        [HttpPost]
        [Route("profile")]
        public ResBase GetUserProfile([FromUri] string token)
        {
            return new LogUser().GetUserProfile(token); 
        }
        [HttpPost]
        [Route("profile_Cedula")]
        public ResBase GetUserProfileByCedula(string token, string cedula)
        {

            return new LogUser().GetUserProfileByCedula(token, cedula);
        }


        [HttpPut]
        [Route("update_profile")]
        public ResBase UpdateUserProfile(ReqUpdateUser req)
        {
            return new LogUser().UpdateUserProfile(req);
        }
    }

}
