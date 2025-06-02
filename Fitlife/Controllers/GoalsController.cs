using BackEnd.Entities;
using BackEnd.Enum;
using BackEnd.Logic.Goals;
using BackEnd.ResAndReq.Req.Goals;
using BackEnd.ResAndReq.Res.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;

namespace Fitlife.Controllers
{
    [RoutePrefix("api/goals")]
    public class GoalController : ApiController
    {
        [HttpPost]
        [Route("premade")]
        public ResGetPreMadeGoals GetPreMadeGoals(ReqGetPreMadeGoals req)
        {
            try
            {
                if (req == null)
                {
                    req = new ReqGetPreMadeGoals();
                }

                if (string.IsNullOrEmpty(req.Token))
                {
                    // Try to get token from authorization header
                    string token = null;
                    if (Request.Headers.Authorization != null && Request.Headers.Authorization.Scheme == "Bearer")
                    {
                        token = Request.Headers.Authorization.Parameter;
                        req.Token = token;
                    }

                    if (string.IsNullOrEmpty(req.Token))
                    {
                        return new ResGetPreMadeGoals
                        {
                            Result = false,
                            Error = new List<Error>
                            {
                                new Error
                                {
                                    ErrorCode = (int)EnumErrores.sesionNula,
                                    Message = "Token de sesión requerido"
                                }
                            }
                        };
                    }
                }

                return new LogGoal().GetPreMadeGoals(req);
            }
            catch (Exception ex)
            {
                return new ResGetPreMadeGoals
                {
                    Result = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            ErrorCode = (int)EnumErrores.excepcionLogica,
                            Message = ex.Message
                        }
                    }
                };
            }
        }

        [HttpPost]
        [Route("assign")]
        public ResAssignPreMadeGoal AssignPreMadeGoal(ReqAssignPreMadeGoal req)
        {
            try
            {
                if (req == null)
                {
                    return new ResAssignPreMadeGoal
                    {
                        Result = false,
                        Error = new List<Error>
                        {
                            new Error
                            {
                                ErrorCode = (int)EnumErrores.requestNulo,
                                Message = "Request nulo"
                            }
                        }
                    };
                }

                if (string.IsNullOrEmpty(req.Token))
                {
                    // Try to get token from authorization header
                    string token = null;
                    if (Request.Headers.Authorization != null && Request.Headers.Authorization.Scheme == "Bearer")
                    {
                        token = Request.Headers.Authorization.Parameter;
                        req.Token = token;
                    }

                    if (string.IsNullOrEmpty(req.Token))
                    {
                        return new ResAssignPreMadeGoal
                        {
                            Result = false,
                            Error = new List<Error>
                            {
                                new Error
                                {
                                    ErrorCode = (int)EnumErrores.sesionNula,
                                    Message = "Token de sesión requerido"
                                }
                            }
                        };
                    }
                }

                return new LogGoal().AssignPreMadeGoal(req);
            }
            catch (Exception ex)
            {
                return new ResAssignPreMadeGoal
                {
                    Result = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            ErrorCode = (int)EnumErrores.excepcionLogica,
                            Message = ex.Message
                        }
                    }
                };
            }
        }
    }
}