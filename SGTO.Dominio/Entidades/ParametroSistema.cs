using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Dominio.Entidades
{
    public class ParametroSistema
    {
        public int IdParametroSistema { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }


        public ParametroSistema()
        {

        }

        public ParametroSistema(int id, string nombre, string valor, string descripcion)
        {
            IdParametroSistema = id;
            Nombre = nombre;
            Valor = valor;
            Descripcion = descripcion;
        }
    }
}
