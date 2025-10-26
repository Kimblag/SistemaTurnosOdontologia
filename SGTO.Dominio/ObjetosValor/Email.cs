using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Dominio.ObjetosValor
{
    public class Email
    {
        public string Valor { get; set; }

        public Email(string valor)
        {
            Valor = valor;
        }

        public bool EsValido()
        {
            return true;
        }

        public string Normalizar()
        {
            return "";
        }

        override public string ToString()
        {
            return Valor;
        }
    }
}
