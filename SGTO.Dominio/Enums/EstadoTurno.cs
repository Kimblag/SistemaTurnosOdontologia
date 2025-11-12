namespace SGTO.Dominio.Enums
{
    public enum EstadoTurno
    {
        Nuevo = 'N',
        PendienteReprogramacion = 'P', //turnos que necesitan ser reprogramdos de forma manual por cambio de horario del médico
        Reprogramado = 'R',
        NoAsistio = 'X',
        Cancelado = 'C',
        Cerrado = 'Z'
    }
}
