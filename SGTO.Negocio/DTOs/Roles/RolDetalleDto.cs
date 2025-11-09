using System.Collections.Generic;

namespace SGTO.Negocio.DTOs.Roles
{
    public class RolDetalleDto
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public List<int> IdPermisos { get; set; }

        public RolDetalleDto()
        {
            IdPermisos = new List<int>();
        }
    }
}
