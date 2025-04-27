using AccesoDatos;
using BackEnd.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Entidades
{
    public class UsuarioMembresia
    {
        public int UsuarioMembresiaID { get; set; }
        public int UsuarioID { get; set; }
        public int MembresiaID { get; set; }

        public int DiasRestantes { get; set; }


        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento es requerida")]
        public DateTime FechaVencimiento { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        [StringLength(20, ErrorMessage = "El estado no puede exceder 20 caracteres")]
        public string Estado { get; set; } // 'activa', 'vencida', 'cancelada'

        // Navigation properties
        public virtual Usuario Usuario { get; set; }
        public virtual Membresia Membresia { get; set; }



        /// <summary>
        /// DTO para la información de una membresía de usuario con datos adicionales
        /// </summary>
        public class UsuarioMembresiaDTO
        {
            public int UsuarioMembresiaID { get; set; }
            public int UsuarioID { get; set; }
            public string NombreUsuario { get; set; }
            public string ApellidoUsuario { get; set; }
            public int MembresiaID { get; set; }
            public string TipoMembresia { get; set; }
            public decimal Precio { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime FechaVencimiento { get; set; }
            public int DiasRestantes { get; set; }
            public string Estado { get; set; }
        }
    }
}
