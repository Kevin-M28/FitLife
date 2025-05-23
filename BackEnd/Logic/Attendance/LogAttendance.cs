using BackEnd.Entities;
using BackEnd.Enum;
using BackEnd.ResAndReq.Req.Attendance;
using BackEnd.ResAndReq.Res.Attendance;
using Conexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logic
{
    public class LogAttendance
    {
        /// <summary>
        /// Registra la entrada del usuario al gimnasio
        /// </summary>
        /// <param name="token">Token de sesión del usuario</param>
        /// <returns>Resultado del registro de entrada</returns>
        public ResCheckIn RegistrarEntrada(string token)
        {
            ResCheckIn res = new ResCheckIn()
            {
                Error = new List<Error>(),
                Result = false,
                AttendanceMessage = null
            };

            try
            {
                #region Validaciones
                if (string.IsNullOrEmpty(token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.sesionNula,
                        Message = "Token de sesión requerido"
                    });
                    return res;
                }
                #endregion

                using (FitLife2DataContext linq = new FitLife2DataContext())
                {
                    var resultado = linq.sp_RegisterAttendance(token).FirstOrDefault();

                    if (resultado == null || resultado.Result != "SUCCESS")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.errorDesconocido,
                            Message = resultado?.Message ?? "Error desconocido al registrar entrada"
                        });
                    }
                    else
                    {
                        res.Result = true;
                        res.AttendanceMessage = resultado.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Error.Add(new Error
                {
                    ErrorCode = (int)EnumErrores.excepcionLogica,
                    Message = ex.Message
                });
            }

            return res;
        }

        /// <summary>
        /// Registra la salida del usuario del gimnasio
        /// </summary>
        /// <param name="token">Token de sesión del usuario</param>
        /// <returns>Resultado del registro de salida</returns>
        public ResCheckOut RegistrarSalida(string token)
        {
            ResCheckOut res = new ResCheckOut()
            {
                Error = new List<Error>(),
                Result = false,
                CheckInTime = null,
                CheckOutTime = null,
                DurationMinutes = null
            };

            try
            {
                #region Validaciones
                if (string.IsNullOrEmpty(token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.sesionNula,
                        Message = "Token de sesión requerido"
                    });
                    return res;
                }
                #endregion

                using (FitLife2DataContext linq = new FitLife2DataContext())
                {
                    var resultado = linq.sp_RegisterCheckOut(token).FirstOrDefault();

                    if (resultado == null || resultado.Result != "SUCCESS")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.errorDesconocido,
                            Message = resultado?.Message ?? "Error desconocido al registrar salida"
                        });
                    }
                    else
                    {
                        res.Result = true;
                        res.CheckInTime = resultado.CheckInTime;
                        res.CheckOutTime = resultado.CheckOutTime;

                        // Manejar DurationMinutes que puede ser null
                        if (resultado.DurationMinutes.HasValue)
                        {
                            res.DurationMinutes = resultado.DurationMinutes.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.Error.Add(new Error
                {
                    ErrorCode = (int)EnumErrores.excepcionLogica,
                    Message = ex.Message
                });
            }

            return res;
        }

        /// <summary>
        /// Obtiene el estado actual de asistencia del usuario
        /// </summary>
        /// <param name="token">Token de sesión del usuario</param>
        /// <returns>Estado actual de asistencia</returns>
        public ResAttendanceStatus ObtenerEstadoAsistencia(string token)
        {
            ResAttendanceStatus res = new ResAttendanceStatus()
            {
                Error = new List<Error>(),
                Result = false,
                AttendanceStatus = null,
                HasActiveCheckin = false,
                CheckInTime = null,
                MinutesElapsed = null,
                ActionAvailable = null
            };

            try
            {
                #region Validaciones
                if (string.IsNullOrEmpty(token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.sesionNula,
                        Message = "Token de sesión requerido"
                    });
                    return res;
                }
                #endregion

                using (FitLife2DataContext linq = new FitLife2DataContext())
                {
                    var resultado = linq.sp_GetAttendanceStatus(token).FirstOrDefault();

                    if (resultado == null || resultado.Result != "SUCCESS")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.errorDesconocido,
                            Message = resultado?.Message ?? "Error al consultar estado de asistencia"
                        });
                    }
                    else
                    {
                        res.Result = true;
                        res.AttendanceStatus = resultado.AttendanceStatus;
                        res.HasActiveCheckin = resultado.HasActiveCheckin == 1;
                        res.CheckInTime = resultado.CheckInTime;
                        res.MinutesElapsed = resultado.MinutesElapsed;
                        res.ActionAvailable = resultado.ActionAvailable;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Error.Add(new Error
                {
                    ErrorCode = (int)EnumErrores.excepcionLogica,
                    Message = ex.Message
                });
            }

            return res;
        }
    }
}