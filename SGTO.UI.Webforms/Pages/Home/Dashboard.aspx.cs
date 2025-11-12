using SGTO.Comun.DTOs;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Home
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private readonly DashboardService _servicioDashboard = new DashboardService();


        protected int KpiTurnosDia = 0;
        protected int KpiPacientesAtendidos = 0;
        protected int KpiReprogramados = 0;
        protected int KpiCancelados = 0;
        protected string CategoriasCsv = "";
        protected string ValoresCsv = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva(this.Page.Title);
                master.EstablecerTituloSeccion(this.Page.Title);
            }

            if (!IsPostBack)
            {
                CargarDatosDashboard();
            }
        }

        private void CargarDatosDashboard()
        {
            // obtencion de kpis
            DashboardResumenDto resumen = _servicioDashboard.ObtenerResumenDiario();
            KpiTurnosDia = resumen.TurnosDelDia;
            KpiPacientesAtendidos = resumen.PacientesAtendidos;
            KpiReprogramados = resumen.Reprogramados;
            KpiCancelados = resumen.Cancelados;

            List<DashboardActividadSemanalDto> actividad = _servicioDashboard.ObtenerActividadSemanal();

            var sbCategorias = new StringBuilder();
            var sbValores = new StringBuilder();

            for (int i = 0; i < actividad.Count; i++)
            {
                if (i > 0)
                {
                    sbCategorias.Append(",");
                    sbValores.Append(",");
                }

                //  por ejemplo > Lunes, martes...
                sbCategorias.Append("'").Append(actividad[i].Dia.Replace("'", "\\'")).Append("'");
                sbValores.Append(actividad[i].Cantidad);
            }
            CategoriasCsv = sbCategorias.ToString();
            ValoresCsv = sbValores.ToString();
        }


    }
}