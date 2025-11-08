using SGTO.Negocio.DTOs.Turnos;
using System;
using System.Collections.Generic;


namespace SGTO.Negocio.DTOs.Pacientes
{
    public class PacienteDetalleDto
    {
        public int IdPaciente { get; set; }
        public string NombreCompleto { get; set; }
        public string Dni { get; set; }
        public string FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Cobertura { get; set; }
        public string Plan { get; set; }
        public string Estado { get; set; }

        public List<TurnoPacienteDto> Turnos { get; set; } = new List<TurnoPacienteDto>();
    }
}
