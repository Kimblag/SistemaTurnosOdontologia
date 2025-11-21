using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Configuracion.Roles
{
    public partial class Detalle : System.Web.UI.Page
    {
        private readonly RolService _servicioRol = new RolService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.ConfigurarBotonVolver(true, "~/Pages/Configuracion/Roles/Index.aspx");
                master.EstablecerOpcionMenuActiva("Configuracion");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Vea los detalles y permisos asociados al Rol");
            }

            if (!IsPostBack)
            {
                string idStr = Request.QueryString["id-rol"];
                int idRol;

                bool esValido = !string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out idRol);

                if (!esValido)
                {
                    Response.Redirect("~/Pages/Configuracion/Roles/Index.aspx", false);
                    return;
                }
            }
        }
    }
}