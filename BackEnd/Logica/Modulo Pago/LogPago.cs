using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entidades;

namespace BackEnd.Logica.Modulo_Pago
{
    public class LogPagos
    {
        public Response.Modulo_Pago.ResInsertPago InsertPago(Request.Modulo_Pago.ReqInsertPago req)
        {
            Response.Modulo_Pago.ResInsertPago res = new Response.Modulo_Pago.ResInsertPago
            {
                error = new List<Error>(),
                resultado = false,
                pago = new Entidades.Pago()
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

                if (req.Monto <= 0)
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)Enum.EnumErrores.montoInvalido,
                        Message = "El monto debe ser mayor que cero"
                    });
                    return res;
                }

                if (string.IsNullOrEmpty(req.MetodoPago) || !new[] { "efectivo", "tarjeta", "transferencia", "pendiente" }.Contains(req.MetodoPago))
                {
                    res.error.Add(new Error
                    {
                        ErrorCode = (int)Enum.EnumErrores.metodoPagoInvalido,
                        Message = "Método de pago no válido"
                    });
                    return res;
                }
                #endregion

                using (AccesoDatos.FitlifeDataContext db = new AccesoDatos.FitlifeDataContext())
                {
                    // Crear el comando para ejecutar el SP
                    var command = db.Connection.CreateCommand();
                    command.CommandText = "SP_InsertPago";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Agregar parámetros
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@UsuarioID", req.UsuarioID));
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Monto", req.Monto));
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@MetodoPago", req.MetodoPago));
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ComprobanteRuta",
                        string.IsNullOrEmpty(req.ComprobanteRuta) ? (object)System.DBNull.Value : req.ComprobanteRuta));

                    // Abrir conexión y ejecutar
                    db.Connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            res.pago.PagoID = reader.GetInt32(reader.GetOrdinal("PagoID"));
                            res.pago.UsuarioID = reader.GetInt32(reader.GetOrdinal("UsuarioID"));
                            res.pago.NombreCompleto = reader.GetString(reader.GetOrdinal("NombreCompleto"));
                            res.pago.Monto = reader.GetDecimal(reader.GetOrdinal("Monto"));
                            res.pago.FechaPago = reader.GetDateTime(reader.GetOrdinal("FechaPago"));
                            res.pago.MetodoPago = reader.GetString(reader.GetOrdinal("MetodoPago"));
                            res.pago.Estado = reader.GetString(reader.GetOrdinal("Estado"));

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

        // Aquí irían los demás métodos para las otras operaciones de pagos...
    }
}
