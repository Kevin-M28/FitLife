using BackEnd.Entities;
using BackEnd.Enum;
using BackEnd.ResAndReq.Req.Payment;
using BackEnd.ResAndReq.Res.Payment;
using Conexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logic.Payment

{
    public class LogPayment
    {
        /// <summary>
        /// Registers a new payment for membership
        /// </summary>
        /// <param name="req">Payment registration request</param>
        /// <param name="token">User session token</param>
        /// <returns>Payment registration result</returns>
        public ResRegisterPayment RegistrarPago(ReqRegisterPayment req, string token)
        {
            ResRegisterPayment res = new ResRegisterPayment()
            {
                Error = new List<Error>(),
                Result = false,
                PaymentMessage = null
            };

            try
            {
                #region Validaciones
                if (string.IsNullOrEmpty(token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.sesionNula,
                        Message = "Token de sesión requerido"
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

                if (string.IsNullOrEmpty(req.MembershipTypeName))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.membresiaFaltante,
                        Message = "Tipo de membresía requerido"
                    });
                }

                if (req.Amount <= 0)
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.montoInvalido,
                        Message = "Monto debe ser mayor a cero"
                    });
                }

                if (string.IsNullOrEmpty(req.PaymentMethodName))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.metodoPagoInvalido,
                        Message = "Método de pago requerido"
                    });
                }

                if (res.Error.Any())
                {
                    return res;
                }
                #endregion

                using (FitLifeDataContext linq = new FitLifeDataContext())
                {
                    var resultado = linq.sp_RegisterPayment(
                        token,
                        req.MembershipTypeName,
                        req.Amount,
                        req.PaymentMethodName,
                        req.ReceiptFilePath
                    ).FirstOrDefault();

                    if (resultado == null || resultado.Result != "SUCCESS")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.errorDesconocido,
                            Message = resultado?.Message ?? "Error desconocido al registrar pago"
                        });
                    }
                    else
                    {
                        res.Result = true;
                        res.PaymentMessage = resultado.Message;
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
        /// Approves a pending payment (Admin only)
        /// </summary>
        /// <param name="req">Payment approval request</param>
        /// <param name="token">Admin session token</param>
        /// <returns>Payment approval result</returns>
        public ResApprovePayment AprobarPago(ReqApprovePayment req, string token)
        {
            ResApprovePayment res = new ResApprovePayment()
            {
                Error = new List<Error>(),
                Result = false,
                ApprovalMessage = null
            };

            try
            {
                #region Validaciones
                if (string.IsNullOrEmpty(token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.sesionNula,
                        Message = "Token de sesión requerido"
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

                if (string.IsNullOrEmpty(req.UserCedula))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.cedulaFaltante,
                        Message = "Cédula de usuario requerida"
                    });
                }

                if (req.PaymentAmount <= 0)
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.montoInvalido,
                        Message = "Monto debe ser mayor a cero"
                    });
                }

                if (res.Error.Any())
                {
                    return res;
                }
                #endregion

                using (FitLifeDataContext linq = new FitLifeDataContext())
                {
                    var resultado = linq.sp_ApprovePayment(
                        token,
                        req.PaymentDate,
                        req.PaymentAmount,
                        req.UserCedula
                    ).FirstOrDefault();

                    if (resultado == null || resultado.Result != "SUCCESS")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.errorDesconocido,
                            Message = resultado?.Message ?? "Error desconocido al aprobar pago"
                        });
                    }
                    else
                    {
                        res.Result = true;
                        res.ApprovalMessage = resultado.Message;
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
        /// Gets payment history for a user
        /// </summary>
        /// <param name="token">User session token</param>
        /// <returns>User's payment history</returns>
        public ResGetPaymentHistory ObtenerHistorialPagos(string token)
        {
            ResGetPaymentHistory res = new ResGetPaymentHistory()
            {
                Error = new List<Error>(),
                Result = false,
                Payments = new List<Entities.Payment>()
            };

            try
            {
                #region Validaciones
                if (string.IsNullOrEmpty(token))
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.sesionNula,
                        Message = "Token de sesión requerido"
                    });
                    return res;
                }
                #endregion

                using (FitLifeDataContext linq = new FitLifeDataContext())
                {
                    var resultados = linq.sp_GetPaymentHistory(token);
                    
                    foreach (var pago in resultados)
                    {
                        if (pago.Result == "SUCCESS")
                        {
                            res.Payments.Add(new Entities.Payment
                            {
                                Amount = pago.Amount ?? 0,
                                PaymentMethodName = pago.PaymentMethod,
                                PaymentDate = pago.PaymentDate ?? DateTime.MinValue,
                                ReceiptFilePath = pago.ReceiptFilePath,
                                Status = pago.PaymentStatus,
                                MembershipName = pago.MembershipName,
                                StartDate = pago.StartDate ?? DateTime.MinValue,
                                EndDate = pago.EndDate ?? DateTime.MinValue
                            });
                        }
                    }

                    res.Result = true;
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
       
    }
}