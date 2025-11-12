using SGTO.Negocio.DTOs;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGTO.Negocio.Servicios.Exportacion;

namespace SGTO.UI.Webforms.Pages.Reportes
{
    public partial class Pacientes : System.Web.UI.Page
    {
        private readonly ReporteService _servicioReportes = new ReporteService();

        private const string KEY_REPORTE_FECHA_DESDE = "FiltroReporteFechaDesde";
        private const string KEY_REPORTE_FECHA_HASTA = "FiltroReporteFechaHasta";
        private const string KEY_REPORTE_COBERTURA = "FiltroReporteCobertura";
        private const string KEY_REPORTE_PLAN = "FiltroReportePlan";


        protected void Page_Load(object sender, EventArgs e)
        {

            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Reportes");
                master.EstablecerTituloSeccion(this.Page.Title);
            }

            if (!IsPostBack)
            {
                CargarCoberturas();
                CargarPlanes();

                // verificar si hay filtros
                txtFechaDesde.Text = Session[KEY_REPORTE_FECHA_DESDE] as string ?? string.Empty;
                txtFechaHasta.Text = Session[KEY_REPORTE_FECHA_HASTA] as string ?? string.Empty;
                ddlCobertura.SelectedValue = Session[KEY_REPORTE_COBERTURA] as string ?? string.Empty;
                ddlPlan.SelectedValue = Session[KEY_REPORTE_PLAN] as string ?? string.Empty;

                AplicarFiltros();
            }
        }


        private void CargarCoberturas()
        {
            try
            {
                ddlCobertura.Items.Clear();
                ddlCobertura.Items.Add(new System.Web.UI.WebControls.ListItem("Todas", ""));

                List<CoberturaDto> coberturas = _servicioReportes.ListarCoberturas("activo");
                Session["Coberturas"] = coberturas;

                foreach (CoberturaDto c in coberturas)
                {
                    ddlCobertura.Items.Add(new System.Web.UI.WebControls.ListItem(c.Nombre, c.IdCobertura.ToString()));
                }
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error al cargar coberturas",
                    "Ocurrió un error inesperado al intentar obtener las coberturas. " + ex.Message
                );
            }
        }

        private void CargarPlanes()
        {
            try
            {
                ddlPlan.Items.Clear();
                ddlPlan.Items.Add(new System.Web.UI.WebControls.ListItem("Todos", ""));

                List<PlanDto> planes = _servicioReportes.ListarPlanes("activo");
                Session["Planes"] = planes;

                foreach (PlanDto p in planes)
                {
                    ddlPlan.Items.Add(new System.Web.UI.WebControls.ListItem(p.Nombre, p.IdPlan.ToString()));
                }
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error al cargar planes",
                    "Ocurrió un error inesperado al intentar obtener los planes. " + ex.Message
                );
            }
        }

        private void ActualizarKpis(DateTime? fechaDesde, DateTime? fechaHasta)
        {
            try
            {
                var kpis = _servicioReportes.ObtenerKpisPacientes(fechaDesde, fechaHasta);
                lblTotalPacientes.Text = kpis.TotalPacientes.ToString();
                lblAtendidos.Text = kpis.Atendidos.ToString();
                lblNuevos.Text = kpis.NuevosEnPeriodo.ToString();
                lblConCobertura.Text = kpis.ConCobertura.ToString();
                lblParticulares.Text = kpis.Particulares.ToString();
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error al actualizar indicadores",
                    "Ocurrió un error al intentar actualizar los KPIs. " + ex.Message
                );
            }
        }



        private void AplicarFiltros()
        {
            try
            {
                DateTime? fechaDesde = string.IsNullOrWhiteSpace(txtFechaDesde.Text)
                    ? null
                    : (DateTime?)Convert.ToDateTime(txtFechaDesde.Text);

                DateTime? fechaHasta = string.IsNullOrWhiteSpace(txtFechaHasta.Text)
                    ? null
                    : (DateTime?)Convert.ToDateTime(txtFechaHasta.Text);

                int? idCobertura = string.IsNullOrEmpty(ddlCobertura.SelectedValue)
                    ? null
                    : (int?)Convert.ToInt32(ddlCobertura.SelectedValue);

                int? idPlan = string.IsNullOrEmpty(ddlPlan.SelectedValue)
                    ? null
                    : (int?)Convert.ToInt32(ddlPlan.SelectedValue);

                bool flowControl = VerificarCoherenciaFechas(fechaDesde, fechaHasta);
                if (!flowControl)
                {
                    return;
                }

                Session[KEY_REPORTE_FECHA_DESDE] = txtFechaDesde.Text;
                Session[KEY_REPORTE_FECHA_HASTA] = txtFechaHasta.Text;
                Session[KEY_REPORTE_COBERTURA] = ddlCobertura.SelectedValue;
                Session[KEY_REPORTE_PLAN] = ddlPlan.SelectedValue;

                var lista = _servicioReportes.ObtenerReportePacientesFiltrado(fechaDesde, fechaHasta, idCobertura, idPlan);
                gvPacientes.DataSource = lista;
                gvPacientes.DataBind();

                ActualizarKpis(fechaDesde, fechaHasta);
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error al aplicar filtros",
                    "Ocurrió un error inesperado al intentar aplicar los filtros del reporte. " + ex.Message
                );
            }
        }

        private bool VerificarCoherenciaFechas(DateTime? fechaDesde, DateTime? fechaHasta)
        {
            if (fechaDesde.HasValue && fechaHasta.HasValue && fechaHasta.Value < fechaDesde.Value)
            {

                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Fechas inválidas",
                    "La fecha 'Hasta' no puede ser anterior a la fecha 'Desde'. Verifique los valores ingresados.",
                    "Resultado",
                    null,
                    "abrirModalResultado"
                );

                return false;
            }

            return true;
        }

        protected void btnAplicarFiltros_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtFechaDesde.Text = string.Empty;
            txtFechaHasta.Text = string.Empty;
            ddlCobertura.SelectedIndex = 0;
            ddlPlan.SelectedIndex = 0;

            Session[KEY_REPORTE_FECHA_DESDE] = null;
            Session[KEY_REPORTE_FECHA_HASTA] = null;
            Session[KEY_REPORTE_COBERTURA] = null;
            Session[KEY_REPORTE_PLAN] = null;

            AplicarFiltros();
        }

        protected void btnExportarPdf_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime? fechaDesde = string.IsNullOrWhiteSpace(txtFechaDesde.Text)
                    ? null : (DateTime?)Convert.ToDateTime(txtFechaDesde.Text);

                DateTime? fechaHasta = string.IsNullOrWhiteSpace(txtFechaHasta.Text)
                    ? null : (DateTime?)Convert.ToDateTime(txtFechaHasta.Text);

                int? idCobertura = string.IsNullOrEmpty(ddlCobertura.SelectedValue)
                    ? null : (int?)Convert.ToInt32(ddlCobertura.SelectedValue);

                int? idPlan = string.IsNullOrEmpty(ddlPlan.SelectedValue)
                    ? null : (int?)Convert.ToInt32(ddlPlan.SelectedValue);

                var lista = _servicioReportes.ObtenerReportePacientesFiltrado(fechaDesde, fechaHasta, idCobertura, idPlan);

                byte[] pdfBytes = GeneradorPdf.GenerarReportePacientesPdf(lista);

                // esto permite enviar la respuesta al navegador, si indicamos el content type, es capaz de renderizar correctamente
                // el pdf
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "inline;filename=ReportePacientes.pdf");
                Response.OutputStream.Write(pdfBytes, 0, pdfBytes.Length);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error al exportar PDF",
                    "Ocurrió un error al generar el reporte en PDF. " + ex.Message
                );
            }
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime? fechaDesde = string.IsNullOrWhiteSpace(txtFechaDesde.Text)
                    ? null : (DateTime?)Convert.ToDateTime(txtFechaDesde.Text);

                DateTime? fechaHasta = string.IsNullOrWhiteSpace(txtFechaHasta.Text)
                    ? null : (DateTime?)Convert.ToDateTime(txtFechaHasta.Text);

                int? idCobertura = string.IsNullOrEmpty(ddlCobertura.SelectedValue)
                    ? null : (int?)Convert.ToInt32(ddlCobertura.SelectedValue);

                int? idPlan = string.IsNullOrEmpty(ddlPlan.SelectedValue)
                    ? null : (int?)Convert.ToInt32(ddlPlan.SelectedValue);

                var lista = _servicioReportes.ObtenerReportePacientesFiltrado(fechaDesde, fechaHasta, idCobertura, idPlan);

                byte[] csvBytes = GeneradorCsv.GenerarReportePacientesCsv(lista);

                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=ReportePacientes.csv");
                Response.BinaryWrite(csvBytes);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error al exportar CSV",
                    "Ocurrió un error al generar el reporte en CSV. " + ex.Message
                );
            }
        }

        protected void gvPacientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPacientes.PageIndex = e.NewPageIndex;
            AplicarFiltros();
        }

        protected void ddlCobertura_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlCobertura.SelectedValue))
            {
                CargarPlanes();
                return;
            }

            int idCobertura = Convert.ToInt32(ddlCobertura.SelectedValue);

            List<PlanDto> planes = Session["Planes"] as List<PlanDto>;
            if (planes == null) return;

            var planesFiltrados = planes.Where(p => p.IdCobertura == idCobertura).ToList();

            ddlPlan.Items.Clear();
            ddlPlan.Items.Add(new System.Web.UI.WebControls.ListItem("Todos", ""));
            foreach (var plan in planesFiltrados)
            {
                ddlPlan.Items.Add(new System.Web.UI.WebControls.ListItem(plan.Nombre, plan.IdPlan.ToString()));
            }
        }

        protected void ddlPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlPlan.SelectedValue))
                return;

            int idPlan = Convert.ToInt32(ddlPlan.SelectedValue);

            List<PlanDto> planes = Session["Planes"] as List<PlanDto>;
            if (planes == null) return;

            var planSeleccionado = planes.FirstOrDefault(p => p.IdPlan == idPlan);
            if (planSeleccionado != null)
            {
                ddlCobertura.SelectedValue = planSeleccionado.IdCobertura.ToString();
            }
        }
    }
}