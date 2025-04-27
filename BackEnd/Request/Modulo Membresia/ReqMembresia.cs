using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace BackEnd.Request.Modulo_Membresia
{
    /// <summary>
    /// Request para obtener una membresía por ID
    /// </summary>
    public class ReqMembresia
    {
        public int membresiaID { get; set; }
    }

    /// <summary>
    /// Request para obtener la membresía de un usuario
    /// </summary>
    public class ReqObtenerMembresiaUsuario
    {
        public int usuarioID { get; set; }
    }

    /// <summary>
    /// Request para asignar una membresía a un usuario
    /// </summary>
    public class ReqAsignarMembresia
    {
        public int usuarioID { get; set; }
        public int membresiaID { get; set; }
        public DateTime? fechaInicio { get; set; }
        public int? adminID { get; set; }
    }

    /// <summary>
    /// Request para registrar un pago
    /// </summary>
    public class ReqRegistrarPago
    {
        public int usuarioID { get; set; }
        public decimal monto { get; set; }
        public string metodoPago { get; set; }
        public string comprobanteRuta { get; set; }
    }

    /// <summary>
    /// Request para cancelar una membresía
    /// </summary>
    public class ReqCancelarMembresia
    {
        public int usuarioMembresiaID { get; set; }
        public int usuarioID { get; set; }
    }

    /// <summary>
    /// Request para obtener el historial de membresías
    /// </summary>
    public class ReqObtenerHistorialMembresias
    {
        public int usuarioID { get; set; }
    }

    /// <summary>
    /// Request para verificar si un usuario tiene membresía activa
    /// </summary>
    public class ReqVerificarMembresia
    {
        public int usuarioID { get; set; }
    }
}