using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Medicos;
using SGTO.Negocio.DTOs.Pacientes;
using SGTO.Negocio.DTOs.Turnos;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
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

            try
            {
                PacienteEdicionDto paciente = _servicioPaciente.ObtenerPorId(idPaciente);

                hdnIdPaciente.Value = paciente.IdPaciente.ToString();

                CargarPacienteTextbox(paciente);
                CargarEspecialidadesDropdown();
                CargarEstadosTurno();
                PreCargarCoberturaYPlan(paciente.IdCobertura, paciente.IdPlan);

                ddlEstadoTurno.SelectedValue = "N";
                ddlEstadoTurno.Enabled = false;

                if (paciente.Estado.ToLower()[0] == 'i')
                    BloquearControlesPorPacienteInactivo();
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error",
                    ex.Message,
                    "Volver",
                    VirtualPathUtility.ToAbsolute("~/Pages/Pacientes/Index.aspx"),
                    "abrirModalResultado"
                );
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


        public void CargarFechasDisponiblesDropdown(int idMedico, DateTime? fechaTurnoActual = null)
        {
            List<DateTime> fechas = _servicioTurno.ObtenerFechasDisponibles(idMedico, CANTIDAD_SEMANAS);

            ddlFecha.Items.Clear();

            if (fechaTurnoActual.HasValue)
            {
                if (!fechas.Exists(f => f.Date == fechaTurnoActual.Value.Date))
                {
                    fechas.Add(fechaTurnoActual.Value.Date);
                }
            }

            fechas.Sort();

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

            if (horaTurnoActual.HasValue)
            {
                if (fecha.Date == DateTime.Parse(hdnFechaInicioOriginal.Value).Date)
                {
                    if (!horas.Contains(horaTurnoActual.Value))
                    {
                        horas.Add(horaTurnoActual.Value);
                    }
                }
            }


            horas.Sort();

            if (horas.Count == 0)
            {
                ddlHora.Items.Add(new ListItem("Sin horarios disponibles", ""));
                ddlHora.Enabled = false;
                return;
            }

            ddlHora.Items.Add(new ListItem("Seleccione una hora", ""));

            foreach (TimeSpan h in horas)
            {
                ddlHora.Items.Add(new ListItem(h.ToString(@"hh\:mm"), h.ToString()));
            }


            if (horaTurnoActual.HasValue)
            {
                string horaStr = horaTurnoActual.Value.ToString();
                if (ddlHora.Items.FindByValue(horaStr) != null)
                {
                    ddlHora.SelectedValue = horaStr;
                }
            }

            ddlHora.Enabled = true;
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
                if (idTurno <= 0) throw new ArgumentException("ID de turno inválido.");

                TurnoEdicionDto turno = _servicioTurno.ObtenerParaEdicion(idTurno);

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

                CargarFechasDisponiblesDropdown(turno.IdMedico, turno.FechaInicio);

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
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Operación no permitida", ex.Message, "Resultado", VirtualPathUtility.ToAbsolute("~/Pages/Turnos/Index"), "abrirModalResultado");
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


        private bool ValidarDatosEntrada(out int idPaciente, out int idMedico, out int idEspecialidad, out int idCobertura, out int idPlan, out DateTime fechaInicio)
        {
            idPaciente = 0; idMedico = 0; idEspecialidad = 0; idCobertura = 0; idPlan = 0; fechaInicio = DateTime.MinValue;

            if (!int.TryParse(hdnIdPaciente.Value, out idPaciente) || idPaciente <= 0)
            {
                MostrarError("No se ha identificado al paciente. Intente recargar la página.");
                return false;
            }

            if (ddlEspecialidad.SelectedIndex <= 0 || string.IsNullOrEmpty(ddlEspecialidad.SelectedValue) || ddlEspecialidad.SelectedValue == "0")
            {
                MostrarError("Debe seleccionar una especialidad.");
                return false;
            }
            idEspecialidad = int.Parse(ddlEspecialidad.SelectedValue);

            if (ddlMedico.SelectedIndex <= 0 || string.IsNullOrEmpty(ddlMedico.SelectedValue))
            {
                MostrarError("Debe seleccionar un médico.");
                return false;
            }
            idMedico = int.Parse(ddlMedico.SelectedValue);

            if (string.IsNullOrEmpty(ddlCobertura.SelectedValue))
            {
                MostrarError("Debe seleccionar una cobertura.");
                return false;
            }
            idCobertura = int.Parse(ddlCobertura.SelectedValue);

            if (!string.IsNullOrEmpty(ddlPlan.SelectedValue))
            {
                int.TryParse(ddlPlan.SelectedValue, out idPlan);
            }

            if (ddlFecha.SelectedIndex <= 0 || string.IsNullOrEmpty(ddlFecha.SelectedValue))
            {
                MostrarError("Debe seleccionar una fecha para el turno.");
                return false;
            }

            if (ddlHora.SelectedIndex < 0 || string.IsNullOrEmpty(ddlHora.SelectedValue))
            {
                MostrarError("Debe seleccionar una hora.");
                return false;
            }

            string fechaStr = ddlFecha.SelectedValue; // formato yyyy-MM-dd
            string horaStr = ddlHora.SelectedItem.Text; // formato HH:mm

            if (!DateTime.TryParse($"{fechaStr} {horaStr}", out fechaInicio))
            {
                MostrarError("La fecha u hora seleccionada no tiene un formato válido.");
                return false;
            }

            return true;
        }

        private void MostrarError(string mensaje)
        {
            MensajeUiHelper.SetearYMostrar(this.Page, "Atención", mensaje, "Cerrar", null, "abrirModalResultado");
        }


        private void CrearTurno()
        {
            if (!ValidarDatosEntrada(out int idPaciente, out int idMedico, out int idEspecialidad, out int idCobertura, out int idPlan, out DateTime fechaInicio))
            {
                return;
            }

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
            if (!int.TryParse(hdnIdTurno.Value, out int idTurno) || idTurno == 0)
            {
                MostrarError("No se ha identificado el turno a editar.");
                return;
            }

            if (!ValidarDatosEntrada(out int idPaciente, out int idMedico, out int idEspecialidad, out int idCobertura, out int idPlan, out DateTime fechaInicio))
            {
                return;
            }

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

            string rutaReprogramacion = Server.MapPath("~/Plantillas/Email/ReprogramacionTurno.html");
            string rutaCancelar = Server.MapPath("~/Plantillas/Email/CancelacionTurno.html");
            _servicioTurno.Editar(dto, rutaReprogramacion, rutaCancelar);

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