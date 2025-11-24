
using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Medicos;
using SGTO.Negocio.Seguridad;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Seguridad;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Medicos
{
    public partial class Medicos : System.Web.UI.Page
    {
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();
        private readonly MedicoService _servicioMedico = new MedicoService();
        private readonly EspecialidadService _servicioEspecialidad = new EspecialidadService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.EstaLogueado())
            {
                Response.Redirect("~/Pages/Login/Index.aspx");
                return;
            }

            if (!_servicioAutorizacion.TienePermiso(SessionManager.Usuario, "MEDICOS", "VER"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Medicos");
                master.EstablecerTituloSeccion("Staff Médico");
                master.EstablecerSubtituloSeccion("Consulte la nómina de profesionales, sus especialidades y días de atención.");
            }

            if (!IsPostBack)
            {
                CargarCombos();
                CargarMedicosConFiltros();
            }
        }


        private void CargarCombos()
        {
            try
            {
                ddlEspecialidad.Items.Clear();
                ddlEspecialidad.Items.Add(new ListItem("Todas las especialidades", ""));

                var especialidades = _servicioEspecialidad.Listar("activas");
                foreach (var esp in especialidades)
                {
                    ddlEspecialidad.Items.Add(new ListItem(esp.Nombre, esp.Nombre));
                }
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "No se pudieron cargar las especialidades: " + ex.Message);
            }
        }

        private bool EsCoincidenciaDeBusqueda(MedicoListadoDto medico, string[] palabrasClave)
        {
            // creé este método para hacer búsquedas por tokens eb kugar de usar un simple contains
            // ya que había una inconsistencia al buscar.
            // Por ejemplo en el listado se ve Blandon Kim, pero si buscamos "kim blandon"
            // el contains no lo ubica porque compara siguiendo el orden exacto de los caracteres.

            if (palabrasClave == null || palabrasClave.Length == 0)
                return true;

            string dniMed = medico.Dni ?? "";
            string matricula = medico.Matricula ?? "";
            string nombreMed = ValidadorCampos.NormalizarTexto(medico.NombreCompleto) ?? "";

            // agregar espacios entre cada token para evitar errores de palabras pegadas a la siguiente
            string datosMedicoConcatenados = string.Format("{0} {1} {2}", nombreMed, dniMed, matricula);

            // se verifica que todos los tokens existan
            foreach (string palabra in palabrasClave)
            {
                //si falta al menos una, ya no es coincidencia
                if (!datosMedicoConcatenados.Contains(palabra))
                {
                    return false;
                }
            }
            return true;
        }
        private void CargarMedicosConFiltros()
        {
            List<MedicoListadoDto> todosLosMedicos = new List<MedicoListadoDto>();
            List<MedicoListadoDto> listaFiltrada = new List<MedicoListadoDto>();

            try
            {
                todosLosMedicos = _servicioMedico.Listar();
            }
            catch (Exception ex)
            {
                gvMedicos.DataSource = null;
                gvMedicos.DataBind();
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "No se pudo cargar la lista de médicos. " + ex.Message);
                return;
            }

            string textoBuscar = ValidadorCampos.NormalizarTexto(txtBuscar.Text.Trim());
            string[] palabrasClave = string.IsNullOrEmpty(textoBuscar)
                    ? new string[0]
                    : textoBuscar.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string especialidadSeleccionada = ddlEspecialidad.SelectedValue;
            string estadoSeleccionado = ddlEstado.SelectedValue;

            foreach (MedicoListadoDto m in todosLosMedicos)
            {
                bool cumple = true;
                bool coincideTexto = EsCoincidenciaDeBusqueda(m, palabrasClave);

                if (cumple && !string.IsNullOrEmpty(especialidadSeleccionada))
                {
                    bool tieneLaEspecialidad = false;

                    if (m.NombresEspecialidades != null)
                    {
                        foreach (string espNombre in m.NombresEspecialidades)
                        {
                            if (espNombre.Equals(especialidadSeleccionada, StringComparison.OrdinalIgnoreCase))
                            {
                                tieneLaEspecialidad = true;
                                break;
                            }
                        }
                    }

                    if (!tieneLaEspecialidad)
                    {
                        cumple = false;
                    }
                }

                if (cumple && !string.IsNullOrEmpty(estadoSeleccionado))
                {
                    bool esActivoDto = m.Estado != null && m.Estado.Trim().ToLower().StartsWith("act");

                    bool buscaActivos = estadoSeleccionado == "Activo";
                    bool buscaInactivos = estadoSeleccionado == "Inactivo";

                    if (buscaActivos && !esActivoDto) cumple = false;
                    if (buscaInactivos && esActivoDto) cumple = false;
                }

                if (cumple && coincideTexto)
                {
                    listaFiltrada.Add(m);
                }
            }

            gvMedicos.DataSource = listaFiltrada;
            gvMedicos.DataBind();
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            gvMedicos.PageIndex = 0;
            CargarMedicosConFiltros();
        }


        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = string.Empty;
            ddlEspecialidad.SelectedIndex = 0;
            ddlEstado.SelectedValue = "Activo";

            gvMedicos.PageIndex = 0;
            CargarMedicosConFiltros();
        }


        protected void gvMedicos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMedicos.PageIndex = e.NewPageIndex;
            CargarMedicosConFiltros();
        }


        protected void gvMedicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Ver")
            {
                if (int.TryParse(e.CommandArgument.ToString(), out int idMedico))
                {
                    Response.Redirect($"~/Pages/Medicos/Detalle?id-medico={idMedico}", false);
                }
            }
        }


        protected void gvMedicos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MedicoListadoDto medicoDto = (MedicoListadoDto)e.Row.DataItem;
                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");

                if (lblEstado != null && medicoDto != null)
                {
                    bool activo = medicoDto.Estado.ToLower().StartsWith("act");

                    if (activo)
                    {
                        lblEstado.Attributes["class"] = "badge badge-success";
                        lblEstado.InnerText = "Activo";
                    }
                    else
                    {
                        lblEstado.Attributes["class"] = "badge badge-warning";
                        lblEstado.InnerText = "Inactivo";
                    }
                }
            }
        }


    }
}