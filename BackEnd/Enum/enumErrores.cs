using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Enum
{
    public enum enumErrores
    {
        requestNulo = -1,
        excepcionLogica = -2,
        excepcionBaseDatos = -3,
        usuarioFaltante = 1,
        idFaltante = 2,
        correoFaltante = 3,
        correoIncorrecto = 4,
        passwordFaltante = 5,
        passwordMuyDebil = 6,
        nombreFaltante = 7,
        apellidoFaltante = 8,
        rolFaltante = 9,
        sesionCerrada = 10,
        verificacionFallida = 11,
        rolNoExistente = 12,
        gimnasioNoExistente = 13,
        permisosDenegados = 14, // Nuevo error para acceso denegado
        membresiaNoActiva = 15, // Nuevo error para membresía no activa
        idRecursoFaltante = 16, // ID genérico faltante
        claseLlena = 17, // Clase llena
        reservaExistente = 18, // Ya existe una reserva
        recursoNoExistente = 19 // Recurso no encontrado genérico
    }
}
