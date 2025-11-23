using SGTO.Datos.Repositorios;
using SGTO.Negocio.DTOs.Medicos;
using SGTO.Negocio.Mappers;
using System;
using System.Collections.Generic;

namespace SGTO.Negocio.Servicios
{
    public class MedicoService
    {

        private readonly MedicoRepositorio _repositorioMedico;
        private readonly TurnoRepositorio _repositorioTurno;
        private readonly HorarioSemanalRepositorio _repositorioHorario;


        public MedicoService()
        {
            _repositorioMedico = new MedicoRepositorio();
            _repositorioTurno = new TurnoRepositorio();
            _repositorioHorario = new HorarioSemanalRepositorio();
        }

        public List<MedicoListadoDto> Listar(string estado = null)
        {
            try
            {
                return MedicoMapper.MapearAListado(_repositorioMedico.Listar(estado));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MedicoListadoDto> ListarPorEspecialidad(int idEspecialidad)
        {
            try
            {
                return MedicoMapper.MapearAListado(_repositorioMedico.ListarPorEspecialidad(idEspecialidad));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MedicoDetalleDto ObtenerDetalle(int id)
        {
            try
            {
                var medicoEntidad = _repositorioMedico.ObtenerPorId(id);
                if (medicoEntidad == null) return null;

                var historial = _repositorioTurno.ObtenerHistorialPorMedico(id);

                var dto = MedicoMapper.MapearADetalleDto(medicoEntidad);

                List<string> pacientesUnicos = new List<string>();
                foreach (var turno in historial)
                {
                    if (!pacientesUnicos.Contains(turno.Paciente))
                    {
                        pacientesUnicos.Add(turno.Paciente);
                    }
                }
                dto.CantidadPacientesAtendidos = pacientesUnicos.Count;

                var horariosEntidad = _repositorioHorario.ObtenerPorMedico(id);

                foreach (var h in horariosEntidad)
                {
                    string nombreDia = "";

                    switch (h.DiaSemana)
                    {
                        case 1: nombreDia = "Lunes"; break;
                        case 2: nombreDia = "Martes"; break;
                        case 3: nombreDia = "Miércoles"; break;
                        case 4: nombreDia = "Jueves"; break;
                        case 5: nombreDia = "Viernes"; break;
                        case 6: nombreDia = "Sábado"; break;
                        case 7: nombreDia = "Domingo"; break;
                        default: nombreDia = "Desconocido"; break;
                    }

                    string rango = $"{h.HoraInicio.ToString(@"hh\:mm")} - {h.HoraFin.ToString(@"hh\:mm")}";

                    dto.Horarios.Add(new MedicoHorarioDetalleDto
                    {
                        Dia = nombreDia,
                        RangoHorario = rango
                    });
                }

                dto.HistorialTurnos = historial;

                return dto;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }

}