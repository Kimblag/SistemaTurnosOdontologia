namespace SGTO.Comun.DTOs
{
    public class ReporteCoberturasDto
    {
        public string Cobertura { get; set; }
        public string Estado { get; set; }
        public int CantidadPlanes { get; set; }
        public int TurnosAgendados { get; set; } // aqui mostraremos TODOS
        public int TurnosRealizados { get; set; } // solo los que se realizaron (cerrados)
        public decimal TotalFacturado { get; set; }
        public decimal A_Cargo_OS { get; set; } // lo que la os debe pagar
        public decimal A_Cargo_Paciente { get; set; }
    }

    public class ReportePlanesDto
    {
        public string Cobertura { get; set; }
        public string Plan { get; set; }
        public string Estado { get; set; }
        public decimal PorcentajeCubierto { get; set; }

        public int TurnosRealizados { get; set; }
        public decimal TotalFacturado { get; set; }
        public decimal A_Cargo_OS { get; set; }
        public decimal A_Cargo_Paciente { get; set; }
    }
}