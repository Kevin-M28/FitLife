using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos;
using BackEnd.Entidades;

using BackEnd.Enum;
using BackEnd.Response.Modulo_Membresia;
using BackEnd.Request.Modulo_Membresia;


namespace BackEnd.Logica.Modulo_Membresia
{
    public class LogicaMembresia
    {
        /// <summary>
        /// Obtiene todas las membresías disponibles
        /// </summary>
        public ResObtenerMembresias ObtenerTodasMembresias()
        {
            ResObtenerMembresias res = new ResObtenerMembresias();
            res.error = new List<Error>();
            res.resultado = false;

            try
            {
                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    var membresias = linq.Membresia.ToList();

                    if (membresias != null && membresias.Any())
                    {
                        res.membresias = membresias.Select(m => new Entidades.Membresia
                        {
                            MembresiaID = m.MembresiaID,
                            Tipo = m.Tipo,
                            Precio = m.Precio,
                            DuracionDias = m.DuracionDias,
                            Descripcion = m.Descripcion
                        }).ToList();

                        res.resultado = true;
                    }
                    else
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.sinRegistros,
                            Message = "No se encontraron membresías disponibles"
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

        /// <summary>
        /// Obtiene detalles de una membresía por su ID
        /// </summary>
        public ResObtenerMembresia ObtenerMembresiaPorId(Request.Modulo_Membresia.ReqMembresia req)
        {
            ResObtenerMembresia res = new ResObtenerMembresia();
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

                if (req.membresiaID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "ID de membresía no válido"
                    });
                    return res;
                }
                #endregion

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    var membresia = linq.Membresia.FirstOrDefault(m => m.MembresiaID == req.membresiaID);

                    if (membresia != null)
                    {
                        res.membresia = new Entidades.Membresia
                        {
                            MembresiaID = membresia.MembresiaID,
                            Tipo = membresia.Tipo,
                            Precio = membresia.Precio,
                            DuracionDias = membresia.DuracionDias,
                            Descripcion = membresia.Descripcion
                        };

                        res.resultado = true;
                    }
                    else
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.membresiaNoEncontrada,
                            Message = $"No se encontró la membresía con ID {req.membresiaID}"
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

        /// <summary>
        /// Obtiene la membresía activa de un usuario
        /// </summary>
        public ResObtenerMembresiaUsuario ObtenerMembresiaPorUsuario(ReqObtenerMembresiaUsuario req)
        {
            ResObtenerMembresiaUsuario res = new ResObtenerMembresiaUsuario();
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

                if (req.usuarioID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "ID de usuario no válido"
                    });
                    return res;
                }
                #endregion

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    // Utilizamos una consulta LINQ para obtener los datos necesarios
                    var membresiaUsuario = (from um in linq.UsuarioMembresia
                                            join m in linq.Membresia on um.MembresiaID equals m.MembresiaID
                                            join u in linq.Usuario on um.UsuarioID equals u.UsuarioID
                                            where um.UsuarioID == req.usuarioID && um.Estado == "activa"
                                            orderby um.FechaVencimiento descending
                                            select new
                                            {
                                                um.UsuarioMembresiaID,
                                                um.UsuarioID,
                                                u.Nombre,
                                                u.Apellido,
                                                um.MembresiaID,
                                                m.Tipo,
                                                m.Precio,
                                                um.FechaInicio,
                                                um.FechaVencimiento,
                                                DiasRestantes = (int)System.Data.Linq.SqlClient.SqlMethods.DateDiffDay(
                                                    DateTime.Now, um.FechaVencimiento),
                                                um.Estado
                                            }).FirstOrDefault();

                    if (membresiaUsuario != null)
                    {
                        // Use fully qualified System.Enum
                        res.usuarioMembresia = new Entidades.UsuarioMembresia
                        {
                            UsuarioMembresiaID = membresiaUsuario.UsuarioMembresiaID,
                            UsuarioID = membresiaUsuario.UsuarioID,
                            MembresiaID = membresiaUsuario.MembresiaID,
                            FechaInicio = membresiaUsuario.FechaInicio,
                            FechaVencimiento = membresiaUsuario.FechaVencimiento,
                            Estado = TryParseEstadoMembresia(membresiaUsuario.Estado),
                            DiasRestantes = membresiaUsuario.DiasRestantes > 0 ? membresiaUsuario.DiasRestantes : 0
                        };

                        res.membresia = new Entidades.Membresia
                        {
                            MembresiaID = membresiaUsuario.MembresiaID,
                            Tipo = membresiaUsuario.Tipo,
                            Precio = membresiaUsuario.Precio
                        };

                        res.nombreUsuario = membresiaUsuario.Nombre;
                        res.apellidoUsuario = membresiaUsuario.Apellido;

                        res.resultado = true;
                    }
                    else
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.sinMembresiaActiva,
                            Message = "El usuario no tiene una membresía activa"
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

        /// <summary>
        /// Asigna una nueva membresía a un usuario
        /// </summary>
        public ResAsignarMembresia AsignarMembresia(ReqAsignarMembresia req)
        {
            ResAsignarMembresia res = new ResAsignarMembresia();
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

                if (req.usuarioID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "ID de usuario no válido"
                    });
                    return res;
                }

                if (req.membresiaID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "ID de membresía no válido"
                    });
                    return res;
                }
                #endregion

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    var resultado = linq.SP_InsertUsuarioMembresia(
                        req.usuarioID,
                        req.membresiaID,
                        req.fechaInicio,
                        req.adminID).FirstOrDefault();

                    if (resultado != null)
                    {
                        res.usuarioMembresiaID = resultado.UsuarioMembresiaID;
                        res.fechaInicio = resultado.FechaInicio;
                        res.fechaVencimiento = resultado.FechaVencimiento;
                        res.tipoMembresia = resultado.TipoMembresia;
                        res.diasRestantes = resultado.DiasRestantes ?? 0;
                        res.resultado = true;
                    }
                    else
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.errorProcesamiento,
                            Message = "No se pudo asignar la membresía"
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

        /// <summary>
        /// Registra un pago para una membresía
        /// </summary>
        public ResRegistrarPago RegistrarPago(ReqRegistrarPago req)
        {
            ResRegistrarPago res = new ResRegistrarPago();
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

                if (req.usuarioID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "ID de usuario no válido"
                    });
                    return res;
                }

                if (req.monto <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.montoInvalido,
                        Message = "El monto debe ser mayor que 0"
                    });
                    return res;
                }

                if (string.IsNullOrEmpty(req.metodoPago))
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.parametroFaltante,
                        Message = "El método de pago es requerido"
                    });
                    return res;
                }
                #endregion

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    var resultado = linq.SP_InsertPago(
                        req.usuarioID,
                        req.monto,
                        req.metodoPago,
                        req.comprobanteRuta).FirstOrDefault();

                    if (resultado != null)
                    {
                        res.pagoID = resultado.PagoID;
                        res.monto = resultado.Monto;
                        res.fechaPago = (DateTime)resultado.FechaPago;
                        res.estado = resultado.Estado;
                        res.resultado = true;
                    }
                    else
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.errorProcesamiento,
                            Message = "No se pudo registrar el pago"
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

        /// <summary>
        /// Cancela una membresía de usuario
        /// </summary>
        public ResCancelarMembresia CancelarMembresia(ReqCancelarMembresia req)
        {
            ResCancelarMembresia res = new ResCancelarMembresia();
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

                if (req.usuarioMembresiaID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "ID de membresía de usuario no válido"
                    });
                    return res;
                }

                if (req.usuarioID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "ID de usuario no válido"
                    });
                    return res;
                }
                #endregion

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    // Verificar si el usuario es dueño de la membresía
                    var usuarioMembresia = linq.UsuarioMembresia.FirstOrDefault(
                        um => um.UsuarioMembresiaID == req.usuarioMembresiaID &&
                              um.UsuarioID == req.usuarioID);

                    if (usuarioMembresia == null)
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.accesoDenegado,
                            Message = "El usuario no es dueño de esta membresía"
                        });
                        return res;
                    }

                    // Actualizar el estado de la membresía a cancelada
                    usuarioMembresia.Estado = "cancelada";
                    linq.SubmitChanges();

                    // Registrar notificación
                    linq.Notificacion.InsertOnSubmit(new AccesoDatos.Notificacion
                    {
                        UsuarioID = req.usuarioID,
                        Tipo = "anuncio",
                        Mensaje = "Has cancelado tu membresía. Si deseas renovarla, comunícate con administración.",
                        FechaEnvio = DateTime.Now,
                        EstadoLeido = false
                    });
                    linq.SubmitChanges();

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

        /// <summary>
        /// Obtiene el historial de membresías de un usuario
        /// </summary>
        public ResObtenerHistorialMembresias ObtenerHistorialMembresias(ReqObtenerHistorialMembresias req)
        {
            ResObtenerHistorialMembresias res = new ResObtenerHistorialMembresias();
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

                if (req.usuarioID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "ID de usuario no válido"
                    });
                    return res;
                }
                #endregion

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    var historial = (from um in linq.UsuarioMembresia
                                     join m in linq.Membresia on um.MembresiaID equals m.MembresiaID
                                     where um.UsuarioID == req.usuarioID
                                     orderby um.FechaInicio descending
                                     select new
                                     {
                                         um.UsuarioMembresiaID,
                                         um.UsuarioID,
                                         um.MembresiaID,
                                         m.Tipo,
                                         m.Precio,
                                         m.DuracionDias,
                                         m.Descripcion,
                                         um.FechaInicio,
                                         um.FechaVencimiento,
                                         um.Estado
                                     }).ToList();

                    if (historial != null && historial.Any())
                    {
                        res.historialMembresias = new List<Entidades.UsuarioMembresia>();

                        foreach (var item in historial)
                        {
                            var membresia = new Entidades.Membresia
                            {
                                MembresiaID = item.MembresiaID,
                                Tipo = item.Tipo,
                                Precio = item.Precio,
                                DuracionDias = item.DuracionDias,
                                Descripcion = item.Descripcion
                            };

                            var usuarioMembresia = new Entidades.UsuarioMembresia
                            {
                                UsuarioMembresiaID = item.UsuarioMembresiaID,
                                UsuarioID = item.UsuarioID,
                                MembresiaID = item.MembresiaID,
                                FechaInicio = item.FechaInicio,
                                FechaVencimiento = item.FechaVencimiento,
                                Estado = TryParseEstadoMembresia(item.Estado).ToString(),
                                Membresia = membresia
                            };

                            res.historialMembresias.Add(usuarioMembresia);
                        }

                        res.resultado = true;
                    }
                    else
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.sinRegistros,
                            Message = "El usuario no tiene historial de membresías"
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

        /// <summary>
        /// Verifica si un usuario tiene una membresía activa
        /// </summary>
        public ResVerificarMembresia VerificarMembresiaActiva(ReqVerificarMembresia req)
        {
            ResVerificarMembresia res = new ResVerificarMembresia();
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

                if (req.usuarioID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "ID de usuario no válido"
                    });
                    return res;
                }
                #endregion

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    var tieneMembresia = linq.UsuarioMembresia.Any(
                        um => um.UsuarioID == req.usuarioID &&
                              um.Estado == "activa" &&
                              um.FechaVencimiento >= DateTime.Now);

                    res.tieneMembresia = tieneMembresia;

                    // También verificamos si el usuario está en morosidad
                    var estaEnMorosidad = linq.Morosidad.Any(m => m.UsuarioID == req.usuarioID);
                    res.estaEnMorosidad = estaEnMorosidad;

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

        /// <summary>
        /// Helper method to safely parse the Estado string to EnumEstadoMembresia
        /// </summary>
        private EnumEstadoMembresia TryParseEstadoMembresia(string estado)
        {
            if (string.IsNullOrEmpty(estado))
                return EnumEstadoMembresia.vencida;

            // Try to parse the string to the enum
            if (System.Enum.TryParse(estado, true, out EnumEstadoMembresia result))
                return result;

            return EnumEstadoMembresia.vencida;
        }
    }
}
