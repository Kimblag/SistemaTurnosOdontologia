using System.Collections.Generic;

namespace SGTO.Negocio.DTOs.Medicos
{
    public class MedicoListadoDto
    {
        public int IdMedico { get; set; }
        public string NombreCompleto { get; set; }
        public string Dni { get; set; }
        public string Matricula { get; set; }
        public string Telefono { get; set; }
        public string Estado { get; set; }
        public List<string> NombresEspecialidades  { get; set; }
    }
}