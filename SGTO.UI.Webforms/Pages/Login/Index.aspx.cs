using SGTO.Negocio.DTOs.Seguridad;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Seguridad;
using SGTO.UI.Webforms.Seguridad;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Login
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly ServicioAutenticacion _servicioAutenticacion = new ServicioAutenticacion();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // si el usuario esta logueado, enviar al inicio
                if (SessionManager.EstaLogueado())
                {
                    RedirigirSegunPermisos(SessionManager.Usuario);
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // validar los required
            if (!Page.IsValid) return;

            try
            {
                // iniciar autenticacion
                UsuarioSesionDto usuarioSesion = _servicioAutenticacion.Autenticar(txtUsuario.Text.Trim(), txtPassword.Text.Trim());

                SessionManager.Usuario = usuarioSesion;

                RedirigirSegunPermisos(usuarioSesion);
            }
            catch (ExcepcionAutenticacion ex)
            {
                MostrarError(ex.Message);
            }
            catch (Exception)
            {

                MostrarError("Ocurrió un error inesperado al intentar ingresar. Intente nuevamente.");
            }
        }

        private void RedirigirSegunPermisos(UsuarioSesionDto usuario)
        {
            string urlDestino = NavegacionHelper.ObtenerUrlInicial(usuario);
            Response.Redirect(urlDestino, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void MostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            pnlError.Visible = true;
        }

    }
}