using SGTO.Comun.Validacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Dominio.ObjetosValor
{
    public class DocumentoIdentidad
    {
        public string Numero { get; }

        public DocumentoIdentidad(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("El documento no puede estar vacío.");

            if (!ValidadorCampos.EsEnteroPositivo(valor) || valor.Length < 7 || valor.Length > 9)
                throw new ArgumentException("El DNI debe contener entre 7 y 9 dígitos numéricos.");

            Numero = valor.Trim();
        }

        public override string ToString()
        {
            return Numero;
        }
    }
}
