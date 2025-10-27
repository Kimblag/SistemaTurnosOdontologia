using SGTO.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Coberturas
{
    public partial class CoberturasForm : System.Web.UI.UserControl
    {
        public bool ModoEdicion { get; set; } = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<Plan> planes = new List<Plan>();
            gvPlanes.DataSource = planes;
            gvPlanes.DataBind();

            if (!IsPostBack)
            {
                panelPlanes.Visible = !ModoEdicion;
            }
        }

        protected void btnNuevoPlan_Click(object sender, EventArgs e)
        {

        }

        public void gvPlanes_RowDataBound(object sender, GridViewRowEventArgs e) { }
        public void gvPlanes_PageIndexChanging(object sender, GridViewPageEventArgs e) { }
        public void gvPlanes_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int idPlan = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Pages/CoberturasPlanes/EditarPlan?id-plan={idPlan}", false);
            }
            else if (e.CommandName == "Eliminar")
            {

            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/CoberturasPlanes/Index", false);
        }
    }
}