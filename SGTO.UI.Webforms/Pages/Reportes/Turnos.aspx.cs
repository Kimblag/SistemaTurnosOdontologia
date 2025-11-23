using SGTO.Comun.DTOs;
using SGTO.Dominio.Enums;
using SGTO.Negocio.Seguridad;
using SGTO.Negocio.Servicios;
using SGTO.Negocio.Servicios.Exportacion;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Seguridad;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Reportes
{
    public partial class Turnos : System.Web.UI.Page
    {
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();
        private readonly ReporteService _servicioReportes = new ReporteService();
        private readonly MedicoService _servicioMedicos = new MedicoService();

        private const string KEY_FILTRO_DESDE = "RepTurnos_Desde";
        private const string KEY_FILTRO_HASTA = "RepTurnos_Hasta";
        private const string KEY_FILTRO_ESTADO = "RepTurnos_Estado";
        private const string KEY_FILTRO_MEDICO = "RepTurnos_Medico";
        private const string KEY_FILTRO_ESPECIALIDAD = "RepTurnos_Esp";

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
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Consulte estadísticas operativas y exporte información histórica del sistema.");
            }

            if (!IsPostBack)
            {
                CargarListasDesplegables();
                RecuperarFiltrosDeSesion();
                EjecutarReporte();
            }
        }

        private void CargarListasDesplegables()
        {
            try
            {
                var medicos = _servicioMedicos.Listar("Activos");
                ddlMedico.Items.Clear();
                ddlMedico.Items.Add(new ListItem("Todos", ""));
                foreach (var m in medicos)
                {
                    ddlMedico.Items.Add(new ListItem(m.NombreCompleto, m.IdMedico.ToString()));
                }

                var especialidades = _servicioReportes.ListarEspecialidades("A");
                ddlEspecialidad.Items.Clear();
                ddlEspecialidad.Items.Add(new ListItem("Todas", ""));
                foreach (var esp in especialidades)
                {
                    ddlEspecialidad.Items.Add(new ListItem(esp.Nombre, esp.IdEspecialidad.ToString()));
                }
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this, "Error", "No se pudieron cargar los filtros: " + ex.Message, "Cerrar", null, "abrirModalResultado");
            }
        }

        private void EjecutarReporte()
        {
            try
            {
                DateTime? desde = string.IsNullOrEmpty(txtFechaDesde.Text) ? (DateTime?)null : DateTime.Parse(txtFechaDesde.Text);
                DateTime? hasta = string.IsNullOrEmpty(txtFechaHasta.Text) ? (DateTime?)null : DateTime.Parse(txtFechaHasta.Text);
                string estado = ddlEstado.SelectedValue;
                int? idMedico = string.IsNullOrEmpty(ddlMedico.SelectedValue) ? (int?)null : int.Parse(ddlMedico.SelectedValue);
                int? idEsp = string.IsNullOrEmpty(ddlEspecialidad.SelectedValue) ? (int?)null : int.Parse(ddlEspecialidad.SelectedValue);

                var lista = _servicioReportes.ObtenerReporteTurnosFiltrado(desde, hasta, estado, idMedico, idEsp);
                gvTurnos.DataSource = lista;
                gvTurnos.DataBind();

                var kpis = _servicioReportes.ObtenerKpisTurnos(desde, hasta);

                lblTotal.Text = kpis.TotalTurnos.ToString();
                lblAtendidos.Text = kpis.Atendidos.ToString();
                lblCancelados.Text = kpis.Cancelados.ToString();
                lblAusentes.Text = kpis.Ausentes.ToString();
                lblPendientes.Text = kpis.Pendientes.ToString();

                Session[KEY_FILTRO_DESDE] = txtFechaDesde.Text;
                Session[KEY_FILTRO_HASTA] = txtFechaHasta.Text;
                Session[KEY_FILTRO_ESTADO] = estado;
                Session[KEY_FILTRO_MEDICO] = ddlMedico.SelectedValue;
                Session[KEY_FILTRO_ESPECIALIDAD] = ddlEspecialidad.SelectedValue;

            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this, "Error", "Error al generar reporte: " + ex.Message, "Cerrar", null, "abrirModalResultado");
            }
        }

        private void RecuperarFiltrosDeSesion()
        {
            if (Session[KEY_FILTRO_DESDE] != null) txtFechaDesde.Text = Session[KEY_FILTRO_DESDE].ToString();
            if (Session[KEY_FILTRO_HASTA] != null) txtFechaHasta.Text = Session[KEY_FILTRO_HASTA].ToString();
            if (Session[KEY_FILTRO_ESTADO] != null) ddlEstado.SelectedValue = Session[KEY_FILTRO_ESTADO].ToString();
            if (Session[KEY_FILTRO_MEDICO] != null) ddlMedico.SelectedValue = Session[KEY_FILTRO_MEDICO].ToString();
            if (Session[KEY_FILTRO_ESPECIALIDAD] != null) ddlEspecialidad.SelectedValue = Session[KEY_FILTRO_ESPECIALIDAD].ToString();
        }

        protected void btnAplicarFiltros_Click(object sender, EventArgs e)
        {
            gvTurnos.PageIndex = 0;
            EjecutarReporte();
        }

        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtFechaDesde.Text = "";
            txtFechaHasta.Text = "";
            ddlEstado.SelectedIndex = 0;
            ddlMedico.SelectedIndex = 0;
            ddlEspecialidad.SelectedIndex = 0;

            Session[KEY_FILTRO_DESDE] = null;
            Session[KEY_FILTRO_HASTA] = null;
            Session[KEY_FILTRO_ESTADO] = null;
            Session[KEY_FILTRO_MEDICO] = null;
            Session[KEY_FILTRO_ESPECIALIDAD] = null;

            EjecutarReporte();
        }

        protected void gvTurnos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTurnos.PageIndex = e.NewPageIndex;
            EjecutarReporte();
        }

        protected void btnExportarPdf_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime? fechaDesde = string.IsNullOrWhiteSpace(txtFechaDesde.Text)
                    ? null : (DateTime?)Convert.ToDateTime(txtFechaDesde.Text);

                DateTime? fechaHasta = string.IsNullOrWhiteSpace(txtFechaHasta.Text)
                    ? null : (DateTime?)Convert.ToDateTime(txtFechaHasta.Text);

                int? idMedico = string.IsNullOrEmpty(ddlMedico.SelectedValue)
                    ? null : (int?)Convert.ToInt32(ddlMedico.SelectedValue);

                int? idEspecialidad = string.IsNullOrEmpty(ddlEspecialidad.SelectedValue)
                    ? null : (int?)Convert.ToInt32(ddlEspecialidad.SelectedValue);

                string estadoTurno = ddlEstado.SelectedValue ?? string.Empty;


                var lista = _servicioReportes.ObtenerReporteTurnosFiltrado(fechaDesde, fechaHasta, estadoTurno, idMedico, idEspecialidad);

                byte[] pdfBytes = GeneradorPdf.GenerarReporteTurnosPdf(lista);

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "inline;filename=ReportePacientes.pdf");
                Response.OutputStream.Write(pdfBytes, 0, pdfBytes.Length);
                Response.Flush();
                Response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
                // agregué esta excepcion porque el response.END hace que se lance una excepción al terminar bruscamente la solicitud
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
                DateTime? desde = string.IsNullOrEmpty(txtFechaDesde.Text) ? (DateTime?)null : DateTime.Parse(txtFechaDesde.Text);
                DateTime? hasta = string.IsNullOrEmpty(txtFechaHasta.Text) ? (DateTime?)null : DateTime.Parse(txtFechaHasta.Text);
                string estado = ddlEstado.SelectedValue;
                int? idMedico = string.IsNullOrEmpty(ddlMedico.SelectedValue) ? (int?)null : int.Parse(ddlMedico.SelectedValue);
                int? idEsp = string.IsNullOrEmpty(ddlEspecialidad.SelectedValue) ? (int?)null : int.Parse(ddlEspecialidad.SelectedValue);

                var lista = _servicioReportes.ObtenerReporteTurnosFiltrado(desde, hasta, estado, idMedico, idEsp);

                byte[] csvBytes = GeneradorCsv.GenerarReporteTurnosCsv(lista);

                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", $"attachment;filename=ReporteTurnos_{DateTime.Now:yyyyMMdd}.csv");
                Response.BinaryWrite(csvBytes);
                Response.Flush();
                Response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this, "Error al exportar", "No se pudo generar el CSV: " + ex.Message, "Cerrar", null, "abrirModalResultado");
            }
        }


        protected void gvTurnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var turno = (ReporteTurnosDto)e.Row.DataItem;

                string estadoTurno = turno.Estado != null ? turno.Estado.ToLower() : "";

                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");

                if (lblEstado != null)
                {
                    lblEstado.Attributes["class"] = TurnoUiHelper.ObtenerCssEstadoTurnoBadge(estadoTurno);
                    lblEstado.InnerText = TurnoUiHelper.ObtenerTextoEstado(estadoTurno);
                }
            }
        }

    }
}