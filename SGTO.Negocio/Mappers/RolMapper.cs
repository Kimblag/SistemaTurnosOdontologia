using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs.Roles;
using System.Collections.Generic;

namespace SGTO.Negocio.Mappers
{
    public static class RolMapper
    {
        public static RolListadoDto MapearAListadoDto(Rol rol)
        {
            RolListadoDto dto = new RolListadoDto();
            dto.IdRol = rol.IdRol;
            dto.Nombre = rol.Nombre;
            dto.Descripcion = rol.Descripcion;
            dto.Estado = rol.Estado.ToString();
            dto.CantidadPermisos = rol.Permisos != null ? rol.Permisos.Count : 0;
            return dto;
        }

        public static RolDetalleDto MapearADetalleDto(Rol rol)
        {
            RolDetalleDto dto = new RolDetalleDto();
            dto.IdRol = rol.IdRol;
            dto.Nombre = rol.Nombre;
            dto.Descripcion = rol.Descripcion;
            dto.Estado = rol.Estado.ToString();

            if (rol.Permisos != null)
            {
                for (int i = 0; i < rol.Permisos.Count; i++)
                {
                    dto.IdPermisos.Add(rol.Permisos[i].IdPermiso);
                }
            }

            return dto;
        }

        public static Rol MapearAEntidadDesdeCrear(RolCrearDto dto)
        {
            Rol rol = new Rol();
            rol.Nombre = dto.Nombre;
            rol.Descripcion = dto.Descripcion;
            rol.Estado = EnumeracionMapperNegocio.MapearEstadoEntidad(dto.Estado);
            rol.Permisos = new List<Permiso>();
            return rol;
        }

        public static Rol MapearAEntidadDesdeEditar(RolDetalleDto dto)
        {
            Rol rol = new Rol();
            rol.IdRol = dto.IdRol;
            rol.Nombre = dto.Nombre;
            rol.Descripcion = dto.Descripcion;
            rol.Estado = EnumeracionMapperNegocio.MapearEstadoEntidad(dto.Estado);
            rol.Permisos = new List<Permiso>();
            return rol;
        }
    }
}
