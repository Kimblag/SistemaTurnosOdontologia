using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Coberturas
{
    public partial class PlanesListado : System.Web.UI.UserControl
    {
        private readonly PlanService _servicioPlanes = new PlanService();
        private const string KEY_ESTADO_PLANES = "FiltroEstadoPlanes";
        private const string KEY_COBERTURA_PLANES = "FiltroCoberturaPlanes";
        private const string KEY_TEXTO_PLANES = "FiltroTextoPlanes";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCoberturasDropdown();

                RestaurarFiltrosDesdeSession();

                CargarPlanes();
            }
        }

        public void gvPlanes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PlanDto planDto = (PlanDto)e.Row.DataItem;

                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");
                if (lblEstado != null && planDto != null)
                {
                    if (planDto.Estado.ToLower() == "Activo".ToLower())
                    {
                        lblEstado.Attributes["class"] = "badge badge-success";
                    }
                    else
                    {
                        lblEstado.Attributes["class"] = "badge badge-warning";
                    }
                }
            }
        }

        public void gvPlanes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPlanes.PageIndex = e.NewPageIndex;
            AplicarFiltros();
        }

        public void gvPlanes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sort")
                return;

            int idPlan = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Pages/CoberturasPlanes/EditarPlan?id-plan={idPlan}", false);
            }
        }

        private void CargarPlanes(string estado = null)
        {
            try
            {
                var listado = _servicioPlanes.Listar(estado);
                gvPlanes.DataSource = listado;
                gvPlanes.DataBind();
            }
            catch (Exception)
            {
                gvPlanes.DataSource = new List<PlanDto>();
                gvPlanes.DataBind();

                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error al cargar planes",
                    "Ocurrió un error inesperado al intentar obtener la lista de planes.");
            }
        }

        protected void btnNuevoPlan_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/CoberturasPlanes/NuevoPlan", false);
        }

        private void CargarCoberturasDropdown()
        {
            try
            {
                CoberturaService servicioCobertura = new CoberturaService();
                List<CoberturaDto> coberturas = servicioCobertura.Listar("activo");

                ddlCoberturas.DataSource = coberturas;
                ddlCoberturas.DataTextField = "Nombre";
                ddlCoberturas.DataValueField = "IdCobertura";
                ddlCoberturas.DataBind();

                ddlCoberturas.Items.Insert(0, new ListItem("Todas", "todos"));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                ddlCoberturas.Items.Clear();
                ddlCoberturas.Items.Add(new ListItem("Error al cargar", ""));
            }
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Session[KEY_ESTADO_PLANES] = null;
            Session[KEY_COBERTURA_PLANES] = null;
            Session[KEY_TEXTO_PLANES] = null;

            ddlEstado.SelectedValue = "todos";
            ddlCoberturas.SelectedIndex = 0;
            txtBuscarPlanes.Text = string.Empty;

            CargarPlanes();
        }

        private void AplicarFiltros()
        {
            string estadoSeleccionado = ddlEstado.SelectedValue;
            string textoBusqueda = txtBuscarPlanes.Text.Trim().ToLower();
            string coberturaSeleccionada = ddlCoberturas.SelectedValue;

            Session[KEY_ESTADO_PLANES] = estadoSeleccionado == "todos" ? null : estadoSeleccionado;
            Session[KEY_COBERTURA_PLANES] = coberturaSeleccionada == "todos" ? null : coberturaSeleccionada;
            Session[KEY_TEXTO_PLANES] = string.IsNullOrEmpty(textoBusqueda) ? null : textoBusqueda;

            List<PlanDto> lista = _servicioPlanes.Listar(Session[KEY_ESTADO_PLANES] as string);

            var coberturaSesion = Session[KEY_COBERTURA_PLANES] as string;
            if (!string.IsNullOrEmpty(coberturaSesion))
            {
                int idCobertura = Convert.ToInt32(coberturaSesion);
                lista = lista.FindAll(dto => dto.IdCobertura == idCobertura);
            }

            var textoSesion = Session[KEY_TEXTO_PLANES] as string;
            if (!string.IsNullOrEmpty(textoSesion))
            {
                lista = lista.FindAll(dto =>
                    (dto.Nombre != null && dto.Nombre.ToLower().Contains(textoSesion)) ||
                    (dto.Descripcion != null && dto.Descripcion.ToLower().Contains(textoSesion)) ||
                    (dto.NombreCobertura != null && dto.NombreCobertura.ToLower().Contains(textoSesion))
                );
            }

            gvPlanes.DataSource = lista;
            gvPlanes.DataBind();
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void ddlCoberturas_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }


        private void RestaurarFiltrosDesdeSession()
        {

            var estadoGuardado = Session[KEY_ESTADO_PLANES] as string;
            if (!string.IsNullOrEmpty(estadoGuardado))
                ddlEstado.SelectedValue = estadoGuardado;
            else
                ddlEstado.SelectedValue = "todos";


            var coberturaGuardada = Session[KEY_COBERTURA_PLANES] as string;
            if (!string.IsNullOrEmpty(coberturaGuardada))
            {
                var item = ddlCoberturas.Items.FindByValue(coberturaGuardada);
                if (item != null)
                    ddlCoberturas.SelectedValue = coberturaGuardada;
            }
            else
            {
                ddlCoberturas.SelectedValue = "todos";
            }

            var textoGuardado = Session[KEY_TEXTO_PLANES] as string;
            if (!string.IsNullOrEmpty(textoGuardado))
                txtBuscarPlanes.Text = textoGuardado;
        }


    }
}