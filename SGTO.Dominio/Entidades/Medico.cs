using SGTO.Dominio.Entidades.Base;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Dominio.Entidades
{
    public class Medico : Persona
    {
        public int IdMedico { get; set; }
        public string Matricula { get; set; }
        public List<Especialidad> Especialidades { get; set; }
        public List<Turno> TurnosAsignados { get; set; }
        public Usuario Usuario { get; set; } // el usuario de la app

        public Medico(string nombre, string apellido, DocumentoIdentidad dni,
           DateTime fechaNacimiento, Genero genero, Telefono telefono, Email email,
           string matricula, List<Especialidad> especialidades, Usuario usuario)
           : base(nombre, apellido, dni, fechaNacimiento, genero, telefono, email)
        {
            Matricula = matricula;
            Especialidades = especialidades ?? new List<Especialidad>();
            TurnosAsignados = new List<Turno>();
            Usuario = usuario;
        }

        public Medico(int idMedico, string nombre, string apellido, DocumentoIdentidad dni,
            DateTime fechaNacimiento, Genero genero, Telefono telefono, Email email,
            string matricula, List<Especialidad> especialidades,
            List<Turno> turnosAsignados, Usuario usuario)
            : base(nombre, apellido, dni, fechaNacimiento, genero, telefono, email)
        {
            IdMedico = idMedico;
            Matricula = matricula;
            Especialidades = especialidades ?? new List<Especialidad>();
            TurnosAsignados = turnosAsignados ?? new List<Turno>();
            Usuario = usuario;
        }

        public void AgregarEspecialidad(Especialidad especialidad) { }
        public List<Turno> ObtenerTurnosPorFechas(DateTime fecha)
        {
            return new List<Turno>();
        }

        public bool EstaDisponible(DateTime inicio, DateTime fin)
        {
            return true;
        }
    }
}
