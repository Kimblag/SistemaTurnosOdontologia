using System.Collections.Generic;

namespace SGTO.Negocio.DTOs.Roles
{
    public class RolCrearDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public List<int> IdPermisos { get; set; }

        public RolCrearDto()
        {
            IdPermisos = new List<int>();
        }
    }
}
