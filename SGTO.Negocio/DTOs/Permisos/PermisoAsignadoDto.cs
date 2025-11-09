namespace SGTO.Negocio.DTOs.Permisos
{
    public class PermisoAsignadoDto
    {
        public int IdPermiso { get; set; }
        public string Modulo { get; set; }
        public string Accion { get; set; }
        public bool Asignado { get; set; }
    }
}
