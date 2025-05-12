using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos;
using BackEnd.Entidades;
using BackEnd.Enum;
using Sesion = BackEnd.Entidades.Sesion;

namespace BackEnd
{
    public static class SessionValidator
    {
        public static Sesion ValidarSesion(String Sesion, out List<Error> errores)
        {
            errores = new List<Error>();

            if (Sesion == null)
            {
                errores.Add(new Error
                {
                    ErrorCode = (int)EnumErrores.sesionNula,
                    Message = "No se ha proporcionado información de sesión"
                });
                return null;
            }

            // Validar que el token existe y sea válido usando el SP_IniciarSesion
            using (FitlifeDataContext db = new FitlifeDataContext())
            {
                var command = db.Connection.CreateCommand();
                command.CommandText = "SP_ValidarSesionInterno";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Token", Sesion));

                try
                {
                    db.Connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            errores.Add(new Error
                            {
                                ErrorCode = (int)EnumErrores.sesionNoEncontrada,
                                Message = "La sesión no es válida o no existe"
                            });
                            return null;
                        }


                        Sesion sesion = new Sesion
                        {
                            Id_User = reader.GetInt32(0),
                            Estado = reader.GetString(1),
                            FechaExpira = reader.GetDateTime(2),
                            Tipo = reader.GetString(3)
                        };

                        if (sesion.FechaExpira < DateTime.Now)
                        {
                            errores.Add(new Error
                            {
                                ErrorCode = (int)EnumErrores.sesionExpirada,
                                Message = "La sesión ha expirado. Por favor, inicie sesión nuevamente"
                            });
                            return null;
                        }

                        if (sesion.Estado != "activa")
                        {
                            errores.Add(new Error
                            {
                                ErrorCode = sesion.Estado == "expirada" ?
                                            (int)EnumErrores.sesionExpirada :
                                            (int)EnumErrores.sesionCerrada,
                                Message = sesion.Estado == "expirada" ?
                                         "La sesión ha expirado. Por favor, inicie sesión nuevamente" :
                                         "La sesión ha sido cerrada. Por favor, inicie sesión nuevamente"
                            });
                            return null;
                        }


                        // Session is valid
                        return sesion;
                    }
                }
                catch (Exception ex)
                {
                    errores.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.excepcionLogica,
                        Message = "Error al validar la sesión: " + ex.Message
                    });
                    return null;
                }
            }
        }
    }
}
