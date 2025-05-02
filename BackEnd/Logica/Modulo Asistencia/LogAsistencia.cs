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
                    if (!linq.Usuarios.Any(u => u.UsuarioID == req.UsuarioID))
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
                    if (!linq.Gimnasios.Any(g => g.GimnasioID == req.GimnasioID))
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
    }
}
            
   

