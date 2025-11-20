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
            int idPaciente = ExtraerIdPaciente();
            int idTurno = ExtraerIdTurno();

            ModoEdicion = idTurno != 0;

            hdnIdTurno.Value = idTurno.ToString();

            if (idPaciente == 0 && idTurno == 0)
            {
                Response.Redirect("~/Pages/Pacientes/Index", false);
                return;
            }

            if (!IsPostBack)
            {
                if (idPaciente != 0)
                    CargarPaciente(idPaciente);

                if (idTurno != 0)
                {
                    ddlEstadoTurno.Enabled = true;
                    CargarTurnoExistente(idTurno);
                }

                ModalHelper.MostrarModalDesdeSession(
                   this.Page,
                   "TurnoMensajeTitulo",
                   "TurnoMensajeDesc",
                   "/Pages/Turnos/Index",
                   "abrirModalResultado");
            }
        }

        private void CargarPaciente(int idPaciente)
        {
            // consultamos los datos del paciente para precargarlos en el textbox.
            PacienteEdicionDto paciente = _servicioPaciente.ObtenerPorId(idPaciente);
            CargarPacienteTextbox(paciente);
            CargarEspecialidadesDropdown();
            CargarEstadosTurno();
            PreCargarCoberturaYPlan(paciente.IdCobertura, paciente.IdPlan);

            // el estado turno inicia como nuevo y no puede cambiarse si se está creando
            ddlEstadoTurno.SelectedValue = "N";
            ddlEstadoTurno.Enabled = false;

            // si el paciente está inactivo, no podemos agendar turnos.
            if (paciente.Estado.ToLower()[0] == 'i')
                BloquearControlesPorPacienteInactivo();
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
            ddlEstadoTurno.Items.Add(new ListItem("Reprogramado", "R"));
            ddlEstadoTurno.Items.Add(new ListItem("No asistió", "X"));
            ddlEstadoTurno.Items.Add(new ListItem("Cancelado", "C"));
        }


        private void CargarMedicoDropdown(int idEspecialidad)
        {
            try
            {
                ddlMedico.Items.Clear();
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
                ddlEspecialidad.Items.Add(new ListItem("Error al cargar especialidades", "0"));
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


        private void CargarHorasDisponibles(int idMedico, DateTime fecha, TimeSpan? horaTurnoActual = null)
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


            if (horaTurnoActual.HasValue)
            {
                bool existe = false;
                foreach (ListItem item in ddlHora.Items)
                {
                    if (item.Value == horaTurnoActual.Value.ToString())
                    {
                        existe = true;
                        break;
                    }
                }

                if (!existe)
                    ddlHora.Items.Insert(1, new ListItem(horaTurnoActual.Value.ToString(@"hh\:mm"), horaTurnoActual.Value.ToString()));

                // seleccionar la hora actual
                foreach (ListItem item in ddlHora.Items)
                {
                    if (item.Value == horaTurnoActual.Value.ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }

            ddlHora.Enabled = horas.Count > 0 || horaTurnoActual.HasValue;
        }


        private void PreCargarCoberturaYPlan(int idCobertura, int idPlan = 0)
        {
            ddlCobertura.Items.Clear();
            ddlCobertura.Items.Add(new ListItem("Seleccione una cobertura", ""));

            List<CoberturaDto> coberturas = _servicioCobertura.Listar("activas");

            CoberturaDto coberturaSeleccionada = null;

            foreach (CoberturaDto c in coberturas)
            {
                ddlCobertura.Items.Add(new ListItem(c.Nombre, c.IdCobertura.ToString()));
                if (c.IdCobertura == idCobertura)
                    coberturaSeleccionada = c;
            }

            ddlCobertura.SelectedValue = idCobertura.ToString();

            if (coberturaSeleccionada == null || coberturaSeleccionada.Estado.ToLower()[0] != 'a')
            {
                alertCobertura.Attributes["class"] = "alert alert-warning py-1 px-2";
                alertCobertura.InnerText = "La cobertura del paciente está inactiva.";
            }

            // Planes
            ddlPlan.Items.Clear();
            ddlPlan.Items.Add(new ListItem("Seleccione un plan", ""));

            List<PlanDto> planes = _servicioPlan.ListarPorCobertura(idCobertura);
            PlanDto planSeleccionado = null;

            if (planes.Count > 0)
            {
                foreach (PlanDto p in planes)
                {
                    ddlPlan.Items.Add(new ListItem(p.Nombre, p.IdPlan.ToString()));
                    if (p.IdPlan == idPlan)
                        planSeleccionado = p;
                }
            }
            else
            {
                // no hay planes para esta cobertura
                ddlPlan.Enabled = false;
            }

            if (idPlan != 0 && planSeleccionado != null)
            {
                if (planSeleccionado.Estado.ToLower()[0] != 'a')
                {
                    alertPlan.Attributes["class"] = "alert alert-warning py-1 px-2";
                    alertPlan.InnerText = "El plan del paciente está inactivo.";
                }
                else if (planSeleccionado.IdCobertura != idCobertura)
                {
                    alertPlan.Attributes["class"] = "alert alert-warning py-1 px-2";
                    alertPlan.InnerText = "El plan no coincide con su cobertura.";
                }

                ddlPlan.SelectedValue = idPlan.ToString();
            }
        }


        private void CargarTurnoExistente(int idTurno)
        {

            try
            {
                TurnoEdicionDto turno = _servicioTurno.ObtenerPorId(idTurno);
                if (turno == null)
                    throw new Exception("No se encontró el turno.");

                // guardamos los valores originales en los hidden fields, de manera que podamos detectar cambios
                hdnIdTurno.Value = turno.IdTurno.ToString();
                hdnIdPaciente.Value = turno.IdPaciente.ToString();

                hdnFechaInicioOriginal.Value = turno.FechaInicio.ToString("yyyy-MM-dd HH:mm");
                hdnIdMedicoOriginal.Value = turno.IdMedico.ToString();
                hdnIdEspecialidadOriginal.Value = turno.IdEspecialidad.ToString();
                hdnIdCoberturaOriginal.Value = turno.IdCobertura.ToString();
                hdnIdPlanOriginal.Value = turno.IdPlan.ToString();
                hdnEstadoOriginal.Value = turno.Estado.ToString();

                // pre cargar el paciente
                txtPaciente.Text = turno.NombreCompletoPaciente;

                // cargar los dropdowns en orden, ya que estos están en cascada

                CargarEspecialidadesDropdown();
                ddlEspecialidad.SelectedValue = turno.IdEspecialidad.ToString();

                CargarMedicoDropdown(turno.IdEspecialidad);
                ddlMedico.SelectedValue = turno.IdMedico.ToString();

                CargarFechasDisponiblesDropdown(turno.IdMedico);

                string fechaFormato = turno.FechaInicio.ToString("yyyy-MM-dd");
                if (ddlFecha.Items.FindByValue(fechaFormato) != null)
                    ddlFecha.SelectedValue = fechaFormato;

                CargarHorasDisponibles(turno.IdMedico, turno.FechaInicio, turno.FechaInicio.TimeOfDay);

                PreCargarCoberturaYPlan(turno.IdCobertura, turno.IdPlan);

                CargarEstadosTurno();
                ddlEstadoTurno.SelectedValue = turno.Estado.ToString();
                ddlEstadoTurno.Enabled = true;

                txtObservaciones.Text = turno.Observaciones;


            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Dato inválido", ex.Message, "Resultado", null, "abrirModalResultado");
            }

        }


        protected void btnCancelarTurno_Click(object sender, EventArgs e)
        {
            if (ModoEdicion)
            {
                Response.Redirect("~/Pages/Turnos/Index", false);
            }
            else
            {
                Response.Redirect("~/Pages/Pacientes/Index", false);
            }
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

        private void CrearTurno()
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

        private void EditarTurno()
        {

            TurnoEdicionDto dto = new TurnoEdicionDto
            {
                IdTurno = int.Parse(hdnIdTurno.Value),
                IdPaciente = int.Parse(hdnIdPaciente.Value),
                IdEspecialidad = int.Parse(ddlEspecialidad.SelectedValue),
                IdMedico = int.Parse(ddlMedico.SelectedValue),
                IdCobertura = int.Parse(ddlCobertura.SelectedValue),
                IdPlan = string.IsNullOrEmpty(ddlPlan.SelectedValue) ? 0 : int.Parse(ddlPlan.SelectedValue),
                Estado = ddlEstadoTurno.SelectedValue.ToUpper()[0],
                FechaInicio = DateTime.Parse($"{ddlFecha.SelectedValue} {ddlHora.SelectedItem.Text}"),
                FechaFin = DateTime.Parse($"{ddlFecha.SelectedValue} {ddlHora.SelectedItem.Text}").AddHours(DURACION_TURNO),
                Observaciones = txtObservaciones.Text?.Trim()
            };

            _servicioTurno.Editar(dto);

            MensajeUiHelper.SetearYMostrar(
                this.Page,
                "Turno modificado",
                $"El turno se ha modificado correctamente.",
                "Resultado",
                VirtualPathUtility.ToAbsolute($"~/Pages/Turnos/Detalle?id-turno={dto.IdTurno}"),
                "abrirModalResultado"
            );
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {

                if (ModoEdicion)
                    EditarTurno();
                else
                    CrearTurno();


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