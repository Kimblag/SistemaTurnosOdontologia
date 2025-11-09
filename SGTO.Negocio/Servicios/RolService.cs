using SGTO.Comun.Validacion;
using SGTO.Datos.Infraestructura;
using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Negocio.DTOs.Roles;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Mappers;
using System;
using System.Collections.Generic;

namespace SGTO.Negocio.Servicios
{
    public class RolService
    {
        private readonly RolRepositorio _repositorioRol;
        private readonly PermisoRepositorio _permisoRepositorio;

        public RolService()
        {
            _repositorioRol = new RolRepositorio();
            _permisoRepositorio = new PermisoRepositorio();
        }

        public List<RolListadoDto> Listar(string estado = null, string nombre = null)
        {
            List<Rol> roles = _repositorioRol.Listar(estado, nombre);
            List<RolListadoDto> dtos = new List<RolListadoDto>();
            foreach (Rol rol in roles)
            {
                dtos.Add(RolMapper.MapearAListadoDto(rol));
            }
            return dtos;
        }

        public RolDetalleDto ObtenerPorId(int idRol)
        {
            Rol rol = _repositorioRol.ObtenerPorId(idRol);
            if (rol == null)
                return null;

            return RolMapper.MapearADetalleDto(rol);
        }

        public bool Crear(RolCrearDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            ValidarDatosBasicosParaCrear(dto);

            if (_repositorioRol.ExistePorNombre(dto.Nombre))
                throw new ExcepcionReglaNegocio("Ya existe un rol con ese nombre.");

            List<Permiso> permisosDisponibles = _permisoRepositorio.Listar();

            ValidarPermisosSeleccionados(dto.IdPermisos, permisosDisponibles);

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                datos.IniciarTransaccion();
                try
                {
                    dto.Estado = "Activo";

                    Rol nuevoRol = RolMapper.MapearAEntidadDesdeCrear(dto);

                    int idNuevoRol = _repositorioRol.Crear(nuevoRol, datos);

                    for (int i = 0; i < dto.IdPermisos.Count; i++)
                    {
                        int idPermiso = dto.IdPermisos[i];
                        _repositorioRol.InsertarPermisoARol(idNuevoRol, idPermiso, datos);
                    }

                    datos.ConfirmarTransaccion();
                    return true;
                }
                catch
                {
                    datos.RollbackTransaccion();
                    throw;
                }
            }
        }


        public bool Modificar(RolDetalleDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // verificar que exista
            Rol rolActual = _repositorioRol.ObtenerPorId(dto.IdRol);
            if (rolActual == null)
                throw new ExcepcionReglaNegocio("El rol indicado no existe.");

            // validar datos básicos
            ValidarDatosBasicosParaEditar(dto);

            // si cambió el nombre, validar que no haya otro igual
            if (!string.Equals(rolActual.Nombre.Trim(), dto.Nombre.Trim(), StringComparison.OrdinalIgnoreCase)
                && _repositorioRol.ExistePorNombre(dto.Nombre))
            {
                throw new ExcepcionReglaNegocio("Ya existe un rol con ese nombre.");
            }

            List<Permiso> permisosDisponibles = _permisoRepositorio.Listar();

            // validar permisos enviados ya que debe tener al menos 1
            ValidarPermisosSeleccionados(dto.IdPermisos, permisosDisponibles);

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                datos.IniciarTransaccion();
                try
                {
                    Rol rolMod = RolMapper.MapearAEntidadDesdeEditar(dto);
                    _repositorioRol.Modificar(rolMod, datos);

                    // primero eliminamos los permisos actuales
                    _repositorioRol.EliminarPermisosPorRol(dto.IdRol, datos);

                    // y agregamos los nuevos
                    for (int i = 0; i < dto.IdPermisos.Count; i++)
                    {
                        int idPermiso = dto.IdPermisos[i];
                        _repositorioRol.InsertarPermisoARol(dto.IdRol, idPermiso, datos);
                    }

                    datos.ConfirmarTransaccion();
                    return true;
                }
                catch
                {
                    datos.RollbackTransaccion();
                    throw;
                }
            }
        }


        public bool DarDeBaja(int idRol)
        {
            Rol rol = _repositorioRol.ObtenerPorId(idRol);
            if (rol == null)
                throw new ExcepcionReglaNegocio("No se encontró el rol indicado.");

            if (rol.Estado == EstadoEntidad.Inactivo)
                throw new ExcepcionReglaNegocio("El rol ya está inactivo.");

            if (_repositorioRol.TieneUsuariosAsociados(idRol))
                throw new ExcepcionReglaNegocio("No se puede dar de baja el rol porque hay usuarios activos que lo utilizan.");

            try
            {
                _repositorioRol.DarDeBaja(idRol);
                return true;
            }
            catch
            {
                throw;
            }

        }


        public List<Permiso> ListarPermisos()
        {
            return _permisoRepositorio.Listar();
        }


        private void ValidarDatosBasicosParaCrear(RolCrearDto dto)
        {
            if (!ValidadorCampos.EsTextoObligatorio(dto.Nombre))
                throw new ExcepcionReglaNegocio("El nombre del rol es obligatorio.");

            if (!ValidadorCampos.EsSoloLetrasYEspacios(dto.Nombre))
                throw new ExcepcionReglaNegocio("El nombre del rol solo puede contener letras y espacios.");

            if (!ValidadorCampos.TieneLongitudMaxima(dto.Nombre, 50))
                throw new ExcepcionReglaNegocio("El nombre del rol no puede superar los 50 caracteres.");

            if (!string.IsNullOrWhiteSpace(dto.Descripcion))
            {
                if (!ValidadorCampos.TieneLongitudMinima(dto.Descripcion, 10))
                    throw new ExcepcionReglaNegocio("La descripción debe tener al menos 10 caracteres si se completa.");

                if (!ValidadorCampos.TieneLongitudMaxima(dto.Descripcion, 200))
                    throw new ExcepcionReglaNegocio("La descripción no puede superar los 200 caracteres.");
            }
        }

        private void ValidarDatosBasicosParaEditar(RolDetalleDto dto)
        {
            if (!ValidadorCampos.EsTextoObligatorio(dto.Nombre))
                throw new ExcepcionReglaNegocio("El nombre del rol es obligatorio.");

            if (!ValidadorCampos.EsSoloLetrasYEspacios(dto.Nombre))
                throw new ExcepcionReglaNegocio("El nombre del rol solo puede contener letras y espacios.");

            if (!ValidadorCampos.TieneLongitudMaxima(dto.Nombre, 50))
                throw new ExcepcionReglaNegocio("El nombre del rol no puede superar los 50 caracteres.");

            if (!string.IsNullOrWhiteSpace(dto.Descripcion))
            {
                if (!ValidadorCampos.TieneLongitudMinima(dto.Descripcion, 10))
                    throw new ExcepcionReglaNegocio("La descripción debe tener al menos 10 caracteres si se completa.");

                if (!ValidadorCampos.TieneLongitudMaxima(dto.Descripcion, 200))
                    throw new ExcepcionReglaNegocio("La descripción no puede superar los 200 caracteres.");
            }
        }

        private void ValidarPermisosSeleccionados(List<int> idsSeleccionados, List<Permiso> permisosDisponibles)
        {
            if (idsSeleccionados == null || idsSeleccionados.Count == 0)
                throw new ExcepcionReglaNegocio("Debe asignar al menos un permiso al rol.");

            for (int i = 0; i < idsSeleccionados.Count; i++)
            {
                int idBuscado = idsSeleccionados[i];
                bool existe = false;

                for (int j = 0; j < permisosDisponibles.Count; j++)
                {
                    if (permisosDisponibles[j].IdPermiso == idBuscado)
                    {
                        existe = true;
                        break;
                    }
                }

                if (!existe)
                    throw new ExcepcionReglaNegocio("Se intentó asignar un permiso inexistente.");
            }
        }

    }
}
