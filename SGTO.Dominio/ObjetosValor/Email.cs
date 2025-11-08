
using SGTO.Comun.Validacion;
using System;

namespace SGTO.Dominio.ObjetosValor
{
    public class Email
    {
        public string Valor { get; }

        public Email(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                throw new ArgumentException("El email no puede estar vacío");
            }

            if (!ValidadorCampos.EsEmailValido(valor))
            {
                throw new ArgumentException("El formato del email es inválido.");
            }

            Valor = valor.Trim().ToLower();
        }


        override public string ToString()
        {
            return Valor;
        }
    }
}
