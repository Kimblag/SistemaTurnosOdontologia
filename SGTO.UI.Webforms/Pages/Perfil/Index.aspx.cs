using SGTO.Negocio.DTOs.Usuarios;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.Seguridad;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Perfil
{
    public partial class MiPerfil : System.Web.UI.Page
    {
        private readonly UsuarioService _usuarioService = new UsuarioService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.EstaLogueado())
            {
                Response.Redirect("~/Pages/Login/Index.aspx");
                return;
            }

            if (!IsPostBack)
            {
                var master = Master as MasterPages.SiteMaster;
                if (master != null)
                {
                    master.ConfigurarBotonVolver(false, "");
                    master.EstablecerTituloSeccion("Mi Perfil");
                    master.EstablecerSubtituloSeccion("Gestión de datos personales y seguridad");
                }

                CargarDatos();
            }
        }

        private void CargarDatos()
        {
            try
            {
                int idUsuario = SessionManager.Usuario.IdUsuario;
                UsuarioDetalleDto usuario = _usuarioService.ObtenerDetalle(idUsuario);

                if (usuario != null)
                {
                    lblNombreCompleto.Text = $"{usuario.Nombre} {usuario.Apellido}";
                    lblRol.Text = usuario.Rol;
                    lblUsuario.Text = usuario.NombreUsuario;
                    lblEmail.Text = usuario.Email;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar perfil: " + ex.Message, true);
            }
        }

        protected void btnGuardarPass_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                int idUsuario = SessionManager.Usuario.IdUsuario;
                _usuarioService.CambiarPassword(idUsuario, txtPassActual.Text, txtPassNueva.Text);

                MostrarMensaje("La contraseña ha sido actualizada correctamente.", false);

                txtPassActual.Text = string.Empty;
                txtPassNueva.Text = string.Empty;
                txtPassConfirmar.Text = string.Empty;
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MostrarMensaje(ex.Message, true);
            }
            catch (Exception)
            {
                MostrarMensaje("Ocurrió un error inesperado al intentar cambiar la contraseña.", true);
            }
        }

        private void MostrarMensaje(string texto, bool esError)
        {
            lblMensaje.Text = texto;
            pnlMensaje.CssClass = "alert alert-dismissible fade show mb-4 " + (esError ? "alert-danger" : "alert-success");
            pnlMensaje.Visible = true;
        }
    }
}
