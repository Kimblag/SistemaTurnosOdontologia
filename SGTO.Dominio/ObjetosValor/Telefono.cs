using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Dominio.ObjetosValor
{
    public class Telefono
    {
        public string Numero { get; set; }

        public Telefono(string valor)
        {
            Numero = valor;
        }

        public bool EsValido()
        {
            return true;
        }

        public string Normalizar()
        {
            return "";
        }
    }
}
