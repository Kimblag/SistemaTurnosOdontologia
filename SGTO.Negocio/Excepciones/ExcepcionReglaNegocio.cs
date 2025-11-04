using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Negocio.Excepciones
{
    // clase para lanzar excepciones de negocio para reglas inherentes al dominio, por ejemplo
    // una cobertura que tenga turnos activos no puede ser dada de baja.
    public class ExcepcionReglaNegocio : Exception
    {
        public ExcepcionReglaNegocio()
        {

        }

        public ExcepcionReglaNegocio(string mensaje)
            : base(mensaje)
        {
        }

        public ExcepcionReglaNegocio(string mensaje, Exception innerException)
            : base(mensaje, innerException)
        {
        }
    }
}
