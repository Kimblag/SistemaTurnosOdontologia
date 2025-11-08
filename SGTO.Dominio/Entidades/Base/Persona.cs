using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using System;

namespace SGTO.Dominio.Entidades.Base
{
    public abstract class Persona
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DocumentoIdentidad Dni { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public Genero Genero { get; set; }
        public Telefono Telefono { get; set; }
        public Email Email { get; set; }
        public EstadoEntidad Estado { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaModificacion { get; set; }


        protected Persona(string nombre, string apellido, DocumentoIdentidad dni, DateTime fechaNacimiento, Genero genero, Telefono telefono, Email email)
        {
            Nombre = nombre;
            Apellido = apellido;
            Dni = dni;
            FechaNacimiento = fechaNacimiento;
            Genero = genero;
            Telefono = telefono;
            Email = email;
            Estado = EstadoEntidad.Activo;
            FechaAlta = DateTime.Today;
        }

        protected Persona()
        {
            
        }

        public string NombreCompleto()
        {
            return $"{Nombre} {Apellido}";
        }

        public int CalcularEdad()
        {
            DateTime hoy = DateTime.Today;

            int edad = hoy.Year - FechaNacimiento.Year;

            // verificar si ya cumplió años
            if (hoy.Month < FechaNacimiento.Month
                || (hoy.Month == FechaNacimiento.Month
                    && hoy.Day < FechaNacimiento.Day))
            {
                edad--;
            }
            return edad;
        }


        public void Activar()
        {

        }
        public void Desactivar()
        {

        }
    }
}
