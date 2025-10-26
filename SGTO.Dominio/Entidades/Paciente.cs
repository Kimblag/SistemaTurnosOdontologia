using SGTO.Dominio.Entidades.Base;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using System;
using System.Collections.Generic;

namespace SGTO.Dominio.Entidades
{
    public class Paciente : Persona
    {
        public int IdPaciente { get; set; }
        public Cobertura Cobertura { get; set; }
        public Plan Plan { get; set; }
        public List<Turno> Turnos { get; set; }
        public List<HistoriaClinicaRegistro> HistoriaClinica { get; set; }


        public Paciente(string nombre, string apellido, DocumentoIdentidad dni,
            DateTime fechaNacimiento, Genero genero, Telefono telefono, Email email,
            Cobertura cobertura, Plan plan)
            : base(nombre, apellido, dni, fechaNacimiento, genero, telefono, email)
        {
            Cobertura = cobertura;
            Plan = plan;
            Turnos = new List<Turno>();
            HistoriaClinica = new List<HistoriaClinicaRegistro>();
        }

        public Paciente(int idPaciente, string nombre, string apellido, DocumentoIdentidad dni,
            DateTime fechaNacimiento, Genero genero, Telefono telefono,
            Email email, Cobertura cobertura, Plan plan,
            List<Turno> turnos, List<HistoriaClinicaRegistro> historiaClinica)
            : base(nombre, apellido, dni, fechaNacimiento, genero, telefono, email)
        {
            IdPaciente = idPaciente;
            Cobertura = cobertura;
            Plan = plan;
            Turnos = turnos;
            HistoriaClinica = historiaClinica;
        }

        public void AgregarTurno(Turno turno)
        {
            Turnos.Add(turno);
        }

        public void AgregarRegistroHistoria(HistoriaClinicaRegistro registro)
        {
            HistoriaClinica.Add(registro);
        }

        public List<Turno> ObtenerTurnosPorEstado(EstadoTurno estado)
        {
            List<Turno> turnosFiltrados = new List<Turno>();

            foreach (Turno turno in Turnos)
            {
                if (turno.Estado == estado)
                {
                    turnosFiltrados.Add(turno);
                }
            }
            return turnosFiltrados;
        }

        public bool TieneTurnoEnHorario(DateTime fechaHora)
        {
            return true;
        }
    }
}
