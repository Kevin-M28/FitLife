using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Entidades
{
    /// <summary>
    /// Representa una membresía disponible en el sistema
    /// </summary>
    public class Membresia
    {
        public int MembresiaID { get; set; }

        [Required(ErrorMessage = "El tipo de membresía es requerido")]
        [StringLength(50, ErrorMessage = "El tipo no puede exceder 50 caracteres")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La duración es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La duración debe ser mayor que 0")]
        public int DuracionDias { get; set; }

        [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
        public string Descripcion { get; set; }

        // Navigation properties
        public virtual ICollection<UsuarioMembresia> UsuarioMembresias { get; set; }
    }

    /// <summary>
    /// DTO para transferir información de membresías
    /// </summary>
    public class MembresiaDTO
    {
        public int MembresiaID { get; set; }
        public string Tipo { get; set; }
        public decimal Precio { get; set; }
        public int DuracionDias { get; set; }
        public string Descripcion { get; set; }
    }



    /// <summary>
    /// Modelo para solicitar una nueva membresía
    /// </summary>
    public class NuevaMembresiaRequest
    {
        [Required(ErrorMessage = "El ID de usuario es requerido")]
        public int UsuarioID { get; set; }

        [Required(ErrorMessage = "El ID de membresía es requerido")]
        public int MembresiaID { get; set; }

        public DateTime? FechaInicio { get; set; }

        public int? AdminID { get; set; } // Opcional, solo si es asignado por un admin
    }

    /// <summary>
    /// Modelo para registrar un pago de membresía
    /// </summary>
    public class PagoMembresiaRequest
    {
        [Required(ErrorMessage = "El ID de usuario es requerido")]
        public int UsuarioID { get; set; }

        [Required(ErrorMessage = "El monto es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que 0")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El método de pago es requerido")]
        [StringLength(20, ErrorMessage = "El método de pago no puede exceder 20 caracteres")]
        public string MetodoPago { get; set; } // 'efectivo', 'tarjeta', 'transferencia', 'pendiente'

        public string ComprobanteRuta { get; set; }
    }
}