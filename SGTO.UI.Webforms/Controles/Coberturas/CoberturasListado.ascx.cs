using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGTO.Negocio.Servicios;
using SGTO.Negocio.DTOs;

namespace SGTO.UI.Webforms.Controles.Coberturas
{
    public partial class CoberturasListado : System.Web.UI.UserControl
    {

        private readonly CoberturaService _servicioCobertura = new CoberturaService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarCoberturas();
        }

        public void gvCoberturas_RowDataBound(object sender, GridViewRowEventArgs e) { }

        public void gvCoberturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCoberturas.PageIndex = e.NewPageIndex;
            CargarCoberturas();
        }

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
            try
            {
                List<CoberturaDto> lista = _servicioCobertura.Listar();

                gvCoberturas.DataSource = lista;
                gvCoberturas.DataBind();
            }
            catch (Exception)
            {
                // mostrar error en la UI.
                throw;
            }
        }

        protected void btnNuevaCobertura_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/CoberturasPlanes/NuevaCobertura", false);
        }
    }
}