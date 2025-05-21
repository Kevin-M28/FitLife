using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entities;
using BackEnd.Enum;
using BackEnd.Helpers;
using BackEnd.ResAndReq.Req.Notification;
using BackEnd.ResAndReq.Res.Notification;
using Conexion;

namespace BackEnd.Logic.Notification
{
    public class LogNotification
    {
        public ResGetUnreadNotifications GetUnreadNotifications(ReqGetUnreadNotifications req)
        {
            var res = new ResGetUnreadNotifications
            {
                Error = new List<Error>(),
                Result = false
            };

            try
            {
                if (req == null || string.IsNullOrEmpty(req.Token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.datosFaltantes,
                        Message = "Token es requerido"
                    });
                    return res;
                }

                using (var db = new FitLifeDataContext())
                {
                    var conn = db.Connection;
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "sp_GetUnreadNotifications";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        var p = cmd.CreateParameter();
                        p.ParameterName = "@Token";
                        p.Value = req.Token;
                        cmd.Parameters.Add(p);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                res.Result = true; // éxito sin notificaciones
                                return res;
                            }

                            while (reader.Read())
                            {
                                var result = reader.TryGetString("Result");
                                if (result == "FAILED")
                                {
                                    res.Error.Add(new Error
                                    {
                                        ErrorCode = (int)EnumErrores.excepcionLogica,
                                        Message = reader.TryGetString("Message") ?? "Error al obtener notificaciones"
                                    });
                                    return res;
                                }

                                // Si hay notificaciones
                                if (!string.IsNullOrEmpty(reader.TryGetString("Title")))
                                {
                                    res.Notifications.Add(new Entities.Notification
                                    {
                                        Type = reader.TryGetString("Type"),
                                        Title = reader.TryGetString("Title"),
                                        Message = reader.TryGetString("Message"),
                                        SentAt = reader.TryGetDate("SentAt"),
                                        IsRead = reader.IsDBNull(reader.GetOrdinal("IsRead")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IsRead"))
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


        public ResMarkNotificationAsRead MarkNotificationAsRead(ReqMarkNotificationAsRead req)
        {
            var res = new ResMarkNotificationAsRead
            {
                Error = new List<Error>(),
                Result = false
            };

            try
            {
                using (var db = new FitLifeDataContext())
                {
                    var conn = db.Connection;
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "sp_MarkNotificationAsRead";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        Action<string, object> add = (name, value) =>
                        {
                            var p = cmd.CreateParameter();
                            p.ParameterName = name;
                            p.Value = value ?? DBNull.Value;
                            cmd.Parameters.Add(p);
                        };

                        add("@Token", req.Token);
                        add("@NotificationTitle", req.NotificationTitle);
                        add("@NotificationSentAt", req.NotificationSentAt);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string result = reader.TryGetString("Result");
                                string message = reader.TryGetString("Message");

                                if (result == "SUCCESS")
                                {
                                    res.Result = true;
                                }
                                else
                                {
                                    res.Error.Add(new Error
                                    {
                                        ErrorCode = (int)EnumErrores.excepcionLogica,
                                        Message = message
                                    });
                                }
                            }
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

        public ResSendNotification SendNotification(ReqSendNotification req)
        {
            var res = new ResSendNotification
            {
                Error = new List<Error>(),
                Result = false
            };

            try
            {
                using (var db = new FitLifeDataContext())
                {
                    var conn = db.Connection;
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "sp_SendNotification";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        Action<string, object> add = (name, value) =>
                        {
                            var p = cmd.CreateParameter();
                            p.ParameterName = name;
                            p.Value = value ?? DBNull.Value;
                            cmd.Parameters.Add(p);
                        };

                        add("@Token", req.Token);
                        add("@TargetCedula", req.TargetCedula);
                        add("@NotificationType", req.NotificationType);
                        add("@Title", req.Title);
                        add("@Message", req.Message);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string result = reader.TryGetString("Result");
                                string message = reader.TryGetString("Message");

                                if (result == "SUCCESS")
                                {
                                    res.Result = true;
                                }
                                else
                                {
                                    res.Error.Add(new Error
                                    {
                                        ErrorCode = (int)EnumErrores.excepcionLogica,
                                        Message = message
                                    });
                                }
                            }
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
