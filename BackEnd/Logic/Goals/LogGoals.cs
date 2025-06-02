using BackEnd.Entities;
using BackEnd.Enum;
using BackEnd.Helpers;
using BackEnd.ResAndReq.Req.Goals;
using BackEnd.ResAndReq.Res.Goals;
using Conexion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackEnd.Logic.Goals
{
    public class LogGoal
    {
        public ResGetPreMadeGoals GetPreMadeGoals(ReqGetPreMadeGoals req)
        {
            var res = new ResGetPreMadeGoals
            {
                Error = new List<Error>(),
                Result = false
            };

            try
            {
                if (string.IsNullOrEmpty(req.Token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.sesionNula,
                        Message = "Token de sesión requerido"
                    });
                    return res;
                }

                using (var db = new FitLife2DataContext())
                {
                    var conn = db.Connection;
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "sp_GetPreMadeGoals";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        void AddParam(string name, object value)
                        {
                            var p = cmd.CreateParameter();
                            p.ParameterName = name;
                            p.Value = value ?? DBNull.Value;
                            cmd.Parameters.Add(p);
                        }

                        AddParam("@Token", req.Token);
                        AddParam("@Difficulty", req.Difficulty);

                        using (var reader = cmd.ExecuteReader())
                        {
                            // Check if we have any rows
                            if (!reader.HasRows)
                            {
                                if (reader.Read())
                                {
                                    string result = reader["Result"].ToString();
                                    string message = reader["Message"].ToString();

                                    if (result == "FAILED")
                                    {
                                        res.Error.Add(new Error
                                        {
                                            ErrorCode = (int)EnumErrores.excepcionLogica,
                                            Message = message
                                        });
                                        return res;
                                    }
                                }

                                // No goals found, but query was successful
                                res.Result = true;
                                return res;
                            }

                            // Process all rows
                            while (reader.Read())
                            {
                                string result = reader["Result"].ToString();

                                if (result == "FAILED")
                                {
                                    res.Error.Add(new Error
                                    {
                                        ErrorCode = (int)EnumErrores.excepcionLogica,
                                        Message = reader["Message"].ToString()
                                    });
                                    return res;
                                }

                                // Add pre-made goal to result
                                res.PreMadeGoals.Add(new PreMadeGoal
                                {
                                    PreMadeGoalID = Convert.ToInt32(reader["PreMadeGoalID"]),
                                    Name = reader["Name"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    GoalTypeName = reader["GoalTypeName"].ToString(),
                                    DefaultTargetValue = Convert.ToDecimal(reader["DefaultTargetValue"]),
                                    DefaultDurationDays = Convert.ToInt32(reader["DefaultDurationDays"]),
                                    Difficulty = reader["Difficulty"].ToString()
                                });
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

        public ResAssignPreMadeGoal AssignPreMadeGoal(ReqAssignPreMadeGoal req)
        {
            var res = new ResAssignPreMadeGoal
            {
                Error = new List<Error>(),
                Result = false
            };

            try
            {
                if (string.IsNullOrEmpty(req.Token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.sesionNula,
                        Message = "Token de sesión requerido"
                    });
                    return res;
                }

                if (req.PreMadeGoalID <= 0)
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.datosFaltantes,
                        Message = "ID de meta predefinida inválido"
                    });
                    return res;
                }

                using (var db = new FitLife2DataContext())
                {
                    var conn = db.Connection;
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "sp_AssignPreMadeGoalToUser";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        void AddParam(string name, object value)
                        {
                            var p = cmd.CreateParameter();
                            p.ParameterName = name;
                            p.Value = value ?? DBNull.Value;
                            cmd.Parameters.Add(p);
                        }

                        AddParam("@Token", req.Token);
                        AddParam("@PreMadeGoalID", req.PreMadeGoalID);
                        AddParam("@CustomTargetValue", req.CustomTargetValue);
                        AddParam("@CustomTargetDate", req.CustomTargetDate);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string result = reader["Result"].ToString();
                                string message = reader["Message"].ToString();

                                if (result == "FAILED")
                                {
                                    res.Error.Add(new Error
                                    {
                                        ErrorCode = (int)EnumErrores.excepcionLogica,
                                        Message = message
                                    });
                                }
                                else
                                {
                                    res.Result = true;

                                    // Only try to read these if we have a successful result
                                    res.GoalTypeID = reader["GoalTypeID"] != DBNull.Value ?
                                        Convert.ToInt32(reader["GoalTypeID"]) : (int?)null;

                                    res.TargetValue = reader["TargetValue"] != DBNull.Value ?
                                        Convert.ToDecimal(reader["TargetValue"]) : (decimal?)null;

                                    res.StartDate = reader["StartDate"] != DBNull.Value ?
                                        Convert.ToDateTime(reader["StartDate"]) : (DateTime?)null;

                                    res.TargetDate = reader["TargetDate"] != DBNull.Value ?
                                        Convert.ToDateTime(reader["TargetDate"]) : (DateTime?)null;
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