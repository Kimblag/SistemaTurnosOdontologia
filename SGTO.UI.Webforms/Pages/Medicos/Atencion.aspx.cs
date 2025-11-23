using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.HistoriaClinica;
using SGTO.Negocio.DTOs.Turnos;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Seguridad;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Seguridad;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Medicos
{
    public partial class Atencion : System.Web.UI.Page
    {
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();
        private readonly TurnoService _servicioTurno = new TurnoService();
        private readonly HistoriaClinicaService _servicioHistoria = new HistoriaClinicaService();
        private readonly TratamientoService _servicioTratamiento = new TratamientoService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.EstaLogueado())
            {
                Response.Redirect("~/Pages/Login/Index.aspx");
                return;
            }

            if (!_servicioAutorizacion.TienePermiso(SessionManager.Usuario, "ATENCION", "VER"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

            if (Master is SiteMaster master)
            {
                master.ConfigurarBotonVolver(true, "~/Pages/Turnos/Index.aspx");
                master.EstablecerOpcionMenuActiva("Medicos");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("En esta sección puedes cargar los detalles de la atención brindada al paciente");
            }

            if (!IsPostBack)
            {
                CargarDatosIniciales();
            }
        }

        private void CargarDatosIniciales()
        {
            string idTurnoStr = Request.QueryString["id"];
            if (int.TryParse(idTurnoStr, out int idTurno))
            {
                try
                {
                    TurnoDetalleDto turno = _servicioTurno.ObtenerDetallePorId(idTurno);

                    if (turno.Estado == "Cerrado" || turno.Estado == "Cancelado")
                    {
                        MensajeUiHelper.SetearYMostrar(this, "Error", "Este turno ya no se puede atender.", "Volver", "~/Pages/Turnos/Index", "abrirModalResultado");
                        btnGuardar.Enabled = false;
                        return;
                    }

                    hdnIdTurno.Value = turno.IdTurno.ToString();
                    txtPacienteNombre.Text = turno.NombrePaciente;
                    txtEspecialidad.Text = turno.Especialidad;
                    txtFecha.Text = turno.FechaInicio.ToString("dd/MM/yyyy");
                    txtHora.Text = turno.FechaInicio.ToString("HH:mm");
                    txtCobertura.Text = string.IsNullOrEmpty(turno.Plan) || turno.Plan == "-"
                                        ? turno.Cobertura
                                        : $"{turno.Cobertura} - {turno.Plan}";

                    CargarTratamientos(turno.IdEspecialidad);
                }
                catch (Exception ex)
                {
                    MensajeUiHelper.SetearYMostrar(this, "Error", "No se pudo cargar el turno: " + ex.Message, "Volver", "~/Pages/Turnos/Index", "abrirModalResultado");
                }
            }
            else
            {
                Response.Redirect("~/Pages/Turnos/Index.aspx");
            }
        }

        private void CargarTratamientos(int idEspecialidad)
        {
            try
            {
                List<TratamientoDto> tratamientos = _servicioTratamiento.ListarPorEspecialidad(idEspecialidad);

                if (tratamientos.Count > 0)
                {
                    pnlTratamientoSeleccion.Visible = true;
                    pnlTratamientoManual.Visible = false;

                    ddlTratamiento.DataSource = tratamientos;
                    ddlTratamiento.DataTextField = "Nombre";
                    ddlTratamiento.DataValueField = "IdTratamiento";
                    ddlTratamiento.DataBind();
                    ddlTratamiento.Items.Insert(0, new ListItem("Seleccione...", "0"));
                }
                else
                {
                    pnlTratamientoSeleccion.Visible = false;
                    pnlTratamientoManual.Visible = true;
                }
            }
            catch (Exception)
            {
                pnlTratamientoSeleccion.Visible = false;
                pnlTratamientoManual.Visible = true;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    int idUsuarioLogueado = ObtenerIdUsuarioActual();

                    if (ddlTratamiento.SelectedValue == "0")
                    {
                        MensajeUiHelper.SetearYMostrar(this, "Error de Sesión", "No se pudo identificar al usuario. Por favor inicie sesión nuevamente.", "Ir al Login", "~/Pages/Login/Index.aspx", "abrirModalResultado");
                        return;
                    }

                    if (pnlTratamientoSeleccion.Visible && ddlTratamiento.SelectedValue == "0")
                    {
                        MensajeUiHelper.SetearYMostrar(this, "Atención", "Debe seleccionar un tratamiento realizado.", "Cerrar", null, "abrirModalResultado");
                        return;
                    }

                    var dto = new HistoriaClinicaCreacionDto
                    {
                        IdTurno = int.Parse(hdnIdTurno.Value),
                        IdTratamiento = int.Parse(ddlTratamiento.SelectedValue),
                        Diagnostico = txtDiagnostico.Text.Trim(),
                        Observaciones = txtObservaciones.Text.Trim()
                    };

                    if (pnlTratamientoSeleccion.Visible)
                    {
                        dto.IdTratamiento = int.Parse(ddlTratamiento.SelectedValue);
                        dto.TratamientoManual = null;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(txtTratamientoManual.Text))
                        {
                            MensajeUiHelper.SetearYMostrar(this, "Error", "Debe especificar el tratamiento manual.", "Cerrar", null, "abrirModalResultado");
                            return;
                        }
                        dto.IdTratamiento = 0;
                        dto.TratamientoManual = txtTratamientoManual.Text.Trim();
                    }

                    _servicioHistoria.RegistrarAtencion(dto, idUsuarioLogueado);

                    MensajeUiHelper.SetearYMostrar(
                        this,
                        "Atención Finalizada",
                        "La historia clínica se guardó correctamente y el turno ha sido cerrado.",
                        "Volver al Listado",
                        VirtualPathUtility.ToAbsolute("~/Pages/Turnos/Index"),
                        "abrirModalResultado"
                    );
                }
                catch (ExcepcionReglaNegocio ex)
                {
                    MensajeUiHelper.SetearYMostrar(this, "No se pudo guardar", ex.Message, "Cerrar", null, "abrirModalResultado");
                }
                catch (Exception)
                {
                    MensajeUiHelper.SetearYMostrar(this, "Error Crítico", "Ocurrió un error inesperado al procesar la solicitud.", "Cerrar", null, "abrirModalResultado");
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Turnos/Index.aspx");
        }

        private int ObtenerIdUsuarioActual()
        {

            if (SessionManager.EstaLogueado())
            {
                return SessionManager.Usuario.IdUsuario;
            }
            return 0;
        }

    }
}