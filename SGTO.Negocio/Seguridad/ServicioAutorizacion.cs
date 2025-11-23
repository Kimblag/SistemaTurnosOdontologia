using SGTO.Negocio.DTOs.Seguridad;
using System;
using System.Collections.Generic;

namespace SGTO.Negocio.Seguridad
{
    public class ServicioAutorizacion
    {
        public bool TienePermiso(UsuarioSesionDto usuario, string modulo, string accion)
        {
            if (usuario == null || usuario.Permisos == null)
                return false;

            if (usuario.EsAdmin)
                return true;

            string clavePermiso = $"{modulo.ToUpper()}_{accion.ToUpper()}";

            return usuario.Permisos.Contains(clavePermiso);
        }

        public bool TieneRol(UsuarioSesionDto usuario, string nombreRol)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.NombreRol))
                return false;

            return usuario.NombreRol.Equals(nombreRol, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool EsAdministrador(UsuarioSesionDto usuario)
        {
            return TieneRol(usuario, "Administrador");
        }

        public bool EsMedico(UsuarioSesionDto usuario)
        {
            return TieneRol(usuario, "Médico");
        }

        public bool EsRecepcionista(UsuarioSesionDto usuario)
        {
            return TieneRol(usuario, "Recepcionista");
        }
    }
}
