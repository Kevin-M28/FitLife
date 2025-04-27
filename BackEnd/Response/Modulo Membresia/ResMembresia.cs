using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BackEnd.Entidades;

namespace BackEnd.Response.Modulo_Membresia
{
    /// <summary>
    /// Respuesta para obtener todas las membresías
    /// </summary>
    public class ResObtenerMembresias
    {
        public bool resultado { get; set; }
        public List<Membresia> membresias { get; set; }
        public List<Error> error { get; set; }
    }

    /// <summary>
    /// Respuesta para obtener una membresía específica
    /// </summary>
    public class ResObtenerMembresia
    {
        public bool resultado { get; set; }
        public Membresia membresia { get; set; }
        public List<Error> error { get; set; }
    }

    /// <summary>
    /// Respuesta para obtener la membresía de un usuario
    /// </summary>
    public class ResObtenerMembresiaUsuario
    {
        public bool resultado { get; set; }
        public UsuarioMembresia usuarioMembresia { get; set; }
        public Membresia membresia { get; set; }
        public string nombreUsuario { get; set; }
        public string apellidoUsuario { get; set; }
        public List<Error> error { get; set; }
    }

    /// <summary>
    /// Respuesta para asignar una membresía
    /// </summary>
    public class ResAsignarMembresia
    {
        public bool resultado { get; set; }
        public int usuarioMembresiaID { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public string tipoMembresia { get; set; }
        public int diasRestantes { get; set; }
        public List<Error> error { get; set; }
    }

    /// <summary>
    /// Respuesta para registrar un pago
    /// </summary>
    public class ResRegistrarPago
    {
        public bool resultado { get; set; }
        public int pagoID { get; set; }
        public decimal monto { get; set; }
        public DateTime fechaPago { get; set; }
        public string estado { get; set; }
        public List<Error> error { get; set; }
    }

    /// <summary>
    /// Respuesta para cancelar una membresía
    /// </summary>
    public class ResCancelarMembresia
    {
        public bool resultado { get; set; }
        public List<Error> error { get; set; }
    }

    /// <summary>
    /// Respuesta para obtener el historial de membresías
    /// </summary>
    public class ResObtenerHistorialMembresias
    {
        public bool resultado { get; set; }
        public List<UsuarioMembresia> historialMembresias { get; set; }
        public List<Error> error { get; set; }
    }

    /// <summary>
    /// Respuesta para verificar si un usuario tiene membresía activa
    /// </summary>
    public class ResVerificarMembresia
    {
        public bool resultado { get; set; }
        public bool tieneMembresia { get; set; }
        public bool estaEnMorosidad { get; set; }
        public List<Error> error { get; set; }
    }
}