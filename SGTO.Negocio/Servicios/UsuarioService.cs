using SGTO.Datos.Infraestructura;
using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
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
        private readonly RolRepositorio _repositorioRol;
        private readonly MedicoRepositorio _repositorioMedico;

        public UsuarioService()
        {
            _repositorioUsuario = new UsuarioRepositorio();
            _repositorioRol = new RolRepositorio();
            _repositorioMedico = new MedicoRepositorio();
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

                        Medico entidadMedico = MedicoMapper.MapearDesdeCrearDto(nuevoMedico, idUsuario);
                        _repositorioMedico.Crear(entidadMedico, datos);
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



    }
}
