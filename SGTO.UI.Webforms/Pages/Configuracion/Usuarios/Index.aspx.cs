using SGTO.UI.Webforms.MasterPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Configuracion.Usuarios
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Configuracion");
                master.EstablecerTituloSeccion(this.Page.Title);
            }
        }

        protected void btnNuevoUsuario_Click(object sender, EventArgs e)
        {

        }

        protected void gvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}