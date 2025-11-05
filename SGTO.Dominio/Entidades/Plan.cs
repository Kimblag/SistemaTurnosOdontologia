using SGTO.Dominio.Enums;

namespace SGTO.Dominio.Entidades
{
    public class Plan
    {
        public int IdPlan { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal PorcentajeCobertura { get; set; }
        public Cobertura Cobertura { get; set; }
        public EstadoEntidad Estado { get; set; }

        public Plan()
        {

        }


        public Plan(string nombre, decimal porcentajeCobertura, Cobertura cobertura, string descripcion)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            PorcentajeCobertura = porcentajeCobertura;
            Cobertura = cobertura;
            Estado = EstadoEntidad.Activo;
        }

        public Plan(int idPlan, string nombre, string descripcion,
            decimal porcentajeCobertura, Cobertura cobertura, EstadoEntidad estado)
        {
            IdPlan = idPlan;
            Nombre = nombre;
            Descripcion = !string.IsNullOrEmpty(descripcion) ? descripcion : string.Empty;
            PorcentajeCobertura = porcentajeCobertura;
            Cobertura = cobertura;
            Estado = estado;
        }


        public decimal AplicarCobertura(decimal monto)
        {
            return 0;
        }

        public bool EstaActivo()
        {
            return Estado == EstadoEntidad.Activo;
        }

    }
}
