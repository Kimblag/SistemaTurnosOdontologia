using SGTO.UI.Webforms.MasterPages;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Turnos
{
    public partial class Editar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Turnos");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Modifique la fecha y horario. Recuerde que los cambios de estado notificarán al paciente por email.");
            }
        }
    }
}