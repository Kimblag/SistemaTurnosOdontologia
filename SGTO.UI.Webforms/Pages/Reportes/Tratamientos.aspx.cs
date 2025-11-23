using SGTO.Negocio.DTOs; 
using SGTO.Comun.DTOs;  
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
    public partial class Tratamientos : System.Web.UI.Page
    {
        private readonly ReporteService _servicioReportes = new ReporteService();

        private const string KEY_REP_TRAT_DESDE = "FiltroRepTratDesde";
        private const string KEY_REP_TRAT_HASTA = "FiltroRepTratHasta";
        private const string KEY_REP_TRAT_ESP = "FiltroRepTratEsp";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Reportes");
                master.EstablecerTituloSeccion("Reporte de Tratamientos");
                master.EstablecerSubtituloSeccion("Métricas de uso y rendimiento económico de tratamientos.");
            }

            if (!IsPostBack)
            {
                CargarEspecialidades();

                txtFechaDesde.Text = Session[KEY_REP_TRAT_DESDE] as string ?? string.Empty;
                txtFechaHasta.Text = Session[KEY_REP_TRAT_HASTA] as string ?? string.Empty;
                ddlEspecialidad.SelectedValue = Session[KEY_REP_TRAT_ESP] as string ?? string.Empty;

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
                DateTime? fDesde = string.IsNullOrWhiteSpace(txtFechaDesde.Text) ? null : (DateTime?)Convert.ToDateTime(txtFechaDesde.Text);
                DateTime? fHasta = string.IsNullOrWhiteSpace(txtFechaHasta.Text) ? null : (DateTime?)Convert.ToDateTime(txtFechaHasta.Text);
                int? idEsp = string.IsNullOrEmpty(ddlEspecialidad.SelectedValue) ? null : (int?)Convert.ToInt32(ddlEspecialidad.SelectedValue);

                if (fDesde.HasValue && fHasta.HasValue && fHasta < fDesde)
                {
                    MensajeUiHelper.SetearYMostrar(this.Page, "Fechas inválidas", "La fecha hasta no puede ser menor a la fecha desde.", "Error", null, "abrirModalResultado");
                    return;
                }

                Session[KEY_REP_TRAT_DESDE] = txtFechaDesde.Text;
                Session[KEY_REP_TRAT_HASTA] = txtFechaHasta.Text;
                Session[KEY_REP_TRAT_ESP] = ddlEspecialidad.SelectedValue;

                var lista = _servicioReportes.ObtenerReporteTratamientosFiltrado(fDesde, fHasta, idEsp);
                gvTratamientos.DataSource = lista;
                gvTratamientos.DataBind();

                ActualizarKpis(fDesde, fHasta);
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
                lblIngresosEstimados.Text = kpis.IngresoTotalEstimado.ToString("C"); // Formato Moneda
            }
            catch { }
        }

        protected void btnAplicarFiltros_Click(object sender, EventArgs e) => AplicarFiltros();

        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtFechaDesde.Text = "";
            txtFechaHasta.Text = "";
            ddlEspecialidad.SelectedIndex = 0;
            Session[KEY_REP_TRAT_DESDE] = null;
            Session[KEY_REP_TRAT_HASTA] = null;
            Session[KEY_REP_TRAT_ESP] = null;
            AplicarFiltros();
        }

        protected void gvTratamientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTratamientos.PageIndex = e.NewPageIndex;
            AplicarFiltros();
        }



        protected void btnExportarPdf_Click(object sender, EventArgs e)
        {
    
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
   
        }
    }
}