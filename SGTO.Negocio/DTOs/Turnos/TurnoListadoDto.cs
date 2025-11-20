using System;
namespace SGTO.Negocio.DTOs
{
    public class TurnoListadoDto
    {
        public int IdTurno { get; set; }
        public string DniPaciente { get; set; }
        public string NombrePaciente { get; set; }
        public int IdMedico { get; set; }
        public string Matricula { get; set; }
        public string NombreMedico { get; set; }
        public int IdEspecialidad { get; set; }
        public string Especialidad { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public int IdCobertura { get; set; }
        public string Cobertura { get; set; }
        public int IdPlan { get; set; }
        public string Plan { get; set; }
        public string Estado { get; set; }
    }
}
