namespace SGTO.Negocio.DTOs
{
    public class UsuarioListadoDto
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public string Estado { get; set; }
    }
}
