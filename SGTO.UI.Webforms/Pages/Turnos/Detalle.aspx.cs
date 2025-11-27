using SGTO.Negocio.DTOs.Turnos;
using SGTO.Negocio.Seguridad;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Seguridad;
using SGTO.UI.Webforms.Utils;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Turnos
{
    public partial class Detalle : System.Web.UI.Page
    {
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();
        private readonly TurnoService _servicioTurno = new TurnoService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.EstaLogueado())
            {
                Response.Redirect("~/Pages/Login/Index.aspx");
                return;
            }

            if (!_servicioAutorizacion.TienePermiso(SessionManager.Usuario, "TURNOS", "VER"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

            if (Master is SiteMaster master)
            {
                master.ConfigurarBotonVolver(true, "~/Pages/Turnos/Index");
                master.EstablecerOpcionMenuActiva("turnos");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Visualización completa de la información administrativa y el registro clínico asociado.");
            }

            int idTurno = ExtraerIdTurno();
            if (idTurno == 0)
            {
                Response.Redirect("~/Pages/Turnos/Index", false);
                return;
            }

            if (!IsPostBack)
                CargarDetalleTurno(idTurno);


            ModalHelper.MostrarModalDesdeSession(
                   this.Page,
                   "TurnoMensajeTitulo",
                   "TurnoMensajeDesc",
                   "/Pages/Turnos/Index",
                   "abrirModalResultado");
        }

        private int ExtraerIdTurno()
        {
            string qs = Request.QueryString["id-turno"];
            return int.TryParse(qs, out int id) ? id : 0;
        }


        private void CargarDetalleTurno(int idTurno)
        {
            try
            {
                TurnoDetalleDto turno = _servicioTurno.ObtenerDetallePorId(idTurno);

                if (turno == null)
                {
                    MensajeUiHelper.SetearYMostrar(this.Page, "Error", "El turno solicitado no existe.", "Error", "/Pages/Turnos/Index", "abrirModalResultado");
                    return;
                }

                lblNombrePaciente.Text = turno.NombrePaciente;
                lblNombreMedico.Text = turno.NombreMedico;
                lblFechaHora.Text = string.Format("{0:dd/MM/yyyy HH:mm} - {1:HH:mm}", turno.FechaInicio, turno.FechaFin);

                lblEspecialidad.Text = turno.Especialidad;
                if (!string.IsNullOrEmpty(turno.Plan) && turno.Plan != "-")
                {
                    lblCoberturaPlan.Text = turno.Cobertura + " / " + turno.Plan;
                }
                else
                {
                    lblCoberturaPlan.Text = turno.Cobertura;
                }
                lblObservaciones.Text = string.IsNullOrEmpty(turno.Observaciones)
                                        ? "<em>Sin observaciones registradas.</em>"
                                        : turno.Observaciones;

                lblEstado.Text = turno.Estado;
                lblEstado.CssClass = TurnoUiHelper.ObtenerCssEstadoTurnoBadge(turno.Estado);

                if (!string.IsNullOrEmpty(turno.TratamientoAplicado) || !string.IsNullOrEmpty(turno.Diagnostico))
                {
                    phDetalleClinico.Visible = true;
                    lblTratamiento.Text = turno.TratamientoAplicado;
                    lblDiagnostico.Text = turno.Diagnostico;

                    lblObservacionesClinicas.Text = string.IsNullOrEmpty(turno.ObservacionesClinicas)
                                                    ? "Sin observaciones médicas adicionales."
                                                    : turno.ObservacionesClinicas;
                }
                else
                {
                    phDetalleClinico.Visible = false;
                }

                bool esEditable = TurnoUiHelper.EsEditable(turno.Estado);

                bool usuarioTienePermiso = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "TURNOS", "EDITAR");

                btnEditar.Visible = esEditable && usuarioTienePermiso;

                // guardar el id en view state por si el usuario hace clic en editar
                ViewState["IdTurnoActual"] = idTurno;
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Dato inválido", ex.Message, "Resultado", null, "abrirModalResultado");
            }
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            if (ViewState["IdTurnoActual"] != null)
            {
                int id = (int)ViewState["IdTurnoActual"];

                Response.Redirect("~/Pages/Turnos/Editar.aspx?id-turno=" + id, false);
            }
        }
    }
}