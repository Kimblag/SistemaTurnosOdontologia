using SGTO.UI.Webforms.MasterPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Configuracion.Roles
{
    public partial class Editar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.ConfigurarBotonVolver(true, "~/Pages/Configuracion/Roles/Index.aspx");
                master.EstablecerOpcionMenuActiva("Configuracion");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Actualice el nombre del rol o ajuste los permisos asignados al grupo.");
            }

            if (!IsPostBack)
            {
                RolesFormControl.ModoEdicion = true;
            }
        }
    }
}