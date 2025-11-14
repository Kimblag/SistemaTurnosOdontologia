using SGTO.Negocio.DTOs.Medicos;
using System;
using System.Collections.Generic;

namespace SGTO.Negocio.DTOs
{
    public class MedicoCrearDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NumeroDocumento { get; set; }
        public string Genero { get; set; } // M, F, O, N
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Matricula { get; set; }
        public int IdUsuario { get; set; }
        public List<int> IdEspecialidades { get; set; }
        public string Estado { get; set; } = "A"; // por defecto debe ser activo
        public List<HorarioSemanalDto> HorariosSemanales { get; set; } = new List<HorarioSemanalDto>(); // importante!! para lso horarios de disponibilidad del médico

    }
}
