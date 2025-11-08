using SGTO.Dominio.Entidades;
using System;
using System.Security.Cryptography;

namespace SGTO.Dominio.ObjetosValor
{
    public class HorarioTurno
    {
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }

        public HorarioTurno(DateTime inicio, DateTime fin, bool validar = true, int duracionEsperadaMinutos = 60)
        {
            // constructor que recibe el parámetro opcional porque si viene de base de datos no hay que validar
            // solo cuando es creación.
            if (inicio >= fin)
                throw new ArgumentException("La hora de inicio debe ser anterior a la hora de fin.");

            var duracion = (fin - inicio).TotalMinutes;
            if (duracion != duracionEsperadaMinutos)
                throw new ArgumentException($"La duración del turno debe ser de {duracionEsperadaMinutos} minutos.");


            if (validar && fin < DateTime.Now)
                throw new ArgumentException("El horario del turno no puede estar en el pasado.");

            Inicio = inicio;
            Fin = fin;
        }

        public override string ToString()
        {
            return $"{Inicio:HH:mm} - {Fin:HH:mm}";
        }

        public bool EsValido()
        {
            DateTime ahora = DateTime.Now;

            // la fecha no puede comenzar luego del fin
            if (Inicio >= Fin)
                return false;

            // no puede estar en el pasado
            if (Fin < ahora)
                return false;

            // duraci[on minima de 15 minutos
            TimeSpan duracion = Fin - Inicio;
            if (duracion.TotalMinutes != 60)
                return false;

            return true;
        }


        public int DuracionMinutos()
        {
            //Devuelve la duración del turno en minutos
            TimeSpan diferencia = Fin - Inicio;
            return (int)diferencia.TotalMinutes;
        }

        public bool EsMultiploDe60()
        {
            //Indica si la duración del turno es un múltiplo de 60 minutos(por ejemplo, 1h, 2hs).
            int duracion = DuracionMinutos();

            if (duracion <= 0)
                return false;

            return duracion % 60 == 0;
        }


        public bool ContieneMomento(DateTime momento)
        {
            // Indica si un momento dado cae dentro del rango de este horario.
            return momento >= Inicio && momento < Fin;
        }

        public bool SeSuperponeCon(HorarioTurno otro)
        {
            //Indica si este horario se superpone con otro.
            if (otro == null)
                return false;

            return Inicio < otro.Fin && Fin > otro.Inicio;
        }



    }
}
