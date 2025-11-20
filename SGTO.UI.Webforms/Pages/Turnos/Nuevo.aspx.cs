using SGTO.UI.Webforms.MasterPages;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Turnos
{
    public partial class Nuevo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("turnos");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Seleccione la especialidad, médico y horario disponible para el paciente seleccionado.");
            }
        }
    }
}