namespace SGTO.Comun.DTOs
{
    public class ReporteTratamientosDto
    {
        public int IdTratamiento { get; set; }
        public string Nombre { get; set; }
        public string Especialidad { get; set; }
        public decimal CostoBase { get; set; }
        public string Estado { get; set; }

        public int CantidadRealizados { get; set; }
        // agrego estas props para poder mostrar una visión realista de lo que es el reporte de ttos 
        // ya que estaba mostrando información que no es real porque tenemos cobertura de obras sociales que afectan en los valores
        // saque le valor total estimado porque no se debe sumar los valores en crudo.
        public decimal TotalFacturado { get; set; } // valor total sin descuento
        public decimal TotalCobradoPaciente { get; set; }  // el copago que paga el pcte
        public decimal TotalCobradoObraSocial { get; set; } // lo que debe reclamar en la OS o prepaga
    }
}