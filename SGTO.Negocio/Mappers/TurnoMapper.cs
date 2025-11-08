using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs.Turnos;
using System.Collections.Generic;


namespace SGTO.Negocio.Mappers
{
    public static class TurnoMapper
    {
        public static List<TurnoPacienteDto> MapearListaTurnoPacienteDto(List<Turno> turnos)
        {
            List<TurnoPacienteDto> lista = new List<TurnoPacienteDto>();

            foreach (Turno turno in turnos)
            {
                lista.Add(new TurnoPacienteDto()
                {
                    IdTurnoPaciente = turno.IdTurno,
                    Fecha = turno.Horario.Inicio.ToString("dd/MM/yyyy"),
                    Hora = turno.Horario.Inicio.ToString("HH:mm"),
                    Medico = $"{turno.Medico.Apellido} {turno.Medico.Nombre}",
                    Observaciones = string.IsNullOrEmpty(turno.Observaciones) ? "-" : turno.Observaciones,
                    Estado = turno.Estado.ToString()
                });
            }
            return lista;
        }


    }
}
