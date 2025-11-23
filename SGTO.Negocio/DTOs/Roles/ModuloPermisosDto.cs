namespace SGTO.Negocio.DTOs.Roles
{
    public class ModuloPermisosDto
    {
        public string NombreModulo { get; set; }
        public int IdPermisoVer { get; set; }
        public bool AsignadoVer { get; set; }

        public int IdPermisoCrear { get; set; }
        public bool AsignadoCrear { get; set; }

        public int IdPermisoEditar { get; set; }
        public bool AsignadoEditar { get; set; }

        public int IdPermisoEliminar { get; set; }
        public bool AsignadoEliminar { get; set; }

        public int IdPermisoActivar { get; set; }
        public bool AsignadoActivar { get; set; }

        public int IdPermisoDesactivar { get; set; }
        public bool AsignadoDesactivar { get; set; }
    }
}
