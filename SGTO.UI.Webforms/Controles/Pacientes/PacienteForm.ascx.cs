using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Pacientes;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Pacientes
{
    public partial class PacienteForm : System.Web.UI.UserControl
    {

        private readonly CoberturaService _servicioCobertura = new CoberturaService();
        private readonly PlanService _servicioPlan = new PlanService();
        private readonly PacienteService _servicioPaciente = new PacienteService();

        public bool ModoEdicion { get; set; } = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            int idPaciente = ExtraerIdPaciente();
            if (idPaciente != 0)
            {
                ModoEdicion = true;

            }
            else
            {
                ddlEstado.Enabled = false;
                ddlEstado.SelectedValue = "A";
            }

            if (!IsPostBack)
            {
                CargarCoberturas();

                if (ModoEdicion)
                {
                    CargarDetallePaciente(idPaciente);
                }

                ModalHelper.MostrarModalDesdeSession(this.Page, "PacienteMensajeTitulo", "PacienteMensajeDesc", "/Pages/Pacientes/Index");
            }

            ddlCobertura.AutoPostBack = true;
            ddlCobertura.SelectedIndexChanged += ddlCobertura_SelectedIndexChanged;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            AjustarRangoFechaNacimiento();
        }

        private void AjustarRangoFechaNacimiento()
        {
            DateTime hoy = DateTime.Today;
            DateTime minimo = hoy.AddYears(-120); // no más de 120 años

            rvFechaNacimiento.MinimumValue = minimo.ToString("yyyy-MM-dd");
            rvFechaNacimiento.MaximumValue = hoy.ToString("yyyy-MM-dd");
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Pacientes/Index", false);
        }

        protected void ddlCobertura_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlCobertura.SelectedValue, out int idCobertura))
                CargarPlanesPorCobertura(idCobertura);
            else
                CargarPlanesPorCobertura(0);
        }

        private int ExtraerIdPaciente()
        {
            string idStr = Request.QueryString["id-paciente"] ?? string.Empty;
            return int.TryParse(idStr, out int id) ? id : 0;
        }

        public void CargarCoberturas()
        {
            try
            {
                ddlCobertura.Items.Clear();
                ddlCobertura.Items.Add(new ListItem("Seleccione una cobertura...", ""));

                List<CoberturaDto> coberturas = new List<CoberturaDto>();

                if (ModoEdicion)
                {
                    coberturas = _servicioCobertura.Listar();

                }
                else
                {
                    coberturas = _servicioCobertura.Listar("activo");
                }

                if (coberturas == null || coberturas.Count == 0)
                {
                    ddlCobertura.Items.Clear();
                    ddlCobertura.Items.Add(new ListItem("No hay coberturas disponibles", ""));
                    ddlCobertura.Enabled = false;
                    btnGuardar.Enabled = false;
                    return;
                }

                foreach (var c in coberturas)
                {
                    ddlCobertura.Items.Add(new ListItem(c.Nombre, c.IdCobertura.ToString()));
                }

                ddlCobertura.Enabled = true;
                btnGuardar.Enabled = true;
            }
            catch (Exception ex)
            {
                ddlCobertura.Items.Clear();
                ddlCobertura.Items.Add(new ListItem("Error al cargar coberturas", ""));
                ddlCobertura.Enabled = false;
                btnGuardar.Enabled = false;

                Debug.WriteLine("Error al cargar coberturas: " + ex.Message);
            }
        }


        private void CargarPlanesPorCobertura(int idCobertura)
        {
            try
            {
                ddlPlan.Items.Clear();
                ddlPlan.Items.Add(new ListItem("Seleccione un plan...", ""));

                if (idCobertura <= 0)
                {
                    ddlPlan.Enabled = false;
                    return;
                }

                var planes = _servicioPlan.ListarPorCobertura(idCobertura, "activo");

                if (planes == null || planes.Count == 0)
                {
                    ddlPlan.Items.Clear();
                    ddlPlan.Items.Add(new ListItem("No hay planes disponibles", ""));
                    ddlPlan.Enabled = false;
                    return;
                }

                foreach (var p in planes)
                {
                    ddlPlan.Items.Add(new ListItem(p.Nombre, p.IdPlan.ToString()));
                }

                ddlPlan.Enabled = true;
            }
            catch (Exception ex)
            {
                ddlPlan.Items.Clear();
                ddlPlan.Items.Add(new ListItem("Error al cargar planes", ""));
                ddlPlan.Enabled = false;
                Debug.WriteLine("Error al cargar planes: " + ex.Message);
            }
        }

        private void CargarDetallePaciente(int idPaciente)
        {
            try
            {
                PacienteEdicionDto dto = _servicioPaciente.ObtenerPorId(idPaciente);
                if (dto == null)
                {
                    MensajeUiHelper.SetearYMostrar(this.Page,
                        "Paciente no encontrado",
                        "No se encontró el paciente solicitado.",
                        "Resultado",
                        VirtualPathUtility.ToAbsolute("~/Pages/Pacientes/Index"),
                        "abrirModalResultado");
                    return;
                }

                CargarDatosPaciente(dto);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al cargar paciente: " + ex.Message);
                MensajeUiHelper.SetearYMostrar(this.Page,
                    "Error inesperado",
                    "Ocurrió un error al intentar cargar el paciente.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Pacientes/Index"),
                    "abrirModalResultado");
            }
        }

        public void CargarDatosPaciente(PacienteEdicionDto dto)
        {
            try
            {
                ddlCobertura.AutoPostBack = false;

                CargarCoberturas();

                if (dto.IdCobertura.HasValue &&
                    ddlCobertura.Items.FindByValue(dto.IdCobertura.Value.ToString()) != null)
                {
                    ddlCobertura.SelectedValue = dto.IdCobertura.Value.ToString();
                    CargarPlanesPorCobertura(dto.IdCobertura.Value);
                }

                if (dto.IdPlan.HasValue &&
                    ddlPlan.Items.FindByValue(dto.IdPlan.Value.ToString()) != null)
                {
                    ddlPlan.SelectedValue = dto.IdPlan.Value.ToString();
                }

                txtNombre.Text = dto.Nombre;
                txtApellido.Text = dto.Apellido;
                txtDni.Text = dto.Dni;
                txtTelefono.Text = dto.Telefono;
                txtEmail.Text = dto.Email;
                txtFechaNacimiento.Text = dto.FechaNacimiento.ToString("yyyy-MM-dd");
                ddlGenero.SelectedValue = dto.Genero.ToString() ?? "";
                ddlEstado.SelectedValue = dto.Estado.ToString() ?? "A";
                ddlEstado.Enabled = true;

                ddlCobertura.AutoPostBack = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al cargar datos del paciente: " + ex.Message);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ModoEdicion)
            {
                ModificarPaciente();
            }
            else
            {
                CrearNuevoPaciente();
            }
        }


        private void ModificarPaciente()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CrearNuevoPaciente()
        {
            try
            {
                ValidarCamposFormulario();

                PacienteCreacionDto pacienteDto = new PacienteCreacionDto()
                {
                    Dni = txtDni.Text.Trim(),
                    FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text),
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Genero = ddlGenero.SelectedValue,
                    Telefono = txtTelefono.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Estado = ddlEstado.SelectedValue,
                    IdCobertura = int.Parse(ddlCobertura.SelectedValue),
                    IdPlan = string.IsNullOrEmpty(ddlPlan.SelectedValue) ? 0 : int.Parse(ddlPlan.SelectedValue)
                };

                _servicioPaciente.Crear(pacienteDto);

                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Paciente creado",
                    "El paciente se ha creado correctamente.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Pacientes/Index"),
                    "abrirModalResultado"
                );
            }
            catch (ArgumentException ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page,
                    "Dato inválido",
                    ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado");
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page,
                    "Operación no permitida",
                    ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado");
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page,
                    "Error inesperado",
                    "Ocurrió un error al registrar el paciente. " + ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado");
            }
        }

        private void ValidarCamposFormulario()
        {
            // validar superficialmente la estructura de los datos y que no hayan vacíos para los obligatorios

            if (!ValidadorCampos.EsTextoObligatorio(txtNombre.Text))
                throw new ArgumentException("El nombre es obligatorio");

            if (!ValidadorCampos.EsSoloLetrasYEspacios(txtNombre.Text))
                throw new ArgumentException("El nombre solo puede contener letras y espacios.");

            if (!ValidadorCampos.EsSoloLetrasYEspacios(txtApellido.Text))
                throw new ArgumentException("El apellido solo puede contener letras y espacios.");

            if (!ValidadorCampos.EsEnteroPositivo(txtDni.Text))
                throw new ArgumentException("El DNI debe ser numérico.");

            if (!DateTime.TryParse(txtFechaNacimiento.Text, out DateTime fechaNac)
                || !ValidadorCampos.EsFechaNacimientoValida(fechaNac))
                throw new ArgumentException("La fecha de nacimiento no es válida.");

            if (!ValidadorCampos.EsTelefonoValido(txtTelefono.Text))
                throw new ArgumentException("El teléfono no tiene un formato válido.");

            if (!string.IsNullOrEmpty(txtEmail.Text) && !ValidadorCampos.EsEmailValido(txtEmail.Text))
                throw new ArgumentException("El email no tiene un formato válido.");
        }


    }
}