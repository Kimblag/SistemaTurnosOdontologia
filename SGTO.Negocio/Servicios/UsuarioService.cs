using SGTO.Datos.Infraestructura;
using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Medicos;
using SGTO.Negocio.DTOs.Usuarios;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Mappers;
using SGTO.Negocio.Seguridad;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SGTO.Negocio.Servicios
{
    public class UsuarioService
    {
        private readonly UsuarioRepositorio _repositorioUsuario;
        private readonly MedicoRepositorio _repositorioMedico;
        private readonly ParametroSistemaRepositorio _repositorioParametrosSistema;
        private readonly HorarioSemanalRepositorio _repositorioHorarioSemanal;
        private readonly TurnoRepositorio _repositorioTurno;

        public UsuarioService()
        {
            _repositorioUsuario = new UsuarioRepositorio();
            _repositorioMedico = new MedicoRepositorio();
            _repositorioParametrosSistema = new ParametroSistemaRepositorio();
            _repositorioHorarioSemanal = new HorarioSemanalRepositorio();
            _repositorioTurno = new TurnoRepositorio();
        }

        public List<UsuarioListadoDto> Listar(string estado = null)
        {
            try
            {
                return UsuarioMapper.MapearListaAListadoDto(_repositorioUsuario.Listar(estado));
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void Crear(UsuarioCrearDto nuevoUsuario, MedicoCrearDto nuevoMedico = null)
        {
            if (EsNombreUsuarioReservado(nuevoUsuario.NombreUsuario))
                throw new ExcepcionReglaNegocio($"El nombre de usuario '{nuevoUsuario.NombreUsuario}' no está permitido por seguridad.");

            if (_repositorioUsuario.ExisteNombreUsuario(nuevoUsuario.NombreUsuario))
                throw new ExcepcionReglaNegocio($"El nombre de usuario '{nuevoUsuario.NombreUsuario}' ya está en uso.");

            if (_repositorioUsuario.ExisteEmail(nuevoUsuario.Email))
                throw new ExcepcionReglaNegocio($"Ya existe un usuario con el email '{nuevoUsuario.Email}'.");

            // validar reglad de integridad
            if (nuevoUsuario.IdRol == 3 && nuevoMedico == null)
            {
                throw new ExcepcionReglaNegocio("No se puede crear un usuario con rol 'Médico' sin los datos del perfil profesional.");
            }

            if (nuevoMedico != null)
            {
                // validar edad del médico
                int edadMinima = 21;
                if (nuevoMedico.FechaNacimiento > DateTime.Today.AddYears(-edadMinima))
                    throw new ExcepcionReglaNegocio($"El médico debe ser mayor de {edadMinima} años.");

                // validar matrícula única y dni
                if (_repositorioMedico.ExistePorMatricula(nuevoMedico.Matricula))
                    throw new ExcepcionReglaNegocio($"Ya existe un médico con la matrícula '{nuevoMedico.Matricula}'.");

                if (_repositorioMedico.ExistePorDocumento(nuevoMedico.NumeroDocumento))
                    throw new ExcepcionReglaNegocio($"Ya existe un médico con el DNI '{nuevoMedico.NumeroDocumento}'.");

                // validar horarios
                if (nuevoMedico.HorariosSemanales == null || nuevoMedico.HorariosSemanales.Count == 0)
                    throw new ExcepcionReglaNegocio("No se puede crear un médico sin horarios de atención definidos.");

                // validar solapamientos 
                if (ExisteSolapamientoInterno(nuevoMedico.HorariosSemanales))
                    throw new ExcepcionReglaNegocio("Los horarios definidos presentan solapamientos (se superponen entre sí). Por favor verifique los rangos.");
            }


            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.IniciarTransaccion();

                    Rol rol = new Rol { IdRol = nuevoUsuario.IdRol };

                    Usuario entidadUsuario = UsuarioMapper.MapearAEntidadDesdeCrear(nuevoUsuario, rol);
                    entidadUsuario.PasswordHash = PasswordHasher.Hash(nuevoUsuario.Password);
                    entidadUsuario.FechaAlta = DateTime.Now;
                    entidadUsuario.FechaModificacion = DateTime.Now;

                    int idUsuario = _repositorioUsuario.Crear(entidadUsuario, datos);

                    if (nuevoMedico != null)
                    {
                        Medico entidadMedico = MedicoMapper.MapearDesdeCrearDto(nuevoMedico, idUsuario);
                        entidadMedico.FechaAlta = DateTime.Now;
                        entidadMedico.FechaModificacion = DateTime.Now;

                        int idMedico = _repositorioMedico.Crear(entidadMedico, datos);

                        if (nuevoMedico.IdEspecialidades != null)
                        {
                            foreach (int idEsp in nuevoMedico.IdEspecialidades)
                            {
                                _repositorioMedico.CrearEspecialidadMedico(idMedico, idEsp, datos);
                            }
                        }

                        List<HorarioSemanalMedico> horarios = new List<HorarioSemanalMedico>();

                        foreach (HorarioSemanalDto dto in nuevoMedico.HorariosSemanales)
                        {
                            horarios.Add(new HorarioSemanalMedico
                            {
                                Medico = new Medico { IdMedico = idMedico },
                                DiaSemana = dto.DiaSemana,
                                HoraInicio = dto.HoraInicio,
                                HoraFin = dto.HoraFin,
                                Estado = EstadoEntidad.Activo
                            });
                        }

                        ValidarHorariosDentroDelRangoClinica(horarios);

                        _repositorioHorarioSemanal.Crear(horarios, datos);

                    }

                    datos.ConfirmarTransaccion();
                }
                catch (Exception ex)
                {
                    datos.RollbackTransaccion();
                    Debug.WriteLine("Error al crear usuario/médico: " + ex.Message);
                    throw new Exception("Error crítico al registrar el usuario. Operación cancelada.");
                }
            }
        }


        public void Editar(UsuarioEdicionDto usuarioDto, MedicoEdicionDto medicoDto = null)
        {
            Usuario usuarioActual = _repositorioUsuario.ObtenerPorId(usuarioDto.IdUsuario);
            ValidarReglasDeIntegridad(usuarioDto, usuarioActual);

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.IniciarTransaccion();

                    Rol rol = new Rol { IdRol = usuarioDto.IdRol };

                    Usuario usuarioEditado = UsuarioMapper.MapearAEntidadDesdeEditar(usuarioDto, rol);

                    if (!string.IsNullOrWhiteSpace(usuarioDto.Password))
                        usuarioEditado.PasswordHash = PasswordHasher.Hash(usuarioDto.Password);

                    usuarioEditado.FechaModificacion = DateTime.Now;

                    _repositorioUsuario.Editar(usuarioEditado, datos);

                    if (medicoDto == null)
                    {
                        datos.ConfirmarTransaccion();
                        return;
                    }

                    Medico medicoActual = _repositorioMedico.ObtenerPorUsuarioId(usuarioDto.IdUsuario);
                    if (medicoActual == null)
                        throw new ExcepcionReglaNegocio("El médico asociado no existe.");

                    if (_repositorioMedico.ExistePorMatriculaEnOtro(medicoDto.Matricula, usuarioDto.IdUsuario))
                        throw new ExcepcionReglaNegocio($"Ya existe un médico con la matrícula '{medicoDto.Matricula}'.");

                    if (_repositorioMedico.ExistePorDocumentoEnOtro(medicoDto.NumeroDocumento, usuarioDto.IdUsuario))
                        throw new ExcepcionReglaNegocio($"Ya existe un médico con el DNI '{medicoDto.NumeroDocumento}'.");


                    if (medicoDto.IdEspecialidades == null || medicoDto.IdEspecialidades.Count == 0)
                        throw new ExcepcionReglaNegocio("El médico debe tener al menos una especialidad.");


                    Medico medicoEditado = MedicoMapper.MapearDesdeEdicionDto(medicoDto);
                    medicoEditado.Usuario = new Usuario { IdUsuario = usuarioDto.IdUsuario };
                    medicoEditado.FechaModificacion = DateTime.Now;

                    _repositorioMedico.Editar(medicoEditado, datos);


                    _repositorioMedico.EliminarEspecialidadesDeMedico(medicoActual.IdMedico, datos);

                    foreach (int idEsp in medicoDto.IdEspecialidades)
                        _repositorioMedico.CrearEspecialidadMedico(medicoActual.IdMedico, idEsp, datos);


                    if (medicoDto.HorariosSemanales != null)
                    {
                        List<HorarioSemanalMedico> horarios = new List<HorarioSemanalMedico>();

                        foreach (HorarioSemanalDto dto in medicoDto.HorariosSemanales)
                        {
                            horarios.Add(new HorarioSemanalMedico
                            {
                                Medico = new Medico { IdMedico = medicoActual.IdMedico },
                                DiaSemana = dto.DiaSemana,
                                HoraInicio = dto.HoraInicio,
                                HoraFin = dto.HoraFin,
                                Estado = EstadoEntidad.Activo
                            });
                        }

                        ValidarHorariosDentroDelRangoClinica(horarios);
                        ValidarConflictosDeHorario(medicoActual.IdMedico, medicoDto.HorariosSemanales);

                        _repositorioHorarioSemanal.EliminarPorMedico(medicoActual.IdMedico, datos);
                        if (horarios.Count > 0)
                        {
                            _repositorioHorarioSemanal.Crear(horarios, datos);
                        }
                    }

                    datos.ConfirmarTransaccion();
                }
                catch (ExcepcionReglaNegocio)
                {
                    datos.RollbackTransaccion();
                    throw;
                }
                catch (Exception ex)
                {
                    datos.RollbackTransaccion();
                    Debug.WriteLine("Error al editar usuario/médico: " + ex.Message);
                    throw new Exception("Error al editar el usuario. La operación fue revertida.");
                }
            }
        }

        private void ValidarReglasDeIntegridad(UsuarioEdicionDto usuarioDto, Usuario usuarioActual)
        {
            if (usuarioActual == null)
                throw new ExcepcionReglaNegocio("El usuario no existe.");

            // como existe un usuario root qu eNO puede modificarse hay que validar por si alguien logra pasar los controles de seguridad
            if (usuarioActual.NombreUsuario.ToLower() == "root")
            {
                if (usuarioDto.Estado.StartsWith("I", StringComparison.OrdinalIgnoreCase))
                {
                    throw new ExcepcionReglaNegocio("El usuario 'root' es el Super Administrador y no puede ser desactivado.");
                }

                if (usuarioDto.IdRol != usuarioActual.Rol.IdRol)
                {
                    throw new ExcepcionReglaNegocio("No se puede cambiar el rol del usuario 'root'.");
                }
            }

            // validaciones de integridad
            bool seEstaDandoDeBaja = usuarioDto.Estado.StartsWith("I", StringComparison.OrdinalIgnoreCase)
                             && EnumeracionMapperNegocio.ObtenerChar(usuarioActual.Estado) == 'A';

            if (seEstaDandoDeBaja)
            {
                // validar si es un admin y si lo es, entonces debemos buscar en la bd a ver si hay otros. El último admin no puede darse de baja.
                if (usuarioActual.Rol.Nombre.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                {
                    int otrosAdmins = _repositorioUsuario.ContarOtrosAdministradoresActivos(usuarioDto.IdUsuario);
                    if (otrosAdmins == 0)
                    {
                        throw new ExcepcionReglaNegocio("No se puede desactivar este usuario porque es el único Administrador activo del sistema.");
                    }
                }

                // validar que un médico con turnos futuros NO pueda darse de baja.
                if (usuarioActual.Rol.Nombre.Equals("Médico", StringComparison.OrdinalIgnoreCase))
                {
                    Medico medicoAsociado = _repositorioMedico.ObtenerPorUsuarioId(usuarioDto.IdUsuario);
                    if (medicoAsociado != null)
                    {
                        bool tieneTurnos = _repositorioTurno.ExisteTurnoFuturoActivoPorMedico(medicoAsociado.IdMedico);
                        if (tieneTurnos)
                        {
                            throw new ExcepcionReglaNegocio("No se puede dar de baja al médico porque tiene turnos agendados pendientes o futuros. Cancele o reprograme los turnos antes de desactivar la cuenta.");
                        }
                    }
                }
            }

            if (_repositorioUsuario.ExisteNombreUsuarioEnOtroUsuario(usuarioDto.NombreUsuario, usuarioDto.IdUsuario))
                throw new ExcepcionReglaNegocio($"El nombre de usuario '{usuarioDto.NombreUsuario}' ya está en uso.");

            if (_repositorioUsuario.ExisteEmailEnOtroUsuario(usuarioDto.Email, usuarioDto.IdUsuario))
                throw new ExcepcionReglaNegocio($"El email '{usuarioDto.Email}' ya está registrado.");
        }

        public UsuarioDetalleDto ObtenerDetalle(int idUsuario)
        {
            Usuario usuario = _repositorioUsuario.ObtenerPorId(idUsuario);
            if (usuario == null)
                throw new ExcepcionReglaNegocio("El usuario no existe.");

            Medico medico = null;
            if (usuario.Rol != null && usuario.Rol.Nombre.Equals("Médico", StringComparison.OrdinalIgnoreCase))
                medico = _repositorioMedico.ObtenerPorUsuarioId(idUsuario);

            return UsuarioMapper.MapearADetalleDto(usuario, medico);
        }

        public UsuarioDetalleDto ObtenerDetalleMedico(int idMedico)
        {
            Medico medico = _repositorioMedico.ObtenerPorId(idMedico);
            if (medico == null)
                throw new ExcepcionReglaNegocio("El médico no existe.");

            Usuario usuario = _repositorioUsuario.ObtenerPorId(medico.Usuario.IdUsuario);
            if (usuario == null)
                throw new ExcepcionReglaNegocio("No se encontró el usuario asociado al médico.");

            return UsuarioMapper.MapearADetalleDto(usuario, medico);
        }

        private void ValidarHorariosDentroDelRangoClinica(List<HorarioSemanalMedico> horarios)
        {
            ParametroSistemaRepositorio repositorioParametro = new ParametroSistemaRepositorio();
            var (horaApertura, horaCierre) = repositorioParametro.ObtenerHorarioClinica();

            foreach (HorarioSemanalMedico horario in horarios)
            {
                if (horario.HoraInicio < horaApertura || horario.HoraFin > horaCierre)
                {
                    throw new ExcepcionReglaNegocio(
                        $"El horario del día {horario.DiaSemana} ({horario.HoraInicio:hh\\:mm}-{horario.HoraFin:hh\\:mm}) " +
                        $"está fuera del horario permitido de la clínica ({horaApertura:hh\\:mm}-{horaCierre:hh\\:mm})."
                    );
                }
            }
        }

        public (TimeSpan HoraApertura, TimeSpan HoraCierre) ObtenerHorarioClinica()
        {
            return _repositorioParametrosSistema.ObtenerHorarioClinica();
        }


        private void ValidarConflictosDeHorario(int idMedico, List<HorarioSemanalDto> nuevosHorarios)
        {
            // listamos los turnos futuros del médico que esten pendientes por atender (Nuevos, reprogramdos)
            List<Turno> turnosFuturos = _repositorioTurno.ObtenerFuturosActivosPorMedico(idMedico);

            if (turnosFuturos.Count == 0) return; // no hay turnos, no hay conflicto

            // verificamos que todos los turnos que están agendados estén dentro del rango de los nuevos días de atención
            foreach (Turno turno in turnosFuturos)
            {
                // normalizamos los días para que el lunes sea = 1
                int diaSemanaTurno = (int)turno.Horario.Inicio.DayOfWeek;
                if (diaSemanaTurno == 0) diaSemanaTurno = 7;

                TimeSpan horaInicioTurno = turno.Horario.Inicio.TimeOfDay;
                TimeSpan horaFinTurno = turno.Horario.Fin.TimeOfDay;

                bool turnoQuedaCubierto = false;

                // buscar si en el nuevo turno cubre al turno ya agendado
                foreach (HorarioSemanalDto rango in nuevosHorarios)
                {
                    //  si el día coincide
                    if (rango.DiaSemana == diaSemanaTurno)
                    {
                        // el turno está dentro dl rango
                        if (horaInicioTurno >= rango.HoraInicio && horaFinTurno <= rango.HoraFin)
                        {
                            turnoQuedaCubierto = true;
                            break;
                        }
                    }
                }

                // si los horarios quedan fuera, entonces lanzamos el error
                if (!turnoQuedaCubierto)
                {
                    string nombreDia = ObtenerNombreDiaNegocio(diaSemanaTurno);

                    throw new ExcepcionReglaNegocio(
                        string.Format("No se puede modificar el horario porque existe un conflicto con un turno agendado. " +
                                      $"El día {nombreDia} {turno.Horario.Inicio.ToString("dd/MM/yyyy")} a las {turno.Horario.Inicio.ToString("HH:mm")}hs quedaría fuera del nuevo horario de atención. " +
                                      "Debe reprogramar o cancelar ese turno antes de cambiar la disponibilidad.")
                    );
                }
            }
        }

        private string ObtenerNombreDiaNegocio(int dia)
        {
            switch (dia)
            {
                case 1: return "Lunes";
                case 2: return "Martes";
                case 3: return "Miércoles";
                case 4: return "Jueves";
                case 5: return "Viernes";
                case 6: return "Sábado";
                case 7: return "Domingo";
                default: return "Día desconocido";
            }
        }


        private bool ExisteSolapamientoInterno(List<HorarioSemanalDto> horarios)
        {
            // método para evitar que nos manden en un mismo día algo como: Lunes 9 - 13hs y segundo rango 11 - 14hs

            // ordenar por dia y hora de inicio
            for (int i = 0; i < horarios.Count - 1; i++)
            {
                for (int j = 0; j < horarios.Count - 1 - i; j++)
                {
                    bool debeIntercambiar = false;

                    // comparamos por día primero
                    if (horarios[j].DiaSemana > horarios[j + 1].DiaSemana)
                    {
                        debeIntercambiar = true;
                    }
                    // si es el mismo día, comparamos por hora inicio
                    else if (horarios[j].DiaSemana == horarios[j + 1].DiaSemana)
                    {
                        if (horarios[j].HoraInicio > horarios[j + 1].HoraInicio)
                        {
                            debeIntercambiar = true;
                        }
                    }

                    if (debeIntercambiar)
                    {
                        var temp = horarios[j];
                        horarios[j] = horarios[j + 1];
                        horarios[j + 1] = temp;
                    }
                }
            }

            // verificar solapamientos 
            for (int i = 0; i < horarios.Count - 1; i++)
            {
                var actual = horarios[i];
                var siguiente = horarios[i + 1];

                // si son del mismo día
                if (actual.DiaSemana == siguiente.DiaSemana)
                {
                    // Si el fin del actual pisa el inicio del siguiente

                    if (actual.HoraFin > siguiente.HoraInicio)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool EsNombreUsuarioReservado(string nombreUsuario)
        {
            string[] reservados = { "admin", "administrator", "administrador", "root", "system", "sistema", "soporte" };
            string normalizado = nombreUsuario.Trim().ToLower();

            foreach (string palabra in reservados)
            {
                if (normalizado == palabra) return true;
            }
            return false;
        }

        public void CambiarPassword(int idUsuario, string passwordActual, string passwordNueva)
        {
            Usuario usuario = _repositorioUsuario.ObtenerPorId(idUsuario);
            if (usuario == null)
                throw new ExcepcionReglaNegocio("El usuario no existe.");

            if (!PasswordHasher.Verify(passwordActual, usuario.PasswordHash))
            {
                throw new ExcepcionReglaNegocio("La contraseña actual ingresada es incorrecta.");
            }

            if (string.IsNullOrWhiteSpace(passwordNueva) || passwordNueva.Length < 4)
            {
                throw new ExcepcionReglaNegocio("La nueva contraseña debe tener al menos 4 caracteres.");
            }

            string nuevoHash = PasswordHasher.Hash(passwordNueva);
            _repositorioUsuario.ActualizarPassword(idUsuario, nuevoHash);
        }

    }
}
