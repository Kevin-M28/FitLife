using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos;
using BackEnd.Entidades;
using BackEnd.Enum;
using BackEnd.Request.Modulo_Asistencia;
using BackEnd.Response;
using BackEnd.Response.Modulo_Asistencia;

namespace BackEnd.Logica.Modulo_Logica
{
    public class LogAsistencia
    {
        public ResRegistrarAsistencia RegistrarAsistencia(ReqRegistrarAsistencia req)
        {
            ResRegistrarAsistencia res = new ResRegistrarAsistencia
            {
                error = new List<Error>(),
                resultado = false,
                asistencia = new Entidades.Asistencia()
            };

            try
            {
                #region Validaciones
                if (req == null)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                    return res;
                }

                if (req.UsuarioID <= 0 || (req.MetodoRegistro == "manual" && req.AdminID <= 0))
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.usuarioNoEncontrado,
                        Message = "Usuario o administrador inválido"
                    });
                    return res;
                }

                if (string.IsNullOrEmpty(req.MetodoRegistro) || !new[] { "QR", "NFC", "manual" }.Contains(req.MetodoRegistro))
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.metodoRegistroFaltante,
                        Message = "Método de registro no válido (debe ser QR, NFC o manual)"
                    });
                    return res;
                }
                #endregion

                using (FitlifeDataContext db = new FitlifeDataContext())
                {
                    // Create the SQL command directly to handle multiple result sets
                    var command = db.Connection.CreateCommand();
                    command.CommandText = "SP_RegistrarAsistencia";
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters with proper null handling
                    command.Parameters.Add(new SqlParameter("@UsuarioID", req.UsuarioID));
                    command.Parameters.Add(new SqlParameter("@MetodoRegistro", req.MetodoRegistro));
                    command.Parameters.Add(new SqlParameter("@AdminID", req.MetodoRegistro == "manual" ? (object)req.AdminID : DBNull.Value));

                    // Handle nullable DateTime parameter
                    var fechaParam = new SqlParameter("@FechaHoraEntrada", SqlDbType.DateTime);
                    fechaParam.Value = req.FechaHoraEntrada != default(DateTime) ? (object)req.FechaHoraEntrada : DBNull.Value;
                    command.Parameters.Add(fechaParam);

                    db.Connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // This handles both entry and exit scenarios
                            res.asistencia.Mensaje = reader["Mensaje"] as string;
                            res.asistencia.UsuarioID = req.UsuarioID;

                            if (!reader.IsDBNull(reader.GetOrdinal("FechaHoraEntrada")))
                            {
                                res.asistencia.FechaHoraEntrada = reader.GetDateTime(reader.GetOrdinal("FechaHoraEntrada"));
                            }

                            // Check for exit scenario columns
                            try
                            {
                                int salidaOrdinal = reader.GetOrdinal("FechaHoraSalida");
                                if (!reader.IsDBNull(salidaOrdinal))
                                {
                                    res.asistencia.FechaHoraSalida = reader.GetDateTime(salidaOrdinal);
                                }
                            }
                            catch (IndexOutOfRangeException) { /* Column not present */ }

                            try
                            {
                                int duracionOrdinal = reader.GetOrdinal("DuracionMinutos");
                                if (!reader.IsDBNull(duracionOrdinal))
                                {
                                    res.asistencia.DuracionMinutos = reader.GetInt32(duracionOrdinal);
                                }
                            }
                            catch (IndexOutOfRangeException) { /* Column not present */ }

                            // Check for entry scenario columns
                            try
                            {
                                int totalAsistOrdinal = reader.GetOrdinal("TotalAsistencias");
                                if (!reader.IsDBNull(totalAsistOrdinal))
                                {
                                    res.asistencia.TotalAsistencias = reader.GetInt32(totalAsistOrdinal);
                                }
                            }
                            catch (IndexOutOfRangeException) { /* Column not present */ }

                            res.asistencia.MetodoRegistro = req.MetodoRegistro;
                            res.resultado = true;
                        }
                        else
                        {
                            res.error.Add(new Error
                            {
                                ErrorCode = (int)EnumErrores.errorDesconocido,
                                Message = "No se pudo registrar la asistencia - posiblemente el usuario no tiene membresía activa o tiene pagos pendientes"
                            });
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                res.error.Add(new Error
                {
                    ErrorCode = (int)EnumErrores.excepcionBaseDatos,
                    Message = sqlEx.Message
                });
            }
            catch (Exception ex)
            {
                res.error.Add(new Error
                {
                    ErrorCode = (int)EnumErrores.excepcionLogica,
                    Message = ex.Message
                });
            }

            return res;
        }
        public ResObtenerHistorialAsistencia ObtenerHistorialAsistencia(ReqObtenerHistorialAsistencia req)
        {
            ResObtenerHistorialAsistencia res = new ResObtenerHistorialAsistencia();
            res.error = new List<Error>();
            res.resultado = false;

            try
            {
                #region Validaciones
                if (req == null)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                    return res;
                }

                if (req.UsuarioID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "UsuarioID inválido"
                    });
                    return res;
                }
                #endregion

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    // Verificamos si el usuario existe
                    if (!linq.Usuario.Any(u => u.UsuarioID == req.UsuarioID))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.usuarioNoEncontrado,
                            Message = "Usuario no encontrado"
                        });
                        return res;
                    }

                    // Inicializamos el objeto de respuesta
                    res.historialAsistencia = new HistorialAsistencia
                    {
                        Detalles = new List<AsistenciaDetalle>(),
                        Resumen = new AsistenciaResumen(),
                        EstadisticasPorDia = new List<AsistenciaPorDiaSemana>(),
                        EstadisticasPorHora = new List<AsistenciaPorHora>()
                    };

                    // Usar ADO.NET directamente para obtener todos los conjuntos de resultados
                    using (var connection = new SqlConnection(linq.Connection.ConnectionString))
                    {
                        connection.Open();

                        using (var command = new SqlCommand("SP_GetHistorialAsistenciaUsuario", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;

                            // Agregar parámetros
                            command.Parameters.AddWithValue("@UsuarioID", req.UsuarioID);

                            // Para parámetros opcionales, usar DBNull.Value si son nulos
                            command.Parameters.AddWithValue("@FechaInicio",
                                req.FechaInicio.HasValue ? (object)req.FechaInicio.Value : DBNull.Value);

                            command.Parameters.AddWithValue("@FechaFin",
                                req.FechaFin.HasValue ? (object)req.FechaFin.Value : DBNull.Value);

                            command.Parameters.AddWithValue("@SoloCompletadas", req.SoloCompletadas);

                            using (var reader = command.ExecuteReader())
                            {
                                // Primer conjunto de resultados: Detalles de asistencia
                                while (reader.Read())
                                {
                                    res.historialAsistencia.Detalles.Add(new AsistenciaDetalle
                                    {
                                        AsistenciaID = reader.GetInt32(reader.GetOrdinal("AsistenciaID")),
                                        FechaHoraEntrada = reader.GetDateTime(reader.GetOrdinal("FechaHoraEntrada")),
                                        FechaHoraSalida = reader.IsDBNull(reader.GetOrdinal("FechaHoraSalida")) ?
                                                          (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaHoraSalida")),
                                        DuracionMinutos = reader.IsDBNull(reader.GetOrdinal("DuracionMinutos")) ?
                                                         (int?)null : reader.GetInt32(reader.GetOrdinal("DuracionMinutos")),
                                        MetodoRegistro = reader.GetString(reader.GetOrdinal("MetodoRegistro")),
                                        Estado = reader.GetString(reader.GetOrdinal("Estado")),
                                        DuracionActual = reader.GetInt32(reader.GetOrdinal("DuracionActual")),
                                        DuracionFormateada = reader.GetString(reader.GetOrdinal("DuracionFormateada"))
                                    });
                                }

                                // Segundo conjunto de resultados: Resumen
                                reader.NextResult();
                                if (reader.Read())
                                {
                                    res.historialAsistencia.Resumen = new AsistenciaResumen
                                    {
                                        TotalAsistencias = reader.GetInt32(reader.GetOrdinal("TotalAsistencias")),
                                        AsistenciasCompletadas = reader.GetInt32(reader.GetOrdinal("AsistenciasCompletadas")),
                                        AsistenciasEnProgreso = reader.GetInt32(reader.GetOrdinal("AsistenciasEnProgreso")),
                                        AsistenciasFinDeSemana = reader.GetInt32(reader.GetOrdinal("AsistenciasFinDeSemana")),
                                        DuracionPromedioMinutos = reader.IsDBNull(reader.GetOrdinal("DuracionPromedioMinutos")) ?
                                                                 0 : reader.GetDouble(reader.GetOrdinal("DuracionPromedioMinutos")),
                                        DuracionMaximaMinutos = reader.IsDBNull(reader.GetOrdinal("DuracionMaximaMinutos")) ?
                                                               0 : reader.GetInt32(reader.GetOrdinal("DuracionMaximaMinutos"))
                                    };
                                }

                                // Tercer conjunto de resultados: Estadísticas por día
                                reader.NextResult();
                                while (reader.Read())
                                {
                                    res.historialAsistencia.EstadisticasPorDia.Add(new AsistenciaPorDiaSemana
                                    {
                                        DiaSemana = reader.GetInt32(reader.GetOrdinal("DiaSemana")),
                                        NombreDia = reader.GetString(reader.GetOrdinal("NombreDia")),
                                        CantidadAsistencias = reader.GetInt32(reader.GetOrdinal("CantidadAsistencias"))
                                    });
                                }

                                // Cuarto conjunto de resultados: Estadísticas por hora
                                reader.NextResult();
                                while (reader.Read())
                                {
                                    res.historialAsistencia.EstadisticasPorHora.Add(new AsistenciaPorHora
                                    {
                                        Hora = reader.GetInt32(reader.GetOrdinal("Hora")),
                                        CantidadAsistencias = reader.GetInt32(reader.GetOrdinal("CantidadAsistencias"))
                                    });
                                }
                            }
                        }
                    }

                    res.resultado = true;
                }
            }
            catch (Exception ex)
            {
                res.error.Add(new Error
                {
                    ErrorCode = (int)EnumErrores.excepcionLogica,
                    Message = ex.Message
                });
            }

            return res;
        }

        public ResReporteAsistencia GenerarReporteAsistencia(ReqReporteAsistencia req)
        {
            ResReporteAsistencia res = new ResReporteAsistencia();
            res.error = new List<Error>();
            res.resultado = false;

            try
            {
                #region Validaciones
                if (req == null)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                    return res;
                }

                if (req.GimnasioID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "GimnasioID inválido"
                    });
                    return res;
                }

                if (string.IsNullOrEmpty(req.Periodo))
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.periodoFaltante,
                        Message = "Debe especificar un periodo (diario, semanal, mensual)"
                    });
                    return res;
                }

                if (req.Periodo != "diario" && req.Periodo != "semanal" && req.Periodo != "mensual")
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.periodoInvalido,
                        Message = "Periodo no válido. Debe ser 'diario', 'semanal' o 'mensual'"
                    });
                    return res;
                }
                #endregion

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    // Verificamos si el gimnasio existe
                    if (!linq.Gimnasio.Any(g => g.GimnasioID == req.GimnasioID))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.gimnasioNoEncontrado,
                            Message = "Gimnasio no encontrado"
                        });
                        return res;
                    }

                    // Usamos ADO.NET para ejecutar el SP y obtener los resultados
                    using (var connection = new SqlConnection(linq.Connection.ConnectionString))
                    {
                        connection.Open();

                        using (var command = new SqlCommand("SP_GenerarReporteAsistencia", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            // Añadir parámetros
                            command.Parameters.AddWithValue("@GimnasioID", req.GimnasioID);
                            command.Parameters.AddWithValue("@Periodo", req.Periodo);

                            // Para parámetros opcionales, usar DBNull.Value si son nulos
                            command.Parameters.AddWithValue("@FechaInicio",
                                req.FechaInicio.HasValue ? (object)req.FechaInicio.Value : DBNull.Value);

                            command.Parameters.AddWithValue("@FechaFin",
                                req.FechaFin.HasValue ? (object)req.FechaFin.Value : DBNull.Value);

                            using (var reader = command.ExecuteReader())
                            {
                                // Leer el primer conjunto de resultados (resumen del reporte)
                                if (reader.Read())
                                {
                                    res.Reporte = new ReporteGeneradoAsistencia
                                    {
                                        ReporteID = reader.GetInt32(reader.GetOrdinal("ReporteID")),
                                        NombreGimnasio = reader.GetString(reader.GetOrdinal("NombreGimnasio")),
                                        FechaGeneracion = reader.GetDateTime(reader.GetOrdinal("FechaGeneracion")),
                                        Periodo = reader.GetString(reader.GetOrdinal("Periodo")),
                                        RangoFechas = reader.GetString(reader.GetOrdinal("RangoFechas")),
                                        TotalAsistencias = reader.GetInt32(reader.GetOrdinal("TotalAsistencias")),
                                        UsuariosUnicos = reader.GetInt32(reader.GetOrdinal("UsuariosUnicos")),
                                        PromedioAsistenciasDiarias = reader.GetDouble(reader.GetOrdinal("PromedioAsistenciasDiarias")),
                                        DuracionPromedioMinutos = reader.GetDouble(reader.GetOrdinal("DuracionPromedioMinutos")),
                                        DatosJSON = reader.IsDBNull(reader.GetOrdinal("JsonCompleto")) ?
                                    null : reader.GetString(reader.GetOrdinal("JsonCompleto"))
                                    };
                                }
                            }
                        }
                    }

                    res.resultado = res.Reporte != null;
                }
            }
            catch (Exception ex)
            {
                res.error.Add(new Error
                {
                    ErrorCode = (int)EnumErrores.excepcionLogica,
                    Message = ex.Message
                });
            }

            return res;
        }


        public ResEstadisticasAsistencia GetEstadisticasAsistencia(ReqEstadisticasAsistencia req)
        {
            ResEstadisticasAsistencia res = new ResEstadisticasAsistencia
            {
                error = new List<Error>(),
                resultado = false,
                Estadisticas = new EstadisticasAsistencia()
            };

            try
            {
                #region Validaciones
                if (req == null)
                {
                    res.error.Add(new Error { ErrorCode = (int)EnumErrores.requestNulo, Message = "Request nulo" });
                    return res;
                }
                if (req.GimnasioID <= 0)
                {
                    res.error.Add(new Error { ErrorCode = (int)EnumErrores.gimnasioInvalido, Message = "Gimnasio inválido" });
                    return res;
                }
                #endregion

                using (FitlifeDataContext db = new FitlifeDataContext())
                {
                    // Create the SQL command directly
                    var command = db.Connection.CreateCommand();
                    command.CommandText = "SP_GetEstadisticasAsistencia";
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    command.Parameters.Add(new SqlParameter("@GimnasioID", req.GimnasioID));
                    command.Parameters.Add(new SqlParameter("@FechaInicio", req.FechaInicio ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@FechaFin", req.FechaFin ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@TipoEstadistica", req.TipoEstadistica ?? "todo"));


                    db.Connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        // Process each result set based on the TipoEstadistica
                        if (req.TipoEstadistica == "todo" || req.TipoEstadistica == "general")
                        {
                            if (reader.Read())
                            {
                                res.Estadisticas.Generales = new EstadisticasGenerales
                                {
                                    UsuariosUnicos = reader["UsuariosUnicos"] as int? ?? 0,
                                    TotalAsistencias = reader["TotalAsistencias"] as int? ?? 0,
                                    DuracionPromedioMinutos = reader["DuracionPromedioMinutos"] as double? ?? 0,
                                    PromedioAsistenciasDiarias = Convert.ToDouble(reader["PromedioAsistenciasDiarias"] as decimal? ?? 0),
                                    MaximoAsistenciasPorUsuario = reader["MaximoAsistenciasPorUsuario"] as int? ?? 0
                                };
                            }
                            reader.NextResult();
                        }

                        if (req.TipoEstadistica == "todo" || req.TipoEstadistica == "frecuencia")
                        {
                            res.Estadisticas.UsuariosFrecuentes = new List<UsuarioFrecuente>();
                            while (reader.Read())
                            {
                                res.Estadisticas.UsuariosFrecuentes.Add(new UsuarioFrecuente
                                {
                                    UsuarioID = reader["UsuarioID"] as int? ?? 0,
                                    NombreCompleto = reader["NombreCompleto"] as string ?? string.Empty,
                                    CantidadAsistencias = reader["CantidadAsistencias"] as int? ?? 0,
                                    UltimaAsistencia = reader["UltimaAsistencia"] as DateTime? ?? DateTime.MinValue,
                                    DuracionPromedioMinutos = reader["DuracionPromedioMinutos"] as double? ?? 0
                                });
                            }
                            reader.NextResult();
                        }

                        if (req.TipoEstadistica == "todo" || req.TipoEstadistica == "pico")
                        {
                            // First result set for pico: Asistencias por día
                            res.Estadisticas.AsistenciasPorDia = new List<AsistenciaPorDia>();
                            while (reader.Read())
                            {
                                res.Estadisticas.AsistenciasPorDia.Add(new AsistenciaPorDia
                                {
                                    DiaSemana = reader["DiaSemana"] as int? ?? 0,
                                    NombreDia = reader["NombreDia"] as string ?? string.Empty,
                                    CantidadAsistencias = reader["CantidadAsistencias"] as int? ?? 0,
                                    UsuariosUnicos = reader["UsuariosUnicos"] as int? ?? 0
                                });
                            }
                            reader.NextResult();

                            // Second result set for pico: Asistencias por hora
                            res.Estadisticas.AsistenciasPorHora = new List<AsistenciaPorHoraE>();
                            while (reader.Read())
                            {
                                res.Estadisticas.AsistenciasPorHora.Add(new AsistenciaPorHoraE
                                {
                                    Hora = reader["Hora"] as int? ?? 0,
                                    FranjaHoraria = reader["FranjaHoraria"] as string ?? string.Empty,
                                    CantidadAsistencias = reader["CantidadAsistencias"] as int? ?? 0,
                                    UsuariosUnicos = reader["UsuariosUnicos"] as int? ?? 0
                                });
                            }
                            reader.NextResult();

                            // Third result set for pico: Asistencias por fecha
                            res.Estadisticas.AsistenciasPorFecha = new List<AsistenciaPorFecha>();
                            while (reader.Read())
                            {
                                res.Estadisticas.AsistenciasPorFecha.Add(new AsistenciaPorFecha
                                {
                                    Fecha = reader["Fecha"] as DateTime? ?? DateTime.MinValue,
                                    CantidadAsistencias = reader["CantidadAsistencias"] as int? ?? 0,
                                    UsuariosUnicos = reader["UsuariosUnicos"] as int? ?? 0
                                });
                            }
                            reader.NextResult();
                        }

                        if (req.TipoEstadistica == "todo" || req.TipoEstadistica == "duracion")
                        {
                            // First result set for duracion: Asistencias por rango de duración
                            res.Estadisticas.AsistenciasPorDuracion = new List<AsistenciaPorDuracion>();
                            while (reader.Read())
                            {
                                res.Estadisticas.AsistenciasPorDuracion.Add(new AsistenciaPorDuracion
                                {
                                    RangoDuracion = reader["RangoDuracion"] as string ?? string.Empty,
                                    CantidadAsistencias = reader["CantidadAsistencias"] as int? ?? 0,
                                    DuracionPromedioMinutos = reader["DuracionPromedioMinutos"] as double? ?? 0
                                });
                            }
                            reader.NextResult();

                            // Second result set for duracion: Usuarios por tiempo total
                            res.Estadisticas.UsuariosPorTiempo = new List<UsuarioPorTiempo>();
                            while (reader.Read())
                            {
                                res.Estadisticas.UsuariosPorTiempo.Add(new UsuarioPorTiempo
                                {
                                    UsuarioID = reader["UsuarioID"] as int? ?? 0,
                                    NombreCompleto = reader["NombreCompleto"] as string ?? string.Empty,
                                    CantidadAsistencias = reader["CantidadAsistencias"] as int? ?? 0,
                                    TiempoTotalMinutos = reader["TiempoTotalMinutos"] as int? ?? 0,
                                    DuracionPromedioMinutos = reader["DuracionPromedioMinutos"] as double? ?? 0,
                                    DuracionMaximaMinutos = reader["DuracionMaximaMinutos"] as int? ?? 0
                                });
                            }
                        }
                    }

                    res.resultado = true;
                }
            }
            catch (Exception ex)
            {
                res.error.Add(new Error
                {
                    ErrorCode = (int)EnumErrores.excepcionLogica,
                    Message = ex.Message
                });
            }

            return res;
        }
    }
}
            
   

