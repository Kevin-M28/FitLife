using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BackEnd.Request;
using BackEnd.Entidades;
using BackEnd.Enum;
using BackEnd.Response;
using AccesoDatos;

namespace BackEnd.Logica
{
    public class LogUsuario
    {
        private const int ID_GYM_DEFAULT = 1; // Gimnasio predeterminado
        private const int ID_ROL_MIEMBRO = 1; // Rol de miembro regular

        public ResInsertarUsuario insertar(ReqInsertarUsuario req)
        {
            ResInsertarUsuario res = new ResInsertarUsuario();
            res.error = new List<Error>();
            res.resultado = false;

            try
            {
                #region validaciones
                if (req == null)
                {
                    Error error = new Error();
                    error.ErrorCode = (int)enumErrores.requestNulo;
                    error.Message = "Request nulo";
                    res.error.Add(error);
                    res.resultado = false;
                    return res;
                }

                if (req.usuario == null)
                {
                    Error error = new Error();
                    error.ErrorCode = (int)enumErrores.usuarioFaltante;
                    error.Message = "Usuario no proporcionado";
                    res.error.Add(error);
                    res.resultado = false;
                    return res;
                }

                // Si no se especifica un gimnasio, usar el predeterminado
                if (req.usuario.idGym <= 0)
                {
                    req.usuario.idGym = ID_GYM_DEFAULT;
                }

                // Si no se especifica un rol, usar el de miembro
                if (req.usuario.idRol <= 0)
                {
                    req.usuario.idRol = ID_ROL_MIEMBRO;
                }

                // Validaciones de datos
                if (String.IsNullOrEmpty(req.usuario.nombre))
                {
                    Error error = new Error();
                    error.ErrorCode = (int)enumErrores.nombreFaltante;
                    error.Message = "Nombre vacío";
                    res.error.Add(error);
                }

                if (String.IsNullOrEmpty(req.usuario.apellido1))
                {
                    Error error = new Error();
                    error.ErrorCode = (int)enumErrores.apellidoFaltante;
                    error.Message = "Apellido vacío";
                    res.error.Add(error);
                }

                if (String.IsNullOrEmpty(req.usuario.correoElectronico))
                {
                    Error error = new Error();
                    error.ErrorCode = (int)enumErrores.correoFaltante;
                    error.Message = "Correo electrónico vacío";
                    res.error.Add(error);
                }
                else if (!this.EsCorreoValido(req.usuario.correoElectronico))
                {
                    Error error = new Error();
                    error.ErrorCode = (int)enumErrores.correoIncorrecto;
                    error.Message = "Correo electrónico incorrecto";
                    res.error.Add(error);
                }

                if (String.IsNullOrEmpty(req.usuario.password))
                {
                    Error error = new Error();
                    error.ErrorCode = (int)enumErrores.passwordFaltante;
                    error.Message = "Contraseña vacía";
                    res.error.Add(error);
                }
                else if (!this.EsPasswordSeguro(req.usuario.password))
                {
                    Error error = new Error();
                    error.ErrorCode = (int)enumErrores.passwordMuyDebil;
                    error.Message = "Contraseña demasiado débil. Debe tener al menos 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial";
                    res.error.Add(error);
                }
                #endregion

                // Si hay errores, terminar
                if (res.error.Any())
                {
                    res.resultado = false;
                    return res;
                }

                // Generar ID de usuario (usualmente el correo hasta el @ o nombre de usuario)
                string idUsuario = GenerarIdUsuario(req.usuario.correoElectronico, req.usuario.nombre);
                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    Console.WriteLine($"Database: {linq.Connection.Database}");
                    Console.WriteLine($"Server: {linq.Connection.DataSource}");
                    // Rest of your code...
                }
                // Generar código de verificación
                string codigoVerificacion = GenerarPin(5);


                //validar si el id usuario ya existe me trae directame y sigue generando el id_hasta que no exista
                while (true)
                {
                    using (FitlifeDataContext linq = new FitlifeDataContext())
                    {
                        var existeUsuario = linq.users.Any(u => u.id_user == idUsuario);
                        if (existeUsuario == false)
                        {
                            break; // ID de usuario único encontrado
                        }
                        else
                        {
                            idUsuario = GenerarIdUsuario(req.usuario.correoElectronico, req.usuario.nombre);
                        }
                    }
                }

                using (FitlifeDataContext linq = new FitlifeDataContext())
                {
                    try
                    {
                        var resultado = linq.sp_CreateUser(
                         idUsuario,
                         req.usuario.idGym,
                         req.usuario.nombre,
                         req.usuario.apellido1,
                         req.usuario.apellido2 ?? string.Empty,
                         req.usuario.idRol,
                         req.usuario.password,
                         req.usuario.correoElectronico);

                        // IMPORTANT: Set result to true here
                        res.resultado = true;

                        // Si llegamos aquí, el usuario fue creado con éxito

                        // Enviar email de verificación

                        /*
                        if (HelperMailcs.EnviarCorreoVerificacion(req.usuario.correoElectronico, req.usuario.nombre, codigoVerificacion))
                        {
                            res.resultado = true;
                        }
                        else
                        {
                            // El usuario se creó pero hubo un problema al enviar el email
                            res.resultado = true; // Aún consideramos éxito, pero registrar advertencia
                            Console.WriteLine("ADVERTENCIA: No se pudo enviar el correo de verificación");
                        }*/
      
                    }
                    catch (Exception ex)
                    {
                        Error error = new Error();
                        error.ErrorCode = (int)enumErrores.excepcionBaseDatos;
                        error.Message = ex.Message;
                        res.error.Add(error);
                        res.resultado = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.ErrorCode = (int)enumErrores.excepcionLogica;
                error.Message = ex.Message;
                res.error.Add(error);
                res.resultado = false;
            }

            return res;
        }
        #region helpers

        private string GenerarIdUsuario(string correo, string nombre)
        {
            if (!string.IsNullOrEmpty(correo) && correo.Contains("@"))
            {
                return correo.Substring(0, correo.IndexOf('@')).ToLower().Substring(0,5) + Guid.NewGuid().ToString("N").Substring(0, 5);
            }
            else if (!string.IsNullOrEmpty(nombre))
            {
                // Fallback: usar el nombre (normalizado) + un número aleatorio
                string nombreNormalizado = Regex.Replace(nombre.ToLower(), @"[^a-z0-9]", "");
                return nombreNormalizado + new Random().Next(1000, 9999);
            }
            else
            {
                // Último recurso: generar un ID aleatorio
                return "user" + Guid.NewGuid().ToString("N").Substring(0, 8);
            }
        }

        public bool EsCorreoValido(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(correo, patron);
        }

        public bool EsPasswordSeguro(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Al menos 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial
            string patron = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            return Regex.IsMatch(password, patron);
        }

        public string GenerarPin(int longitud)
        {
            Random rnd = new Random();
            StringBuilder pin = new StringBuilder();

            for (int i = 0; i < longitud; i++)
            {
                pin.Append(rnd.Next(0, 10));
            }

            return pin.ToString();
        }

        #endregion
    }
}
