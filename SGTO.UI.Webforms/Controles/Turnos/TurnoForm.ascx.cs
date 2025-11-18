using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Medicos;
using SGTO.Negocio.DTOs.Pacientes;
using SGTO.Negocio.DTOs.Turnos;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Turnos
{
    public partial class TurnoForm : System.Web.UI.UserControl
    {

        private readonly TurnoService _servicioTurno = new TurnoService();
        private readonly PacienteService _servicioPaciente = new PacienteService();
        private readonly EspecialidadService _servicioEspecialidad = new EspecialidadService();
        private readonly MedicoService _servicioMedico = new MedicoService();
        private readonly CoberturaService _servicioCobertura = new CoberturaService();
        private readonly PlanService _servicioPlan = new PlanService();

        private const int CANTIDAD_SEMANAS = 4;
        private const int DURACION_TURNO = 1; // 1 HORA

        public bool ModoEdicion { get; set; } = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int idPaciente = ExtraerIdPaciente();
                int idTurno = ExtraerIdTurno();

                if (idPaciente == 0 && idTurno == 0)
                {
                    Response.Redirect("~/Pages/Pacientes/Index", false);
                    return;
                }

                ModoEdicion = idTurno != 0;

                PacienteEdicionDto paciente = _servicioPaciente.ObtenerPorId(idPaciente);

                CargarPacienteTextbox(paciente);
                CargarEspecialidadesDropdown();
                CargarEstadosTurno();
                PreCargarCoberturaYPlan(paciente);

                if (!ModoEdicion)
                {
                    ddlEstadoTurno.SelectedValue = "N";
                    ddlEstadoTurno.Enabled = false;
                }
                else
                {
                    ddlEstadoTurno.Enabled = true;
                    CargarEstadoTurnoExistente(idTurno);
                }

                if (paciente.Estado.ToLower()[0] == 'i')
                    BloquearControlesPorPacienteInactivo();

                ModalHelper.MostrarModalDesdeSession(
                   this.Page,
                   "TurnoMensajeTitulo",
                   "TurnoMensajeDesc",
                   "/Pages/Turnos/Index",
                   "abrirModalResultado");
            }
        }

        private int ExtraerIdPaciente()
        {
            string idString = Request.QueryString["id-paciente"] ?? string.Empty;
            if (!string.IsNullOrEmpty(idString) && int.TryParse(idString, out int id))
            {
                return id;
            }
            return 0;
        }

        private int ExtraerIdTurno()
        {
            string qs = Request.QueryString["id-turno"];
            return int.TryParse(qs, out int id) ? id : 0;
        }


        private void CargarPacienteTextbox(PacienteEdicionDto paciente)
        {
            txtPaciente.Text = $"{paciente.Apellido}, {paciente.Nombre}";
        }


        private void CargarEstadosTurno()
        {
            ddlEstadoTurno.Items.Clear();

            ddlEstadoTurno.Items.Add(new ListItem("Nuevo", "N"));
            ddlEstadoTurno.Items.Add(new ListItem("Pendiente Reprogramación", "P"));
            ddlEstadoTurno.Items.Add(new ListItem("Reprogramado", "R"));
            ddlEstadoTurno.Items.Add(new ListItem("No asistió", "X"));
            ddlEstadoTurno.Items.Add(new ListItem("Cancelado", "C"));
            ddlEstadoTurno.Items.Add(new ListItem("Cerrado", "Z"));
        }


        private void CargarMedicoDropdown(int idEspecialidad)
        {
            try
            {
                List<MedicoListadoDto> medicosDto = _servicioMedico.ListarPorEspecialidad(idEspecialidad);
                ddlMedico.Items.Add(new ListItem("Seleccione un médico", ""));

                foreach (MedicoListadoDto medico in medicosDto)
                    ddlMedico.Items.Add(new ListItem(medico.NombreCompleto, medico.IdMedico.ToString()));

                ddlMedico.Enabled = true;
            }
            catch
            {
                ddlMedico.Items.Clear();
                ddlMedico.Items.Add(new ListItem("Error al cargar médicos", ""));
                ddlMedico.Enabled = false;
                btnGuardar.Enabled = false;
            }
        }


        private void CargarEspecialidadesDropdown()
        {
            try
            {
                List<EspecialidadDto> especialidades = _servicioEspecialidad.Listar("activas");

                ddlEspecialidad.DataSource = especialidades;
                ddlEspecialidad.DataTextField = "Nombre";
                ddlEspecialidad.DataValueField = "IdEspecialidad";
                ddlEspecialidad.DataBind();

                ddlEspecialidad.Items.Insert(0, new ListItem("Seleccione una especialidad", "0"));
            }
            catch
            {
                ddlEspecialidad.Items.Clear();
                ddlEspecialidad.Items.Add(new ListItem("[Error al cargar especialidades]", "0"));
                ddlEspecialidad.Enabled = false;
            }
        }


        public void CargarFechasDisponiblesDropdown(int idMedico)
        {
            List<DateTime> fechas = _servicioTurno.ObtenerFechasDisponibles(idMedico, CANTIDAD_SEMANAS);

            ddlFecha.Items.Clear();

            if (fechas.Count == 0)
            {
                ddlFecha.Items.Add(new ListItem("No hay fechas disponibles", ""));
                ddlFecha.Enabled = false;
                return;
            }

            ddlFecha.Items.Add(new ListItem("Seleccione una fecha", ""));

            foreach (DateTime f in fechas)
                ddlFecha.Items.Add(new ListItem(f.ToString("dddd dd/MM/yyyy"), f.ToString("yyyy-MM-dd")));

            ddlFecha.Enabled = true;
        }


        private void CargarHorasDisponibles(int idMedico, DateTime fecha)
        {
            List<TimeSpan> horas = _servicioTurno.ObtenerSlotsDisponibles(idMedico, fecha);

            ddlHora.Items.Clear();

            if (horas.Count == 0)
            {
                ddlHora.Items.Add(new ListItem("Sin horarios disponibles", ""));
                ddlHora.Enabled = false;
                return;
            }

            ddlHora.Items.Add(new ListItem("Seleccione una hora", ""));

            foreach (TimeSpan h in horas)
                ddlHora.Items.Add(new ListItem(h.ToString(@"hh\:mm"), h.ToString()));

            ddlHora.Enabled = true;
        }


        private void PreCargarCoberturaYPlan(PacienteEdicionDto paciente)
        {
            ddlCobertura.Items.Clear();
            ddlCobertura.Items.Add(new ListItem("Seleccione una cobertura", ""));

            List<CoberturaDto> coberturas = _servicioCobertura.Listar("activas");

            foreach (CoberturaDto c in coberturas)
                ddlCobertura.Items.Add(new ListItem(c.Nombre, c.IdCobertura.ToString()));

            ddlCobertura.SelectedValue = paciente.IdCobertura.ToString();

            CoberturaDto cobertura = _servicioCobertura.ObtenerPorId(paciente.IdCobertura);

            if (cobertura == null || cobertura.Estado.ToLower()[0] != 'a')
            {
                alertCobertura.Attributes["class"] = "alert alert-warning py-1 px-2";
                alertCobertura.InnerText = "La cobertura del paciente está inactiva.";
            }


            ddlPlan.Items.Clear();
            ddlPlan.Items.Add(new ListItem("Seleccione un plan", ""));

            List<PlanDto> planes = _servicioPlan.ListarPorCobertura(paciente.IdCobertura);

            foreach (PlanDto p in planes)
                ddlPlan.Items.Add(new ListItem(p.Nombre, p.IdPlan.ToString()));

            if (paciente.IdPlan != 0)
            {
                PlanDto plan = _servicioPlan.ObtenerPorId(paciente.IdPlan);

                if (plan == null || plan.Estado.ToLower()[0] != 'a')
                {
                    alertCobertura.Attributes["class"] = "alert alert-warning py-1 px-2";
                    alertCobertura.InnerText = "El plan del paciente está inactivo.";
                }
                else if (plan.IdCobertura != paciente.IdCobertura)
                {
                    alertPlan.Attributes["class"] = "alert alert-warning py-1 px-2";
                    alertPlan.InnerText = "El plan no coincide con su cobertura.";
                }

                ddlPlan.SelectedValue = paciente.IdPlan.ToString();
            }
        }


        private void CargarEstadoTurnoExistente(int idTurno)
        {
            TurnoEdicionDto turno = _servicioTurno.ObtenerPorId(idTurno);

            if (turno != null)
                ddlEstadoTurno.SelectedValue = turno.Estado[0].ToString();
        }


        protected void btnCancelarTurno_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Pacientes/Index", false);
        }

        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMedico.Items.Clear();
            ddlFecha.Items.Clear();
            ddlHora.Items.Clear();

            ddlMedico.Enabled = ddlFecha.Enabled = ddlHora.Enabled = false;

            if (ddlEspecialidad.SelectedValue == "0")
            {
                ddlMedico.Items.Add(new ListItem("Seleccione un médico", ""));
                return;
            }

            CargarMedicoDropdown(int.Parse(ddlEspecialidad.SelectedValue));
        }


        protected void ddlMedico_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFecha.Items.Clear();
            ddlHora.Items.Clear();
            ddlHora.Enabled = false;

            if (string.IsNullOrEmpty(ddlMedico.SelectedValue))
            {
                ddlFecha.Enabled = false;
                ddlFecha.Items.Add(new ListItem("Seleccione una fecha", ""));
                return;
            }

            CargarFechasDisponiblesDropdown(int.Parse(ddlMedico.SelectedValue));
        }


        protected void ddlFecha_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlHora.Items.Clear();

            if (string.IsNullOrEmpty(ddlFecha.SelectedValue))
            {
                ddlHora.Enabled = false;
                ddlHora.Items.Add(new ListItem("Seleccione una hora", ""));
                return;
            }

            DateTime fecha = DateTime.Parse(ddlFecha.SelectedValue);
            CargarHorasDisponibles(int.Parse(ddlMedico.SelectedValue), fecha);
        }


        protected void ddlCobertura_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPlan.Items.Clear();
            ddlPlan.Items.Add(new ListItem("Seleccione un plan", ""));

            int idCobertura = int.Parse(ddlCobertura.SelectedValue);

            List<PlanDto> planes = _servicioPlan.ListarPorCobertura(idCobertura);

            foreach (PlanDto p in planes)
                ddlPlan.Items.Add(new ListItem(p.Nombre, p.IdPlan.ToString()));
        }


        private void BloquearControlesPorPacienteInactivo()
        {
            ddlEspecialidad.Enabled = false;
            ddlMedico.Enabled = false;
            ddlFecha.Enabled = false;
            ddlHora.Enabled = false;
            ddlCobertura.Enabled = false;
            ddlPlan.Enabled = false;
            ddlEstadoTurno.Enabled = false;
            btnGuardar.Enabled = false;

            alertPacienteInactivo.Attributes["class"] = "alert alert-warning py-1 px-2";
            alertPacienteInactivo.InnerText = "El paciente se encuentra inactivo, no es posible agendar un turno.";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                TurnoCreacionDto dto = new TurnoCreacionDto
                {
                    IdPaciente = int.Parse(Request.QueryString["id-paciente"]),

                    IdEspecialidad = int.Parse(ddlEspecialidad.SelectedValue),
                    IdMedico = int.Parse(ddlMedico.SelectedValue),
                    IdCobertura = int.Parse(ddlCobertura.SelectedValue),

                    IdPlan = string.IsNullOrEmpty(ddlPlan.SelectedValue)
                        ? 0
                        : int.Parse(ddlPlan.SelectedValue),

                    Estado = ddlEstadoTurno.SelectedValue.ToUpper()[0],

                    //fecha y hora seleccionada
                    FechaInicio = DateTime.Parse($"{ddlFecha.SelectedValue} {ddlHora.SelectedItem.Text}"),

                    FechaFin = DateTime.Parse($"{ddlFecha.SelectedValue} {ddlHora.SelectedItem.Text}")
                        .AddHours(DURACION_TURNO),

                    Observaciones = txtObservaciones.Text?.Trim()
                };

                string rutaPlantilla = Server.MapPath("~/Plantillas/Email/ConfirmacionTurno.html");
                int idTurno = _servicioTurno.Crear(dto, rutaPlantilla);


                MensajeUiHelper.SetearYMostrar(
                   this.Page,
                   "Turno agendado",
                   $"El turno se ha agendado correctamente.",
                   "Resultado",
                   VirtualPathUtility.ToAbsolute($"~/Pages/Turnos/Detalle?id-turno={idTurno}"),
                   "abrirModalResultado"
               );
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Operación no permitida", ex.Message, "Resultado", null, "abrirModalResultado");
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Dato inválido", ex.Message, "Resultado", null, "abrirModalResultado");
            }
        }


    }
}