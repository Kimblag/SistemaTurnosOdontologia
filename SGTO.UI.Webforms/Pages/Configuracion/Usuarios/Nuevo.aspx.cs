using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Usuarios;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Utils;
using System;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Configuracion.Usuarios
{
    public partial class Nuevo : System.Web.UI.Page
    {

        private readonly EspecialidadService _servicioEspecialidad = new EspecialidadService();
        private readonly UsuarioService _servicioUsuario = new UsuarioService();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Master is MasterPages.SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Configuracion");
                master.EstablecerTituloSeccion(this.Page.Title);
            }

            if (!IsPostBack)
            {
                CalcularFechaMaximaValida();

                lblPaso.InnerText = "Paso 1 de 2 · Seleccione el rol del usuario.";
                CargarEspecialidades();
            }
        }

        protected void ddlRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool tieneRol = !string.IsNullOrEmpty(ddlRol.SelectedValue);

            panelCamposGenerales.Visible = tieneRol;
            panelAcciones.Visible = tieneRol;
            panelCamposMedico.Visible = (ddlRol.SelectedValue == "Médico");

            lblPaso.InnerText = tieneRol
                ? "Paso 2 de 2 · Complete los datos del usuario."
                : "Paso 1 de 2 · Seleccione el rol del usuario.";
        }

        private void CargarEspecialidades()
        {
            try
            {
                ddlEspecialidad.Items.Clear();
                ddlEspecialidad.Items.Add(new ListItem("Seleccione una especialidad...", ""));

                var especialidades = _servicioEspecialidad.ObtenerTodasDto("activas");

                if (especialidades == null || especialidades.Count == 0)
                {
                    ddlEspecialidad.Items.Clear();
                    ddlEspecialidad.Items.Add(new ListItem("No hay especialidades disponibles", ""));
                    ddlEspecialidad.Enabled = false;
                    btnGuardar.Enabled = false;
                    return;
                }

                foreach (var esp in especialidades)
                {
                    ddlEspecialidad.Items.Add(new ListItem(esp.Nombre, esp.IdEspecialidad.ToString()));
                }

                ddlEspecialidad.Enabled = true;
                btnGuardar.Enabled = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al cargar especialidades: " + ex.Message);
                ddlEspecialidad.Items.Clear();
                ddlEspecialidad.Items.Add(new ListItem("Error al cargar especialidades", ""));
                ddlEspecialidad.Enabled = false;
                btnGuardar.Enabled = false;
            }
        }


        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Configuracion/Usuarios/Index.aspx", false);
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Configuracion/Usuarios/Index.aspx", false);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCamposFormulario();

                UsuarioCrearDto usuarioDto = new UsuarioCrearDto()
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    NombreUsuario = txtNombreUsuario.Text.Trim(),
                    Password = txtPassword.Text,
                    ConfirmarPassword = txtConfirmarPassword.Text,
                    IdRol = ObtenerIdRol(ddlRol.SelectedValue),
                    Estado = ddlEstado.SelectedValue
                };

                MedicoCrearDto medicoDto = null;

                if (ddlRol.SelectedValue == "Médico")
                {
                    if (string.IsNullOrEmpty(ddlEspecialidad.SelectedValue))
                        throw new ArgumentException("Debe seleccionar una especialidad para el médico.");

                    medicoDto = new MedicoCrearDto()
                    {
                        Nombre = txtNombre.Text.Trim(),
                        Apellido = txtApellido.Text.Trim(),
                        NumeroDocumento = txtDni.Text.Trim(),
                        Genero = ddlGenero.SelectedValue,
                        FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text),
                        Telefono = txtTelefono.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Matricula = txtMatricula.Text.Trim(),
                        IdEspecialidad = int.Parse(ddlEspecialidad.SelectedValue),
                        Estado = ddlEstado.SelectedValue
                    };
                }

                _servicioUsuario.Crear(usuarioDto, medicoDto);

                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Usuario creado",
                    "El usuario se ha creado correctamente.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Configuracion/Usuarios/Index.aspx"),
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
                    "Ocurrió un error al registrar el usuario. " + ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado");
            }
        }


        private void ValidarCamposFormulario()
        {
            if (!ValidadorCampos.EsTextoObligatorio(txtNombre.Text))
                throw new ArgumentException("El nombre es obligatorio.");

            if (!ValidadorCampos.EsSoloLetrasYEspacios(txtNombre.Text))
                throw new ArgumentException("El nombre solo puede contener letras y espacios.");

            if (!ValidadorCampos.EsSoloLetrasYEspacios(txtApellido.Text))
                throw new ArgumentException("El apellido solo puede contener letras y espacios.");

            if (!ValidadorCampos.EsEmailValido(txtEmail.Text))
                throw new ArgumentException("El email no tiene un formato válido.");

            if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text))
                throw new ArgumentException("El nombre de usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(txtPassword.Text) || string.IsNullOrWhiteSpace(txtConfirmarPassword.Text))
                throw new ArgumentException("Debe ingresar y confirmar la contraseña.");

            if (txtPassword.Text != txtConfirmarPassword.Text)
                throw new ArgumentException("Las contraseñas no coinciden.");

            if (ddlRol.SelectedValue == "Médico")
            {
                if (!ValidadorCampos.EsEnteroPositivo(txtDni.Text))
                    throw new ArgumentException("El DNI debe ser numérico.");

                if (!DateTime.TryParse(txtFechaNacimiento.Text, out DateTime fechaNac))
                    throw new ArgumentException("Debe ingresar una fecha de nacimiento válida.");

                if (!ValidadorCampos.EsTelefonoValido(txtTelefono.Text))
                    throw new ArgumentException("El teléfono no tiene un formato válido.");

                if (string.IsNullOrWhiteSpace(txtMatricula.Text))
                    throw new ArgumentException("La matrícula es obligatoria para el médico.");
            }
        }

        private int ObtenerIdRol(string nombreRol)
        {
            switch (nombreRol)
            {
                case "Administrador": return 1;
                case "Recepcionista": return 2;
                case "Médico": return 3;
                default: throw new ExcepcionReglaNegocio("Rol no reconocido.");
            }
        }


        private void CalcularFechaMaximaValida()
        {
            // fecha máxima válida: mínimo 20 años
            DateTime fechaMaxima = DateTime.Now.AddYears(-20);
            rvFechaNac.MaximumValue = fechaMaxima.ToString("yyyy-MM-dd");
        }



    }
}