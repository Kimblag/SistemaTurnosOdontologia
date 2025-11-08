using SGTO.Comun.Validacion;
using System;

namespace SGTO.Dominio.ObjetosValor
{
    public class Telefono
    {
        public string Numero { get; }

        public Telefono(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("El teléfono no puede estar vacío.");

            if (!ValidadorCampos.EsTelefonoValido(valor))
                throw new ArgumentException("El formato del teléfono no es válido.");

            Numero = valor.Trim();
        }

        public override string ToString()
        {
            return Numero;
        }
    }
}
