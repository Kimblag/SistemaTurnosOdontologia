using System;

namespace SGTO.Dominio.ObjetosValor
{
    public class HorarioTurno
    {
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }

        public HorarioTurno(DateTime inicio, DateTime fin)
        {
            Inicio = inicio;
            Fin = fin;
        }

        public bool EsValido()
        {
            // validar que la fecha sea válida (no sea mayor qu ehoy, etc)

            return true;
        }

        public int DuracionMinutos()
        {
            return 0;
        }

        public bool EsMultiploDe60()
        {
            return true;
        }

    }
}
