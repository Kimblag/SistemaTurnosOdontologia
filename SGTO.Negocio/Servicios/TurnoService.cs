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


        public int Crear(TurnoCreacionDto dto, string rutaPlantillaEmail)
        {
            // validar que el turno sea válido: no solapa, no agenda el mismo día par ala misma especialidad o médico, horarios
            //correctos y no en el pasado, cambios de estado lógicos: Uno cerrado o cancelaod no puede cambiar de estado, su ciclo
            //de vida muere allí.
            ValidarReglasDeAgendamiento(
                dto.IdPaciente,
                dto.IdMedico,
                dto.IdEspecialidad,
                dto.IdCobertura,
                dto.IdPlan,
                dto.FechaInicio,
                dto.FechaFin,
                0
            );

            Turno turno = TurnoMapper.MapearACreacion(dto);
            int idTurno = _repositorioTurno.Crear(turno);

            // enviar la notificación al paciente
            var paciente = _repositorioPaciente.ObtenerPorId(dto.IdPaciente);
            var medico = _repositorioMedico.ObtenerPorId(dto.IdMedico);
            var especialidad = _repositorioEspecialidad.ObtenerPorId(dto.IdEspecialidad);

            EnviarEmailConfirmacion(paciente, medico, especialidad, dto.FechaInicio, dto.Observaciones, rutaPlantillaEmail);

            return idTurno;
        }


        public void Editar(TurnoEdicionDto dto, string rutaPlantillaReprogramacion, string rutaPlantillaCancelacion, int idUsuarioModificacion = 0)
        {
            // consultar los datos del turno actual
            Turno turnoExistente = _repositorioTurno.ObtenerPorId(dto.IdTurno);
            if (turnoExistente == null)
                throw new ExcepcionReglaNegocio("El turno no existe.");

            EstadoTurno estadoOriginal = turnoExistente.Estado;
            DateTime fechaOriginal = turnoExistente.Horario.Inicio;

            // validar el tipo de edición:
            // es una cancelación o notificar que no asistió:
            bool esCancelacionOAusencia = (dto.Estado == 'X' || dto.Estado == 'C');
            // es una reprogramación
            bool esReprogramacion = !esCancelacionOAusencia;

            // si es una cancelación o ausencia, solo se debe guardar en la base de datos el cambio de estado:
            // JAMAAS cambiar los datos, se mantienen IGUAL porque debemos mantener trazabilidad, no importa
            // si al recepcionista se le dio por cambiar los datos originales, se ignoran.
            if (esCancelacionOAusencia)
            {
                ValidarCambioEstado(turnoExistente, dto.Estado, dto.FechaInicio, dto.FechaFin);

                // aqui volvemos a copiar los datos originales al dto, es importante esto.
                dto.IdPaciente = turnoExistente.Paciente.IdPaciente;
                dto.IdMedico = turnoExistente.Medico.IdMedico;
                dto.IdEspecialidad = turnoExistente.Especialidad.IdEspecialidad;
                dto.FechaInicio = turnoExistente.Horario.Inicio;
                dto.FechaFin = turnoExistente.Horario.Fin;

                TurnoMapper.MapearEdicion(turnoExistente, dto, false);
            }
            else if (esReprogramacion)
            {
                // si es reprogramación vamos a validar que efectivamente el dto traiga una nueva fecha y/u hora
                // ya que puede ser que la reprogramación sea solo horaria o completa (cambio de día)
                bool cambioHorario = turnoExistente.Horario.Inicio != dto.FechaInicio || turnoExistente.Horario.Fin != dto.FechaFin;

                // validamos las reglas de agendamiento que establecimos porque no pude solapar ni nada, aquí es igual que el crear
                ValidarReglasDeAgendamiento(
                    dto.IdPaciente,
                    dto.IdMedico,
                    dto.IdEspecialidad,
                    dto.IdCobertura,
                    dto.IdPlan,
                    dto.FechaInicio,
                    dto.FechaFin,
                    dto.IdTurno // mandamos el id para evitar que se compare a sí mismo.
                );

                // si el estado del turno existente era 'Nuevo' y efectivamente se cambió la ´hora o fecha,
                // entonces cambiamos estado a Repogramdo
                if (cambioHorario && turnoExistente.Estado == EstadoTurno.Nuevo)
                    dto.Estado = 'R';

                ValidarCambioEstado(turnoExistente, dto.Estado, dto.FechaInicio, dto.FechaFin);
                TurnoMapper.MapearEdicion(turnoExistente, dto, true);
            }
            _repositorioTurno.Actualizar(turnoExistente, idUsuarioModificacion);
            GestionarNotificacionesEdicion(dto, estadoOriginal, fechaOriginal, rutaPlantillaReprogramacion, rutaPlantillaCancelacion);
        }


        /* estos métodos son lso que nos ayudarán a validar las reglas de negocio*/
        private void ValidarReglasDeAgendamiento(int idPaciente, int idMedico,
            int idEspecialidad, int idCobertura, int idPlan, DateTime inicio,
            DateTime fin, int idTurnoExcluir)
        {
            // validamos que las entidades existan y que no haya inconsistencias en cuanto:
            // - paciente que no existe o está inactivo
            // - paciente cuya cobertura y plan no coincida con la registrada en el sistema
            // - especialidad que no coincide con el médico o que no existe o que esté inactiva
            // - médico que no existe o esté inactivo
            // - Cobertura o plan que no exista o que esté inactiva
            // - plan que no pertenece a la cobertura indicada

            ValidarPaciente(idPaciente);
            ValidarEspecialidad(idEspecialidad);
            ValidarMedico(idMedico, idEspecialidad);
            ValidarCoberturaYPlan(idCobertura, idPlan, idPaciente);

            // validar consistencia en las horas, no podemos tener un turno que inicie después de la hora de fin.
            ValidarFechaYHora(inicio, fin);

            // verificar la disponibilidad del médico
            ValidarDisponibilidadMedico(idMedico, inicio, fin, idTurnoExcluir);

            // verificar que el paciente NO: tenga un turno solapado, un turno con
            // la misma especialidad el mismo día, un turno con el mismo médico el mismo día
            // un turno que termine cuando inicia otro turno (DEBERIA por lo menos tener horas de diferencia y ser de distintas especialidades)
            ValidarTurnosPacienteMismoDia(idPaciente, idMedico, idEspecialidad, inicio, fin, idTurnoExcluir);
        }

        private void ValidarPaciente(int idPaciente)
        {
            Paciente paciente = _repositorioPaciente.ObtenerPorId(idPaciente);
            if (paciente == null) throw new ExcepcionReglaNegocio("El paciente no existe.");
            if (paciente.Estado.ToString().ToLower()[0] != 'a') throw new ExcepcionReglaNegocio("El paciente está inactivo.");
        }

        private void ValidarEspecialidad(int idEspecialidad)
        {
            Especialidad especialidad = _repositorioEspecialidad.ObtenerPorId(idEspecialidad);
            if (especialidad == null) throw new ExcepcionReglaNegocio("La especialidad no existe.");
            if (especialidad.Estado.ToString().ToLower()[0] != 'a') throw new ExcepcionReglaNegocio("La especialidad está inactiva.");
        }

        private void ValidarMedico(int idMedico, int idEspecialidad)
        {
            Medico medico = _repositorioMedico.ObtenerPorId(idMedico);
            if (medico == null) throw new ExcepcionReglaNegocio("El médico no existe.");
            if (medico.Estado == EstadoEntidad.Inactivo) throw new ExcepcionReglaNegocio("El médico se encuentra inactivo.");

            if (medico.Especialidades != null && medico.Especialidades.Count > 0)
            {
                if (!medico.Especialidades.Exists(e => e.IdEspecialidad == idEspecialidad))
                    throw new ExcepcionReglaNegocio("El médico no pertenece a la especialidad seleccionada.");
            }
        }

        private void ValidarCoberturaYPlan(int idCobertura, int idPlan, int idPaciente)
        {
            Cobertura cobertura = _repositorioCobertura.ObtenerPorId(idCobertura);
            if (cobertura == null)
                throw new ExcepcionReglaNegocio("La cobertura no existe.");
            if (cobertura.Estado == EstadoEntidad.Inactivo)
                throw new ExcepcionReglaNegocio("La cobertura seleccionada está inactiva.");

            Paciente paciente = _repositorioPaciente.ObtenerPorId(idPaciente);
            if (idCobertura != paciente.Cobertura.IdCobertura)
                throw new ExcepcionReglaNegocio("La cobertura no coincide con la del paciente.");

            if (idPlan != 0)
            {
                Plan plan = _repositorioPlan.ObtenerPorId(idPlan);
                if (plan == null)
                    throw new ExcepcionReglaNegocio("El plan no existe.");
                if (plan.Estado == EstadoEntidad.Inactivo)
                    throw new ExcepcionReglaNegocio("El plan seleccionado está inactivo.");
                if (plan.Cobertura.IdCobertura != idCobertura)
                    throw new ExcepcionReglaNegocio("El plan no coincide con la cobertura seleccionada.");
            }
        }

        private void ValidarFechaYHora(DateTime inicio, DateTime fin)
        {
            if (fin <= inicio)
                throw new ExcepcionReglaNegocio("La fecha de fin debe ser posterior al inicio.");
            if (inicio.Date < DateTime.Today)
                throw new ExcepcionReglaNegocio("No se pueden agendar turnos en el pasado.");
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

        private void ValidarTurnosPacienteMismoDia(int idPaciente, int idMedico, int idEspecialidad, DateTime fechaInicio, DateTime fechaFin, int idTurnoExcluir)
        {
            // consultamos TODOOS los turnos dle paciente que estén dentro del rango en que inicia el turno que 
            //tratamos de editar - agendar y le sumamos 1 día para completar el rango.
            List<Turno> turnosPaciente = _repositorioTurno.ObtenerTurnosPorPacienteEnRango(
                idPaciente,
                fechaInicio.Date,
                fechaInicio.Date.AddDays(1)
            );

            // recorremos cada turno para poder comparar correctamente
            foreach (Turno turno in turnosPaciente)
            {
                if (turno.IdTurno == idTurnoExcluir) continue;

                //verificar que NO solape con otro turno
                if (fechaInicio < turno.Horario.Fin && fechaFin > turno.Horario.Inicio)
                    throw new ExcepcionReglaNegocio($"El paciente ya tiene un turno en el horario {turno.Horario.Inicio:HH:mm} - {turno.Horario.Fin:HH:mm}.");

                // verificar que NO intente agendar un turno o reprogramarlo para la misma especialidad el mismo día.
                if (turno.Especialidad.IdEspecialidad == idEspecialidad)
                    throw new ExcepcionReglaNegocio($"El paciente ya tiene un turno de {turno.Especialidad.Nombre} este día.");

                // verificar que NO sea un turno para el mismo médico el mismo día.
                if (turno.Medico.IdMedico == idMedico)
                    throw new ExcepcionReglaNegocio($"El paciente ya tiene un turno con este médico en el día seleccionado.");
            }

            // validar que si tiene un turno con distintas especialidades en el mismo día, al menos tengan 1 hora de diferencia entre sí.
            // por seguridad se hace esto ya que puede alargarse la atención de un turno
            foreach (Turno turno in turnosPaciente)
            {
                if (turno.IdTurno == idTurnoExcluir) continue;

                double diferenciaEntrada = Math.Abs((turno.Horario.Fin - fechaInicio).TotalMinutes);
                double diferenciaSalida = Math.Abs((turno.Horario.Inicio - fechaFin).TotalMinutes);

                if (diferenciaEntrada < 60 || diferenciaSalida < 60)
                    throw new ExcepcionReglaNegocio("Debe haber al menos 1 hora de diferencia entre turnos del paciente.");
            }
        }

        private void ValidarCambioEstado(Turno turnoExistente, char estadoNuevo, DateTime nuevaFechaInicio, DateTime nuevaFechaFin)
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

                // se puede marcar como no asistio luego de pasadas 20 minutos despues de la hora de inicio
                DateTime toleranciaInicio = turnoExistente.Horario.Inicio.AddMinutes(20);

                if (DateTime.Now < toleranciaInicio)
                {
                    throw new ExcepcionReglaNegocio("No puede marcar 'No Asistió' en un turno futuro o que recién comienza. Espere al horario del turno.");
                }
            }

            // un turno no se permite cerrar manualmente
            if (estadoNuevo == 'Z')
                throw new ExcepcionReglaNegocio("El turno no puede ser cerrado manualmente. Solo el médico puede cerrarlo al generar la historia clínica.");
        }

        private void GestionarNotificacionesEdicion(TurnoEdicionDto dto, EstadoTurno estadoOriginal, DateTime fechaOriginal, string plantillaReprogramacion, string plantillaCancelacion)
        {
            var paciente = _repositorioPaciente.ObtenerPorId(dto.IdPaciente);
            var medico = _repositorioMedico.ObtenerPorId(dto.IdMedico);
            var especialidad = _repositorioEspecialidad.ObtenerPorId(dto.IdEspecialidad);

            if (dto.Estado == 'C' && estadoOriginal != EstadoTurno.Cancelado)
            {
                EnviarEmailNotificacion(paciente, medico, especialidad, dto.FechaInicio,
                    "Turno Cancelado", "Su turno ha sido cancelado.", plantillaCancelacion);
            }
            else if (dto.Estado == 'R')
            {
                if (estadoOriginal != EstadoTurno.Reprogramado || fechaOriginal != dto.FechaInicio)
                {
                    EnviarEmailNotificacion(paciente, medico, especialidad, dto.FechaInicio,
                        "Turno Reprogramado", "Su turno ha sido reprogramado.", plantillaReprogramacion);
                }
            }
        }

    }
}


