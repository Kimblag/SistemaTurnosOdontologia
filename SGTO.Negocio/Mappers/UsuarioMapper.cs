using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Medicos;
using SGTO.Negocio.DTOs.Usuarios;
using System.Collections.Generic;

namespace SGTO.Negocio.Mappers
{
    public static class UsuarioMapper
    {
        public static UsuarioListadoDto MapearAListadoDto(Usuario usuario)
        {
            return new UsuarioListadoDto
            {
                IdUsuario = usuario.IdUsuario,
                NombreCompleto = $"{usuario.Apellido}, {usuario.Nombre}",
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email.Valor,
                IdRol = usuario.Rol.IdRol,
                NombreRol = usuario.Rol.Nombre,
                Estado = usuario.Estado == EstadoEntidad.Activo ? "Activo" : "Inactivo"
            };
        }

        public static List<UsuarioListadoDto> MapearListaAListadoDto(List<Usuario> usuarios)
        {
            var lista = new List<UsuarioListadoDto>();
            foreach (var usuario in usuarios)
                lista.Add(MapearAListadoDto(usuario));
            return lista;
        }

        public static UsuarioDetalleDto MapearADetalleDto(Usuario usuario, Medico medico = null)
        {
            UsuarioDetalleDto dto = new UsuarioDetalleDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email.Valor,
                NombreUsuario = usuario.NombreUsuario,
                IdRol = usuario.Rol.IdRol,
                Rol = usuario.Rol.Nombre,
                Estado = usuario.Estado == EstadoEntidad.Activo ? "Activo" : "Inactivo",
                FechaAlta = usuario.FechaAlta,
                FechaModificacion = usuario.FechaModificacion,
                Medico = null
            };

            if (medico != null)
            {
                dto.Medico = new MedicoDetalleDto
                {
                    IdMedico = medico.IdMedico,
                    NumeroDocumento = medico.Dni.Numero,
                    FechaNacimiento = medico.FechaNacimiento,
                    Genero = EnumeracionMapperNegocio.ObtenerChar(medico.Genero).ToString(),
                    Telefono = medico.Telefono.Numero,
                    Email = medico.Email.Valor,
                    Matricula = medico.Matricula,
                    IdEspecialidad = medico.Especialidades != null && medico.Especialidades.Count > 0
                        ? medico.Especialidades[0].IdEspecialidad
                        : 0,
                    Especialidad = medico.Especialidades != null && medico.Especialidades.Count > 0
                        ? medico.Especialidades[0].Nombre
                        : null,
                    Estado = medico.Estado.ToString()
                };
            }

            return dto;
        }

        public static UsuarioEdicionDto MapearAEdicionDto(Usuario usuario)
        {
            return new UsuarioEdicionDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email.Valor,
                NombreUsuario = usuario.NombreUsuario,
                IdRol = usuario.Rol.IdRol,
                Estado = usuario.Estado.ToString()
            };
        }


        public static Usuario MapearAEntidadDesdeCrear(UsuarioCrearDto dto, Rol rol)
        {
            var email = new Email(dto.Email);

            return new Usuario
            {
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                Email = email,
                NombreUsuario = dto.NombreUsuario.Trim(),
                PasswordHash = string.Empty,
                Rol = rol,
                Estado = EnumeracionMapperNegocio.MapearEstadoEntidad(dto.Estado)
            };
        }

        public static Usuario MapearAEntidadDesdeEditar(UsuarioEdicionDto dto, Rol rol)
        {
            var email = new Email(dto.Email);
            var estado = dto.Estado == "Activo" ? EstadoEntidad.Activo : EstadoEntidad.Inactivo;

            return new Usuario
            {
                IdUsuario = dto.IdUsuario,
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                Email = email,
                NombreUsuario = dto.NombreUsuario.Trim(),
                PasswordHash = dto.Password,
                Rol = rol,
                Estado = estado
            };
        }



    }
}
