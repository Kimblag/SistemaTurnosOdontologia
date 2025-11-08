using SGTO.Dominio.Enums;
using System;


namespace SGTO.Dominio.Entidades
{
    public class Tratamiento
    {

        public int IdTratamiento { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal CostoBase { get; set; }
        public Especialidad Especialidad { get; set; }
        public EstadoEntidad Estado { get; set; }

        public Tratamiento()
        {

        }

        public Tratamiento(string nombre, string descripcion, decimal costoBase,
            Especialidad especialidad)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            CostoBase = costoBase;
            Especialidad = especialidad;
            Estado = EstadoEntidad.Activo;
        }

        public Tratamiento(int idTratamiento, string nombre, string descripcion, decimal costoBase,
           Especialidad especialidad, EstadoEntidad estado)
        {
            IdTratamiento = idTratamiento;
            Nombre = nombre;
            Descripcion = descripcion;
            CostoBase = costoBase;
            Especialidad = especialidad;
            Estado = EstadoEntidad.Activo;
        }

        public bool PerteneceA(Especialidad especialidad)
        {
            return true;
        }

        public decimal CalcularCostoFinal(decimal porcentajeCobertura)
        {
            return 0;
        }
    }
}
