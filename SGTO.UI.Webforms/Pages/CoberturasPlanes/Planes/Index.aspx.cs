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

        private const string KEY_BUSQUEDA = "FiltroPlanBusqueda";
        private const string KEY_CAMPO = "FiltroPlanCampo";
        private const string KEY_CRITERIO = "FiltroPlanCriterio";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Coberturas");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Administración de Obras Sociales, Prepagas y sus respectivos planes de cobertura.");
            }

            if (!IsPostBack)
            {
                txtBuscarPlanes.Text = Session[KEY_BUSQUEDA] as string ?? string.Empty;

                string campo = Session[KEY_CAMPO] as string;
                if (!string.IsNullOrEmpty(campo))
                {
                    if (ddlCampo.Items.FindByValue(campo) != null)
                    {
                        ddlCampo.SelectedValue = campo;
                        CargarCriterios(campo);
                    }
                }

                string criterio = Session[KEY_CRITERIO] as string;
                if (!string.IsNullOrEmpty(criterio) && ddlCriterio.Items.FindByValue(criterio) != null)
                {
                    ddlCriterio.SelectedValue = criterio;
                    ddlCriterio.Enabled = true;
                }

                AplicarFiltros();

                MensajeUiHelper.MostrarModal(this.Page);
            }
        }


        private void CargarCriterios(string campo)
        {
            ddlCriterio.Items.Clear();
            ddlCriterio.Enabled = false;

            if (string.IsNullOrEmpty(campo))
            {
                ddlCriterio.Items.Add(new ListItem("Seleccione un criterio", ""));
                return;
            }

            campo = campo.ToLower();
            ddlCriterio.Items.Add(new ListItem("Seleccione un criterio", ""));
            ddlCriterio.Enabled = true;

            try
            {
                if (campo == "estado")
                {
                    ddlCriterio.Items.Add(new ListItem("Activo", "A"));
                    ddlCriterio.Items.Add(new ListItem("Inactivo", "I"));
                }
                else if (campo == "cobertura")
                {
                    var coberturas = _servicioCobertura.Listar();
                    foreach (var cob in coberturas)
                    {
                        ddlCriterio.Items.Add(new ListItem(cob.Nombre, cob.IdCobertura.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error cargando criterios: " + ex.Message);
                ddlCriterio.Items.Add(new ListItem("Error al cargar", ""));
            }

            if (ddlCriterio.Items.Count > 0)
                ddlCriterio.SelectedIndex = 0;
        }


        private void AplicarFiltros()
        {
            string textoBusqueda = txtBuscarPlanes.Text.Trim();
            string campo = ddlCampo.SelectedValue;
            string criterio = ddlCriterio.SelectedValue;

            Session[KEY_BUSQUEDA] = string.IsNullOrEmpty(textoBusqueda) ? null : textoBusqueda;
            Session[KEY_CAMPO] = string.IsNullOrEmpty(campo) ? null : campo;
            Session[KEY_CRITERIO] = string.IsNullOrEmpty(criterio) ? null : criterio;

            List<PlanDto> listaCompleta = new List<PlanDto>();

            try
            {
                listaCompleta = _servicioPlanes.Listar();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error listando planes: " + ex.Message);
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "No se pudo cargar la lista.", "Cerrar", null, "abrirModalResultado");
                return;
            }

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                string texto = ValidadorCampos.NormalizarTexto(textoBusqueda);
                List<PlanDto> filtrada = new List<PlanDto>();

                foreach (var p in listaCompleta)
                {
                    string nombreNorm = ValidadorCampos.NormalizarTexto(p.Nombre);
                    string cobNorm = ValidadorCampos.NormalizarTexto(p.NombreCobertura);

                    if ((!string.IsNullOrEmpty(nombreNorm) && nombreNorm.Contains(texto)) ||
                        (!string.IsNullOrEmpty(cobNorm) && cobNorm.Contains(texto)))
                    {
                        filtrada.Add(p);
                    }
                }
                listaCompleta = filtrada;
            }

            if (!string.IsNullOrEmpty(campo) && !string.IsNullOrEmpty(criterio))
            {
                List<PlanDto> filtrada = new List<PlanDto>();

                if (campo == "Estado")
                {
                    foreach (var p in listaCompleta)
                    {
                        string estadoLetra = p.Estado.ToLower().StartsWith("act") ? "A" : "I";
                        if (estadoLetra == criterio)
                        {
                            filtrada.Add(p);
                        }
                    }
                }
                else if (campo == "Cobertura")
                {
                    if (int.TryParse(criterio, out int idCob))
                    {
                        foreach (var p in listaCompleta)
                        {
                            if (p.IdCobertura == idCob)
                            {
                                filtrada.Add(p);
                            }
                        }
                    }
                }
                listaCompleta = filtrada;
            }

            gvPlanes.DataSource = listaCompleta;
            gvPlanes.DataBind();
        }

        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string campo = ddlCampo.SelectedValue;
            Session[KEY_CAMPO] = string.IsNullOrEmpty(campo) ? null : campo;
            CargarCriterios(campo);
            Session[KEY_CRITERIO] = null;
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Session[KEY_BUSQUEDA] = null;
            Session[KEY_CAMPO] = null;
            Session[KEY_CRITERIO] = null;

            txtBuscarPlanes.Text = string.Empty;
            ddlCampo.SelectedIndex = 0;
            ddlCriterio.Items.Clear();
            ddlCriterio.Items.Add(new ListItem("Seleccione un criterio", ""));
            ddlCriterio.Enabled = false;

            AplicarFiltros();
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

       

        protected void btnNuevoPlan_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/CoberturasPlanes/NuevoPlan", false);
        }



        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void ddlCoberturas_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }


        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            string tipo = hdnTipoEliminar.Value;
            int id = Convert.ToInt32(hdnIdEliminar.Value);

            try
            {
                TurnoService servicioTurno = new TurnoService();
                if (tipo == "cobertura")
                {
                    CoberturaService servicioCobertura = new CoberturaService();
                    servicioCobertura.DarDeBaja(id, servicioTurno);

                    MensajeUiHelper.SetearMensaje("Cobertura dada de baja", "La cobertura y sus planes fueron dados de baja correctamente.");
                }
                else if (tipo == "plan")
                {

                    PlanService servicioPlan = new PlanService();
                    servicioPlan.DarDeBaja(id, servicioTurno);

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