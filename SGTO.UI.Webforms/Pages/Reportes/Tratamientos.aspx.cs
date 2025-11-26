using SGTO.Negocio.DTOs;
using SGTO.Comun.DTOs;
using SGTO.Negocio.Servicios;
using SGTO.Negocio.Servicios.Exportacion;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Utils;
using SGTO.Negocio.Seguridad;
using SGTO.UI.Webforms.Seguridad;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Reportes
{
    public partial class Tratamientos : System.Web.UI.Page
    {
        private readonly ReporteService _servicioReportes = new ReporteService();
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();

        private const string KEY_REP_TRAT_ESP = "FiltroRepTratEsp";
        private const string KEY_REP_TRAT_ESTADO = "FiltroRepTratEstado";

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
                master.EstablecerTituloSeccion("Reporte de Tratamientos");
                master.EstablecerSubtituloSeccion("Métricas de uso y rendimiento económico de tratamientos.");
            }

            if (!IsPostBack)
            {
                CargarEspecialidades();

                ddlEspecialidad.SelectedValue = Session[KEY_REP_TRAT_ESP] as string ?? string.Empty;

                string estadoSession = Session[KEY_REP_TRAT_ESTADO] as string;

                if (ddlEstado.Items.FindByValue(estadoSession) != null)
                {
                    ddlEstado.SelectedValue = estadoSession;
                }
                else
                {
                    ddlEstado.SelectedValue = "A"; 
                }

                AplicarFiltros();
            }
        }

        private void CargarEspecialidades()
        {
            try
            {
                ddlEspecialidad.Items.Clear();
                ddlEspecialidad.Items.Add(new ListItem("Todas", ""));

                var lista = _servicioReportes.ListarEspecialidades("activo");
                foreach (var item in lista)
                {
                    ddlEspecialidad.Items.Add(new ListItem(item.Nombre, item.IdEspecialidad.ToString()));
                }
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "Error al cargar especialidades: " + ex.Message);
            }
        }

        private void AplicarFiltros()
        {
            try
            {
                int? idEsp = string.IsNullOrEmpty(ddlEspecialidad.SelectedValue) ? null : (int?)Convert.ToInt32(ddlEspecialidad.SelectedValue);
                string estado = ddlEstado.SelectedValue;

                Session[KEY_REP_TRAT_ESP] = ddlEspecialidad.SelectedValue;
                Session[KEY_REP_TRAT_ESTADO] = estado;

                var lista = _servicioReportes.ObtenerReporteTratamientosFiltrado(idEsp, estado);
                gvTratamientos.DataSource = lista;
                gvTratamientos.DataBind();

                ActualizarKpis(null, null);
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "Error al generar reporte: " + ex.Message);
            }
        }

        private void ActualizarKpis(DateTime? fDesde, DateTime? fHasta)
        {
            try
            {
                var kpis = _servicioReportes.ObtenerKpisTratamientos(fDesde, fHasta);

                lblTotalCatalogo.Text = kpis.TotalEnCatalogo.ToString();
                lblTotalRealizados.Text = kpis.TotalRealizados.ToString();
                lblIngresosBrutos.Text = kpis.TotalFacturado.ToString("C0");
                lblIngresosOS.Text = kpis.TotalCobradoObraSocial.ToString("C0");
                lblIngresosPac.Text = kpis.TotalCobradoPaciente.ToString("C0");
            }
            catch { }
        }

        protected void btnAplicarFiltros_Click(object sender, EventArgs e) => AplicarFiltros();

        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            ddlEspecialidad.SelectedIndex = 0;
            ddlEstado.SelectedValue = "A"; 

            Session[KEY_REP_TRAT_ESP] = null;
            Session[KEY_REP_TRAT_ESTADO] = null;

            AplicarFiltros();
        }

        protected void gvTratamientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTratamientos.PageIndex = e.NewPageIndex;
            AplicarFiltros();
        }

        protected void btnExportarPdf_Click(object sender, EventArgs e)
        {
            try
            {
                int? idEsp = string.IsNullOrEmpty(ddlEspecialidad.SelectedValue) ? null : (int?)Convert.ToInt32(ddlEspecialidad.SelectedValue);
                string estado = ddlEstado.SelectedValue;

                var lista = _servicioReportes.ObtenerReporteTratamientosFiltrado(idEsp, estado);

                byte[] bytes = GeneradorPdf.GenerarReporteTratamientosPdf(lista);

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "inline;filename=ReporteTratamientos.pdf");
                Response.OutputStream.Write(bytes, 0, bytes.Length);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                if (!(ex is System.Threading.ThreadAbortException))
                {
                    MensajeUiHelper.SetearYMostrar(this.Page, "Error al exportar PDF", "Ocurrió un problema: " + ex.Message);
                }
            }
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                int? idEsp = string.IsNullOrEmpty(ddlEspecialidad.SelectedValue) ? null : (int?)Convert.ToInt32(ddlEspecialidad.SelectedValue);
                string estado = ddlEstado.SelectedValue;

                var lista = _servicioReportes.ObtenerReporteTratamientosFiltrado(idEsp, estado);

                byte[] bytes = GeneradorCsv.GenerarReporteTratamientosCsv(lista);

                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=ReporteTratamientos.csv");
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                if (!(ex is System.Threading.ThreadAbortException))
                {
                    MensajeUiHelper.SetearYMostrar(this.Page, "Error al exportar Excel", "Ocurrió un problema: " + ex.Message);
                }
            }
        }
    }
}