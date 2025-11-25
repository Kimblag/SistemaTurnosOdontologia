using SGTO.Comun.DTOs;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Seguridad;
using SGTO.Negocio.Servicios;
using SGTO.Negocio.Servicios.Exportacion;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Seguridad;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Reportes
{
    public partial class Coberturas : System.Web.UI.Page
    {
        private readonly ReporteService _servicioReportes = new ReporteService();
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();

        private const string KEY_COB_ESTADO = "FiltroCobEstado";
        private const string KEY_COB_ORDEN = "FiltroCobOrden";
        private const string KEY_COB_ID = "FiltroCobId";
        private const string KEY_TAB_INDEX = "FiltroCobTab";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.EstaLogueado())
            {
                Response.Redirect("~/Pages/Login/Index.aspx");
                return;
            }

            if (!_servicioAutorizacion.TienePermiso(SessionManager.Usuario, "REPORTES", "VER"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Reportes");
                master.EstablecerTituloSeccion("Reporte de Coberturas");
                master.EstablecerSubtituloSeccion("Catálogo y detalle de planes.");
            }

            if (!IsPostBack)
            {
                CargarFiltrosCobertura();
                RestaurarSesion();
                CargarDatos();
            }
        }

        private void CargarFiltrosCobertura()
        {
            try
            {
                ddlCoberturaFiltro.Items.Clear();
                ddlCoberturaFiltro.Items.Add(new ListItem("Todas", ""));

                var lista = _servicioReportes.ListarCoberturas("activo");

                foreach (var item in lista)
                {
                    ddlCoberturaFiltro.Items.Add(
                        new ListItem(item.Nombre, item.IdCobertura.ToString()));
                }
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(
                    this.Page, "Error",
                    "Error cargando coberturas: " + ex.Message);
            }
        }

        private void CargarDatos()
        {
            try
            {
                Session[KEY_COB_ESTADO] = ddlEstado.SelectedValue;
                Session[KEY_COB_ORDEN] = ddlOrden.SelectedValue;
                Session[KEY_COB_ID] = ddlCoberturaFiltro.SelectedValue;
                Session[KEY_TAB_INDEX] = mvReportes.ActiveViewIndex;

                string estado = ddlEstado.SelectedValue;
                string orden = ddlOrden.SelectedValue;
                int? idCob = string.IsNullOrEmpty(ddlCoberturaFiltro.SelectedValue)
                    ? (int?)null
                    : Convert.ToInt32(ddlCoberturaFiltro.SelectedValue);

                if (mvReportes.ActiveViewIndex == 0)
                {
                    // TAB 1: COBERTURAS
                    var datos = _servicioReportes.ObtenerReporteCoberturas(estado);

                    if (idCob.HasValue)
                    {
                        string nombre = ddlCoberturaFiltro.SelectedItem.Text;

                        var filtrados = new List<ReporteCoberturasDto>();
                        foreach (var item in datos)
                        {
                            if (item.Cobertura == nombre)
                                filtrados.Add(item);
                        }

                        gvCoberturas.DataSource = filtrados;
                    }
                    else
                    {
                        gvCoberturas.DataSource = datos;
                    }

                    gvCoberturas.DataBind();
                }
                else
                {
                    // TAB 2: PLANES
                    var datos = _servicioReportes.ObtenerReportePlanes(idCob, estado, orden);
                    gvPlanes.DataSource = datos;
                    gvPlanes.DataBind();
                }
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(
                    this.Page, "Error",
                    "Error al generar reporte: " + ex.Message);
            }
        }

        private void RestaurarSesion()
        {
            if (Session[KEY_COB_ESTADO] != null)
                ddlEstado.SelectedValue = Session[KEY_COB_ESTADO].ToString();
            else
                ddlEstado.SelectedValue = "A";

            if (Session[KEY_COB_ORDEN] != null)
                ddlOrden.SelectedValue = Session[KEY_COB_ORDEN].ToString();

            if (Session[KEY_COB_ID] != null)
            {
                var valor = Session[KEY_COB_ID].ToString();
                var item = ddlCoberturaFiltro.Items.FindByValue(valor);
                if (item != null)
                    ddlCoberturaFiltro.SelectedValue = item.Value;
            }

            if (Session[KEY_TAB_INDEX] != null)
            {
                int index = (int)Session[KEY_TAB_INDEX];
                mvReportes.ActiveViewIndex = index;
                ActualizarTabs(index);
            }
        }

        protected void btnEjecutar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            ddlEstado.SelectedValue = "A";
            ddlOrden.SelectedIndex = 0;
            ddlCoberturaFiltro.SelectedIndex = 0;

            Session.Remove(KEY_COB_ESTADO);
            Session.Remove(KEY_COB_ORDEN);
            Session.Remove(KEY_COB_ID);

            CargarDatos();
        }

        protected void tabCoberturas_Click(object sender, EventArgs e)
        {
            mvReportes.ActiveViewIndex = 0;
            ActualizarTabs(0);
            CargarDatos();
        }

        protected void tabPlanes_Click(object sender, EventArgs e)
        {
            mvReportes.ActiveViewIndex = 1;
            ActualizarTabs(1);
            CargarDatos();
        }

        private void ActualizarTabs(int index)
        {
            tabCoberturas.CssClass = index == 0 ? "nav-link active" : "nav-link";
            tabPlanes.CssClass = index == 1 ? "nav-link active" : "nav-link";
        }

        protected void btnExportarPdf_Click(object sender, EventArgs e)
        {

        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
   
        }
    }
}
