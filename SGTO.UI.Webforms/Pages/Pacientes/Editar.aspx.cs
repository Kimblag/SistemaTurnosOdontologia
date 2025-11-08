using SGTO.UI.Webforms.MasterPages;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Pacientes
{
    public partial class Editar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Pacientes");
                master.EstablecerTituloSeccion(this.Page.Title);
            }

            if (!IsPostBack)
            {
                PacienteFormControl.ModoEdicion = true;
            }
        }

    }
}