using SGTO.Comun.Validacion;
using System;
using System.Collections.Generic;
using System.Web;

namespace SGTO.UI.Webforms.Utils
{
    public static class TurnoUiHelper
    {

        private static readonly Dictionary<string, string> _estadoTurnoColores = new Dictionary<string, string>
        {
            {"nuevo", "badge badge-primary" },
            {"cancelado", "badge badge-danger" },
            {"reprogramado", "badge badge-info" },
            {"noasistio", "badge badge-dark"  },
            {"cerrado", "badge badge-completed" }
        };

        private static readonly Dictionary<string, string> _estadoTurnoTextos = new Dictionary<string, string>
        {
            {"nuevo", "Nuevo" },
            {"cancelado", "Cancelado" },
            {"reprogramado", "Reprogramado" },
            {"noasistio", "No Asistió" },
            {"cerrado", "Cerrado" }
        };

        private static readonly string[] EstadosNoEditables = { "cancelado", "cerrado", "noasistio" };

        public static string ObtenerCssEstadoTurnoBadge(string estadoTurno)
        {
            if (string.IsNullOrEmpty(estadoTurno))
                throw new ArgumentNullException("El valor del estado del turno no puede estar vacío.");

            string estadoTurnoNormalizado = estadoTurno.Trim().ToLower();

            if (_estadoTurnoColores.TryGetValue(estadoTurnoNormalizado, out string cssClass))
            {
                return cssClass;
            }

            return "badge badge-secondary";
        }


        public static string ObtenerTextoEstado(string estadoTurno)
        {
            if (string.IsNullOrWhiteSpace(estadoTurno)) return "Indefinido";

            if (_estadoTurnoTextos.TryGetValue(estadoTurno.ToLower().Trim(), out string textoBonito))
                return textoBonito;

            return "Indefinido";
        }


        public static bool EsEditable(string estadoTurno)
        {
            if (string.IsNullOrWhiteSpace(estadoTurno)) return false;

            string estado = ValidadorCampos.NormalizarTexto(estadoTurno.Replace(" ", ""));

            return !(estado == "cancelado" || estado == "cerrado" || estado == "noasistio" || estado == "no asistio");
        }

    }
}