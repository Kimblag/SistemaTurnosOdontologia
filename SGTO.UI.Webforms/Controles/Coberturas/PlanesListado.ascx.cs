using SGTO.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Coberturas
{
    public partial class PlanesListado : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarPlanes();
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

        private void CargarPlanes()
        {
            // datos de prueba
            var osde = new Cobertura("OSDE", "Cobertura médica integral") { IdCobertura = 1 };
            var osecac = new Cobertura("OSECAC", "Cobertura sindical de empleados de comercio") { IdCobertura = 2 };
            var swiss = new Cobertura("Swiss Medical", "Cobertura privada premium") { IdCobertura = 3 };


            var planes = new List<Plan>
            {
                new Plan("OSDE 210", 70, osde, "Plan base con cobertura nacional"){ IdPlan = 1 },
                new Plan("OSDE 310", 85, osde, "Incluye reintegros y más especialidades"){ IdPlan = 2 },
                new Plan("OSECAC Clásico", 60, osecac, "Plan básico sindical"){ IdPlan = 3 },
                new Plan("OSECAC Premium", 80, osecac, "Mayor cobertura y reintegros"){ IdPlan = 4 },
                new Plan("Swiss Silver", 75, swiss, "Cobertura media"){ IdPlan = 5 },
                new Plan("Swiss Gold", 90, swiss, "Cobertura total con internación privada"){ IdPlan = 6 }
            };

            gvPlanes.DataSource = planes;
            gvPlanes.DataBind();
        }

        protected void btnNuevoPlan_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/CoberturasPlanes/NuevoPlan", false);
        }
    }
}