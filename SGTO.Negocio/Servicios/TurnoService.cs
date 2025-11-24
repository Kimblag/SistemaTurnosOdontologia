using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Seguridad;
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
        private readonly HistoriaClinicaRepositorio _repositorioHistoria;


        public TurnoService()
        {
            _repositorioTurno = new TurnoRepositorio();
            _repositorioPaciente = new PacienteRepositorio();
            _repositorioEspecialidad = new EspecialidadRepositorio();
            _repositorioHorario = new HorarioSemanalRepositorio();
            _repositorioCobertura = new CoberturaRepositorio();
            _repositorioPlan = new PlanRepositorio();
            _repositorioMedico = new MedicoRepositorio();
            _repositorioHistoria = new HistoriaClinicaRepositorio();
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
                return TurnoMapper.MapearListaTurnoListadoDto(_repositorioTurno.Listar(null, null, null, null, null));
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

                DateTime desde = DateTime.Today;
                DateTime hasta = DateTime.Today.AddDays(semanas * 7);

                // se debe recorrer cada día que se encientra dentro del rango
                for (DateTime fecha = desde; fecha < hasta; fecha = fecha.AddDays(1))
                {
                    // normalizar días ya qu een bd los días de semana son distintos. Ahora tendremos lunes == 1 y domingo == 7
                    byte diaSemanaBD = (byte)((int)fecha.DayOfWeek == 0 ? 7 : (int)fecha.DayOfWeek);

                    // verificar si el médico trabaja ese día
                    bool trabajaEseDia = horarios.Exists(h => h.DiaSemana == diaSemanaBD);
                    if (!trabajaEseDia) continue;

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

                List<HorarioSemanalMedico> rangosDelDia = horarios.FindAll(h => h.DiaSemana == diaSemanaBD);
                if (rangosDelDia.Count == 0)
                    return new List<TimeSpan>();

                List<Turno> turnos = _repositorioTurno.ObtenerTurnosPorMedicoEnRango(
                    idMedico,
                    fecha.Date,
                    fecha.Date.AddDays(1)
                );

                List<TimeSpan> slots = new List<TimeSpan>();
                TimeSpan duracion = TimeSpan.FromHours(1);

                // verificamos la hora y día actual para poder ofrecer turnos disponibles en el día actual
                TimeSpan horaActual = DateTime.Now.TimeOfDay;
                bool esHoy = fecha.Date == DateTime.Today;

                foreach (HorarioSemanalMedico rango in rangosDelDia)
                {
                    TimeSpan cursor = rango.HoraInicio;
                    // mientras el rango (hora de inicio del médico) sea menor que la hora del fin de este rango
                    // por ejemplo inicia 9:00 hs y termina a las 12 hs, le sumamos la duración que sería un timespan de 1 hora
                    // 9 : 00 hs + 1:00 hs = 10 : 00 : 00 hs
                    while (cursor.Add(duracion) <= rango.HoraFin)
                    {
                        // si es el dia acyial y la hora de inicio del turno es menor a la hora actual,
                        // significa que el turno ya pasó así que lo obviamos
                        if (esHoy && cursor < horaActual)
                        {
                            cursor = cursor.Add(duracion);
                            continue;
                        }
                        // si timeofday (extrae la hora del datetime, ejemplo 9:00:00) es igual al curso actual, ese rango esta ocupado
                        bool ocupado = turnos.Exists(t => t.Horario.Inicio.TimeOfDay == cursor);
                        if (!ocupado) slots.Add(cursor);

                        // le reasignamos al cursor 1 hora más por ejemplo si cursor era 9:00:00, ahoa sera 10:00:00 para la siguiente vuelta
                        cursor = cursor.Add(duracion);
                    }
                }
                slots.Sort();
                return slots;
            }
            catch
            {
                throw;
            }
        }


        public TurnoEdicionDto ObtenerPorId(int idTurno)
        {
            try
            {
                Turno entidad = _repositorioTurno.ObtenerPorId(idTurno);

                if (entidad == null)
                    throw new ExcepcionReglaNegocio("El turno no existe.");

                return TurnoMapper.MapearAEdicionDto(entidad);
            }
            catch
            {
                throw;
            }
        }


        public TurnoDetalleDto ObtenerDetallePorId(int idTurno)
        {
            Turno entidad = _repositorioTurno.ObtenerPorId(idTurno);

            if (entidad == null)
                throw new ExcepcionReglaNegocio("El turno no existe.");

            TurnoDetalleDto dto = TurnoMapper.MapearADetalleDto(entidad);

            if (entidad.Estado == EstadoTurno.Cerrado)
            {
                try
                {
                    HistoriaClinicaRegistro historia = _repositorioHistoria.ObtenerPorIdTurno(idTurno);

                    if (historia != null)
                    {
                        dto.Diagnostico = historia.Diagnostico;

                        dto.ObservacionesClinicas = historia.Observaciones;

                        if (historia.TratamientoAplicado != null)
                        {
                            dto.TratamientoAplicado = historia.TratamientoAplicado.Nombre;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return dto;
        }


        public int Crear(TurnoCreacionDto dto, string rutaPlantillaEmail)
        {
            Paciente paciente = _repositorioPaciente.ObtenerPorId(dto.IdPaciente);
            Especialidad especialidad = _repositorioEspecialidad.ObtenerPorId(dto.IdEspecialidad);
            Medico medico = _repositorioMedico.ObtenerPorId(dto.IdMedico);

            ValidarReglasNegocioAgendaTurno(dto, paciente, especialidad);

            Turno turno = TurnoMapper.MapearACreacion(dto);
            int idTurno = _repositorioTurno.Crear(turno);

            EnviarEmailConfirmacion(paciente, medico, especialidad, dto.FechaInicio, dto.Observaciones, rutaPlantillaEmail);

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

        private void EnviarEmailNotificacion(Paciente pac, Medico med, Especialidad esp, DateTime fecha, string asunto, string mensajeAdicional, string rutaPlantilla)
        {
            try
            {
                if (string.IsNullOrEmpty(rutaPlantilla)) return;

                EmailService emailService = new EmailService();
                string htmlBase = CargarPlantillaDesdeArchivo(rutaPlantilla);

                string htmlFinal = emailService.GenerarHtmlConfirmacion(htmlBase, pac, med, esp, fecha, mensajeAdicional);

                emailService.Enviar(pac.Email.Valor, asunto, htmlFinal);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error enviando email de notificación: " + ex.Message);
            }
        }


        private void EnviarEmailConfirmacion(Paciente pac, Medico med, Especialidad esp, DateTime fecha, string obs, string ruta)
        {
            EnviarEmailNotificacion(pac, med, esp, fecha, "Confirmación de turno", obs, ruta);
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
            List<HorarioSemanalMedico> rangosDelDia = horarios.FindAll(h => h.DiaSemana == dia);

            if (rangosDelDia.Count == 0)
                throw new ExcepcionReglaNegocio("El médico no atiende ese día.");

            bool horarioValido = false;
            foreach (var rango in rangosDelDia)
            {
                if (dto.FechaInicio.TimeOfDay >= rango.HoraInicio &&
                    dto.FechaFin.TimeOfDay <= rango.HoraFin)
                {
                    horarioValido = true;
                    break;
                }
            }

            if (!horarioValido)
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



        public void Editar(TurnoEdicionDto dto, string rutaPlantillaReprogramacion, string rutaPlantillaCancelacion, int idUsuarioModificacion = 0)
        {
            Turno turnoExistente = null;
            try
            {
                turnoExistente = _repositorioTurno.ObtenerPorId(dto.IdTurno);
            }
            catch (Exception)
            {
                throw;
            }

            if (turnoExistente == null)
                throw new ExcepcionReglaNegocio("El turno no existe.");

            EstadoTurno estadoOriginal = turnoExistente.Estado;
            DateTime fechaOriginal = turnoExistente.Horario.Inicio;


            // validar si es un cambio de estado nada mas como cancelado o no asistió
            bool esCambioEstadoAdministrativo = (dto.Estado == 'X' || dto.Estado == 'C');

            if (esCambioEstadoAdministrativo)
            {
                // si cambia a cancelar o no asistió se debe manetener la información original para trazabilidad.
                dto.IdPaciente = turnoExistente.Paciente.IdPaciente;
                dto.IdMedico = turnoExistente.Medico.IdMedico;
                dto.IdEspecialidad = turnoExistente.Especialidad.IdEspecialidad;
                dto.FechaInicio = turnoExistente.Horario.Inicio;
                dto.FechaFin = turnoExistente.Horario.Fin;

                dto.IdCobertura = turnoExistente.Paciente.Cobertura.IdCobertura;
                if (turnoExistente.Paciente.Plan != null)
                    dto.IdPlan = turnoExistente.Paciente.Plan.IdPlan;

                // validar que si cambia a NO asistió la hora ya haya pasado
                ValidarCambioDeEstado(turnoExistente, dto.Estado, dto.FechaInicio, dto.FechaFin);
                TurnoMapper.MapearEdicion(turnoExistente, dto, false);
            }
            else
            {
                // verificar si está intentando cambiar de horario
                bool esCambioDeHorario = turnoExistente.Horario.Inicio != dto.FechaInicio ||
                                         turnoExistente.Horario.Fin != dto.FechaFin;

                ValidarPaciente(dto.IdPaciente);
                ValidarEspecialidad(dto.IdEspecialidad);
                ValidarMedico(dto.IdMedico, dto.IdEspecialidad);
                ValidarCoberturaYPlan(dto.IdCobertura, dto.IdPlan, dto.IdPaciente);

                ValidarFechaYHora(dto.FechaInicio, dto.FechaFin);

                // Validamos disponibilidad solo si se reprograma o si se intenta guardar cambios en un turno nuevo
                if (esCambioDeHorario || dto.Estado == 'R' || dto.Estado == 'N')
                {
                    ValidarDisponibilidadMedico(dto.IdMedico, dto.FechaInicio, dto.FechaFin, dto.IdTurno);
                    ValidarTurnosPacienteMismoDia(dto.IdPaciente, dto.IdEspecialidad, dto.FechaInicio, dto.IdTurno);
                }

                // Detectar cambio de fecha/hora para forzar estado reprogramado si estaba en Nuevo
                if (turnoExistente.Estado == EstadoTurno.Nuevo && esCambioDeHorario)
                {
                    dto.Estado = 'R';
                }

                ValidarCambioDeEstado(turnoExistente, dto.Estado, dto.FechaInicio, dto.FechaFin);
                TurnoMapper.MapearEdicion(turnoExistente, dto, true);
            }

            _repositorioTurno.Actualizar(turnoExistente, idUsuarioModificacion);

            if (dto.Estado == 'C' && estadoOriginal != EstadoTurno.Cancelado)
            {
                var paciente = _repositorioPaciente.ObtenerPorId(dto.IdPaciente);
                var medico = _repositorioMedico.ObtenerPorId(dto.IdMedico);
                var especialidad = _repositorioEspecialidad.ObtenerPorId(dto.IdEspecialidad);

                EnviarEmailNotificacion(
                    paciente, medico, especialidad, dto.FechaInicio,
                    "Turno Cancelado",
                    "Le informamos que su turno ha sido cancelado.",
                    rutaPlantillaCancelacion
                );
            }
            else if (dto.Estado == 'R')
            {
                if (estadoOriginal != EstadoTurno.Reprogramado || fechaOriginal != dto.FechaInicio)
                {
                    var paciente = _repositorioPaciente.ObtenerPorId(dto.IdPaciente);
                    var medico = _repositorioMedico.ObtenerPorId(dto.IdMedico);
                    var especialidad = _repositorioEspecialidad.ObtenerPorId(dto.IdEspecialidad);

                    EnviarEmailNotificacion(
                        paciente, medico, especialidad, dto.FechaInicio,
                        "Turno Reprogramado",
                        "Turno reprogramado",
                        rutaPlantillaReprogramacion
                    );
                }
            }
        }

        private void ValidarPaciente(int idPaciente)
        {
            Paciente paciente = _repositorioPaciente.ObtenerPorId(idPaciente);
            if (paciente == null)
                throw new ExcepcionReglaNegocio("El paciente no existe.");
            if (paciente.Estado.ToString().ToLower()[0] != 'a')
                throw new ExcepcionReglaNegocio("El paciente está inactivo.");
        }

        private void ValidarEspecialidad(int idEspecialidad)
        {
            Especialidad especialidad = _repositorioEspecialidad.ObtenerPorId(idEspecialidad);
            if (especialidad == null)
                throw new ExcepcionReglaNegocio("La especialidad no existe.");
            if (especialidad.Estado.ToString().ToLower()[0] != 'a')
                throw new ExcepcionReglaNegocio("La especialidad está inactiva.");
        }

        private void ValidarMedico(int idMedico, int idEspecialidad)
        {
            Medico medico = _repositorioMedico.ObtenerPorId(idMedico);
            if (medico == null)
                throw new ExcepcionReglaNegocio("El médico no existe.");
            if (medico.Especialidades.Count > 0)
            {
                Especialidad especialidad = medico.Especialidades.Find(e => e.IdEspecialidad == idEspecialidad);
                if (especialidad == null)
                    throw new ExcepcionReglaNegocio("El médico no pertenece a la especialidad seleccionada.");
            }
        }

        private void ValidarCoberturaYPlan(int idCobertura, int idPlan, int idPaciente)
        {
            Cobertura cobertura = _repositorioCobertura.ObtenerPorId(idCobertura);
            if (cobertura == null || cobertura.Estado.ToString().ToLower()[0] != 'a')
                throw new ExcepcionReglaNegocio("La cobertura seleccionada está inactiva.");

            Paciente paciente = _repositorioPaciente.ObtenerPorId(idPaciente);
            if (idCobertura != paciente.Cobertura.IdCobertura)
                throw new ExcepcionReglaNegocio("La cobertura no coincide con la del paciente.");

            if (idPlan != 0)
            {
                Plan plan = _repositorioPlan.ObtenerPorId(idPlan);
                if (plan == null || plan.Estado.ToString().ToLower()[0] != 'a')
                    throw new ExcepcionReglaNegocio("El plan seleccionado está inactivo.");
                if (plan.Cobertura.IdCobertura != idCobertura)
                    throw new ExcepcionReglaNegocio("El plan no coincide con la cobertura seleccionada.");
            }
        }

        private void ValidarFechaYHora(DateTime inicio, DateTime fin)
        {
            if (fin <= inicio)
                throw new ExcepcionReglaNegocio("La fecha de fin debe ser posterior al inicio.");
        }

        private void ValidarDisponibilidadMedico(int idMedico, DateTime inicio, DateTime fin, int idTurno)
        {
            List<HorarioSemanalMedico> horarios = _repositorioHorario.ObtenerPorMedico(idMedico);
            byte dia = (byte)((int)inicio.DayOfWeek == 0 ? 7 : (int)inicio.DayOfWeek);
            List<HorarioSemanalMedico> rangosDelDia = horarios.FindAll(h => h.DiaSemana == dia);
            if (rangosDelDia.Count == 0)
                throw new ExcepcionReglaNegocio("El médico no atiende ese día.");

            // revisar si el turno a agendar encaja dentro del rango del m[edico
            bool horarioValido = false;
            foreach (var rango in rangosDelDia)
            {

                if (inicio.TimeOfDay >= rango.HoraInicio && fin.TimeOfDay <= rango.HoraFin)
                {
                    horarioValido = true;
                    break;
                }
            }

            if (!horarioValido)
                throw new ExcepcionReglaNegocio("El turno está fuera del horario del médico.");

            // evitar que se solape con otros turnos o consigo mismo
            List<Turno> turnos = _repositorioTurno.ObtenerTurnosPorMedicoEnRango(idMedico, inicio.Date, inicio.Date.AddDays(1));
            bool ocupado = turnos.Exists(t =>
                t.IdTurno != idTurno &&
                (inicio < t.Horario.Fin && fin > t.Horario.Inicio)
            );

            if (ocupado)
                throw new ExcepcionReglaNegocio("El horario seleccionado ya está ocupado por otro turno.");
        }

        private void ValidarTurnosPacienteMismoDia(int idPaciente, int idEspecialidad, DateTime fecha, int idTurno)
        {
            List<Turno> turnosPaciente = _repositorioTurno.ObtenerTurnosPorMedicoEnRango(
                idPaciente, fecha.Date, fecha.Date.AddDays(1)
            );
            bool yaTieneTurno = turnosPaciente.Exists(t => t.IdTurno != idTurno &&
                t.Especialidad.IdEspecialidad == idEspecialidad &&
                t.Estado != EstadoTurno.Cancelado && t.Estado != EstadoTurno.Cerrado
            );
            if (yaTieneTurno)
                throw new ExcepcionReglaNegocio("El paciente ya tiene un turno para este día y especialidad.");
        }

        private void ValidarCambioDeEstado(Turno turnoExistente, char estadoNuevo, DateTime nuevaFechaInicio, DateTime nuevaFechaFin)
        {
            char actual = (char)turnoExistente.Estado;

            if (actual == 'X')
                throw new ExcepcionReglaNegocio("Un turno que no asistió no puede ser modificado.");


            Dictionary<char, char[]> transicionesPermitidas = new Dictionary<char, char[]> {
                { 'N', new [] { 'R', 'C', 'N', 'X' } }, // nuevo solo puede ir a reprogramado, no asistió o cancelado o mantenerse en nuevo
                { 'R', new [] { 'R', 'C', 'X' } }, // un reprogramado puede volver a reprogramarse, cancelarse o no asistió
                { 'X', new char[0] } // no asisitió simplemente no se puede cambiar a nada más
            };

            bool estadoExisteEnDiccionario = transicionesPermitidas.ContainsKey(actual);

            if (!estadoExisteEnDiccionario || Array.IndexOf(transicionesPermitidas[actual], estadoNuevo) == -1)
            {
                string nombreActual = EnumeracionMapperNegocio.ObtenerNombreEstadoTurno(actual);
                string nombreNuevo = EnumeracionMapperNegocio.ObtenerNombreEstadoTurno(estadoNuevo);

                throw new ExcepcionReglaNegocio(string.Format("Un turno en estado {0} no puede cambiar a {1}.", nombreActual, nombreNuevo));
            }


            // validar que se haya cambiado fecha/hora si se marca reprogramado
            if (estadoNuevo == 'R')
            {
                bool mismaFecha = turnoExistente.Horario.Inicio.Date == nuevaFechaInicio.Date;
                bool mismaHora = turnoExistente.Horario.Inicio.TimeOfDay == nuevaFechaInicio.TimeOfDay &&
                                 turnoExistente.Horario.Fin.TimeOfDay == nuevaFechaFin.TimeOfDay;

                if (mismaFecha && mismaHora)
                    throw new ExcepcionReglaNegocio("No se puede marcar como reprogramado sin cambiar la fecha/hora del turno.");
            }

            // validar reprogramado y no asistió según fecha/hora
            if (estadoNuevo == 'X')
            {
                if (actual != 'N' && actual != 'R')
                {
                    throw new ExcepcionReglaNegocio("Solo se pueden marcar como ausentes los turnos nuevos o reprogramados.");
                }

                // se puede marcar como no asistio luego de pasadas 15 minutos despues de la hora de inicio
                DateTime toleranciaInicio = turnoExistente.Horario.Inicio.AddMinutes(15);

                if (DateTime.Now < toleranciaInicio)
                {
                    throw new ExcepcionReglaNegocio("No puede marcar 'No Asistió' en un turno futuro o que recién comienza. Espere al horario del turno.");
                }
            }

            // un turno no se permite cerrar manualmente
            if (estadoNuevo == 'Z')
                throw new ExcepcionReglaNegocio("El turno no puede ser cerrado manualmente. Solo el médico puede cerrarlo al generar la historia clínica.");
        }


        public TurnoEdicionDto ObtenerParaEdicion(int idTurno)
        {
            Turno entidad = _repositorioTurno.ObtenerPorId(idTurno);

            if (entidad == null)
                throw new ExcepcionReglaNegocio("El turno no existe.");

            if (entidad.Estado == EstadoTurno.Cancelado
                || entidad.Estado == EstadoTurno.Cerrado
                || entidad.Estado == EstadoTurno.NoAsistio)
                throw new ExcepcionReglaNegocio($"El turno no es editable, " +
                    $"ya que se encuentra en estado " +
                    $"{EnumeracionMapperNegocio.ObtenerNombreEstadoTurno(EnumeracionMapperNegocio.ObtenerChar(entidad.Estado))}.");

            return TurnoMapper.MapearAEdicionDto(entidad);

        }


        public List<TurnoListadoDto> ListarConFiltros(FiltroTurnoDto filtros, UsuarioSesionDto usuarioSolicitante)
        {
            // si el usuario es médico, ignoramos el filtro.IdMEdico y usamos su propio id
            if (usuarioSolicitante.IdRol == 3)
            {
                if (usuarioSolicitante.IdMedico.HasValue)
                {
                    filtros.IdMedico = usuarioSolicitante.IdMedico.Value;
                }
                else
                {
                    // por seguridad, si el usuario es médico pero no tiene ficha médicoa, no mostramos nada.
                    return new List<TurnoListadoDto>();
                }
            }

            List<Turno> turnos = _repositorioTurno.Listar(
                filtros.FechaInicio,
                filtros.FechaFin,
                filtros.IdMedico,
                filtros.IdPaciente,
                filtros.IdEspecialidad
            );

            return TurnoMapper.MapearListaTurnoListadoDto(turnos);
        }


    }
}


