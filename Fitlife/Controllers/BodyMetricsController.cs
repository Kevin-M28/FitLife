using BackEnd.Entities;
using BackEnd.Enum;
using BackEnd.Logic.BodyMetrics;
using BackEnd.ResAndReq.Req.BodyMetrics;
using BackEnd.ResAndReq.Res.BodyMetrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Fitlife.Controllers
{
    [RoutePrefix("api/bodymetrics")]
    public class BodyMetricsController : ApiController
    {
        public BodyMetricsController() { }

        /// <summary>
        /// Register body metrics
        /// </summary>
        /// <param name="req">Body metrics data</param>
        /// <returns>Registration result</returns>
        [HttpPost]
        [Route("register")]
        public ResRegisterBodyMetrics RegisterBodyMetrics(ReqRegisterBodyMetrics req)
        {
            try
            {
                if (req == null)
                {
                    return new ResRegisterBodyMetrics
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
                        return new ResRegisterBodyMetrics
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

                return new LogBodyMetrics().RegisterBodyMetrics(req);
            }
            catch (Exception ex)
            {
                return new ResRegisterBodyMetrics
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

        /// <summary>
        /// Schedule a future body measurement
        /// </summary>
        /// <param name="req">Schedule data</param>
        /// <returns>Scheduling result</returns>
        [HttpPost]
        [Route("schedule")]
        public ResScheduleMeasurement ScheduleMeasurement(ReqScheduleMeasurement req)
        {
            try
            {
                if (req == null)
                {
                    return new ResScheduleMeasurement
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
                        return new ResScheduleMeasurement
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

                return new LogBodyMetrics().ScheduleMeasurement(req);
            }
            catch (Exception ex)
            {
                return new ResScheduleMeasurement
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

        /// <summary>
        /// Get body metrics history
        /// </summary>
        /// <param name="req">History request with date range</param>
        /// <returns>Metrics history</returns>
        [HttpPost]
        [Route("history")]
        public ResGetBodyMetricsHistory GetBodyMetricsHistory(ReqGetBodyMetricsHistory req)
        {
            try
            {
                if (req == null)
                {
                    req = new ReqGetBodyMetricsHistory();
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
                        return new ResGetBodyMetricsHistory
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

                return new LogBodyMetrics().GetBodyMetricsHistory(req);
            }
            catch (Exception ex)
            {
                return new ResGetBodyMetricsHistory
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