using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using AccesoDatos;
using BackEnd.Enum;
using BackEnd.Request.Modulo_Usuario;
using BackEnd.Response.Modulo_Usuario;
using BackEnd.Entidades;
using Usuario = BackEnd.Entidades.Usuario;
using System.Data;
using System.Data.Linq;

namespace BackEnd.Logica.Modulo_Usuario
{
    public class LogicaUsuario
    {
        // Registration doesn't require session validation
        public ResAgregarUsuario Registrar(ReqAgregarUsuario req)
        {
            ResAgregarUsuario res = new ResAgregarUsuario();
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
                }
                else
                {
                    if (string.IsNullOrEmpty(req.Nombre))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.nombreFaltante,
                            Message = "Nombre vacío"
                        });
                    }

                    if (string.IsNullOrEmpty(req.Apellido))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.apellidoFaltante,
                            Message = "Apellido vacío"
                        });
                    }

                    if (string.IsNullOrEmpty(req.Cedula))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.cedulaFaltante,
                            Message = "Cédula vacía"
                        });
                    }

                    if (string.IsNullOrEmpty(req.correoElectronico))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.correoFaltante,
                            Message = "Correo vacío"
                        });
                    }
                    else if (!EsCorreoValido(req.correoElectronico))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.correoIncorrecto,
                            Message = "Correo no válido"
                        });
                    }

                    if (string.IsNullOrEmpty(req.password))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.passwordFaltante,
                            Message = "Contraseña vacía"
                        });
                    }
                    else if (!EsPasswordSeguro(req.password))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.passwordMuyDebil,
                            Message = "La contraseña debe tener al menos 8 caracteres, una mayúscula, un número y un carácter especial"
                        });
                    }

                    if (req.GimnasioID <= 0)
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.gimnasioFaltante,
                            Message = "Debe seleccionar un gimnasio"
                        });
                    }

                    if (string.IsNullOrEmpty(req.telefono))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.telefonoFaltante,
                            Message = "Teléfono vacío"
                        });
                    }
                }
                #endregion

                if (res.error.Any())
                {
                    return res;
                }

                string llave = Guid.NewGuid().ToString("N");
                string passHash = HashearPassword(req.password, llave);

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    var resultado = linq.SP_RegistrarUsuario(
                        req.GimnasioID,
                        req.Cedula,
                        req.Nombre,
                        req.Apellido,
                        req.correoElectronico,
                        passHash,
                        req.telefono,
                        "usuario",
                        "activo",
                        llave
                    ).FirstOrDefault();

                    if (resultado != null && resultado.UsuarioID > 0)
                    {
                        res.Nombre = resultado.Nombre;
                        res.resultado = true;
                    }
                    else
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.errorDesconocido,
                            Message = "No se pudo registrar el usuario"
                        });
                    }
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

        // Login doesn't require session validation
        public ResLoginUsuario LoginUsuario(ReqLoginUsuario req)
        {
            var res = new ResLoginUsuario();
            res.error = new List<Error>();

            try
            {
                if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
                {
                    res.resultado = false;
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.datosIncompletos,
                        Message = "Correo electrónico o contraseña vacíos"
                    });
                    return res;
                }

                using (var db = new FitlifeDataContext())
                {
                    var llaveUsuario = db.Usuarios
                        .Where(u => u.Email == req.Email)
                        .Select(u => u.Llave)
                        .FirstOrDefault();

                    if (string.IsNullOrEmpty(llaveUsuario))
                    {
                        res.resultado = false;
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.credencialesInvalidas,
                            Message = "Correo electrónico o contraseña incorrectos"
                        });
                        return res;
                    }

                    string passwordHasheado = HashearPassword(req.Password, llaveUsuario);
                    int sessionActiva = 1;

                    var resultado = db.SP_LoginUsuario(req.Email, passwordHasheado, sessionActiva).FirstOrDefault();

                    if (resultado == null)
                    {
                        res.resultado = false;
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.credencialesInvalidas,
                            Message = "Credenciales inválidas o usuario inactivo"
                        });
                        return res;
                    }


                    res.Token = resultado.Token;
                    res.resultado = true;
                }
            }
            catch (Exception ex)
            {
                res.resultado = false;
                res.error.Add(new Error
                {
                    ErrorCode = (int)EnumErrores.excepcionLogica,
                    Message = ex.Message
                });
            }

            return res;
        }

        // CambiarContrasenna requires session validation
        public ResCambiarContrasenna CambiarContrasenna(ReqCambiarContrasenna req)
        {
            ResCambiarContrasenna res = new ResCambiarContrasenna();
            res.error = new List<Error>();
            res.resultado = false;

            // Validate session token
            List<Error> erroresSesion = new List<Error>();
            var sesion = SessionValidator.ValidarSesion(req.Token, out erroresSesion);
            if (sesion == null)
            {
                res.error.AddRange(erroresSesion);
                return res;
            }

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
                        Message = "Usuario no válido"
                    });
                }

                // Check if the user ID matches the session user ID
                if (req.UsuarioID != sesion.Id_User)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.permisoInsuficiente,
                        Message = "No tiene permisos para cambiar la contraseña de otro usuario"
                    });
                    return res;
                }

                if (string.IsNullOrWhiteSpace(req.passwordActual))
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.passwordFaltante,
                        Message = "Debe ingresar la contraseña actual"
                    });
                }

                if (string.IsNullOrWhiteSpace(req.nuevaPassword))
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.passwordFaltante,
                        Message = "Debe ingresar la nueva contraseña"
                    });
                }
                else if (!EsPasswordSeguro(req.nuevaPassword))
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.passwordMuyDebil,
                        Message = "La nueva contraseña no cumple con los requisitos de seguridad"
                    });
                }

                if (res.error.Any())
                    return res;
                #endregion

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    var usuario = linq.Usuarios.FirstOrDefault(u => u.UsuarioID == req.UsuarioID);
                    if (usuario == null)
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.usuarioNoEncontrado,
                            Message = "Usuario no encontrado"
                        });
                        return res;
                    }

                    string hashActual = HashearPassword(req.passwordActual, usuario.Llave);

                    if (hashActual != usuario.ContrasennaHash)
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.credencialesInvalidas,
                            Message = "La contraseña actual es incorrecta"
                        });
                        return res;
                    }

                    string nuevaLlave = Guid.NewGuid().ToString("N");
                    string nuevoHash = HashearPassword(req.nuevaPassword, nuevaLlave);

                    var resultado = linq.SP_CambiarPassword(
                        req.UsuarioID,
                        nuevaLlave,
                        nuevoHash
                    ).FirstOrDefault();

                    if (resultado != null && resultado.Mensaje == "Contraseña actualizada exitosamente")
                    {
                        res.resultado = true;
                        res.Mensaje = resultado.Mensaje;
                    }
                    else
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.errorDesconocido,
                            Message = "No se pudo cambiar la contraseña"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                res.resultado = false;
                res.error.Add(new Error
                {
                    ErrorCode = (int)EnumErrores.excepcionLogica,
                    Message = ex.Message.Contains("La contraseña actual es incorrecta")
                            ? "La contraseña actual es incorrecta"
                            : "Error al cambiar la contraseña"
                });
            }

            return res;
        }

        // AsignarRolUsuario requires session validation and admin privileges
        public ResAsignarRolUsuario AsignarRolUsuario(ReqAsignarRolUsuario req)
        {
            ResAsignarRolUsuario res = new ResAsignarRolUsuario();
            res.error = new List<Error>();
            res.resultado = false;

            // Validate session token
            List<Error> erroresSesion = new List<Error>();
            var sesion = SessionValidator.ValidarSesion(req.Token, out erroresSesion);
            if (sesion == null)
            {
                res.error.AddRange(erroresSesion);
                return res;
            }

            try
            {
                if (req == null)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                    return res;
                }
                if (req.usuarioID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "UsuarioID vacío"
                    });
                    return res;
                }
                if (string.IsNullOrEmpty(req.nuevoRol))
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.rolFaltante,
                        Message = "Rol vacío"
                    });
                    return res;
                }

                // Ensure the admin ID matches the session user ID
                if (req.usuarioAdmin != sesion.Id_User)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.permisoInsuficiente,
                        Message = "No tiene permisos para realizar esta acción"
                    });
                    return res;
                }

                // Verify admin role
                using (FitlifeDataContext checkDb = new FitlifeDataContext())
                {
                    var isAdmin = checkDb.Usuarios
                        .Where(u => u.UsuarioID == sesion.Id_User && u.Rol == "admin")
                        .Any();

                    if (!isAdmin)
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.permisoInsuficiente,
                            Message = "Se requieren privilegios de administrador para esta operación"
                        });
                        return res;
                    }
                }

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    var resultado = linq.SP_AsignarRolUsuario(req.usuarioID, req.nuevoRol, req.usuarioAdmin, "").FirstOrDefault();
                    if (resultado == null || resultado.UsuarioID <= 0)
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.usuarioNoEncontrado,
                            Message = "Usuario no encontrado"
                        });
                        return res;
                    }

                    res.Mensaje = resultado.Mensaje;
                    res.usuario = new Usuario
                    {
                        Id = resultado.UsuarioID,
                        Nombre = resultado.Nombre,
                        Apellido = resultado.Apellido,
                        correoElectronico = resultado.Email,
                        rol = resultado.Rol == "admin" ? EnumRolUsuario.admin : EnumRolUsuario.usuario,
                        estado = resultado.Estado == "activo" ? EnumEstadoUsuario.activo :
                                resultado.Estado == "suspendido" ? EnumEstadoUsuario.suspendido : EnumEstadoUsuario.inactivo
                    };
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

        // ObtenerUsuarioPorEmail requires session validation
        public ResObtenerUsuario ObtenerUsuarioPorEmail(ReqObtenerUsuario req)
        {
            ResObtenerUsuario res = new ResObtenerUsuario();
            res.error = new List<Error>();  // Cambiar esto a List<Error>
            res.resultado = false;

            // Validate session token
            List<Error> erroresSesion = new List<Error>();
            var sesion = SessionValidator.ValidarSesion(req.Token, out erroresSesion);
            if (sesion == null)
            {
                res.error.AddRange(erroresSesion);
                return res;
            }

            try
            {
                if (req == null)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                    return res;
                }

                if (string.IsNullOrEmpty(req.email))
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "Email vacío"
                    });
                    return res;
                }

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    using (SqlConnection connection = new SqlConnection(linq.Connection.ConnectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand("SP_GetUsuarioPorEmail", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Email", req.email);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // 1. Información básica del usuario
                                if (reader.Read())
                                {
                                    res.usuario = new Entidades.Usuario
                                    {

                                        Cedula = reader.GetString(reader.GetOrdinal("Cedula")),
                                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                        Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                                        correoElectronico = reader.GetString(reader.GetOrdinal("Email")),
                                        telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                                        rol = reader.GetString(reader.GetOrdinal("Rol")) == "admin" ?
                                            EnumRolUsuario.admin : EnumRolUsuario.usuario,
                                        estado = reader.GetString(reader.GetOrdinal("Estado")) == "activo" ?
                                            EnumEstadoUsuario.activo :
                                            reader.GetString(reader.GetOrdinal("Estado")) == "suspendido" ?
                                            EnumEstadoUsuario.suspendido : EnumEstadoUsuario.inactivo,
                                        nombreGimnasio = reader.GetString(reader.GetOrdinal("NombreGimnasio")),

                                    };
                                }
                                else
                                {
                                    res.error.Add(new Error
                                    {
                                        ErrorCode = (int)EnumErrores.usuarioNoEncontrado,
                                        Message = "Usuario no encontrado"
                                    });
                                    return res;
                                }

                                // 2. Membresía activa actual
                                if (reader.NextResult() && reader.Read())
                                {
                                    res.membresia = new Entidades.UsuarioMembresia
                                    {
                                        UsuarioMembresiaID = reader.GetInt32(reader.GetOrdinal("UsuarioMembresiaID")),
                                        TipoMembresia = reader.GetString(reader.GetOrdinal("TipoMembresia")),
                                        Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                                        FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                                        FechaVencimiento = reader.GetDateTime(reader.GetOrdinal("FechaVencimiento")),
                                        Estado = reader.GetString(reader.GetOrdinal("EstadoMembresia")),
                                        DiasRestantes = reader.IsDBNull(reader.GetOrdinal("DiasRestantes")) ?
                                            0 : reader.GetInt32(reader.GetOrdinal("DiasRestantes"))
                                    };
                                }

                                // 3. Última métrica corporal - CORREGIDO para C# 7.3
                                if (reader.NextResult() && reader.Read())
                                {
                                    res.metricaCorporal = new Entidades.MetricaCorporal
                                    {
                                        MetricaID = reader.GetInt32(reader.GetOrdinal("MetricaID")),
                                        Peso = reader.GetDecimal(reader.GetOrdinal("Peso")),
                                        Altura = reader.GetDecimal(reader.GetOrdinal("Altura")),
                                        // Usar método helper para nullable decimals
                                        IMC = DecimalHelper.GetNullableDecimal(reader, "IMC"),
                                        PorcentajeGrasa = DecimalHelper.GetNullableDecimal(reader, "PorcentajeGrasa"),
                                        Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha"))
                                    };
                                }

                                // 4. Estadísticas de asistencia - CORREGIDO
                                if (reader.NextResult() && reader.Read())
                                {
                                    res.estadisticasAsistencia = new Entidades.EstadisticasAsistenciaU
                                    {
                                        TotalAsistencias = reader.GetInt32(reader.GetOrdinal("TotalAsistencias")),
                                        UltimaAsistencia = reader.IsDBNull(reader.GetOrdinal("UltimaAsistencia")) ?
                                            (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UltimaAsistencia")),
                                        AsistenciasUltimoMes = reader.GetInt32(reader.GetOrdinal("AsistenciasUltimoMes")),
                                        AsistenciasUltimaSemana = reader.GetInt32(reader.GetOrdinal("AsistenciasUltimaSemana")),
                                        DuracionPromedioMinutos = DecimalHelper.GetNullableDouble(reader, "DuracionPromedioMinutos"),
                                        DiaMasFrecuente = reader.IsDBNull(reader.GetOrdinal("DiaMasFrecuente")) ?
                                            null : reader.GetString(reader.GetOrdinal("DiaMasFrecuente"))
                                    };
                                }

                                // 5. Pagos recientes
                                if (reader.NextResult())
                                {
                                    res.pagosRecientes = new List<Entidades.Pago>();
                                    while (reader.Read())
                                    {
                                        res.pagosRecientes.Add(new Entidades.Pago
                                        {
                                            PagoID = reader.GetInt32(reader.GetOrdinal("PagoID")),
                                            Monto = reader.GetDecimal(reader.GetOrdinal("Monto")),
                                            FechaPago = reader.GetDateTime(reader.GetOrdinal("FechaPago")),
                                            MetodoPago = reader.GetString(reader.GetOrdinal("MetodoPago")),
                                            Estado = reader.GetString(reader.GetOrdinal("EstadoPago")),
                                            EstadoDescripcion = reader.GetString(reader.GetOrdinal("DescripcionEstadoPago"))
                                        });
                                    }
                                }

                                // 6. Notificaciones no leídas
                                if (reader.NextResult() && reader.Read())
                                {
                                    res.resumenNotificaciones = new Entidades.ResumenNotificaciones
                                    {
                                        NotificacionesNoLeidas = reader.GetInt32(reader.GetOrdinal("NotificacionesNoLeidas")),
                                        NotificacionesPago = reader.GetInt32(reader.GetOrdinal("NotificacionesPago")),
                                        NotificacionesLogro = reader.GetInt32(reader.GetOrdinal("NotificacionesLogro")),
                                        NotificacionesAnuncio = reader.GetInt32(reader.GetOrdinal("NotificacionesAnuncio"))
                                    };
                                }

                                // 7. Estado de morosidad
                                if (reader.NextResult() && reader.Read())
                                {
                                    res.estadoMorosidad = new Entidades.EstadoMorosidad
                                    {
                                        EstaEnMorosidad = reader.GetInt32(reader.GetOrdinal("EstaEnMorosidad")),
                                        DiasRetraso = reader.GetInt32(reader.GetOrdinal("DiasRetraso"))
                                    };
                                }

                                // 8. Rutina activa actual
                                if (reader.NextResult() && reader.Read())
                                {
                                    res.rutinaActiva = new Entidades.RutinaActiva
                                    {
                                        RutinaID = reader.GetInt32(reader.GetOrdinal("RutinaID")),
                                        NombreRutina = reader.GetString(reader.GetOrdinal("NombreRutina")),
                                        DescripcionRutina = reader.GetString(reader.GetOrdinal("DescripcionRutina")),
                                        FechaAsignacion = reader.GetDateTime(reader.GetOrdinal("FechaAsignacion")),
                                        DiasDesdeAsignacion = reader.GetInt32(reader.GetOrdinal("DiasDesdeAsignacion")),
                                        AsignadaPor = reader.IsDBNull(reader.GetOrdinal("AsignadaPor")) ?
                                            null : reader.GetString(reader.GetOrdinal("AsignadaPor"))
                                    };
                                }

                                // 9. Metas activas
                                if (reader.NextResult())
                                {
                                    res.metasActivas = new List<Entidades.MetaUsuario>();
                                    while (reader.Read())
                                    {
                                        res.metasActivas.Add(new Entidades.MetaUsuario
                                        {
                                            MetaID = reader.GetInt32(reader.GetOrdinal("MetaID")),
                                            TipoMeta = reader.GetString(reader.GetOrdinal("TipoMeta")),
                                            ValorObjetivo = reader.GetDecimal(reader.GetOrdinal("ValorObjetivo")),
                                            FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                                            FechaObjetivo = reader.GetDateTime(reader.GetOrdinal("FechaObjetivo")),
                                            DiasRestantes = reader.GetInt32(reader.GetOrdinal("DiasRestantes")),
                                            Estado = "activa"
                                        });
                                    }
                                }

                                // 10. Logros obtenidos
                                if (reader.NextResult())
                                {
                                    res.logrosObtenidos = new List<Entidades.Logro>();
                                    while (reader.Read())
                                    {
                                        res.logrosObtenidos.Add(new Entidades.Logro
                                        {
                                            LogroID = reader.GetInt32(reader.GetOrdinal("LogroID")),
                                            Nombre = reader.GetString(reader.GetOrdinal("NombreLogro")),
                                            Descripcion = reader.GetString(reader.GetOrdinal("DescripcionLogro")),
                                            Tipo = reader.GetString(reader.GetOrdinal("TipoLogro")),
                                            FechaObtenido = reader.GetDateTime(reader.GetOrdinal("FechaObtenido"))
                                        });
                                    }
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

        // ActualizarUsuario requires session validation
        public ResActualizarUsuario ActualizarUsuario(ReqActualizarUsuario req)
        {
            ResActualizarUsuario res = new ResActualizarUsuario();
            res.error = new List<Error>();
            res.resultado = false;

            // Validate session token
            List<Error> erroresSesion = new List<Error>();
            var sesion = SessionValidator.ValidarSesion(req.Token, out erroresSesion);
            if (sesion == null)
            {
                res.error.AddRange(erroresSesion);
                return res;
            }

            try
            {
                if (req == null)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                    return res;
                }
                if (req.usuarioID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "UsuarioID vacío"
                    });
                    return res;
                }

                // Ensure the user is updating their own profile or is an admin
                if (req.usuarioID != sesion.Id_User)
                {
                    // Check if the session user is an admin
                    using (FitlifeDataContext checkDb = new FitlifeDataContext())
                    {
                        var isAdmin = checkDb.Usuarios
                            .Where(u => u.UsuarioID == sesion.Id_User && u.Rol == "admin")
                            .Any();

                        if (!isAdmin)
                        {
                            res.error.Add(new Error
                            {
                                ErrorCode = (int)EnumErrores.permisoInsuficiente,
                                Message = "No tiene permisos para actualizar otro usuario"
                            });
                            return res;
                        }
                    }
                }

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    var resultado = linq.SP_UpdateUsuario(req.usuarioID, req.nombre, req.apellido, req.email, req.telefono).FirstOrDefault();
                    if (resultado == null || resultado.UsuarioID <= 0)
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.usuarioNoEncontrado,
                            Message = "Usuario no encontrado"
                        });
                        return res;
                    }

                    res.usuario = new Usuario
                    {
                        Id = resultado.UsuarioID,
                        Cedula = resultado.Cedula,
                        Nombre = resultado.Nombre,
                        Apellido = resultado.Apellido,
                        correoElectronico = resultado.Email,
                        telefono = resultado.Telefono,
                        rol = resultado.Rol == "admin" ? EnumRolUsuario.admin : EnumRolUsuario.usuario,
                        estado = resultado.Estado == "activo" ? EnumEstadoUsuario.activo :
                                resultado.Estado == "suspendido" ? EnumEstadoUsuario.suspendido : EnumEstadoUsuario.inactivo
                    };
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

        // EliminarUsuario requires session validation and admin privileges
        public ResEliminarUsuario EliminarUsuario(ReqEliminarUsuario req)
        {
            ResEliminarUsuario res = new ResEliminarUsuario();
            res.error = new List<Error>();
            res.resultado = false;

            // Validate session token
            List<Error> erroresSesion = new List<Error>();
            var sesion = SessionValidator.ValidarSesion(req.Token, out erroresSesion);
            if (sesion == null)
            {
                res.error.AddRange(erroresSesion);
                return res;
            }

            try
            {
                if (req == null)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                    return res;
                }
                if (req.usuarioID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "UsuarioID vacío"
                    });
                    return res;
                }

                // Verify admin privileges
                using (FitlifeDataContext checkDb = new FitlifeDataContext())
                {
                    var isAdmin = checkDb.Usuarios
                        .Where(u => u.UsuarioID == sesion.Id_User && u.Rol == "admin")
                        .Any();

                    if (!isAdmin)
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.permisoInsuficiente,
                            Message = "Se requieren privilegios de administrador para eliminar usuarios"
                        });
                        return res;
                    }
                }

                // Prevent self-deletion
                if (req.usuarioID == sesion.Id_User)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.operacionNoPermitida,
                        Message = "No puede eliminar su propia cuenta"
                    });
                    return res;
                }

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    // Example stored procedure call for user deletion
                    // Adjust according to your actual stored procedure
                    var command = linq.Connection.CreateCommand();
                    command.CommandText = "SP_EliminarUsuario";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@UsuarioID", req.usuarioID));
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AdminID", sesion.Id_User));

                    linq.Connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    linq.Connection.Close();

                    if (rowsAffected > 0)
                    {
                        res.resultado = true;
                    }
                    else
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.usuarioNoEncontrado,
                            Message = "No se pudo eliminar el usuario. Verifique que exista."
                        });
                    }
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

        // SuspenderUsuario requires session validation and admin privileges
        public ResSuspenderUsuario SuspenderUsuario(ReqSuspenderUsuario req)
        {
            ResSuspenderUsuario res = new ResSuspenderUsuario();
            res.error = new List<Error>();
            res.resultado = false;

            // Validate session token
            List<Error> erroresSesion = new List<Error>();
            var sesion = SessionValidator.ValidarSesion(req.Token, out erroresSesion);
            if (sesion == null)
            {
                res.error.AddRange(erroresSesion);
                return res;
            }

            try
            {
                if (req == null)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                    return res;
                }
                if (req.usuarioID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "UsuarioID vacío"
                    });
                    return res;
                }

                // Verify admin privileges
                using (FitlifeDataContext checkDb = new FitlifeDataContext())
                {
                    var isAdmin = checkDb.Usuarios
                        .Where(u => u.UsuarioID == sesion.Id_User && u.Rol == "admin")
                        .Any();

                    if (!isAdmin)
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.permisoInsuficiente,
                            Message = "Se requieren privilegios de administrador para suspender usuarios"
                        });
                        return res;
                    }
                }

                // Prevent self-suspension
                if (req.usuarioID == sesion.Id_User)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.operacionNoPermitida,
                        Message = "No puede suspender su propia cuenta"
                    });
                    return res;
                }

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    // Example stored procedure call for user suspension
                    // Adjust according to your actual stored procedure
                    var command = linq.Connection.CreateCommand();
                    command.CommandText = "SP_ActualizarEstadoUsuario";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@UsuarioID", req.usuarioID));
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@NuevoEstado", "suspendido"));
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AdminID", sesion.Id_User));
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Motivo", req.motivo ?? "No especificado"));

                    linq.Connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    linq.Connection.Close();

                    if (rowsAffected > 0)
                    {
                        res.resultado = true;
                    }
                    else
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.usuarioNoEncontrado,
                            Message = "No se pudo suspender el usuario. Verifique que exista."
                        });
                    }
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

        // Method to verify a user after registration (no session required)
        public ResVerificarUsuario VerificarUsuario(ReqVerificarUsuario req)
        {
            ResVerificarUsuario res = new ResVerificarUsuario();
            res.error = new List<Error>();
            res.resultado = false;

            try
            {
                if (req == null)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                    return res;
                }

                if (string.IsNullOrEmpty(req.correo))
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.correoFaltante,
                        Message = "Correo vacío"
                    });
                    return res;
                }

                if (string.IsNullOrEmpty(req.codigo))
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.verificacionFallida,
                        Message = "Código de verificación vacío"
                    });
                    return res;
                }

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    // Example stored procedure call for user verification
                    // Adjust according to your actual stored procedure
                    var command = linq.Connection.CreateCommand();
                    command.CommandText = "SP_VerificarUsuario";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Email", req.correo));
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Codigo", req.codigo));

                    linq.Connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool resultado = reader.GetBoolean(reader.GetOrdinal("Resultado"));
                            if (resultado)
                            {
                                res.resultado = true;
                            }
                            else
                            {
                                string mensaje = reader.GetString(reader.GetOrdinal("Mensaje"));
                                res.error.Add(new Error
                                {
                                    ErrorCode = (int)EnumErrores.verificacionFallida,
                                    Message = mensaje
                                });
                            }
                        }
                        else
                        {
                            res.error.Add(new Error
                            {
                                ErrorCode = (int)EnumErrores.verificacionFallida,
                                Message = "Error al verificar el usuario"
                            });
                        }
                    }
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

        // Method to request password reset (no session required)
        public ResResetContrasennaSolicitar SolicitarCambioContrasenna(ReqSolicitarCambioContrasenna req)
        {
            ResResetContrasennaSolicitar res = new ResResetContrasennaSolicitar();
            res.error = new List<Error>();
            res.resultado = false;

            try
            {
                if (req == null)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                    return res;
                }

                if (string.IsNullOrEmpty(req.Email))
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.correoFaltante,
                        Message = "Correo vacío"
                    });
                    return res;
                }

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    // Example stored procedure call for password reset request
                    // Adjust according to your actual stored procedure
                    var command = linq.Connection.CreateCommand();
                    command.CommandText = "SP_ResetPassword_Solicitar";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Email", req.Email));

                    linq.Connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string mensaje = reader.GetString(reader.GetOrdinal("Mensaje"));

                            // Note: Even if the email doesn't exist, we return success for security reasons
                            // but don't actually send a reset token
                            res.resultado = true;

                            // Only if the email exists, we'll have these fields
                            if (!reader.IsDBNull(reader.GetOrdinal("TokenRecuperacion")))
                            {
                                res.ResetToken = reader.GetString(reader.GetOrdinal("TokenRecuperacion"));
                                res.ExpirationDate = reader.GetDateTime(reader.GetOrdinal("FechaExpiracion"));
                                res.UsuarioID = reader.GetInt32(reader.GetOrdinal("UsuarioID"));
                                /*
                                // Here you would send an email with the reset token
                                bool emailSent = HelperMailcs.EnviarCorreoResetPassword(req.Email, res.ResetToken);
                                if (!emailSent)
                                {
                                    // Log this error but still return success to user
                                    Console.WriteLine("Failed to send password reset email to: " + req.Email);
                                }*/
                            }
                        }
                    }
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

        // Method to confirm password reset with token (no session required)
        public ResResetContrasennaConfirmar ResetContrasennaConfirmar(ReqResetContrasennaConfirmar req)
        {
            ResResetContrasennaConfirmar res = new ResResetContrasennaConfirmar();
            res.error = new List<Error>();
            res.resultado = false;

            try
            {
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
                        Message = "ID de usuario inválido"
                    });
                    return res;
                }

                if (string.IsNullOrEmpty(req.ResetToken))
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.datosIncompletos,
                        Message = "Token de restablecimiento vacío"
                    });
                    return res;
                }

                if (string.IsNullOrEmpty(req.NuevaPassword))
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.passwordFaltante,
                        Message = "Nueva contraseña vacía"
                    });
                    return res;
                }

                if (!EsPasswordSeguro(req.NuevaPassword))
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.passwordMuyDebil,
                        Message = "La nueva contraseña no cumple con los requisitos de seguridad"
                    });
                    return res;
                }

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    // Example stored procedure call for password reset confirmation
                    // Adjust according to your actual stored procedure
                    var command = linq.Connection.CreateCommand();
                    command.CommandText = "SP_ResetPassword_Confirmar";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@UsuarioID", req.UsuarioID));
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ResetToken", req.ResetToken));
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@NuevaPassword", req.NuevaPassword));

                    linq.Connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool resultado = reader.GetBoolean(reader.GetOrdinal("Resultado"));
                            string mensaje = reader.GetString(reader.GetOrdinal("Mensaje"));

                            if (resultado)
                            {
                                res.resultado = true;
                            }
                            else
                            {
                                res.error.Add(new Error
                                {
                                    ErrorCode = (int)EnumErrores.verificacionFallida,
                                    Message = mensaje
                                });
                            }
                        }
                        else
                        {
                            res.error.Add(new Error
                            {
                                ErrorCode = (int)EnumErrores.verificacionFallida,
                                Message = "Error al restablecer la contraseña"
                            });
                        }
                    }
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

        #region Helpers
        public bool EsCorreoValido(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            try
            {
                return Regex.IsMatch(correo,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase,
                    TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public bool EsPasswordSeguro(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Mínimo 8 caracteres, al menos una letra mayúscula, un número y un carácter especial
            return Regex.IsMatch(password,
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
        }

        public string GenerarPin(int longitud)
        {
            if (longitud <= 0) return string.Empty;

            Random rnd = new Random();
            StringBuilder pin = new StringBuilder();

            for (int i = 0; i < longitud; i++)
            {
                pin.Append(rnd.Next(0, 10));
            }

            return pin.ToString();
        }

        private string HashearPassword(string passwordUsuario, string key)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(passwordUsuario + key);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        #endregion
    }
}