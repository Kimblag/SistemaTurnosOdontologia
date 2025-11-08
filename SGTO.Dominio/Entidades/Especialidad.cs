using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;

namespace SGTO.Dominio.Entidades
{
    public class Especialidad
    {
        public int IdEspecialidad { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public List<Tratamiento> TratamientosAsociados { get; set; }
        public EstadoEntidad Estado { get; set; }

        public Especialidad()
        {

        }

        public Especialidad(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            TratamientosAsociados = new List<Tratamiento>();
            Estado = EstadoEntidad.Activo;
        }

        public Especialidad(string nombre, string descripcion, List<Tratamiento> tratamientos)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            TratamientosAsociados = tratamientos;
            Estado = EstadoEntidad.Activo;
        }

        public Especialidad(int idEspecialidad, string nombre, string descripcion,
            List<Tratamiento> tratamientosAsociados, EstadoEntidad estado)
        {
            IdEspecialidad = idEspecialidad;
            Nombre = nombre;
            Descripcion = descripcion;
            TratamientosAsociados = tratamientosAsociados;
            Estado = estado;
        }

        public void AgregarTratamiento(Tratamiento tratamiento)
        {

        }

        public bool EsDuplicadoDe(Especialidad otra)
        {
            return false;
        }
    }
}
