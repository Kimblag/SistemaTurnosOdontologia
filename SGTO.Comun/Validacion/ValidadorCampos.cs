using System;
using System.Text.RegularExpressions;

namespace SGTO.Comun.Validacion
{
    public static class ValidadorCampos
    {
        public static bool EsTextoObligatorio(string texto)
        {
            return !string.IsNullOrWhiteSpace(texto);
        }

        public static bool EsAlfanumerico(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return false;

            foreach (char c in texto)
            {
                if (!char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool EsNumerico(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return false;

            foreach (char c in texto)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
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

        public static string NormalizarTexto(string texto)
        {
            return texto?.Trim().ToUpper();
        }

        public static bool EsTextoValido(string texto, int minimo, int maximo)
        {
            return EsTextoObligatorio(texto)
                && TieneLongitudMinima(texto, minimo)
                && TieneLongitudMaxima(texto, maximo);
        }

        public static bool EsEmailValido(string texto)
        {
            string patron = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return Regex.IsMatch(texto ?? string.Empty, patron);
        }

        public static bool EsSoloLetrasYEspacios(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            foreach (char c in texto)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool EsPorcentajeCoberturaValido(decimal? valor)
        {
            if (!valor.HasValue)
                return false;

            if (valor < 0 || valor > 100)
                return false;

            return true;
        }
    }
}
