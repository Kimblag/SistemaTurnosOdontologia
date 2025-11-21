using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.CoberturasPlanes.Planes
{
    public partial class Index : System.Web.UI.Page
    {
        private readonly PlanService _servicioPlanes = new PlanService();
        private readonly CoberturaService _servicioCobertura = new CoberturaService();
        private readonly TurnoService _servicioTurnos = new TurnoService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.ConfigurarBotonVolver(true, "~/Pages/CoberturasPlanes/Index.aspx");
                master.EstablecerOpcionMenuActiva("Coberturas");
                master.EstablecerTituloSeccion("Gestión de Planes");
                master.EstablecerSubtituloSeccion("Administración de Obras Sociales, Prepagas y sus respectivos planes de cobertura.");
            }

            if (!IsPostBack)
            {
                CargarCombos();
                CargarPlanesConFiltros();

                MensajeUiHelper.MostrarModal(this.Page);
            }
        }

        private void CargarCombos()
        {
            try
            {
                ddlCobertura.Items.Clear();
                ddlCobertura.Items.Add(new ListItem("Todas las coberturas", "-1"));

                var coberturas = _servicioCobertura.Listar();
                foreach (var cob in coberturas)
                {
                    ddlCobertura.Items.Add(new ListItem(cob.Nombre, cob.IdCobertura.ToString()));
                }
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "No se pudieron cargar las coberturas: " + ex.Message);
            }
        }

        private void CargarPlanesConFiltros()
        {
            List<PlanDto> todosLosPlanes = new List<PlanDto>();
            List<PlanDto> listaFiltrada = new List<PlanDto>();

            try
            {
                todosLosPlanes = _servicioPlanes.Listar();
            }
            catch (Exception ex)
            {
                gvPlanes.DataSource = null;
                gvPlanes.DataBind();
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "No se pudo cargar la lista de planes. " + ex.Message);
                return;
            }

            string textoBuscar = txtBuscarPlanes.Text.Trim().ToUpper();
            string idCoberturaSeleccionada = ddlCobertura.SelectedValue;
            string estadoSeleccionado = ddlEstado.SelectedValue;

            foreach (PlanDto plan in todosLosPlanes)
            {
                bool cumple = true;

                if (!string.IsNullOrEmpty(textoBuscar))
                {
                    string nombrePlan = plan.Nombre != null ? ValidadorCampos.NormalizarTexto(plan.Nombre) : "";

                    string nombreCobertura = plan.NombreCobertura != null ? ValidadorCampos.NormalizarTexto(plan.NombreCobertura) : "";

                    string textoNorm = ValidadorCampos.NormalizarTexto(textoBuscar);

                    if (!nombrePlan.Contains(textoNorm) && !nombreCobertura.Contains(textoNorm))
                    {
                        cumple = false;
                    }
                }

                if (cumple && idCoberturaSeleccionada != "-1")
                {
                    int idCob = int.Parse(idCoberturaSeleccionada);
                    if (plan.IdCobertura != idCob)
                    {
                        cumple = false;
                    }
                }

                if (cumple && !string.IsNullOrEmpty(estadoSeleccionado))
                {
                    bool esActivoDto = plan.Estado != null && plan.Estado.Trim().ToLower().StartsWith("act");

                    if (estadoSeleccionado == "Activo" && !esActivoDto) cumple = false;
                    if (estadoSeleccionado == "Inactivo" && esActivoDto) cumple = false;
                }

                if (cumple)
                {
                    listaFiltrada.Add(plan);
                }
            }

            gvPlanes.DataSource = listaFiltrada;
            gvPlanes.DataBind();
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            gvPlanes.PageIndex = 0;
            CargarPlanesConFiltros();
        }


        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscarPlanes.Text = string.Empty;
            ddlCobertura.SelectedIndex = 0; 
            ddlEstado.SelectedValue = "Activo";

            gvPlanes.PageIndex = 0;
            CargarPlanesConFiltros();
        }


        protected void btnNuevoPlan_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/CoberturasPlanes/NuevoPlan", false);
        }

        public void gvPlanes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPlanes.PageIndex = e.NewPageIndex;
            CargarPlanesConFiltros();
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

     

        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            string tipo = hdnTipoEliminar.Value;
            int id = Convert.ToInt32(hdnIdEliminar.Value);

            try
            {
                if (tipo == "cobertura")
                {
                    CoberturaService servicioCobertura = new CoberturaService();
                    servicioCobertura.DarDeBaja(id, _servicioTurnos);

                    MensajeUiHelper.SetearMensaje("Cobertura dada de baja", "La cobertura y sus planes fueron dados de baja correctamente.");
                }
                else if (tipo == "plan")
                {

                    PlanService servicioPlan = new PlanService();
                    servicioPlan.DarDeBaja(id, _servicioTurnos);

                    MensajeUiHelper.SetearMensaje("Plan dado de baja", "El plan fue dado de baja correctamente.");
                }
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearMensaje("Operación no permitida", ex.Message);
            }
            catch (Exception)
            {
                MensajeUiHelper.SetearMensaje("Error inesperado", "Ocurrió un error al intentar dar de baja el registro.");
            }

            Response.Redirect(Request.RawUrl, false);
        }

    }
}