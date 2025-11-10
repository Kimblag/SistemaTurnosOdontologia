namespace SGTO.Negocio.DTOs.Usuarios
{
    public class UsuarioEdicionDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; } = string.Empty;  // esto es cuando se cambai una pass
        public string ConfirmarPassword { get; set; } = string.Empty; // idem
        public int IdRol { get; set; }
        public string Estado { get; set; }  // Activo - Inactivo
    }
}
