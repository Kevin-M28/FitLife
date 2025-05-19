using BackEnd.Entities;
using BackEnd.Enum;
using BackEnd.ResAndReq.Req.Membership;
using BackEnd.ResAndReq.Res.Membership;
using Conexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Logic.Membership
{
    public class LogMembership
    {
        /// <summary>
        /// Gets active membership for a user
        /// </summary>
        /// <param name="token">User session token</param>
        /// <returns>Active membership information</returns>
        public ResGetActiveMembership ObtenerMembresiaActiva(string token)
        {
            ResGetActiveMembership res = new ResGetActiveMembership()
            {
                Error = new List<Error>(),
                Result = false,
                Membership = new Entities.Membership()
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
                    var resultado = linq.sp_GetActiveMembership(token).FirstOrDefault();

                    if (resultado == null || resultado.Result != "SUCCESS")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.sinMembresiaActiva,
                            Message = resultado?.Message ?? "No se encontró membresía activa"
                        });
                    }
                    else
                    {
                        res.Result = true;
                        // Try different possible property names based on the stored procedure

                        // Check if MembershipName exists, if not, try Name
                        try
                        {
                            res.Membership.MembershipName = resultado.MembershipName;
                        }
                        catch
                        {
                            // If MembershipName doesn't exist, try Name property
                            var membershipNameProperty = resultado.GetType().GetProperty("Name");
                            if (membershipNameProperty != null)
                            {
                                res.Membership.MembershipName = membershipNameProperty.GetValue(resultado)?.ToString();
                            }
                        }

                        res.Membership.StartDate = resultado.StartDate ?? DateTime.MinValue;
                        res.Membership.EndDate = resultado.EndDate ?? DateTime.MinValue;
                        res.Membership.Status = resultado.Status;
                        res.Membership.Price = resultado.Price ?? 0;
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
        /// Checks membership status including expiration warnings
        /// </summary>
        /// <param name="token">User session token</param>
        /// <returns>Membership status information</returns>
        public ResCheckMembershipStatus VerificarEstadoMembresia(string token)
        {
            ResCheckMembershipStatus res = new ResCheckMembershipStatus()
            {
                Error = new List<Error>(),
                Result = false,
                MembershipStatus = null,
                MembershipName = null,
                EndDate = null,
                DaysRemaining = null
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
                    var resultado = linq.sp_CheckMembershipStatus(token).FirstOrDefault();

                    if (resultado == null || resultado.Result != "SUCCESS")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.errorDesconocido,
                            Message = resultado?.Message ?? "Error al verificar estado de membresía"
                        });
                    }
                    else
                    {
                        res.Result = true;
                        res.MembershipStatus = resultado.MembershipStatus;
                        res.MembershipName = resultado.MembershipName;
                        res.EndDate = resultado.EndDate;
                        res.DaysRemaining = resultado.DaysRemaining;
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
        /// Gets all available membership types
        /// </summary>
        /// <returns>List of available membership types</returns>
        public ResGetMembershipTypes ObtenerTiposMembresia()
        {
            ResGetMembershipTypes res = new ResGetMembershipTypes()
            {
                Error = new List<Error>(),
                Result = false,
                MembershipTypes = new List<MembershipType>()
            };

            try
            {
                using (FitLifeDataContext linq = new FitLifeDataContext())
                {
                    var membershipTypes = linq.MembershipTypes.ToList();

                    foreach (var mt in membershipTypes)
                    {
                        res.MembershipTypes.Add(new MembershipType
                        {
                            MembershipTypeID = mt.MembershipTypeID,
                            Name = mt.Name,
                            Description = mt.Description,
                            DurationDays = mt.DurationDays,
                            Price = mt.Price,
                            CreatedAt = mt.CreatedAt,
                            UpdatedAt = mt.UpdatedAt
                        });
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