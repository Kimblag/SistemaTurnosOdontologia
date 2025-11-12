using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.MasterPages
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MostrarBotonVolver();
        }

        public void EstablecerTituloSeccion(string tituloSeccionActiva)
        {
            TituloSeccion.InnerText = tituloSeccionActiva;
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


        private void MostrarBotonVolver()
        {
            string urlActual = Request.Url.AbsolutePath.ToLowerInvariant();
            string[] segmentos = Request.Url.Segments;

            for (int i = 0; i < segmentos.Length; i++)
                segmentos[i] = segmentos[i].Trim('/').ToLowerInvariant();

            if (!urlActual.Contains("/pages/configuracion/"))
            {
                btnVolver.Visible = false;
                return;
            }

            if (urlActual.EndsWith("/pages/configuracion/index"))
            {
                btnVolver.Visible = false;
                return;
            }

            btnVolver.Visible = true;

            if (segmentos.Length >= 5)
            {
                string modulo = segmentos[3].TrimEnd('/');
                string pagina = segmentos[4].TrimEnd('/');

                if (pagina == "nuevo" || pagina == "editar" || pagina == "detalles")
                {
                    btnVolver.NavigateUrl = $"~/Pages/Configuracion/{modulo}/Index.aspx";
                }
                else if (pagina == "index")
                {
                    btnVolver.NavigateUrl = "~/Pages/Configuracion/Index.aspx";
                }
                else
                {
                    btnVolver.NavigateUrl = $"~/Pages/Configuracion/{modulo}/Index.aspx";
                }
            }
            else
            {
                btnVolver.NavigateUrl = "~/Pages/Configuracion/Index.aspx";
            }
        }

    }
}