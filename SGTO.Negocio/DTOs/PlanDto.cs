namespace SGTO.Negocio.DTOs
{
    public class PlanDto
    {
        public int IdPlan { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal PorcentajeCobertura { get; set; }
        public string Estado { get; set; }
    }
}
