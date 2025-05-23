using BackEnd.Entities;
using BackEnd.Enum;
using BackEnd.Helpers;
using BackEnd.Logic;
using BackEnd.ResAndReq.Req;
using BackEnd.ResAndReq.Req.User;
using BackEnd.ResAndReq.Res;
using BackEnd.ResAndReq.Res.User;
using Conexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace BackEnd.Logic
{
    public class LogUser
    {
        #region User Registration & Authentication

        /// <summary>
        /// Registra un nuevo usuario en el sistema
        /// </summary>
        public ResAddUser UserRegistration(ReqAddUser req)
        {
            ResAddUser res = new ResAddUser()
            {
                Error = new List<Entities.Error>(),
                Result = false,
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
                    if (string.IsNullOrEmpty(req.FirstName))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.nombreFaltante,
                            Message = "Nombre vacío"
                        });
                    }

                    if (string.IsNullOrEmpty(req.LastName))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.apellidoFaltante,
                            Message = "Apellido vacío"
                        });
                    }

                    if (string.IsNullOrEmpty(req.Cedula))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.cedulaFaltante,
                            Message = "Cédula vacía"
                        });
                    }

                    if (string.IsNullOrEmpty(req.Email))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.correoFaltante,
                            Message = "Correo vacío"
                        });
                    }
                    else if (!EsCorreoValido(req.Email))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.correoIncorrecto,
                            Message = "Correo no válido"
                        });
                    }

                    if (string.IsNullOrEmpty(req.PhoneNumber))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.telefonoFaltante,
                            Message = "Teléfono vacío"
                        });
                    }
                }
                #endregion

                if (res.Error.Any())
                {
                    return res;
                }

                // Generar una contraseña segura automaticamente
                string password = Helper.GenerarPassword(8);
                string passHash = Helper.HashearPassword(password);

                using (FitLife2DataContext linq = new FitLife2DataContext())
                {
                    var resultado = linq.sp_RegisterUser(
                        req.Cedula,
                        req.FirstName,
                        req.LastName,
                        req.Email,
                        passHash,
                        req.PhoneNumber,
                        req.BirthDate,
                        req.Address,
                        "User"
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
                        res.Result = true;
                        // Aquí se podría agregar lógica para enviar la contraseña generada por email
                        // TODO: Enviar email con contraseña inicial
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
        /// Iniciar sesión de usuario
        /// </summary>
        public ResUserLogin Login(ReqUserLogin req)
        {
            ResUserLogin res = new ResUserLogin()
            {
                Error = new List<Entities.Error>(),
                Result = false,
                User = new Entities.User(),
                Token = string.Empty,
                ExpiresAt = DateTime.MinValue
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
                    if (string.IsNullOrEmpty(req.Email))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.correoFaltante,
                            Message = "Correo vacío"
                        });
                    }
                    if (string.IsNullOrEmpty(req.Password))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.passwordFaltante,
                            Message = "Password vacío"
                        });
                    }
                }
                #endregion
                if (res.Error.Any())
                {
                    return res;
                }

                using (FitLife2DataContext linq = new FitLife2DataContext())
                {
                    // Obtener el hash de la contraseña almacenada
                    var passwordHash = linq.sp_UserPass(req.Email).ToString();

                    if (passwordHash == null)
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.usuarioNoEncontrado,
                            Message = "Usuario no encontrado"
                        });
                        return res;
                    }

                    // Verificar la contraseña utilizando BCrypt
                    if (!Helper.VerificarPassword(req.Password, passwordHash))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.credencialesInvalidas,
                            Message = "Credenciales inválidas"
                        });
                        return res;
                    }

                    // Crear sesión y obtener información del usuario
                    var resultado = linq.sp_UserLogin(
                        req.Email,
                        passwordHash // Pasar el hash almacenado
                    ).FirstOrDefault();

                    if (resultado == null || resultado.Result == "FAILED")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.errorDesconocido,
                            Message = resultado?.Message ?? "Error de inicio de sesión"
                        });
                    }
                    else
                    {
                        res.Result = true;
                        res.User.Cedula = resultado.Cedula;
                        res.User.FirstName = resultado.FirstName;
                        res.User.LastName = resultado.LastName;
                        res.User.Email = resultado.Email;
                        res.User.PhoneNumber = resultado.PhoneNumber;
                        res.User.BirthDate = resultado.BirthDate;
                        res.User.Address = resultado.Address;
                        res.User.Status = resultado.Status;
                        res.User.Role = resultado.RoleName;
                        res.Token = resultado.Token;
                        res.ExpiresAt = (DateTime)resultado.ExpiresAt;
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
        /// Cierra la sesión del usuario
        /// </summary>
        public ResBase LogOut(ReqBase req)
        {
            ResBase res = new ResBase()
            {
                Error = new List<Entities.Error>(),
                Result = false
            };

            try
            {
                if (string.IsNullOrEmpty(req.Token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.sesionNula,
                        Message = "Token de sesión vacío"
                    });
                    return res;
                }

                using (FitLife2DataContext linq = new FitLife2DataContext())
                {
/*                    var resultado = linq.sp_Logout(req.Token);

                    if (resultado == null || resultado.Result == "FAILED")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.sesionNoEncontrada,
                            Message = resultado?.Message ?? "Error al cerrar sesión"
                        }); 
                    }
                    else
                    {
                        res.Result = true;
                    } */
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
        /// Valida si una sesión es válida
        /// </summary>
        public ResBase ValidateSession(ReqBase req)
        {
            ResBase res = new ResBase()
            {
                Error = new List<Entities.Error>(),
                Result = false
            };

            try
            {
                if (string.IsNullOrEmpty(req.Token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.sesionNula,
                        Message = "Token de sesión vacío"
                    });
                    return res;
                }

                using (FitLife2DataContext linq = new FitLife2DataContext())
                {
                    var resultado = linq.sp_ValidateSession(req.Token).FirstOrDefault();

                    if (resultado == null || resultado.Result == "FAILED")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.sesionExpirada,
                            Message = resultado?.Message ?? "Sesión inválida o expirada"
                        });
                    }
                    else
                    {
                        res.Result = true;
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
        /// Cambia la contraseña del usuario
        /// </summary>
        public ResBase ChangePassword(ReqChangePassword req) {
            ResBase res = new ResBase()
            {
                Error = new List<Entities.Error>(),
                Result = false
            };

            try
            {
                if (string.IsNullOrEmpty(req.Token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.sesionNula,
                        Message = "Token de sesión vacío"
                    });
                    return res;
                }

                if (string.IsNullOrEmpty(req.OldPassword))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.passwordFaltante,
                        Message = "Contraseña antigua vacía"
                    });
                    return res;
                }


                if (string.IsNullOrEmpty(req.NewPassword))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.passwordFaltante,
                        Message = "Contraseña nueva vacía"
                    });
                    return res;
                }

                if (!EsPasswordSeguro(req.NewPassword))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.passwordMuyDebil,
                        Message = "La contraseña debe tener al menos 8 caracteres, incluir una letra mayúscula, una minúscula, un número y un carácter especial"
                    });
                    return res;
                }

                // Hashear la nueva contraseña
                string passwordHash = Helper.HashearPassword(req.NewPassword);

                using (FitLife2DataContext linq = new FitLife2DataContext())
                {


                    var passwordHashDB = linq.sp_UserPass(req.Email).ToString();

                    // Verificar la contraseña antigua utilizando BCrypt
                    if (passwordHashDB != null) {
                        if (!Helper.VerificarPassword(req.OldPassword, passwordHashDB))
                        {
                            res.Error.Add(new Error
                            {
                                ErrorCode = (int)EnumErrores.passwordIncorrecto,
                                Message = "Contraseña antigua incorrecta"
                            });
                            return res;
                        }
                    }     
                    var resultado = linq.sp_ChangePassword(req.Token, passwordHash).FirstOrDefault();

                    if (resultado == null || resultado.Result == "FAILED")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.sesionNoEncontrada,
                            Message = resultado?.Message ?? "Error al cambiar contraseña"
                        });
                    }
                    else
                    {
                        res.Result = true;
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

        #endregion

        #region User Profile Management

        public ResUserProfile GetUserProfile(ReqBase req)
        {
            ResUserProfile res = new ResUserProfile()
            {
                Error = new List<Entities.Error>(),
                Result = false,
                User = null
            };

            try
            {
                if (string.IsNullOrEmpty(req.Token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.sesionNula,
                        Message = "Token de sesión vacío"
                    });
                    return res;
                }

                using (FitLife2DataContext linq = new FitLife2DataContext())
                {
                    var resultado = linq.sp_GetUserProfile(req.Token).FirstOrDefault();

                    if (resultado == null || resultado.Result == "FAILED")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.sesionNoEncontrada,
                            Message = resultado?.Message ?? "Error al obtener perfil"
                        });
                    }
                    else
                    {
                        res.Result = true;
                        res.User = new Entities.User
                        {
                            Cedula = resultado.Cedula,
                            FirstName = resultado.FirstName,
                            LastName = resultado.LastName,
                            Email = resultado.Email,
                            PhoneNumber = resultado.PhoneNumber,
                            BirthDate = resultado.BirthDate,
                            Address = resultado.Address,
                            Status = resultado.Status,
                            Role = resultado.RoleName,
                            CreatedAt = DateTime.Now, // No viene en el resultado del SP
                            UpdatedAt = DateTime.Now  // No viene en el resultado del SP
                        };
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
        /// Obtiene el perfil de un usuario por su cédula
        /// </summary>
        public ResUserProfile GetUserProfileByCedula(ReqGetUserCedula req)
        {
            ResUserProfile res = new ResUserProfile()
            {
                Error = new List<Entities.Error>(),
                Result = false,
                User = null
            };

            try
            {
                if (string.IsNullOrEmpty(req.Token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.sesionNula,
                        Message = "Token de sesión vacío"
                    });
                    return res;
                }

                if (string.IsNullOrEmpty(req.Cedula))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.cedulaFaltante,
                        Message = "Cédula vacía"
                    });
                    return res;
                }

                using (FitLife2DataContext linq = new FitLife2DataContext())
                {
                    var resultado = linq.sp_GetUserProfileByCedula(req.Token, req.Cedula).FirstOrDefault();

                    if (resultado == null || resultado.Result == "FAILED")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.entidadNoEncontrada,
                            Message = resultado?.Message ?? "Error al obtener perfil"
                        });
                    }
                    else
                    {
                        res.Result = true;
                        res.User = new Entities.User
                        {
                            Cedula = resultado.Cedula,
                            FirstName = resultado.FirstName,
                            LastName = resultado.LastName,
                            Email = resultado.Email,
                            PhoneNumber = resultado.PhoneNumber,
                            BirthDate = resultado.BirthDate,
                            Address = resultado.Address,
                            Status = resultado.Status,
                            Role = resultado.RoleName,
                            CreatedAt = DateTime.Now, // No viene en el resultado del SP
                            UpdatedAt = DateTime.Now  // No viene en el resultado del SP
                        };
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

        public ResBase UpdateUserProfile(ReqUpdateUser req)
        {
            ResBase res = new ResBase()
            {
                Error = new List<Entities.Error>(),
                Result = false
            };

            try
            {
                if (string.IsNullOrEmpty(req.Token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.sesionNula,
                        Message = "Token de sesión vacío"
                    });
                    return res;
                }
                if(string.IsNullOrEmpty(req.PhoneNumber))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.telefonoFaltante,
                        Message = "Teléfono vacío"
                    });
                    return res;
                }

                if (string.IsNullOrEmpty(req.Address))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.direccionFaltante,
                        Message = "Dirección vacía"
                    });
                    return res;
                }

                if (req == null)
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                    return res;
                }

                using (FitLife2DataContext linq = new FitLife2DataContext())
                {
                    var resultado = linq.sp_UpdateUserProfile(
                        req.Token,
                        req.PhoneNumber,
                        req.Address
                    ).FirstOrDefault();

                    if (resultado == null || resultado.Result == "FAILED")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.sesionNoEncontrada,
                            Message = resultado?.Message ?? "Error al actualizar perfil"
                        });
                    }
                    else
                    {
                        res.Result = true;
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
        /// Cambia el estado de un usuario (solo administradores)
        /// </summary>
        public ResBase ChangeUserStatus(string token, string targetCedula, string newStatus)
        {
            ResBase res = new ResBase()
            {
                Error = new List<Entities.Error>(),
                Result = false
            };

            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.sesionNula,
                        Message = "Token de sesión vacío"
                    });
                    return res;
                }

                if (string.IsNullOrEmpty(targetCedula))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.cedulaFaltante,
                        Message = "Cédula del usuario a modificar vacía"
                    });
                    return res;
                }

                if (string.IsNullOrEmpty(newStatus))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.datosFaltantes,
                        Message = "Nuevo estado vacío"
                    });
                    return res;
                }

                using (FitLife2DataContext linq = new FitLife2DataContext())
                {
                    var resultado = linq.sp_ChangeUserStatus(token, targetCedula, newStatus).FirstOrDefault();

                    if (resultado == null || resultado.Result == "FAILED")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.permisoInsuficiente,
                            Message = resultado?.Message ?? "Error al cambiar estado de usuario"
                        });
                    }
                    else
                    {
                        res.Result = true;
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

        #endregion

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
        #endregion
    }
}
