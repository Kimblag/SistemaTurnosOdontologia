namespace SGTO.Negocio.DTOs.Permisos
{
    public class PermisoDto
    {
        public string Modulo { get; set; }
        public string Accion { get; set; }

        public PermisoDto() { }

        public PermisoDto(string modulo, string accion)
        {
            Modulo = modulo;
            Accion = accion;
        }

        public override string ToString()
        {
            return $"{Modulo}_{Accion}";
        }
    }
}
