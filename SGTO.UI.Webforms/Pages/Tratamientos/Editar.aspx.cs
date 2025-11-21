using SGTO.UI.Webforms.MasterPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Tratamientos
{
    public partial class Editar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.ConfigurarBotonVolver(true, "~/Pages/Tratamientos/Index.aspx");
                master.EstablecerOpcionMenuActiva("Tratamientos");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Configure la información de la prestación y su arancel correspondiente.");
            }
        }
    }
}