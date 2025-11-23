using System;

namespace SGTO.Negocio.Excepciones
{
    public class ExcepcionAutenticacion : Exception
    {
        public ExcepcionAutenticacion(string mensaje) : base(mensaje)
        {

        }
    }
}
