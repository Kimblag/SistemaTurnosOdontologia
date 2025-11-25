using SGTO.Negocio.Seguridad;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Seguridad;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Configuracion
{
    public partial class Configuracion : System.Web.UI.Page
    {
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.EstaLogueado())
            {
                Response.Redirect("~/Pages/Login/Index.aspx");
                return;
            }

            if (!_servicioAutorizacion.TienePermiso(SessionManager.Usuario, "CONFIGURACION", "VER"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Configuracion");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Panel de administración de seguridad y parámetros globales.");
            }

            if (!IsPostBack)
            {
                ConfigurarVisibilidadCards();
            }
        }
        private void ConfigurarVisibilidadCards()
        {
            var usuario = SessionManager.Usuario;

            bool verUsuarios = _servicioAutorizacion.TienePermiso(usuario, "USUARIOS", "VER");
            bool verRoles = _servicioAutorizacion.TienePermiso(usuario, "ROLES", "VER");
            bool verParametros = _servicioAutorizacion.TienePermiso(usuario, "PARAMETROSISTEMA", "VER");

            colUsuarios.Visible = verUsuarios;
            colRoles.Visible = verRoles;
            colParametros.Visible = verParametros;
            bool flowControl = AplicarCssCardsVisibles(verUsuarios, verRoles, verParametros);
            if (!flowControl)
            {
                return;
            }
        }

        private bool AplicarCssCardsVisibles(bool verUsuarios, bool verRoles, bool verParametros)
        {
            int cantidadVisibles = (verUsuarios ? 1 : 0) + (verRoles ? 1 : 0) + (verParametros ? 1 : 0);

            if (cantidadVisibles == 0) return false;

            string claseCssDinamica = "";

            switch (cantidadVisibles)
            {
                case 3:
                    claseCssDinamica = "col-12 col-md-6 col-lg-4";
                    break;
                case 2:
                    claseCssDinamica = "col-12 col-md-6";
                    break;
                case 1:
                    claseCssDinamica = "col-12 w";
                    break;
            }

            if (verUsuarios)
                colUsuarios.Attributes["class"] = claseCssDinamica;

            if (verRoles)
                colRoles.Attributes["class"] = claseCssDinamica;

            if (verParametros)
                colParametros.Attributes["class"] = claseCssDinamica;
            return true;
        }
    }
}