using SGTO.Negocio.DTOs.Seguridad;
using SGTO.Negocio.Seguridad;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.Seguridad;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.MasterPages
{
    public partial class SiteMaster : MasterPage
    {
        private readonly ParametroService _servicioParametros = new ParametroService();
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.EstaLogueado())
            {
                Response.Redirect("~/Pages/Login/Index");
                return;
            }

            if (!IsPostBack)
            {
                CargarNombreClinica();
                CargarDatosUsuarioHeader();
                ConfigurarMenu(); // mostrar menú segun el permiso
            }
        }


        private void CargarDatosUsuarioHeader()
        {
            UsuarioSesionDto usuario = SessionManager.Usuario;
            if (usuario != null)
            {
                NombreUsuario.InnerText = usuario.NombreCompleto;
                RolUsuario.InnerText = usuario.NombreRol;
            }
        }

        private void ConfigurarMenu()
        {
            UsuarioSesionDto usuario = SessionManager.Usuario;

            // volvermos a validar si hay un usuario, sino ocultamos todo.
            if (usuario == null)
            {
                OcultarTodoElMenu();
                return;
            }

            // verificar el permiso de lectura para mostrar la opcion del menu
            MenuTurnos.Visible = _servicioAutorizacion.TienePermiso(usuario, "TURNOS", "VER");
            MenuPacientes.Visible = _servicioAutorizacion.TienePermiso(usuario, "PACIENTES", "VER");

            MenuMedicos.Visible = _servicioAutorizacion.TienePermiso(usuario, "MEDICOS", "VER");

            MenuCoberturas.Visible = _servicioAutorizacion.TienePermiso(usuario, "COBERTURAS", "VER");
            MenuEspecialidades.Visible = _servicioAutorizacion.TienePermiso(usuario, "ESPECIALIDADES", "VER");
            MenuTratamientos.Visible = _servicioAutorizacion.TienePermiso(usuario, "TRATAMIENTOS", "VER");
            MenuReportes.Visible = _servicioAutorizacion.TienePermiso(usuario, "REPORTES", "VER");

            // para los admins
            MenuConfiguracion.Visible = _servicioAutorizacion.TienePermiso(usuario, "CONFIGURACION", "VER")
                                        || _servicioAutorizacion.TienePermiso(usuario, "USUARIOS", "VER");
        }

        private void OcultarTodoElMenu()
        {
            MenuDashboard.Visible = false;
            MenuTurnos.Visible = false;
            MenuPacientes.Visible = false;
            MenuMedicos.Visible = false;
            MenuCoberturas.Visible = false;
            MenuEspecialidades.Visible = false;
            MenuTratamientos.Visible = false;
            MenuReportes.Visible = false;
            MenuConfiguracion.Visible = false;
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            SessionManager.CerrarSesion();
            Response.Redirect("~/Pages/Login/Index");
        }


        public void EstablecerTituloSeccion(string tituloSeccionActiva)
        {
            TituloSeccion.InnerText = tituloSeccionActiva;
        }

        public void EstablecerSubtituloSeccion(string subtituloSeccionActiva)
        {
            SubtituloSeccion.InnerText = subtituloSeccionActiva;
        }

        private string ObtenerClasesDefaultMenu()
        {
            return "d-flex align-items-center gap-3 px-4 py-2 rounded text-body fw-medium text-decoration-none hover-bg";
        }

        private string NormalizarTexto(string texto)
        {
            //metodo para normalizar los titulos de las secciones que vienen con acentos
            if (string.IsNullOrEmpty(texto)) return string.Empty;

            //quitar acentos y convertir a minúsculas
            var normalized = texto.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public void EstablecerOpcionMenuActiva(string opcionActiva)
        {
            MenuDashboard.Attributes["class"] = ObtenerClasesDefaultMenu();
            MenuTurnos.Attributes["class"] = ObtenerClasesDefaultMenu();
            MenuPacientes.Attributes["class"] = ObtenerClasesDefaultMenu();
            MenuMedicos.Attributes["class"] = ObtenerClasesDefaultMenu();
            MenuCoberturas.Attributes["class"] = ObtenerClasesDefaultMenu();
            MenuEspecialidades.Attributes["class"] = ObtenerClasesDefaultMenu();
            MenuTratamientos.Attributes["class"] = ObtenerClasesDefaultMenu();
            MenuReportes.Attributes["class"] = ObtenerClasesDefaultMenu();
            MenuConfiguracion.Attributes["class"] = ObtenerClasesDefaultMenu();

            string opcionActivaNormalizada = NormalizarTexto(opcionActiva);
            string claseActiva = "menu-item active";

            switch (opcionActivaNormalizada)
            {
                case "inicio":
                    MenuDashboard.Attributes["class"] = claseActiva;
                    break;
                case "turnos":
                    MenuTurnos.Attributes["class"] = claseActiva;
                    break;
                case "pacientes":
                    MenuPacientes.Attributes["class"] = claseActiva;
                    break;
                case "medicos":
                    MenuMedicos.Attributes["class"] = claseActiva;
                    break;
                case "coberturas":
                    MenuCoberturas.Attributes["class"] = claseActiva;
                    break;
                case "especialidades":
                    MenuEspecialidades.Attributes["class"] = claseActiva;
                    break;
                case "tratamientos":
                    MenuTratamientos.Attributes["class"] = claseActiva;
                    break;
                case "reportes":
                    MenuReportes.Attributes["class"] = claseActiva;
                    break;
                case "configuracion":
                    MenuConfiguracion.Attributes["class"] = claseActiva;
                    break;
            }
        }


        private void CargarNombreClinica()
        {
            try
            {
                var parametros = _servicioParametros.Obtener();
                string nombreClinica = parametros?.NombreClinica;

                NombreClinica.InnerText = string.IsNullOrWhiteSpace(nombreClinica)
                    ? "SGTO"
                    : nombreClinica.Trim();
            }
            catch (Exception)
            {
                NombreClinica.InnerText = "Clínica Odontológica";
            }
        }


        public void ConfigurarBotonVolver(bool mostrar, string urlDestino = "")
        {
            btnVolver.Visible = mostrar;
            if (mostrar && !string.IsNullOrEmpty(urlDestino))
            {
                btnVolver.NavigateUrl = urlDestino;
            }
        }


    }
}