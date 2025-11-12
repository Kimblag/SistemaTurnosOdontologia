using System.Collections.Generic;

namespace SGTO.Negocio.DTOs
{
    public class TratamientoDto
    {
        public int IdTratamiento { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal CostoBase { get; set; }
        public int CantidadEspecialidades { get; set; }
        public string Estado { get; set; }
        public string NombreEspecialidad { get; set; }
        public int IdEspecialidad { get; set; }

        public TratamientoDto()
        {
        }
    }
}