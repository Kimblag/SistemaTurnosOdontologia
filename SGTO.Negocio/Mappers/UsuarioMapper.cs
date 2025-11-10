using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using SGTO.Negocio.DTOs;
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
            List<UsuarioListadoDto> lista = new List<UsuarioListadoDto>();
            foreach (var usuario in usuarios)
                lista.Add(MapearAListadoDto(usuario));

            return lista;
        }

        public static UsuarioDetalleDto MapearADetalleDto(Usuario usuario)
        {
            return new UsuarioDetalleDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email.Valor,
                NombreUsuario = usuario.NombreUsuario,
                Rol = usuario.Rol.Nombre,
                Estado = usuario.Estado == EstadoEntidad.Activo ? "Activo" : "Inactivo",
                FechaAlta = usuario.FechaAlta,
                FechaModificacion = usuario.FechaModificacion
            };
        }

        public static Usuario MapearAEntidadDesdeCrear(UsuarioCrearDto dto, Rol rol)
        {
            var email = new Email(dto.Email);

            return new Usuario(
                nombre: dto.Nombre,
                apellido: dto.Apellido,
                email: email,
                nombreUsuario: dto.NombreUsuario,
                passwordHash: dto.Password, //OJO!! SIEMPRE HASHEADA la pass!!! 
                rol: rol
            );
        }

        public static Usuario MapearAEntidadDesdeEditar(UsuarioEdicionDto dto, Rol rol)
        {
            var email = new Email(dto.Email);

            EstadoEntidad estado = dto.Estado == "Activo"
                ? EstadoEntidad.Activo
                : EstadoEntidad.Inactivo;

            Usuario usuario = new Usuario
            {
                IdUsuario = dto.IdUsuario,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = email,
                NombreUsuario = dto.NombreUsuario,
                PasswordHash = dto.Password, // si viene vacío se mantiene el actual
                Rol = rol,
                Estado = estado
            };

            return usuario;
        }

    }
}
