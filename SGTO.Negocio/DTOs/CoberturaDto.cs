using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Negocio.DTOs
{
    public class CoberturaDto
    {
        public int IdCobertura { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int CantidadPlanes { get; set; }
        public string Estado { get; set; }
    }
}
