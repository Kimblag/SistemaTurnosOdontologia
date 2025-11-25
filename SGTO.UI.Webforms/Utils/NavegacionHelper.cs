using SGTO.Negocio.DTOs.Seguridad;
using SGTO.Negocio.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGTO.UI.Webforms.Utils
{
    public static class NavegacionHelper
    {
        private static readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();

        public static string ObtenerUrlInicial(UsuarioSesionDto usuario)
        {
            if (usuario == null) return "~/Pages/Login/Index.aspx";

            var mapaNavegacion = new Dictionary<string, string>
            {
                { "INICIO",         "~/Pages/Home/Dashboard.aspx" },
                { "TURNOS",         "~/Pages/Turnos/Index.aspx" },
                { "PACIENTES",      "~/Pages/Pacientes/Index.aspx" },
                { "MEDICOS",        "~/Pages/Medicos/Index.aspx" },
                { "COBERTURAS",     "~/Pages/CoberturasPlanes/Index.aspx" },
                { "TRATAMIENTOS",   "~/Pages/Tratamientos/Index.aspx" },
                { "ESPECIALIDADES", "~/Pages/Especialidades/Index.aspx" },
                { "REPORTES",       "~/Pages/Reportes/Index.aspx" },
                { "CONFIGURACION",  "~/Pages/Configuracion/Index.aspx" }
            };

            foreach (var item in mapaNavegacion)
            {
                string modulo = item.Key;
                string url = item.Value;

                if (_servicioAutorizacion.TienePermiso(usuario, modulo, "VER"))
                {
                    return url;
                }
            }

            return "~/Pages/Errores/AccesoDenegado.aspx";
        }
    }
}