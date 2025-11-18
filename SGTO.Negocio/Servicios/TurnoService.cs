using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Turnos;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Mappers;
using SGTO.Negocio.Servicios.EmailServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace SGTO.Negocio.Servicios
{
    public class TurnoService
    {
        private readonly TurnoRepositorio _repositorioTurno;
        private readonly PacienteRepositorio _repositorioPaciente;
        private readonly EspecialidadRepositorio _repositorioEspecialidad;
        private readonly HorarioSemanalRepositorio _repositorioHorario;
        private readonly CoberturaRepositorio _repositorioCobertura;
        private readonly PlanRepositorio _repositorioPlan;
        private readonly MedicoRepositorio _repositorioMedico;


        public TurnoService()
        {
            _repositorioTurno = new TurnoRepositorio();
            _repositorioPaciente = new PacienteRepositorio();
            _repositorioEspecialidad = new EspecialidadRepositorio();
            _repositorioHorario = new HorarioSemanalRepositorio();
            _repositorioCobertura = new CoberturaRepositorio();
            _repositorioPlan = new PlanRepositorio();
            _repositorioMedico = new MedicoRepositorio();
        }

        public bool TieneTurnosActivosPorCobertura(int idCobertura)
        {
            try
            {
                return _repositorioTurno.ExisteTurnoActivoPorCobertura(idCobertura);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool TieneTurnosActivosPorPlan(int idPlan)
        {
            try
            {
                return _repositorioTurno.ExisteTurnoActivoPorPlan(idPlan);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool TieneTurnosActivosPorEspecialidad(int idEspecialidad)
        {
            try
            {
                return _repositorioTurno.ExisteTurnoActivoPorEspecialidad(idEspecialidad);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool TieneTurnosActivosPorTratamiento(int idTratamiento)
        {
            try
            {
                return _repositorioTurno.ExisteTurnoActivoPorEspecialidad(idTratamiento);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<TurnoListadoDto> Listar()
        {
            try
            {
                return TurnoMapper.MapearListaTurnoListadoDto(_repositorioTurno.Listar());
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<DateTime> ObtenerFechasDisponibles(int idMedico, int semanas)
        {
            List<DateTime> fechasDisponibles = new List<DateTime>();

            try
            {
                List<HorarioSemanalMedico> horarios = _repositorioHorario.ObtenerPorMedico(idMedico);

                if (horarios.Count == 0)
                    return fechasDisponibles;

                DateTime desde = DateTime.Today.AddDays(1);
                DateTime hasta = DateTime.Today.AddDays(semanas * 7);

                // se debe recorrer cada día que se encientra dentro del rango
                for (DateTime fecha = desde; fecha < hasta; fecha = fecha.AddDays(1))
                {
                    // normalizar días ya qu een bd los días de semana son distintos. Ahora tendremos lunes == 1 y domingo == 7
                    byte diaSemanaBD = (byte)((int)fecha.DayOfWeek == 0 ? 7 : (int)fecha.DayOfWeek);

                    // verificar si el médico trabaja ese día
                    HorarioSemanalMedico horarioDia = horarios.Find(h => h.DiaSemana == diaSemanaBD);
                    if (horarioDia == null)
                        continue;

                    // verificar si existen slots disponibles
                    List<TimeSpan> slots = ObtenerSlotsDisponibles(idMedico, fecha);

                    if (slots.Count > 0)
                        fechasDisponibles.Add(fecha.Date);
                }

                return fechasDisponibles;
            }
            catch
            {
                throw;
            }
        }



        public List<TimeSpan> ObtenerSlotsDisponibles(int idMedico, DateTime fecha)
        {
            try
            {
                List<HorarioSemanalMedico> horarios = _repositorioHorario.ObtenerPorMedico(idMedico);

                // normalizar ya que el día inicia en lunes == 1, pero en la bd domingo == 1:
                byte diaSemanaBD = (byte)((int)fecha.DayOfWeek == 0 ? 7 : (int)fecha.DayOfWeek);

                HorarioSemanalMedico horario = horarios.Find(h => h.DiaSemana == diaSemanaBD);
                if (horario == null)
                    return new List<TimeSpan>();

                List<Turno> turnos = _repositorioTurno.ObtenerTurnosPorMedicoEnRango(
                    idMedico,
                    fecha.Date,
                    fecha.Date.AddDays(1)
                );

                List<TimeSpan> slots = new List<TimeSpan>();

                TimeSpan cursor = horario.HoraInicio;
                TimeSpan duracion = TimeSpan.FromHours(1);

                while (cursor.Add(duracion) <= horario.HoraFin)
                {
                    bool ocupado = turnos.Exists(t =>
                        t.Horario.Inicio.TimeOfDay == cursor
                    );

                    if (!ocupado)
                        slots.Add(cursor);

                    cursor = cursor.Add(duracion);
                }

                return slots;
            }
            catch
            {
                throw;
            }
        }


        public TurnoEdicionDto ObtenerPorId(int idTurno)
        {
            TurnoEdicionDto turno = new TurnoEdicionDto();
            try
            {

                return turno;
            }
            catch (Exception)
            {

                throw;
            }
        }



        public int Crear(TurnoCreacionDto dto, string rutaPlantillaEmail)
        {
            Paciente paciente = _repositorioPaciente.ObtenerPorId(dto.IdPaciente);
            Especialidad especialidad = _repositorioEspecialidad.ObtenerPorId(dto.IdEspecialidad);
            Medico medico = _repositorioMedico.ObtenerPorId(dto.IdMedico);
            ValidarReglasNegocioAgendaTurno(dto, paciente, especialidad);
            Turno turno = TurnoMapper.MapearACreacion(dto);
            int idTurno;
            try
            {
                idTurno = _repositorioTurno.Crear(turno);

            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                EmailService emailService = new EmailService();

                string htmlBase = CargarPlantillaDesdeArchivo(rutaPlantillaEmail);

                string htmlFinal = emailService.GenerarHtmlConfirmacion(
                    htmlBase,
                    paciente,
                    medico,
                    especialidad,
                    dto.FechaInicio,
                    dto.Observaciones
                );

                emailService.Enviar(
                    paciente.Email.Valor,
                    "Confirmación de turno",
                    htmlFinal
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
            return idTurno;
        }

        private string CargarPlantillaDesdeArchivo(string ruta)
        {
            try
            {
                using (var lector = new StreamReader(ruta))
                {
                    return lector.ReadToEnd();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        private void ValidarReglasNegocioAgendaTurno(TurnoCreacionDto dto, Paciente paciente, Especialidad especialidad)
        {
            // validar que el paciente exista

            if (paciente == null)
                throw new ExcepcionReglaNegocio("El paciente no existe.");

            // validar que esté activo el paciente
            if (paciente.Estado.ToString().ToLower()[0] != 'a')
                throw new ExcepcionReglaNegocio("No se puede agendar un turno para un paciente inactivo.");

            // verifivar que la cobertura esté activa
            Cobertura cobertura = _repositorioCobertura.ObtenerPorId(dto.IdCobertura);
            if (cobertura.Estado.ToString().ToLower()[0] != 'a')
                throw new ExcepcionReglaNegocio("La cobertura seleccionada está inactiva.");

            // verificar que la cobertura sea la del paciente.
            if (dto.IdCobertura != paciente.Cobertura.IdCobertura)
                throw new ExcepcionReglaNegocio("La cobertura seleccionada no coincide con la del paciente. " +
                    "Si la cobertura del paciente cambió, entonces debe editar su datos, asignar la cobertura nueva y volver a intentar agendar el turno.");

            // si la cobertura tiene planes, validar que esté activo
            if (dto.IdPlan != 0)
            {
                Plan plan = _repositorioPlan.ObtenerPorId(dto.IdPlan);
                if (plan.Estado.ToString().ToLower()[0] != 'a')
                    throw new ExcepcionReglaNegocio("El plan seleccionado está inactivo.");

                if (paciente.Plan.IdPlan != 0 && dto.IdPlan != paciente.Plan.IdPlan)
                    throw new ExcepcionReglaNegocio("El plan seleccionado no coincide con el del paciente. " +
                        "Si el plan del paciente cambió, entonces debe editar su datos, asignar el plan nuevo y volver a intentar agendar el turno.");
            }

            //validar que la especialidad esté activa.

            if (especialidad.Estado.ToString().ToLower()[0] != 'a')
                throw new ExcepcionReglaNegocio("La especialidad está inactiva.");

            // verificar que el médico de verdad atiende el día indicado
            List<HorarioSemanalMedico> horarios = _repositorioHorario.ObtenerPorMedico(dto.IdMedico);
            byte dia = (byte)((int)dto.FechaInicio.DayOfWeek == 0 ? 7 : (int)dto.FechaInicio.DayOfWeek);
            HorarioSemanalMedico horario = horarios.Find(h => h.DiaSemana == dia);

            if (horario == null)
                throw new ExcepcionReglaNegocio("El médico no atiende ese día.");

            // validar qu ela hora seleccionada esté dentro del rango del horario del médico
            if (dto.FechaInicio.TimeOfDay < horario.HoraInicio ||
                dto.FechaFin.TimeOfDay > horario.HoraFin)
                throw new ExcepcionReglaNegocio("El horario no está dentro de la jornada del médico.");

            // validar que la hora no sea inválida
            if (dto.FechaFin <= dto.FechaInicio)
                throw new ExcepcionReglaNegocio("La fecha de fin debe ser posterior al inicio.");

            // verificar que la hora no esté ocupada por otro turno.
            List<Turno> turnos = _repositorioTurno.ObtenerTurnosPorMedicoEnRango(dto.IdMedico, dto.FechaInicio.Date, dto.FechaInicio.Date.AddDays(1));
            bool ocupado = turnos.Exists(t =>
                 (dto.FechaInicio < t.Horario.Fin && dto.FechaFin > t.Horario.Inicio)
             );
            if (ocupado)
                throw new ExcepcionReglaNegocio("El horario seleccionado ya está ocupado.");

            List<Turno> turnosPacienteEseDia = _repositorioTurno.ObtenerTurnosPorMedicoEnRango(
                dto.IdMedico,
                dto.FechaInicio.Date,
                dto.FechaInicio.Date.AddDays(1)
            );

            // validar si el paciente YA tiene un turno ese día
            bool yaTieneTurnoMismoDia = turnosPacienteEseDia.Exists(t =>
                t.Paciente.IdPaciente == dto.IdPaciente &&
                t.Especialidad.IdEspecialidad == dto.IdEspecialidad &&
                t.Estado != EstadoTurno.Cancelado &&
                t.Estado != EstadoTurno.Cerrado
            );

            if (yaTieneTurnoMismoDia)
                throw new ExcepcionReglaNegocio(
                    "El paciente ya tiene un turno agendado con este médico y especialidad en el mismo día."
                );
        }


    }
}



