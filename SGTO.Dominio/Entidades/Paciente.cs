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

        public Paciente() : base()
        {
            Turnos = new List<Turno>();
            HistoriaClinica = new List<HistoriaClinicaRegistro>();
        }

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
            Turnos = turnos ?? new List<Turno>();
            HistoriaClinica = historiaClinica;
        }

        // metodos de comportamiento de un paciente
        public void AgregarTurno(Turno turno)
        {
            if (turno == null)
                throw new ArgumentException("El turno no puede ser nulo.");
            Turnos.Add(turno);
        }

        public void AgregarRegistroHistoria(HistoriaClinicaRegistro registro)
        {
            if (registro == null)
                throw new ArgumentException("El registro no puede ser nulo.");

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
            if (Turnos == null || Turnos.Count == 0)
                return false;

            foreach (Turno turno in Turnos)
            {
                if (turno.Horario == null)
                    continue;

                if (turno.Estado == EstadoTurno.Cancelado ||
                    turno.Estado == EstadoTurno.Cerrado)
                    continue;

                if (fechaHora >= turno.Horario.Inicio &&
                    fechaHora < turno.Horario.Fin)
                {
                    return true;
                }
            }
            return false;
        }



    }
}
