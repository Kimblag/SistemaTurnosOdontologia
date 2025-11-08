
namespace SGTO.Negocio.DTOs.Pacientes
{
    public class PacienteListadoDto
    {
        public int IdPaciente { get; set; }
        public string NombreCompleto { get; set; }
        public string Dni { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; }

        public int? IdCobertura { get; set; }
        public string NombreCobertura { get; set; }

        public int? IdPlan { get; set; }
        public string NombrePlan { get; set; }
    }
}
