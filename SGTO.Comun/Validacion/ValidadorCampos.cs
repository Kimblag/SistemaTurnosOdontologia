using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SGTO.Comun.Validacion
{
    public static class ValidadorCampos
    {
        private static readonly Regex _emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool EsTextoObligatorio(string texto)
        {
            return !string.IsNullOrWhiteSpace(texto);
        }

        public static bool EsSoloLetrasYEspacios(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            return Regex.IsMatch(texto, @"^[\p{L}\s]+$");
        }

        public static string NormalizarTexto(string texto)
        {
            return texto?.Trim().ToUpper();
        }

        public static string CapitalizarTexto(string texto)
        {
            // para poner en mayúscula el nombre y apellido
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            var palabras = texto.Trim().ToLower().Split(' ');
            for (int i = 0; i < palabras.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(palabras[i]))
                    palabras[i] = char.ToUpper(palabras[i][0]) + palabras[i].Substring(1);
            }
            return string.Join(" ", palabras);
        }

        public static bool EsTextoValido(string texto, int minimo, int maximo)
        {
            return EsTextoObligatorio(texto)
                && TieneLongitudMinima(texto, minimo)
                && TieneLongitudMaxima(texto, maximo);
        }

        public static bool TieneLongitudMinima(string texto, int minimo)
        {
            if (string.IsNullOrEmpty(texto))
                return false;

            return texto.Trim().Length >= minimo;
        }

        public static bool TieneLongitudMaxima(string texto, int maximo)
        {
            if (string.IsNullOrEmpty(texto))
                return false;

            return texto.Trim().Length <= maximo;
        }


        public static bool EsAlfanumerico(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            return Regex.IsMatch(texto, @"^[\p{L}\p{N}\s]+$");
        }


        public static bool EsEnteroPositivo(string texto)
        {
            return Regex.IsMatch(texto ?? string.Empty, @"^\d+$");
        }


        public static bool EsTelefonoValido(string texto)
        {
            return Regex.IsMatch(texto ?? string.Empty, @"^\d{6,15}$");
        }

        public static bool EsPorcentajeCoberturaValido(decimal? valor)
        {
            if (!valor.HasValue)
                return false;

            if (valor < 0 || valor > 100)
                return false;

            return true;
        }

        public static bool EsEmailValido(string texto)
        {
            return !string.IsNullOrWhiteSpace(texto) && _emailRegex.IsMatch(texto);
        }


        public static bool EsFechaNacimientoValida(DateTime fecha)
        {
            DateTime hoy = DateTime.Today;
            return fecha <= hoy && fecha >= hoy.AddYears(-120);
        }

        public static bool EsDecimalValido(string texto, out decimal valor)
        {
            valor = 0;
            if (string.IsNullOrWhiteSpace(texto))
            {
                return false;
            }

            string costoTxt = texto.Trim().Replace(',', '.');
            return decimal.TryParse(costoTxt, NumberStyles.Any, CultureInfo.InvariantCulture, out valor);
        }

    }
}
