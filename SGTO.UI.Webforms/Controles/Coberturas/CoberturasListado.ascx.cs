using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Coberturas
{
    public partial class CoberturasListado : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarCoberturas();
        }

        public void gvCoberturas_RowDataBound(object sender, GridViewRowEventArgs e) { }

        public void gvCoberturas_PageIndexChanging(object sender, GridViewPageEventArgs e) { }

        public void gvCoberturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idCobertura = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Pages/CoberturasPlanes/EditarCobertura?id-cobertura={idCobertura}", false);
            }
            else if (e.CommandName == "Eliminar")
            {

            }
            else if (e.CommandName == "VerPlanes")
            {

            }
        }

        private void CargarCoberturas()
        {
            // datos de prieba
            var lista = new List<Cobertura>
            {
                new Cobertura("OSDE", "Cobertura médica general") { IdCobertura = 1, Planes = new List<Plan>(), Estado = EstadoEntidad.Activo },
                new Cobertura("OSECAC", "Obra social de empleados de comercio") { IdCobertura = 2, Planes = new List<Plan>(), Estado = EstadoEntidad.Activo },
                new Cobertura("Swiss Medical", "Cobertura premium") { IdCobertura = 3, Planes = new List<Plan>(), Estado = EstadoEntidad.Inactivo },
                new Cobertura("Galeno", "Planes con reintegros") { IdCobertura = 4, Planes = new List<Plan>(), Estado = EstadoEntidad.Activo },
                new Cobertura("Medicus", "Cobertura odontológica") { IdCobertura = 5, Planes = new List<Plan>(), Estado = EstadoEntidad.Inactivo },
                new Cobertura("OSDEPYM", "Cobertura para pymes") { IdCobertura = 6, Planes = new List<Plan>(), Estado = EstadoEntidad.Activo },
                new Cobertura("OSPIM", "Mutual de industrias metalúrgicas") { IdCobertura = 7, Planes = new List<Plan>(), Estado = EstadoEntidad.Activo },
                new Cobertura("Sancor Salud", "Cobertura familiar") { IdCobertura = 8, Planes = new List<Plan>(), Estado = EstadoEntidad.Activo },
                new Cobertura("Prevención Salud", "Cobertura joven") { IdCobertura = 9, Planes = new List<Plan>(), Estado = EstadoEntidad.Inactivo },
            };

            gvCoberturas.DataSource = lista;
            gvCoberturas.DataBind();
        }

        protected void btnNuevaCobertura_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/CoberturasPlanes/NuevaCobertura", false);
        }
    }
}