using System;
using System.Collections.Generic;
using SGTO.Comun.DTOs;

namespace SGTO.Negocio.DTOs.Medicos
{
    public class MedicoDetalleDto
    {
        // --- Info Personal ---
        public int IdMedico { get; set; }
        public string NombreCompleto { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; }
        public string Genero { get; set; }

        // --- Info Profesional ---
        public string Matricula { get; set; }
        public DateTime FechaIncorporacion { get; set; }
        public string NombreUsuario { get; set; }

        // Listas
        public List<int> IdEspecialidades { get; set; } = new List<int>();
        public List<string> Especialidades { get; set; } = new List<string>();
        public List<string> CoberturasAceptadas { get; set; } = new List<string>();
        public int CantidadPacientesAtendidos { get; set; }

        // --- Historial ---
        public List<TurnoHistorialDto> HistorialTurnos { get; set; } = new List<TurnoHistorialDto>();
    }
}