using SGTO.Dominio.Entidades.Base;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using System;
using System.Collections.Generic;

namespace SGTO.Dominio.Entidades
{
    public class Medico : Persona
    {
        public int IdMedico { get; set; }
        public string Matricula { get; set; }
        public List<Especialidad> Especialidades { get; set; } = new List<Especialidad>();
        public List<Turno> TurnosAsignados { get; set; } = new List<Turno>();
        public List<HorarioAtencion> HorariosAtencion { get; set; } = new List<HorarioAtencion>();

        public Usuario Usuario { get; set; }

        public Medico()
        {
        }

        public Medico(
            string nombre,
            string apellido,
            DocumentoIdentidad dni,
            DateTime fechaNacimiento,
            Genero genero,
            Telefono telefono,
            Email email,
            string matricula,
            List<Especialidad> especialidades,
            Usuario usuario)
            : base(nombre, apellido, dni, fechaNacimiento, genero, telefono, email)
        {
            Matricula = matricula;
            Especialidades = especialidades ?? new List<Especialidad>();
            Usuario = usuario;
        }
    }
}
