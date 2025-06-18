using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entities;
using BackEnd.Enum;
using BackEnd.Helpers;
using BackEnd.ResAndReq.Req.Routine;
using BackEnd.ResAndReq.Res.Routine;
using Conexion;

namespace BackEnd.Logic.Routine
{
    public class LogRoutine
    {
        public ResCreateRoutine CreateRoutine(ReqCreateRoutine req)
        {
            ResCreateRoutine res = new ResCreateRoutine()
            {
                Error = new List<Entities.Error>(),
                Result = false
            };

            try
            {
                #region Validaciones
                if (req == null)
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                }
                else
                {
                    if (string.IsNullOrEmpty(req.Token))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.tokenFaltante,
                            Message = "Token vacío"
                        });
                    }

                    if (string.IsNullOrEmpty(req.Name))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.nombreFaltante,
                            Message = "Nombre de rutina vacío"
                        });
                    }

                    if (string.IsNullOrEmpty(req.DifficultyLevelName))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.nivelDificultadFaltante,
                            Message = "Nivel de dificultad vacío"
                        });
                    }

                    if (req.DurationInDays <= 0)
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.duracionInvalida,
                            Message = "La duración debe ser mayor a 0"
                        });
                    }
                }
                #endregion

                if (res.Error.Any())
                {
                    return res;
                }

                Entities.Routine routineEntity = req.ToEntity();

                using (FitLife2DataContext linq = new FitLife2DataContext())
                {
                    var resultado = linq.sp_CreateRoutine(
                        req.Token,
                        req.Name,
                        req.Description,
                        req.DifficultyLevelName,
                        req.DurationInDays
                    ).FirstOrDefault();

                    if (resultado == null || resultado.Result == "FAILED")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.errorDesconocido,
                            Message = resultado?.Message ?? "Error desconocido"
                        });
                    }
                    else
                    {
                        res.FromSPResult(resultado);
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

        public ResGetActiveRoutines GetActiveRoutines(ReqGetActiveRoutines req)
        {
            var res = new ResGetActiveRoutines
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
                        ErrorCode = (int)EnumErrores.tokenFaltante,
                        Message = "Token faltante o nulo"
                    });
                    return res;
                }

                using (var db = new FitLife2DataContext())
                {
                    var result = db.sp_GetActiveRoutines(req.Token).ToList();

                    if (result.Count == 0 || result.First().Result == "FAILED")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.sesionNoEncontrada,
                            Message = result.FirstOrDefault()?.Message ?? "No se encontraron rutinas activas"
                        });
                        return res;
                    }

                    foreach (var item in result)
                    {
                        res.ActiveRoutines.Add(new ActiveRoutine
                        {
                            RoutineName = item.RoutineName,
                            DifficultyLevelName = item.DifficultyLevelName,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            Status = item.Status,
                            ProgressPercentage = item.ProgressPercentage
                        });
                    }

                    res.Result = true;
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

        public ResGetRoutineExercises GetRoutineExercises(ReqGetRoutineExercises req)
        {
            var res = new ResGetRoutineExercises
            {
                Error = new List<Error>(),
                Result = false
            };

            try
            {
                if (req == null || string.IsNullOrEmpty(req.Token) || string.IsNullOrEmpty(req.RoutineName))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.datosFaltantes,
                        Message = "Token o nombre de rutina faltante"
                    });
                    return res;
                }

                using (var db = new FitLife2DataContext())
                {
                    // Obtener la conexión del contexto LINQ
                    System.Data.Common.DbConnection connection = db.Connection;

                    // Abrir la conexión si no está abierta
                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    // Crear el comando directamente
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "sp_GetRoutineExercises";

                        // Agregar parámetros
                        var paramToken = command.CreateParameter();
                        paramToken.ParameterName = "@Token";
                        paramToken.Value = req.Token;
                        command.Parameters.Add(paramToken);

                        var paramRoutineName = command.CreateParameter();
                        paramRoutineName.ParameterName = "@RoutineName";
                        paramRoutineName.Value = req.RoutineName;
                        command.Parameters.Add(paramRoutineName);

                        // Ejecutar el comando y leer los resultados
                        using (var reader = command.ExecuteReader())
                        {
                            // Verificar si hay resultados
                            if (!reader.HasRows)
                            {
                                res.Error.Add(new Error
                                {
                                    ErrorCode = (int)EnumErrores.entidadNoEncontrada,
                                    Message = "No se encontraron ejercicios"
                                });
                                return res;
                            }

                            // Leer la primera fila para verificar si hay error
                            reader.Read();

                            // Verificar si hay un error en el resultado
                            int resultIndex = -1;
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if (reader.GetName(i) == "Result")
                                {
                                    resultIndex = i;
                                    break;
                                }
                            }

                            if (resultIndex >= 0)
                            {
                                string resultValue = reader.GetString(resultIndex);
                                if (resultValue == "FAILED")
                                {
                                    int messageIndex = -1;
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        if (reader.GetName(i) == "Message")
                                        {
                                            messageIndex = i;
                                            break;
                                        }
                                    }

                                    string message = messageIndex >= 0 ? reader.GetString(messageIndex) : "Error al obtener ejercicios";

                                    res.Error.Add(new Error
                                    {
                                        ErrorCode = (int)EnumErrores.entidadNoEncontrada,
                                        Message = message
                                    });
                                    return res;
                                }
                            }

                            // Si no hay error, preparamos para leer todas las filas
                            // Necesitamos regresar al inicio del resultado
                            reader.Close();

                            // Re-ejecutar para leer desde el principio
                            using (var readerData = command.ExecuteReader())
                            {
                                // Crear un diccionario para mapear nombres de columnas a índices
                                Dictionary<string, int> columnMap = new Dictionary<string, int>();

                                // Inicializar el diccionario con los nombres de columnas y sus índices
                                for (int i = 0; i < readerData.FieldCount; i++)
                                {
                                    columnMap[readerData.GetName(i)] = i;
                                }

                                // Leer todas las filas
                                while (readerData.Read())
                                {
                                    var exercise = new RoutineDayExercise();

                                    // Obtener DayNumber
                                    if (columnMap.ContainsKey("DayNumber") && !readerData.IsDBNull(columnMap["DayNumber"]))
                                    {
                                        exercise.DayNumber = Convert.ToInt32(readerData.GetValue(columnMap["DayNumber"]));
                                    }

                                    // Obtener DayName
                                    if (columnMap.ContainsKey("DayName") && !readerData.IsDBNull(columnMap["DayName"]))
                                    {
                                        exercise.DayName = readerData.GetString(columnMap["DayName"]);
                                    }

                                    // Obtener ExerciseName
                                    if (columnMap.ContainsKey("ExerciseName") && !readerData.IsDBNull(columnMap["ExerciseName"]))
                                    {
                                        exercise.ExerciseName = readerData.GetString(columnMap["ExerciseName"]);
                                    }

                                    // Obtener ExerciseDescription
                                    if (columnMap.ContainsKey("ExerciseDescription") && !readerData.IsDBNull(columnMap["ExerciseDescription"]))
                                    {
                                        exercise.ExerciseDescription = readerData.GetString(columnMap["ExerciseDescription"]);
                                    }

                                    // Obtener Category
                                    if (columnMap.ContainsKey("Category") && !readerData.IsDBNull(columnMap["Category"]))
                                    {
                                        exercise.Category = readerData.GetString(columnMap["Category"]);
                                    }

                                    // Obtener TargetMuscleGroup
                                    if (columnMap.ContainsKey("TargetMuscleGroup") && !readerData.IsDBNull(columnMap["TargetMuscleGroup"]))
                                    {
                                        exercise.TargetMuscleGroup = readerData.GetString(columnMap["TargetMuscleGroup"]);
                                    }

                                    // Obtener Sets
                                    if (columnMap.ContainsKey("Sets") && !readerData.IsDBNull(columnMap["Sets"]))
                                    {
                                        exercise.Sets = Convert.ToInt32(readerData.GetValue(columnMap["Sets"]));
                                    }

                                    // Obtener Repetitions
                                    if (columnMap.ContainsKey("Repetitions") && !readerData.IsDBNull(columnMap["Repetitions"]))
                                    {
                                        exercise.Repetitions = Convert.ToInt32(readerData.GetValue(columnMap["Repetitions"]));
                                    }

                                    // Obtener RestTimeSeconds
                                    if (columnMap.ContainsKey("RestTimeSeconds") && !readerData.IsDBNull(columnMap["RestTimeSeconds"]))
                                    {
                                        exercise.RestTimeSeconds = Convert.ToInt32(readerData.GetValue(columnMap["RestTimeSeconds"]));
                                    }

                                    // Obtener Instructions
                                    if (columnMap.ContainsKey("Instructions") && !readerData.IsDBNull(columnMap["Instructions"]))
                                    {
                                        exercise.Instructions = readerData.GetString(columnMap["Instructions"]);
                                    }

                                    // Obtener VideoURL
                                    if (columnMap.ContainsKey("VideoURL") && !readerData.IsDBNull(columnMap["VideoURL"]))
                                    {
                                        exercise.VideoURL = readerData.GetString(columnMap["VideoURL"]);
                                    }

                                    // Obtener ImageURL
                                    if (columnMap.ContainsKey("ImageURL") && !readerData.IsDBNull(columnMap["ImageURL"]))
                                    {
                                        exercise.ImageURL = readerData.GetString(columnMap["ImageURL"]);
                                    }

                                    // Obtener TodayProgress
                                    if (columnMap.ContainsKey("TodayProgress") && !readerData.IsDBNull(columnMap["TodayProgress"]))
                                    {
                                        exercise.TodayProgress = Convert.ToDecimal(readerData.GetValue(columnMap["TodayProgress"]));
                                    }
                                    else
                                    {
                                        exercise.TodayProgress = 0;
                                    }

                                    // Agregar el ejercicio a la lista de resultados
                                    res.Exercises.Add(exercise);
                                }
                            }
                        }
                    }

                    // Si llegamos aquí, todo salió bien
                    res.Result = true;
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
        /// ✅ UPDATED: Enhanced RegisterExerciseProgress with correct field mapping
        /// </summary>
        public ResRegisterExerciseProgress RegisterExerciseProgress(ReqRegisterExerciseProgress req)
        {
            var res = new ResRegisterExerciseProgress
            {
                Error = new List<Error>(),
                Result = false
            };

            try
            {
                #region Validaciones
                if (req == null ||
                    string.IsNullOrEmpty(req.Token) ||
                    string.IsNullOrEmpty(req.RoutineName) ||
                    string.IsNullOrEmpty(req.ExerciseName) ||
                    req.CompletedSets < 0 || req.CompletedRepetitions < 0)
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.datosFaltantes,
                        Message = "Datos incompletos o inválidos para registrar progreso"
                    });
                    return res;
                }
                #endregion

                using (var db = new FitLife2DataContext())
                {
                    DbConnection connection = db.Connection;

                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "sp_RegisterExerciseProgress";

                        void AddParam(string name, object value)
                        {
                            var p = command.CreateParameter();
                            p.ParameterName = name;
                            p.Value = value ?? DBNull.Value;
                            command.Parameters.Add(p);
                        }

                        AddParam("@Token", req.Token);
                        AddParam("@RoutineName", req.RoutineName);
                        AddParam("@ExerciseName", req.ExerciseName);
                        AddParam("@CompletedSets", req.CompletedSets);
                        AddParam("@CompletedRepetitions", req.CompletedRepetitions);
                        AddParam("@Weight", req.Weight);
                        AddParam("@Notes", req.Notes);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string result = SafeGetString(reader, "Result");
                                string message = SafeGetString(reader, "Message");

                                if (result == "FAILED")
                                {
                                    res.Error.Add(new Error
                                    {
                                        ErrorCode = (int)EnumErrores.operacionNoPermitida,
                                        Message = message
                                    });
                                    return res;
                                }

                                res.Message = message;
                                res.Result = true;

                                // ✅ UPDATED: Map enhanced response data with correct field names
                                try
                                {
                                    // Basic exercise information
                                    res.ExerciseName = SafeGetString(reader, "ExerciseName");
                                    res.RoutineName = SafeGetString(reader, "RoutineName");
                                    res.DayNumber = SafeGetInt(reader, "DayNumber");
                                    res.DayName = SafeGetString(reader, "DayName");
                                    res.CompletedSets = SafeGetInt(reader, "CompletedSets");
                                    res.CompletedRepetitions = SafeGetInt(reader, "CompletedRepetitions");
                                    res.Weight = SafeGetDecimal(reader, "Weight");

                                    // Progress indicators
                                    res.ExerciseProgressPercentage = SafeGetDecimal(reader, "ExerciseProgressPercentage");
                                    res.IsExerciseCompleted = SafeGetBool(reader, "IsExerciseCompleted");
                                    res.IsDayCompleted = SafeGetBool(reader, "IsDayCompleted");
                                    res.IsRoutineCompleted = SafeGetBool(reader, "IsRoutineCompleted");

                                    // Today's workout statistics
                                    res.TodayTotalSets = SafeGetInt(reader, "TodayTotalSets");
                                    res.TodayTotalReps = SafeGetInt(reader, "TodayTotalReps");
                                    res.TodayTotalWeight = SafeGetDecimal(reader, "TodayTotalWeight");

                                    // Optional gamification fields (for future use)
                                    res.ExperiencePoints = SafeGetInt(reader, "ExperiencePoints");
                                    res.StreakDays = SafeGetInt(reader, "StreakDays");

                                    // ✅ Enhanced logging for debugging
                                    System.Diagnostics.Debug.WriteLine($"✅ Progreso registrado exitosamente:");
                                    System.Diagnostics.Debug.WriteLine($"   Ejercicio: {res.ExerciseName}");
                                    System.Diagnostics.Debug.WriteLine($"   Sets completados: {res.CompletedSets}/{req.CompletedSets}");
                                    System.Diagnostics.Debug.WriteLine($"   Progreso del ejercicio: {res.ExerciseProgressPercentage:F1}%");
                                    System.Diagnostics.Debug.WriteLine($"   Ejercicio completado: {res.IsExerciseCompleted}");
                                    System.Diagnostics.Debug.WriteLine($"   Día completado: {res.IsDayCompleted}");
                                    System.Diagnostics.Debug.WriteLine($"   Rutina completada: {res.IsRoutineCompleted}");
                                    System.Diagnostics.Debug.WriteLine($"   Total del día - Sets: {res.TodayTotalSets}, Reps: {res.TodayTotalReps}, Peso: {res.TodayTotalWeight}kg");
                                }
                                catch (Exception ex)
                                {
                                    // Log but don't fail the main operation
                                    System.Diagnostics.Debug.WriteLine($"⚠️ Warning reading enhanced progress data: {ex.Message}");
                                }
                            }
                            else
                            {
                                res.Error.Add(new Error
                                {
                                    ErrorCode = (int)EnumErrores.errorDesconocido,
                                    Message = "No se obtuvo respuesta del procedimiento"
                                });
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

                // ✅ Enhanced error logging
                System.Diagnostics.Debug.WriteLine($"❌ Error registrando progreso: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"   Stack trace: {ex.StackTrace}");
            }

            return res;
        }

        /// <summary>
        /// ✅ UPDATED: Enhanced helper methods for safe data reading
        /// </summary>
        private string SafeGetString(DbDataReader reader, string columnName)
        {
            try
            {
                int ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
            }
            catch
            {
                return null;
            }
        }

        private int? SafeGetInt(DbDataReader reader, string columnName)
        {
            try
            {
                int ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? (int?)null : reader.GetInt32(ordinal);
            }
            catch
            {
                return null;
            }
        }

        private decimal? SafeGetDecimal(DbDataReader reader, string columnName)
        {
            try
            {
                int ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? (decimal?)null : reader.GetDecimal(ordinal);
            }
            catch
            {
                return null;
            }
        }

        private bool? SafeGetBool(DbDataReader reader, string columnName)
        {
            try
            {
                int ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? (bool?)null : reader.GetBoolean(ordinal);
            }
            catch
            {
                return null;
            }
        }

        public ResRateRoutine RateRoutine(ReqRateRoutine req)
        {
            var res = new ResRateRoutine
            {
                Error = new List<Error>(),
                Result = false
            };

            try
            {
                #region Validaciones
                if (req == null || string.IsNullOrEmpty(req.Token) || string.IsNullOrEmpty(req.RoutineName))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.datosFaltantes,
                        Message = "Token o nombre de rutina faltante"
                    });
                    return res;
                }

                if (req.DifficultyRating < 1 || req.DifficultyRating > 5 ||
                    req.EffectivenessRating < 1 || req.EffectivenessRating > 5 ||
                    req.EnjoymentRating < 1 || req.EnjoymentRating > 5)
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.valorFueraDeRango,
                        Message = "Las calificaciones deben estar entre 1 y 5"
                    });
                    return res;
                }
                #endregion

                using (var db = new FitLife2DataContext())
                {
                    DbConnection connection = db.Connection;

                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "sp_RateRoutine";

                        void AddParam(string name, object value)
                        {
                            var p = command.CreateParameter();
                            p.ParameterName = name;
                            p.Value = value ?? DBNull.Value;
                            command.Parameters.Add(p);
                        }

                        AddParam("@Token", req.Token);
                        AddParam("@RoutineName", req.RoutineName);
                        AddParam("@DifficultyRating", req.DifficultyRating);
                        AddParam("@EffectivenessRating", req.EffectivenessRating);
                        AddParam("@EnjoymentRating", req.EnjoymentRating);
                        AddParam("@Comments", req.Comments);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var result = reader["Result"].ToString();
                                var message = reader["Message"].ToString();

                                if (result == "FAILED")
                                {
                                    res.Error.Add(new Error
                                    {
                                        ErrorCode = (int)EnumErrores.operacionNoPermitida,
                                        Message = message
                                    });
                                }
                                else
                                {
                                    res.Message = message;
                                    res.Result = true;
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

        public ResAssignRoutine AssignRoutineToUser(ReqAssignRoutine req)
        {
            var res = new ResAssignRoutine
            {
                Error = new List<Error>(),
                Result = false
            };

            try
            {
                if (req == null || string.IsNullOrEmpty(req.Token) || string.IsNullOrEmpty(req.RoutineName))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.datosFaltantes,
                        Message = "Token y nombre de rutina son requeridos"
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
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "sp_AssignRoutineToUser";

                        void AddParam(string name, object value)
                        {
                            var p = cmd.CreateParameter();
                            p.ParameterName = name;
                            p.Value = value ?? DBNull.Value;
                            cmd.Parameters.Add(p);
                        }

                        AddParam("@Token", req.Token);
                        AddParam("@RoutineName", req.RoutineName);
                        AddParam("@TargetUserCedula", req.TargetUserCedula);
                        AddParam("@StartDate", req.StartDate);
                        AddParam("@EndDate", req.EndDate);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var result = reader["Result"].ToString();
                                var message = reader["Message"].ToString();

                                if (result == "FAILED")
                                {
                                    res.Error.Add(new Error
                                    {
                                        ErrorCode = (int)EnumErrores.operacionNoPermitida,
                                        Message = message
                                    });
                                }
                                else
                                {
                                    res.AssignedRoutine = new UserAssignedRoutine
                                    {
                                        UserAssignedRoutineID = reader["UserAssignedRoutineID"] as int? ?? 0,
                                        RoutineName = reader["RoutineName"]?.ToString(),
                                        UserName = reader["UserName"]?.ToString(),
                                        StartDate = reader["StartDate"] as DateTime? ?? DateTime.MinValue,
                                        EndDate = reader["EndDate"] as DateTime? ?? DateTime.MinValue,
                                        Status = reader["Status"]?.ToString()
                                    };
                                    res.Message = message;
                                    res.Result = true;
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

        public ResAddExerciseToRoutine AddExerciseToRoutine(ReqAddExerciseToRoutine req)
        {
            var res = new ResAddExerciseToRoutine
            {
                Error = new List<Error>(),
                Result = false
            };

            try
            {
                if (req == null || string.IsNullOrEmpty(req.Token) || string.IsNullOrEmpty(req.RoutineName) ||
                    string.IsNullOrEmpty(req.ExerciseName) || req.DayNumber <= 0 || req.Sets <= 0 || req.Repetitions <= 0)
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.datosFaltantes,
                        Message = "Datos requeridos faltantes o inválidos"
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
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "sp_AddExerciseToRoutine";

                        void Add(string name, object value)
                        {
                            var p = cmd.CreateParameter();
                            p.ParameterName = name;
                            p.Value = value ?? DBNull.Value;
                            cmd.Parameters.Add(p);
                        }

                        Add("@Token", req.Token);
                        Add("@RoutineName", req.RoutineName);
                        Add("@DayNumber", req.DayNumber);
                        Add("@DayName", req.DayName);
                        Add("@ExerciseName", req.ExerciseName);
                        Add("@Sets", req.Sets);
                        Add("@Repetitions", req.Repetitions);
                        Add("@RestTimeSeconds", req.RestTimeSeconds);
                        Add("@Notes", req.Notes);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var result = reader["Result"].ToString();
                                var message = reader["Message"].ToString();

                                if (result == "FAILED")
                                {
                                    res.Error.Add(new Error
                                    {
                                        ErrorCode = (int)EnumErrores.operacionNoPermitida,
                                        Message = message
                                    });
                                }
                                else
                                {
                                    res.Exercise = new RoutineExercise
                                    {
                                        RoutineExerciseID = reader["RoutineExerciseID"] as int? ?? 0,
                                        RoutineName = reader["RoutineName"]?.ToString(),
                                        DayNumber = reader["DayNumber"] as int? ?? 0,
                                        DayName = reader["DayName"]?.ToString(),
                                        ExerciseName = reader["ExerciseName"]?.ToString(),
                                        CategoryName = reader["CategoryName"]?.ToString(),
                                        Sets = reader["Sets"] as int? ?? 0,
                                        Repetitions = reader["Repetitions"] as int? ?? 0,
                                        RestTimeSeconds = reader["RestTimeSeconds"] as int? ?? 0,
                                        Notes = reader["Notes"]?.ToString()
                                    };
                                    res.Message = message;
                                    res.Result = true;
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

        public ResGetRoutineReport GetRoutinesReport(ReqGetRoutineReport req)
        {
            var res = new ResGetRoutineReport
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
                        Message = "Faltan datos obligatorios"
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
                        cmd.CommandText = "sp_GetRoutinesReport";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        void Add(string name, object value)
                        {
                            var p = cmd.CreateParameter();
                            p.ParameterName = name;
                            p.Value = value ?? DBNull.Value;
                            cmd.Parameters.Add(p);
                        }

                        Add("@Token", req.Token);
                        Add("@StartDate", req.StartDate);
                        Add("@EndDate", req.EndDate);
                        Add("@Status", req.Status);
                        Add("@RoutineName", req.RoutineName);
                        Add("@UserCedula", req.UserCedula);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                res.Error.Add(new Error
                                {
                                    ErrorCode = (int)EnumErrores.reporteNoEncontrado,
                                    Message = "No se encontraron datos para el reporte"
                                });
                                return res;
                            }

                            while (reader.Read())
                            {
                                var entry = new RoutineReportEntry
                                {
                                    Cedula = reader.TryGetString("Cedula"),
                                    FullName = reader.TryGetString("FullName"),
                                    RoutineName = reader.TryGetString("RoutineName"),
                                    RoutineDescription = reader.TryGetString("RoutineDescription"),
                                    DifficultyLevel = reader.TryGetString("DifficultyLevel"),
                                    DurationInDays = reader.TryGetInt("DurationInDays"),
                                    StartDate = reader.TryGetDate("StartDate"),
                                    EndDate = reader.TryGetDate("EndDate"),
                                    Status = reader.TryGetString("Status"),
                                    ProgressPercentage = reader.TryGetDecimal("ProgressPercentage"),
                                    DaysRemaining = reader.TryGetInt("DaysRemaining"),

                                    TimesAssigned = reader.TryGetInt("TimesAssigned"),
                                    DifficultyRating = reader.TryGetDecimal("DifficultyRating"),
                                    EffectivenessRating = reader.TryGetDecimal("EffectivenessRating"),
                                    EnjoymentRating = reader.TryGetDecimal("EnjoymentRating"),
                                    OverallRating = reader.TryGetDecimal("OverallRating"),

                                    AvgDifficultyRating = reader.TryGetDecimal("AvgDifficultyRating"),
                                    AvgEffectivenessRating = reader.TryGetDecimal("AvgEffectivenessRating"),
                                    AvgEnjoymentRating = reader.TryGetDecimal("AvgEnjoymentRating"),
                                    AvgOverallRating = reader.TryGetDecimal("AvgOverallRating"),

                                    ExercisesCompleted = reader.TryGetInt("ExercisesCompleted"),
                                    TotalExercises = reader.TryGetInt("TotalExercises")
                                };

                                res.ReportRoutine.Add(entry);
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