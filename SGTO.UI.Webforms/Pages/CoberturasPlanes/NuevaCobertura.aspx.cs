using SGTO.Negocio.Seguridad;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.CoberturasPlanes
{
    public partial class NuevaCobertura : System.Web.UI.Page
    {
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.ConfigurarBotonVolver(true, "~/Pages/CoberturasPlanes/Coberturas/Index.aspx");
                master.EstablecerOpcionMenuActiva("Coberturas");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Defina la entidad prestadora de salud.");
            }
            if (!_servicioAutorizacion.TienePermiso(SessionManager.Usuario, "COBERTURAS", "CREAR"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

            CoberturasFormControl.ModoEdicion = false;
        }
    }
}