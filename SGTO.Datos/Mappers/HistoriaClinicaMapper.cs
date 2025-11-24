using SGTO.Dominio.Entidades;
using System;
using System.Data.SqlClient;

namespace SGTO.Datos.Mappers
{
    public class HistoriaClinicaMapper
    {
        public static HistoriaClinicaRegistro Mapear(SqlDataReader lector)
        {
            HistoriaClinicaRegistro historia = new HistoriaClinicaRegistro();

            if (TieneColumna(lector, "IdHistoriaClinicaRegistro"))
                historia.IdHistoriaClinicaRegistro = lector.GetInt32(lector.GetOrdinal("IdHistoriaClinicaRegistro"));

            if (TieneColumna(lector, "IdTurno"))
            {
                historia.TurnoOrigen = new Turno { IdTurno = lector.GetInt32(lector.GetOrdinal("IdTurno")) };
            }

            if (TieneColumna(lector, "FechaAtencion"))
                historia.FechaAtencion = lector.GetDateTime(lector.GetOrdinal("FechaAtencion"));

            if (TieneColumna(lector, "Diagnostico"))
                historia.Diagnostico = lector.GetString(lector.GetOrdinal("Diagnostico"));

            if (TieneColumna(lector, "Observaciones"))
            {
                historia.Observaciones = lector.IsDBNull(lector.GetOrdinal("Observaciones"))
                    ? string.Empty
                    : lector.GetString(lector.GetOrdinal("Observaciones"));
            }

            if (TieneColumna(lector, "IdTratamiento") || TieneColumna(lector, "NombreTratamiento"))
            {
                historia.TratamientoAplicado = new Tratamiento();

                if (TieneColumna(lector, "IdTratamiento"))
                    historia.TratamientoAplicado.IdTratamiento = lector.GetInt32(lector.GetOrdinal("IdTratamiento"));

                if (TieneColumna(lector, "NombreTratamiento"))
                    historia.TratamientoAplicado.Nombre = lector.GetString(lector.GetOrdinal("NombreTratamiento"));

                if (TieneColumna(lector, "CostoBase"))
                    historia.TratamientoAplicado.CostoBase = lector.GetDecimal(lector.GetOrdinal("CostoBase"));
            }

            if (TieneColumna(lector, "NombreProfesional"))
            {
                historia.Medico = new Medico
                {
                    Apellido = lector.GetString(lector.GetOrdinal("NombreProfesional"))
                };
            }

            if (TieneColumna(lector, "NombreEspecialidad"))
            {
                historia.Especialidad = new Especialidad
                {
                    Nombre = lector.GetString(lector.GetOrdinal("NombreEspecialidad"))
                };
            }

            return historia;
        }


        private static bool TieneColumna(SqlDataReader lector, string nombreColumna)
        {
            for (int i = 0; i < lector.FieldCount; i++)
            {
                if (lector.GetName(i).Equals(nombreColumna, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
