using System;
using System.Collections.Generic;

namespace SGTO.Negocio.DTOs.Medicos
{
    public class MedicoDetalleDto
    {
        public int IdMedico { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; } // M, F, O, N
        public string Telefono { get; set; }
        public string Matricula { get; set; }
        public List<int> IdEspecialidades { get; set; }
        public string Estado { get; set; } // A, I
    }
}
