using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AccesoDatos;
using BackEnd.Entidades;
using BackEnd.Enum;
using BackEnd.Request.Modulo_Usuario;
using BackEnd.Response.Modulo_Usuario;

namespace BackEnd.Logica.Modulo_Usuario
{
    public class LogicaUsuario
    {
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
                    if (string.IsNullOrEmpty(req.usuario.nombre))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.nombreFaltante,
                            Message = "Nombre vacío"
                        });
                    }

                    if (string.IsNullOrEmpty(req.usuario.apellidos))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.apellidoFaltante,
                            Message = "Apellido vacío"
                        });
                    }

                    if (string.IsNullOrEmpty(req.usuario.cedula.ToString()))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.cedulaFaltante,
                            Message = "Cédula vacía"
                        });
                    }

                    if (string.IsNullOrEmpty(req.usuario.correoElectronico))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.correoFaltante,
                            Message = "Correo vacío"
                        });
                    }
                    else if (!EsCorreoValido(req.usuario.correoElectronico))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.correoIncorrecto,
                            Message = "Correo no válido"
                        });
                    }

                    if (string.IsNullOrEmpty(req.usuario.password))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.passwordFaltante,
                            Message = "Contraseña vacía"
                        });
                    }
                    else if (!EsPasswordSeguro(req.usuario.password))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.passwordMuyDebil,
                            Message = "La contraseña debe tener al menos 8 caracteres, una mayúscula, un número y un carácter especial"
                        });
                    }

                    if (req.usuario.GimnasioID <= 0)
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.gimnasioFaltante,
                            Message = "Debe seleccionar un gimnasio"
                        });
                    }

                    if (string.IsNullOrEmpty(req.usuario.telefono))
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
                string passHash = HashearPassword(req.usuario.password, llave);

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    var resultado = linq.SP_RegistrarUsuario(
                        req.usuario.GimnasioID,
                        req.usuario.cedula.ToString(),
                        req.usuario.nombre + " " + req.usuario.apellidos,
                        req.usuario.correoElectronico,
                        passHash,
                        req.usuario.telefono,
                        "usuario",  
                        "activo",   
                        llave
                    ).FirstOrDefault();

                    if (resultado != null && resultado.UsuarioID > 0)
                    {
                        res.UsuarioID = resultado.UsuarioID;
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

                    var llaveUsuario = db.Usuario
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

                    var resultado = db.SP_LoginUsuario(req.Email, passwordHasheado).FirstOrDefault();

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

                    res.usuario = new BackEnd.Entidades.Usuario
                    {
                        Id = (long)(resultado.UsuarioID ?? 0),
                        nombre = resultado.Nombre,
                        correoElectronico = resultado.Email,
                        rol = System.Enum.TryParse(resultado.Rol, out EnumRolUsuario rolParsed) ? rolParsed : EnumRolUsuario.usuario,
                        GimnasioID = resultado.GimnasioID ?? 0,

                        membresiaActiva = new BackEnd.Entidades.UsuarioMembresia
                        {
                            Estado = (resultado.TieneMembresiaActiva ?? false) ? EnumEstadoMembresia.activa : EnumEstadoMembresia.vencida
                        },

                        estado = (resultado.EstaEnMorosidad ?? false) ? EnumEstadoUsuario.suspendido : EnumEstadoUsuario.activo
                    };


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

        public ResCambiarContrasenna CambiarContrasenna(ReqCambiarContrasenna req)
        {
            ResCambiarContrasenna res = new ResCambiarContrasenna();
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
                        Message = "Usuario no válido"
                    });
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
                   
                    var usuario = linq.Usuario.FirstOrDefault(u => u.UsuarioID == req.UsuarioID);
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
                res.resultado=false;
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

        public ResAsignarRolUsuario AsignarRolUsuario(ReqAsignarRolUsuario req)
        {
            ResAsignarRolUsuario res = new ResAsignarRolUsuario();
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
        public ResObtenerUsuario ObtenerUsuarioId(ReqObtenerUsuario req)
        {
            ResObtenerUsuario res = new ResObtenerUsuario();
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
                if (req.usuarioID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "UsuarioID vacío"
                    });
                    return res;
                }
                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    // Ejecutamos el SP pero solo usamos el primer resultado (la información básica del usuario)
                    var usuario = linq.SP_GetUsuarioPorID(req.usuarioID).FirstOrDefault();

                    if (usuario == null)
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.usuarioNoEncontrado,
                            Message = "Usuario no encontrado"
                        });
                        return res;
                    }

                    // Solo devolvemos la información básica del usuario
                    res.usuario = new Entidades.Usuario
                    {
                        Id = usuario.UsuarioID,
                        cedula = usuario.Cedula,
                        nombre = usuario.Nombre,
                        correoElectronico = usuario.Email,
                        telefono = usuario.Telefono,
                        rol = System.Enum.TryParse(usuario.Rol?.ToString(), out EnumRolUsuario rolParsed) ? rolParsed : EnumRolUsuario.usuario,
                        estado = System.Enum.TryParse(usuario.Estado?.ToString(), out EnumEstadoUsuario estadoParsed) ? estadoParsed : EnumEstadoUsuario.inactivo,
                        GimnasioID = usuario.GimnasioID,
                        nombreGimnasio = usuario.NombreGimnasio,

                        // No incluimos el resto de los datos que vienen en los otros conjuntos de resultados
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

        public ResActualizarUsuario AtualizarUsuario(ReqActualizarUsuario req)
        {
            ResActualizarUsuario res = new ResActualizarUsuario();
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
                if (req.usuarioID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.idFaltante,
                        Message = "UsuarioID vacío"
                    });
                    return res;
                }
                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    var resultado = linq.SP_UpdateUsuario(req.usuarioID, req.nombre, req.email, req.telefono).FirstOrDefault();
                    if (resultado == null || resultado.UsuarioID <= 0)
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.usuarioNoEncontrado,
                            Message = "Usuario no encontrado"
                        });
                        return res;
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