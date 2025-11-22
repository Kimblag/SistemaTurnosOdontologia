using SGTO.Negocio.DTOs;
using SGTO.Negocio.Servicios;
using SGTO.Negocio.Servicios.Exportacion;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Reportes
{
    public partial class Medicos : System.Web.UI.Page
    {
        private readonly ReporteService _servicioReportes = new ReporteService();

        private const string KEY_REP_MED_DESDE = "FiltroRepMedDesde";
        private const string KEY_REP_MED_HASTA = "FiltroRepMedHasta";
        private const string KEY_REP_MED_ESP = "FiltroRepMedEsp";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.ConfigurarBotonVolver(true, "~/Pages/Reportes/Index.aspx");
                master.EstablecerOpcionMenuActiva("Reportes");
                master.EstablecerTituloSeccion("Reporte de Médicos");
                master.EstablecerSubtituloSeccion("Métricas de desempeño y actividad del personal médico.");
            }

            if (!IsPostBack)
            {
                CargarEspecialidades();

                txtFechaDesde.Text = Session[KEY_REP_MED_DESDE] as string ?? string.Empty;
                txtFechaHasta.Text = Session[KEY_REP_MED_HASTA] as string ?? string.Empty;
                ddlEspecialidad.SelectedValue = Session[KEY_REP_MED_ESP] as string ?? string.Empty;

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
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "No se pudieron cargar especialidades. " + ex.Message);
            }
        }

        private void AplicarFiltros()
        {
            try
            {
                DateTime? fDesde = string.IsNullOrWhiteSpace(txtFechaDesde.Text) ? null : (DateTime?)Convert.ToDateTime(txtFechaDesde.Text);
                DateTime? fHasta = string.IsNullOrWhiteSpace(txtFechaHasta.Text) ? null : (DateTime?)Convert.ToDateTime(txtFechaHasta.Text);
                int? idEsp = string.IsNullOrEmpty(ddlEspecialidad.SelectedValue) ? null : (int?)Convert.ToInt32(ddlEspecialidad.SelectedValue);

                if (fDesde.HasValue && fHasta.HasValue && fHasta < fDesde)
                {
                    MensajeUiHelper.SetearYMostrar(this.Page, "Fechas inválidas", "La fecha hasta no puede ser menor a la fecha desde.", "Error", null, "abrirModalResultado");
                    return;
                }

                Session[KEY_REP_MED_DESDE] = txtFechaDesde.Text;
                Session[KEY_REP_MED_HASTA] = txtFechaHasta.Text;
                Session[KEY_REP_MED_ESP] = ddlEspecialidad.SelectedValue;

                var lista = _servicioReportes.ObtenerReporteMedicosFiltrado(fDesde, fHasta, idEsp);
                gvMedicos.DataSource = lista;
                gvMedicos.DataBind();

                ActualizarKpis(fDesde, fHasta);
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "Error al generar el reporte. " + ex.Message);
            }
        }

        private void ActualizarKpis(DateTime? fDesde, DateTime? fHasta)
        {
            try
            {
                var kpis = _servicioReportes.ObtenerKpisMedicos(fDesde, fHasta);
                lblTotalMedicos.Text = kpis.TotalMedicos.ToString();
                lblActivos.Text = kpis.Activos.ToString();
                lblTotalTurnos.Text = kpis.TotalTurnosRealizados.ToString();
                lblEspecialidades.Text = kpis.EspecialidadesCubiertas.ToString();
            }
            catch { }
        }

        protected void btnAplicarFiltros_Click(object sender, EventArgs e) => AplicarFiltros();

        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtFechaDesde.Text = "";
            txtFechaHasta.Text = "";
            ddlEspecialidad.SelectedIndex = 0;
            Session[KEY_REP_MED_DESDE] = null;
            Session[KEY_REP_MED_HASTA] = null;
            Session[KEY_REP_MED_ESP] = null;
            AplicarFiltros();
        }

        protected void gvMedicos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMedicos.PageIndex = e.NewPageIndex;
            AplicarFiltros();
        }

        protected void btnExportarPdf_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime? fDesde = string.IsNullOrWhiteSpace(txtFechaDesde.Text) ? null : (DateTime?)Convert.ToDateTime(txtFechaDesde.Text);
                DateTime? fHasta = string.IsNullOrWhiteSpace(txtFechaHasta.Text) ? null : (DateTime?)Convert.ToDateTime(txtFechaHasta.Text);
                int? idEsp = string.IsNullOrEmpty(ddlEspecialidad.SelectedValue) ? null : (int?)Convert.ToInt32(ddlEspecialidad.SelectedValue);

                var lista = _servicioReportes.ObtenerReporteMedicosFiltrado(fDesde, fHasta, idEsp);

                byte[] pdfBytes = GeneradorPdf.GenerarReporteMedicosPdf(lista);

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "inline;filename=ReporteMedicos.pdf");
                Response.OutputStream.Write(pdfBytes, 0, pdfBytes.Length);
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
            
        }
    }
}