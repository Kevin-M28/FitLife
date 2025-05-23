using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entities;
using BackEnd.Enum;
using BackEnd.Helpers;
using BackEnd.ResAndReq.Req.BodyMetrics;
using BackEnd.ResAndReq.Res.BodyMetrics;
using Conexion;
using System.Data.Common;

namespace BackEnd.Logic.BodyMetrics
{
    public class LogBodyMetrics
    {
        /// <summary>
        /// Registers body measurements for the user
        /// </summary>
        /// <param name="req">Measurement data</param>
        /// <returns>Result of the registration</returns>
        public ResRegisterBodyMetrics RegisterBodyMetrics(ReqRegisterBodyMetrics req)
        {
            ResRegisterBodyMetrics res = new ResRegisterBodyMetrics()
            {
                Error = new List<Error>(),
                Result = false,
                Message = null
            };

            try
            {
                #region Validations
                if (string.IsNullOrEmpty(req.Token))
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
                    var resultado = linq.sp_RegisterBodyMetrics(
                        req.Token,
                        req.Weight,
                        req.Height,
                        req.BodyFatPercentage,
                        req.WaistCircumference,
                        req.ChestCircumference,
                        req.ArmCircumference,
                        req.LegCircumference,
                        req.Notes
                    ).FirstOrDefault();

                    if (resultado == null || resultado.Result != "SUCCESS")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.errorDesconocido,
                            Message = resultado?.Message ?? "Error desconocido al registrar métricas"
                        });
                    }
                    else
                    {
                        res.Result = true;
                        res.Message = resultado.Message;
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
        /// Schedules a future body measurement
        /// </summary>
        /// <param name="req">Schedule data</param>
        /// <returns>Result of the scheduling</returns>
        public ResScheduleMeasurement ScheduleMeasurement(ReqScheduleMeasurement req)
        {
            ResScheduleMeasurement res = new ResScheduleMeasurement()
            {
                Error = new List<Error>(),
                Result = false,
                Message = null
            };

            try
            {
                #region Validations
                if (string.IsNullOrEmpty(req.Token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.sesionNula,
                        Message = "Token de sesión requerido"
                    });
                    return res;
                }

                if (req.ScheduledDate < DateTime.Today)
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.fechaInvalida,
                        Message = "La fecha programada debe ser igual o posterior a hoy"
                    });
                    return res;
                }
                #endregion

                using (FitLife2DataContext linq = new FitLife2DataContext())
                {
                    var resultado = linq.sp_ScheduleMetricMeasurement(
                        req.Token,
                        req.ScheduledDate
                    ).FirstOrDefault();

                    if (resultado == null || resultado.Result != "SUCCESS")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.errorDesconocido,
                            Message = resultado?.Message ?? "Error desconocido al programar medición"
                        });
                    }
                    else
                    {
                        res.Result = true;
                        res.Message = resultado.Message;
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
        /// Gets body metrics history for a user
        /// </summary>
        /// <param name="req">History request with date range</param>
        /// <returns>Metrics history</returns>
        public ResGetBodyMetricsHistory GetBodyMetricsHistory(ReqGetBodyMetricsHistory req)
        {
            ResGetBodyMetricsHistory res = new ResGetBodyMetricsHistory()
            {
                Error = new List<Error>(),
                Result = false
            };

            try
            {
                #region Validations
                if (string.IsNullOrEmpty(req.Token))
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
                    var conn = linq.Connection;
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "sp_GetBodyMetricsHistory";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Add parameters
                        Action<string, object> add = (name, value) =>
                        {
                            var p = cmd.CreateParameter();
                            p.ParameterName = name;
                            p.Value = value ?? DBNull.Value;
                            cmd.Parameters.Add(p);
                        };

                        add("@Token", req.Token);
                        add("@StartDate", req.StartDate);
                        add("@EndDate", req.EndDate);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                var noDataResult = reader.Read() ? reader["Result"]?.ToString() : null;
                                var noDataMessage = reader.Read() ? reader["Message"]?.ToString() : null;

                                if (noDataResult == "FAILED")
                                {
                                    res.Error.Add(new Error
                                    {
                                        ErrorCode = (int)EnumErrores.errorDesconocido,
                                        Message = noDataMessage ?? "Error al consultar historial de métricas"
                                    });
                                    return res;
                                }
                                else
                                {
                                    // No data but successful response
                                    res.Result = true;
                                    return res;
                                }
                            }

                            while (reader.Read())
                            {
                                var result = reader.TryGetString("Result");
                                if (result == "FAILED")
                                {
                                    res.Error.Add(new Error
                                    {
                                        ErrorCode = (int)EnumErrores.errorDesconocido,
                                        Message = reader.TryGetString("Message") ?? "Error al consultar historial de métricas"
                                    });
                                    return res;
                                }

                                // Process metrics data
                                if (reader["MeasurementDate"] != DBNull.Value)
                                {
                                    res.Metrics.Add(new BodyMetric
                                    {
                                        MeasurementDate = reader.TryGetDate("MeasurementDate") ?? DateTime.MinValue,
                                        Weight = reader.TryGetDecimal("Weight"),
                                        Height = reader.TryGetDecimal("Height"),
                                        BodyFatPercentage = reader.TryGetDecimal("BodyFatPercentage"),
                                        BMI = reader.TryGetDecimal("BMI"),
                                        WaistCircumference = reader.TryGetDecimal("WaistCircumference"),
                                        ChestCircumference = reader.TryGetDecimal("ChestCircumference"),
                                        ArmCircumference = reader.TryGetDecimal("ArmCircumference"),
                                        LegCircumference = reader.TryGetDecimal("LegCircumference"),
                                        Notes = reader.TryGetString("Notes")
                                    });
                                }
                            }

                            res.Result = true;
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
    }
}