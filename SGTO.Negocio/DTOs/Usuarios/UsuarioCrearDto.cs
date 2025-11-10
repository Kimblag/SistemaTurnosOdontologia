namespace SGTO.Negocio.DTOs.Usuarios
{
    public class UsuarioCrearDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        public string ConfirmarPassword { get; set; }
        public int IdRol { get; set; }
        public string Estado { get; set; }  // Activo - Inactivo
    }
}
