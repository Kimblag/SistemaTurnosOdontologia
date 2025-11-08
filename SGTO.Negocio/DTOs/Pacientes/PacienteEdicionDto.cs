using System;

namespace SGTO.Negocio.DTOs.Pacientes
{
    public class PacienteEdicionDto
    {
        public int IdPaciente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public char Genero { get; set; }  // M, F, O, N
        public string Telefono { get; set; }
        public string Email { get; set; }
        public int? IdCobertura { get; set; }
        public int? IdPlan { get; set; }
        public char Estado { get; set; }  // Activo, Inactivo

        public PacienteEdicionDto() { }

        public PacienteEdicionDto(int idPaciente, string nombre, string apellido, string dni,
            DateTime fechaNacimiento, char genero, string telefono, string email,
            int? idCobertura, int? idPlan, char estado)
        {
            IdPaciente = idPaciente;
            Nombre = nombre;
            Apellido = apellido;
            Dni = dni;
            FechaNacimiento = fechaNacimiento;
            Genero = genero;
            Telefono = telefono;
            Email = email;
            IdCobertura = idCobertura;
            IdPlan = idPlan;
            Estado = estado;
        }
    }

}
