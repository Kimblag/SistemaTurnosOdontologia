using SGTO.Comun.DTOs;
using SGTO.Negocio.DTOs.Seguridad;
using SGTO.Negocio.Seguridad;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Seguridad;
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
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();
        private readonly DashboardService _servicioDashboard = new DashboardService();


        protected int KpiTurnosDia = 0;
        protected int KpiPacientesAtendidos = 0;
        protected int KpiReprogramados = 0;
        protected int KpiCancelados = 0;
        protected string CategoriasCsv = "";
        protected string DataNuevos = "";
        protected string DataReprogramados = "";
        protected string DataCerrados = "";
        protected string DataCancelados = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.EstaLogueado())
            {
                Response.Redirect("~/Pages/Login/Index.aspx");
                return;
            }

            if (!_servicioAutorizacion.TienePermiso(SessionManager.Usuario, "INICIO", "VER"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

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
            UsuarioSesionDto usuarioActual = SessionManager.Usuario;
            // obtencion de kpis
            DashboardResumenDto resumen = _servicioDashboard.ObtenerResumenDiario(usuarioActual);
            KpiTurnosDia = resumen.TurnosDelDia;
            KpiPacientesAtendidos = resumen.PacientesAtendidos;
            KpiReprogramados = resumen.Reprogramados;
            KpiCancelados = resumen.Cancelados;

            List<DashboardActividadSemanalDto> actividad = _servicioDashboard.ObtenerActividadSemanal(usuarioActual);

            var sbCat = new StringBuilder();
            var sbNuevos = new StringBuilder();
            var sbRepro = new StringBuilder();
            var sbCerrados = new StringBuilder();
            var sbCancel = new StringBuilder();

            for (int i = 0; i < actividad.Count; i++)
            {
                if (i > 0)
                {
                    sbCat.Append(","); sbNuevos.Append(","); sbRepro.Append(",");
                    sbCerrados.Append(","); sbCancel.Append(",");
                }

                string etiqueta = $"{actividad[i].Dia} {actividad[i].Fecha.ToString("dd/MM")}";
                sbCat.Append("'").Append(etiqueta).Append("'");

                sbNuevos.Append(actividad[i].CantidadNuevos);
                sbRepro.Append(actividad[i].CantidadReprogramados);
                sbCerrados.Append(actividad[i].CantidadCerrados);
                sbCancel.Append(actividad[i].CantidadCancelados);
            }

            CategoriasCsv = sbCat.ToString();
            DataNuevos = sbNuevos.ToString();
            DataReprogramados = sbRepro.ToString();
            DataCerrados = sbCerrados.ToString();
            DataCancelados = sbCancel.ToString();
        }

    }
}