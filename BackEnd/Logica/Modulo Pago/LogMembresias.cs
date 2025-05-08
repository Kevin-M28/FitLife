using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entidades;

namespace BackEnd.Logica.Modulo_Pago
{
    public class LogMembresias
    {
        public Response.Modulo_Membresia.ResInsertUsuarioMembresia InsertUsuarioMembresia(Request.Modulo_Membresia.ReqInsertUsuarioMembresia req)
        {
            Response.Modulo_Membresia.ResInsertUsuarioMembresia res = new Response.Modulo_Membresia.ResInsertUsuarioMembresia
            {
                error = new List<Error>(),
                resultado = false,
                membresia = new Entidades.UsuarioMembresia()
            };

            try
            {
                #region Validaciones
                if (req == null)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)Enum.EnumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                    return res;
                }

                if (req.UsuarioID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)Enum.EnumErrores.idFaltante,
                        Message = "UsuarioID inválido"
                    });
                    return res;
                }

                if (req.MembresiaID <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)Enum.EnumErrores.membresiaFaltante,
                        Message = "MembresiaID inválido"
                    });
                    return res;
                }
                #endregion

                using (AccesoDatos.FitlifeDataContext db = new AccesoDatos.FitlifeDataContext())
                {
                    // Crear el comando para ejecutar el SP
                    var command = db.Connection.CreateCommand();
                    command.CommandText = "SP_InsertUsuarioMembresia";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Agregar parámetros
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@UsuarioID", req.UsuarioID));
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@MembresiaID", req.MembresiaID));
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FechaInicio",
                        req.FechaInicio.HasValue ? (object)req.FechaInicio.Value : System.DBNull.Value));
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AdminID",
                        req.AdminID.HasValue ? (object)req.AdminID.Value : System.DBNull.Value));

                    // Abrir conexión y ejecutar
                    db.Connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            res.membresia.UsuarioMembresiaID = reader.GetInt32(reader.GetOrdinal("UsuarioMembresiaID"));
                            res.membresia.UsuarioID = reader.GetInt32(reader.GetOrdinal("UsuarioID"));
                            res.membresia.NombreCompleto = reader.GetString(reader.GetOrdinal("NombreCompleto"));
                            res.membresia.MembresiaID = reader.GetInt32(reader.GetOrdinal("MembresiaID"));
                            res.membresia.TipoMembresia = reader.GetString(reader.GetOrdinal("TipoMembresia"));
                            res.membresia.Precio = reader.GetDecimal(reader.GetOrdinal("Precio"));
                            res.membresia.FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio"));
                            res.membresia.FechaVencimiento = reader.GetDateTime(reader.GetOrdinal("FechaVencimiento"));
                            res.membresia.DiasRestantes = reader.GetInt32(reader.GetOrdinal("DiasRestantes"));
                            res.membresia.Estado = reader.GetString(reader.GetOrdinal("Estado"));

                            res.resultado = true;
                        }
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                res.error.Add(new Error
                {
                    ErrorCode = (int)Enum.EnumErrores.excepcionBaseDatos,
                    Message = sqlEx.Message
                });
            }
            catch (Exception ex)
            {
                res.error.Add(new Error
                {
                    ErrorCode = (int)Enum.EnumErrores.excepcionLogica,
                    Message = ex.Message
                });
            }

            return res;
        }

        // Aquí irían los demás métodos para las otras operaciones de membresías...
    }
}
