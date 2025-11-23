namespace SGTO.Comun.DTOs
{
    public class ReporteTurnosKpiDto
    {
        public int TotalTurnos { get; set; }
        public int Atendidos { get; set; } // turnos cerrados
        public int Cancelados { get; set; } // cancelados
        public int Ausentes { get; set; } // no asistió
        public int Reprogramados { get; set; } // reporgramado
        public int Pendientes { get; set; } // nuevos
    }
}
