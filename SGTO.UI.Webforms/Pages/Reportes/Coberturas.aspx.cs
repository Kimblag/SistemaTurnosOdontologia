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
                master.ConfigurarBotonVolver(true, "~/Pages/Reportes/Index.aspx");
                master.EstablecerOpcionMenuActiva("Reportes");
                master.EstablecerTituloSeccion("Reporte de Coberturas y planes");
                master.EstablecerSubtituloSeccion("Catálogo y detalle de coberturas y planes.");
            }

            if (!IsPostBack)
            {
                CargarFiltrosCobertura();
                RestaurarSesion();
                CargarDatos();
                ActualizarKpis(); 
            }
        }

        private void ActualizarKpis()
        {
            try
            {
                var kpis = _servicioReportes.ObtenerKpisCoberturas(null, null);

                lblTotalCoberturas.Text = kpis.TotalCoberturas.ToString();
                lblTotalPlanes.Text = kpis.TotalPlanes.ToString();
                lblMasUsada.Text = kpis.CoberturaMasUsada;

                lblTotalFacturado.Text = kpis.TotalFacturado.ToString("C0");
                lblTurnosOS.Text = kpis.TotalACobrarOS.ToString("C0");
                lblTotalCopagos.Text = kpis.TotalCopagos.ToString("C0");
                lblTurnosOS.ToolTip = "Monto total a reclamar a Obras Sociales";
            }
            catch { }
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
                    ddlCoberturaFiltro.Items.Add(new ListItem(item.Nombre, item.IdCobertura.ToString()));
                }
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "Error cargando coberturas: " + ex.Message);
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
                int? idCob = string.IsNullOrEmpty(ddlCoberturaFiltro.SelectedValue) ? null : (int?)Convert.ToInt32(ddlCoberturaFiltro.SelectedValue);

                if (mvReportes.ActiveViewIndex == 0)
                {
                    var datos = _servicioReportes.ObtenerReporteCoberturas(estado);

                    if (idCob.HasValue)
                    {
                        string nombre = ddlCoberturaFiltro.SelectedItem.Text;

                        List<ReporteCoberturasDto> filtrados = new List<ReporteCoberturasDto>();
                        foreach (var item in datos)
                        {
                            if (item.Cobertura == nombre)
                            {
                                filtrados.Add(item);
                            }
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
                    var datos = _servicioReportes.ObtenerReportePlanes(idCob, estado, orden);
                    gvPlanes.DataSource = datos;
                    gvPlanes.DataBind();
                }
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "Error al generar reporte: " + ex.Message);
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
                var item = ddlCoberturaFiltro.Items.FindByValue(Session[KEY_COB_ID].ToString());
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
            ActualizarKpis(); 
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
            ActualizarKpis();
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
            try
            {
                byte[] bytes = null;
                string filename = "Reporte.pdf";
                string estado = ddlEstado.SelectedValue;

                if (mvReportes.ActiveViewIndex == 0)
                {
                    // Exportar Coberturas
                    filename = "Reporte_Coberturas.pdf";
                    var datos = _servicioReportes.ObtenerReporteCoberturas(estado);

                    if (!string.IsNullOrEmpty(ddlCoberturaFiltro.SelectedValue))
                    {
                        string nombreCob = ddlCoberturaFiltro.SelectedItem.Text;
                        List<ReporteCoberturasDto> filtrados = new List<ReporteCoberturasDto>();
                        foreach (var d in datos)
                        {
                            if (d.Cobertura == nombreCob) filtrados.Add(d);
                        }
                        bytes = GeneradorPdf.GenerarReporteCoberturasPdf(filtrados);
                    }
                    else
                    {
                        bytes = GeneradorPdf.GenerarReporteCoberturasPdf(datos);
                    }
                }
                else
                {
                    filename = "Reporte_Planes.pdf";
                    string orden = ddlOrden.SelectedValue;
                    int? idCob = string.IsNullOrEmpty(ddlCoberturaFiltro.SelectedValue) ? null : (int?)Convert.ToInt32(ddlCoberturaFiltro.SelectedValue);

                    var datos = _servicioReportes.ObtenerReportePlanes(idCob, estado, orden);
                    bytes = GeneradorPdf.GenerarReportePlanesPdf(datos);
                }

                if (bytes != null)
                {
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", $"inline;filename={filename}");
                    Response.OutputStream.Write(bytes, 0, bytes.Length);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                if (!(ex is System.Threading.ThreadAbortException))
                    MensajeUiHelper.SetearYMostrar(this.Page, "Error PDF", ex.Message);
            }
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] bytes = null;
                string filename = "Reporte.csv";
                string estado = ddlEstado.SelectedValue;

                if (mvReportes.ActiveViewIndex == 0)
                {
                    filename = "Reporte_Coberturas.csv";
                    var datos = _servicioReportes.ObtenerReporteCoberturas(estado);

                    if (!string.IsNullOrEmpty(ddlCoberturaFiltro.SelectedValue))
                    {
                        string nombreCob = ddlCoberturaFiltro.SelectedItem.Text;
                        List<ReporteCoberturasDto> filtrados = new List<ReporteCoberturasDto>();
                        foreach (var d in datos)
                        {
                            if (d.Cobertura == nombreCob) filtrados.Add(d);
                        }
                        bytes = GeneradorCsv.GenerarReporteCoberturasCsv(filtrados);
                    }
                    else
                    {
                        bytes = GeneradorCsv.GenerarReporteCoberturasCsv(datos);
                    }
                }
                else
                {
                    filename = "Reporte_Planes.csv";
                    string orden = ddlOrden.SelectedValue;
                    int? idCob = string.IsNullOrEmpty(ddlCoberturaFiltro.SelectedValue) ? null : (int?)Convert.ToInt32(ddlCoberturaFiltro.SelectedValue);

                    var datos = _servicioReportes.ObtenerReportePlanes(idCob, estado, orden);
                    bytes = GeneradorCsv.GenerarReportePlanesCsv(datos);
                }

                if (bytes != null)
                {
                    Response.Clear();
                    Response.ContentType = "text/csv";
                    Response.AddHeader("content-disposition", $"attachment;filename={filename}");
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                if (!(ex is System.Threading.ThreadAbortException))
                    MensajeUiHelper.SetearYMostrar(this.Page, "Error CSV", ex.Message);
            }
        }
    }
}