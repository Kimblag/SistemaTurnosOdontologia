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

        public UsuarioService()
        {
            _repositorioUsuario = new UsuarioRepositorio();
            _repositorioMedico = new MedicoRepositorio();
            _repositorioParametrosSistema = new ParametroSistemaRepositorio();
            _repositorioHorarioSemanal = new HorarioSemanalRepositorio();
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
            if (_repositorioUsuario.ExisteNombreUsuario(nuevoUsuario.NombreUsuario))
                throw new ExcepcionReglaNegocio($"El nombre de usuario '{nuevoUsuario.NombreUsuario}' ya está en uso.");

            if (_repositorioUsuario.ExisteEmail(nuevoUsuario.Email))
                throw new ExcepcionReglaNegocio($"Ya existe un usuario con el email '{nuevoUsuario.Email}'.");

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.IniciarTransaccion();

                    var rol = new Rol { IdRol = nuevoUsuario.IdRol };

                    Usuario entidadUsuario = UsuarioMapper.MapearAEntidadDesdeCrear(nuevoUsuario, rol);
                    entidadUsuario.PasswordHash = PasswordHasher.Hash(nuevoUsuario.Password);
                    entidadUsuario.FechaAlta = DateTime.Now;
                    entidadUsuario.FechaModificacion = DateTime.Now;

                    int idUsuario = _repositorioUsuario.Crear(entidadUsuario, datos);

                    if (nuevoMedico != null)
                    {
                        if (_repositorioMedico.ExistePorMatricula(nuevoMedico.Matricula))
                            throw new ExcepcionReglaNegocio($"Ya existe un médico con la matrícula '{nuevoMedico.Matricula}'.");

                        if (_repositorioMedico.ExistePorDocumento(nuevoMedico.NumeroDocumento))
                            throw new ExcepcionReglaNegocio($"Ya existe un médico con el DNI '{nuevoMedico.NumeroDocumento}'.");

                        if (nuevoMedico.HorariosSemanales == null || nuevoMedico.HorariosSemanales.Count == 0)
                            throw new ExcepcionReglaNegocio("No se puede crear un médico sin horarios de atención definidos.");


                        Medico entidadMedico = MedicoMapper.MapearDesdeCrearDto(nuevoMedico, idUsuario);
                        int idMedico = _repositorioMedico.Crear(entidadMedico, datos);

                   
                        var horarios = new List<HorarioSemanalMedico>();

                        foreach (var dto in nuevoMedico.HorariosSemanales)
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
                        _repositorioHorarioSemanal.GenerarAgendaParaMedico(idMedico, datos);
                        
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
                    Debug.WriteLine("Error al crear usuario/médico: " + ex.Message);
                    throw new Exception("Error al registrar el usuario. La operación fue revertida.");
                }
            }
        }


        public void Editar(UsuarioEdicionDto usuarioDto, MedicoEdicionDto medicoDto = null)
        {
            var usuarioActual = _repositorioUsuario.ObtenerPorId(usuarioDto.IdUsuario);
            if (usuarioActual == null)
                throw new ExcepcionReglaNegocio("El usuario no existe.");

            if (_repositorioUsuario.ExisteNombreUsuarioEnOtroUsuario(usuarioDto.NombreUsuario, usuarioDto.IdUsuario))
                throw new ExcepcionReglaNegocio($"El nombre de usuario '{usuarioDto.NombreUsuario}' ya está en uso.");

            if (_repositorioUsuario.ExisteEmailEnOtroUsuario(usuarioDto.Email, usuarioDto.IdUsuario))
                throw new ExcepcionReglaNegocio($"El email '{usuarioDto.Email}' ya está registrado.");

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.IniciarTransaccion();

                    var rol = new Rol { IdRol = usuarioDto.IdRol };

                    Usuario usuarioEditado = UsuarioMapper.MapearAEntidadDesdeEditar(usuarioDto, rol);
                    if (!string.IsNullOrWhiteSpace(usuarioDto.Password))
                        usuarioEditado.PasswordHash = PasswordHasher.Hash(usuarioDto.Password);
                    usuarioEditado.FechaModificacion = DateTime.Now;

                    _repositorioUsuario.Editar(usuarioEditado, datos);

                    if (medicoDto != null)
                    {
                        var medicoActual = _repositorioMedico.ObtenerPorUsuarioId(usuarioDto.IdUsuario);
                        if (medicoActual == null)
                            throw new ExcepcionReglaNegocio("El médico asociado no existe.");

                        if (_repositorioMedico.ExistePorMatriculaEnOtro(medicoDto.Matricula, usuarioDto.IdUsuario))
                            throw new ExcepcionReglaNegocio($"Ya existe un médico con la matrícula '{medicoDto.Matricula}'.");

                        if (_repositorioMedico.ExistePorDocumentoEnOtro(medicoDto.NumeroDocumento, usuarioDto.IdUsuario))
                            throw new ExcepcionReglaNegocio($"Ya existe un médico con el DNI '{medicoDto.NumeroDocumento}'.");

                        Medico medicoEditado = MedicoMapper.MapearDesdeEdicionDto(medicoDto);
                        _repositorioMedico.Editar(medicoEditado, datos);

                        if (medicoDto.HorariosSemanales != null && medicoDto.HorariosSemanales.Count > 0)
                        {
                            var horarios = new List<HorarioSemanalMedico>();

                            foreach (var dto in medicoDto.HorariosSemanales)
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

                            _repositorioHorarioSemanal.EliminarPorMedico(medicoActual.IdMedico, datos);
                            _repositorioHorarioSemanal.Crear(horarios, datos);
                            _repositorioHorarioSemanal.GenerarAgendaParaMedico(medicoActual.IdMedico, datos);
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
            var repositorioParametro = new ParametroSistemaRepositorio();
            var (horaApertura, horaCierre) = repositorioParametro.ObtenerHorarioClinica();

            foreach (var horario in horarios)
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



    }
}
