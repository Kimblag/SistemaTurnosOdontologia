using System;

namespace SGTO.Negocio.Excepciones
{
    
    public class ExcepcionEntidadNoEncontrada : Exception
    {
        public ExcepcionEntidadNoEncontrada()
        {
        }

        public ExcepcionEntidadNoEncontrada(string message)
            : base(message)
        {
        }

        public ExcepcionEntidadNoEncontrada(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}