using SGTO.Negocio.Seguridad;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Seguridad;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.CoberturasPlanes
{
    public partial class EditarPlan : System.Web.UI.Page
    {
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.ConfigurarBotonVolver(true, "~/Pages/CoberturasPlanes/Planes/Index.aspx");
                master.EstablecerOpcionMenuActiva("Coberturas");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Administre los datos del plan y el nivel de cobertura que ofrece a los afiliados.");
            }

            if (!_servicioAutorizacion.TienePermiso(SessionManager.Usuario, "PLANES", "EDITAR"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

            if (!IsPostBack)
            {
                PlanesFormControl.ModoEdicion = true;
            }
        }
    }
}