using System.Collections.Generic;

namespace SGTO.Negocio.DTOs
{
    public class EspecialidadDto
    {
        public int IdEspecialidad { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int CantidadTratamientos { get; set; }
        public string Estado { get; set; }
        public List<string> NombreTratamientos { get; set; }

        public EspecialidadDto()
        {
            NombreTratamientos = new List<string>();
        }
    }
}