using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs.Medicos;
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
    public partial class Editar : System.Web.UI.Page
    {

        private readonly UsuarioService _servicioUsuario = new UsuarioService();
        private readonly EspecialidadService _servicioEspecialidad = new EspecialidadService();

        private int IdUsuario
        {
            get
            {
                int id;
                if (int.TryParse(Request.QueryString["id-usuario"], out id))
                {
                    return id;
                }
                else
                {
                    return 0;
                }
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Configuracion");
                master.EstablecerTituloSeccion(this.Page.Title);
            }

            if (!IsPostBack)
            {
                if (IdUsuario == 0)
                {
                    MensajeUiHelper.SetearYMostrar(this.Page,
                       "Usuario no encontrado",
                       "No se pudo identificar el usuario a editar.",
                       "Resultado",
                       VirtualPathUtility.ToAbsolute("~/Pages/Configuracion/Usuarios/Index.aspx"),
                       "abrirModalResultado");
                    return;
                }

                CargarEspecialidades();
                CargarUsuario();
            }
        }

        private void CargarUsuario()
        {
            try
            {
                UsuarioDetalleDto dto = _servicioUsuario.ObtenerDetalle(IdUsuario);
                if (dto == null)
                    throw new ExcepcionReglaNegocio("No se encontró el usuario solicitado.");

                txtNombre.Text = dto.Nombre;
                txtApellido.Text = dto.Apellido;
                txtEmail.Text = dto.Email;
                txtNombreUsuario.Text = dto.NombreUsuario;
                ddlRol.SelectedValue = dto.IdRol.ToString();
                ddlEstado.SelectedValue = dto.Estado == "Activo" ? "A" : "I";

                if (dto.Medico != null)
                {
                    MedicoDetalleDto medico = dto.Medico;
                    panelCamposMedico.Visible = true;
                    txtDni.Text = medico.NumeroDocumento;
                    ddlGenero.SelectedValue = medico.Genero;
                    txtFechaNacimiento.Text = medico.FechaNacimiento.ToString("yyyy-MM-dd");
                    txtTelefono.Text = medico.Telefono;
                    txtMatricula.Text = medico.Matricula;
                    ddlEspecialidad.SelectedValue = medico.IdEspecialidad > 0 ? medico.IdEspecialidad.ToString() : "";
                }
                else
                {
                    panelCamposMedico.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al cargar usuario: " + ex.Message);
                MensajeUiHelper.SetearYMostrar(this.Page,
                    "Error al cargar",
                    "No se pudieron cargar los datos del usuario.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Configuracion/Usuarios/Index.aspx"),
                    "abrirModalResultado");
            }
        }

        private void CargarEspecialidades()
        {
            ddlEspecialidad.Items.Clear();
            ddlEspecialidad.Items.Add(new ListItem("Seleccione una especialidad...", ""));
            try
            {
                var especialidades = _servicioEspecialidad.ObtenerTodasDto("activas");
                foreach (var esp in especialidades)
                    ddlEspecialidad.Items.Add(new ListItem(esp.Nombre, esp.IdEspecialidad.ToString()));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al cargar especialidades: " + ex.Message);
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

            if (ddlRol.SelectedValue == "3")
            {
                if (!ValidadorCampos.EsEnteroPositivo(txtDni.Text))
                    throw new ArgumentException("El DNI debe ser numérico.");

                if (!DateTime.TryParse(txtFechaNacimiento.Text, out DateTime fechaNac))
                    throw new ArgumentException("Debe ingresar una fecha de nacimiento válida.");

                if (fechaNac > DateTime.Now.AddYears(-20))
                    throw new ArgumentException("El médico debe tener al menos 20 años.");

                if (!ValidadorCampos.EsTelefonoValido(txtTelefono.Text))
                    throw new ArgumentException("El teléfono no tiene un formato válido.");

                if (string.IsNullOrWhiteSpace(txtMatricula.Text))
                    throw new ArgumentException("La matrícula es obligatoria.");
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Configuracion/Usuarios/Index.aspx", false);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Configuracion/Usuarios/Index.aspx", false);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCamposFormulario();

                var usuarioDto = new UsuarioEdicionDto
                {
                    IdUsuario = IdUsuario,
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    NombreUsuario = txtNombreUsuario.Text.Trim(),
                    Password = txtPassword.Text,
                    IdRol = int.Parse(ddlRol.SelectedValue),
                    Estado = ddlEstado.SelectedValue == "A" ? "Activo" : "Inactivo"
                };

                MedicoEdicionDto medicoDto = null;

                if (ddlRol.SelectedValue == "3") // rol médico
                {
                    medicoDto = new MedicoEdicionDto
                    {
                        IdUsuario = IdUsuario,
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

                _servicioUsuario.Editar(usuarioDto, medicoDto);

                MensajeUiHelper.SetearYMostrar(this.Page,
                    "Cambios guardados",
                    "El usuario se actualizó correctamente.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Configuracion/Usuarios/Index.aspx"),
                    "abrirModalResultado");
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
                    "Ocurrió un error al actualizar el usuario. " + ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado");
            }
        }
    }
}