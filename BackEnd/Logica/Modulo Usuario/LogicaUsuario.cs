using System;
using System.Collections.Generic;
using System.Linq;
using AccesoDatos;
using System.Text;
using BackEnd.Request;
using BackEnd.Response;
using System.Threading.Tasks;
using BackEnd.Request.Modulo_Usuario;
using BackEnd.Response.Modulo_Usuario;
using BackEnd.Entidades;
using BackEnd.Enum;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

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
                    if (String.IsNullOrEmpty(req.usuario.nombre))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.nombreFaltante,
                            Message = "Nombre vacío"
                        });
                    }

                    if (String.IsNullOrEmpty(req.usuario.apellidos))
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.apellidoFaltante,
                            Message = "Apellido vacío"
                        });
                    }

                    if (String.IsNullOrEmpty(req.usuario.correoElectronico))
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

                    if (String.IsNullOrEmpty(req.usuario.password))
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

                    if (String.IsNullOrEmpty(req.usuario.telefono))
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
                    res.resultado = false;
                    return res;
                }

                // Proceso de registro
                int? idBD = 0;       
                int? errorIdBD = 0;    
                string errorMsgBD = "";
                string llave = Guid.NewGuid().ToString("N");
                string passHash = HashearPassword(req.usuario.password, llave);
                string cVerificacion = GenerarPin(5);

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    linq.SP_RegistrarUsuario(
                        req.usuario.GimnasioID,
                        req.usuario.nombre,
                        req.usuario.apellidos,
                        req.usuario.correoElectronico,
                        passHash,
                        req.usuario.telefono,
                        "usuario",
                        llave,
                        cVerificacion,
                        ref idBD,
                        ref errorIdBD,
                        ref errorMsgBD);

                    if (errorIdBD > 0)
                    {
                        res.error.Add(new Error
                        {
                            ErrorCode = errorIdBD.Value,
                            Message = errorMsgBD
                        });
                        res.resultado = false;
                    }
                    else
                    {
                        res.resultado = true;
                        res.UsuarioID = idBD.Value;
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
                res.resultado = false;
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