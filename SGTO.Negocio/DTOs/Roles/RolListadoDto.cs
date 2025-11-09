namespace SGTO.Negocio.DTOs.Roles
{
    public class RolListadoDto
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int CantidadPermisos { get; set; }
    }
}
