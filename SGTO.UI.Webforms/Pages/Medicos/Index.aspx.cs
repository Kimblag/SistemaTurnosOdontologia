
using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Medicos;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
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

        private readonly MedicoService _servicioMedico = new MedicoService();

        private readonly EspecialidadService _servicioEspecialidad = new EspecialidadService();

        private const string KEY_MEDICO_BUSQUEDA = "FiltroMedicoBusqueda";
        private const string KEY_MEDICO_CAMPO = "FiltroMedicoCampo";
        private const string KEY_MEDICO_CRITERIO = "FiltroMedicoCriterio";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Medicos");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Consulte la nómina de profesionales, sus especialidades y días de atención.");
            }
            if (!IsPostBack)
            {
                txtBuscar.Text = Session[KEY_MEDICO_BUSQUEDA] as string ?? string.Empty;

                string campo = Session[KEY_MEDICO_CAMPO] as string;
                if (!string.IsNullOrEmpty(campo))
                {
                    if (ddlCampo.Items.FindByValue(campo) != null)
                    {
                        ddlCampo.SelectedValue = campo;
                        CargarCriterios(campo);
                    }
                }

                string criterio = Session[KEY_MEDICO_CRITERIO] as string;
                if (!string.IsNullOrEmpty(criterio) && ddlCriterio.Items.FindByValue(criterio) != null)
                {
                    ddlCriterio.SelectedValue = criterio;
                    ddlCriterio.Enabled = true;
                }

                AplicarFiltros();
            }
        }

        private void CargarCriterios(string campo)
        {
            ddlCriterio.Items.Clear();
            ddlCriterio.Enabled = false;

            if (string.IsNullOrEmpty(campo))
            {
                ddlCriterio.Items.Add(new ListItem("Seleccione un criterio", ""));
                return;
            }

            campo = campo.ToLower();
            ddlCriterio.Items.Add(new ListItem("Seleccione un criterio", ""));
            ddlCriterio.Enabled = true;

            try
            {
                if (campo == "estado")
                {
                    ddlCriterio.Items.Add(new ListItem("Activo", "A"));
                    ddlCriterio.Items.Add(new ListItem("Inactivo", "I"));
                }
                else if (campo == "especialidad")
                {
                    List<EspecialidadDto> especialidades = _servicioEspecialidad.Listar("activas");
                    foreach (EspecialidadDto esp in especialidades)
                    {
                        ddlCriterio.Items.Add(new ListItem(esp.Nombre, esp.Nombre));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error cargando criterios: " + ex.Message);
                ddlCriterio.Items.Add(new ListItem("Error al cargar", ""));
            }

            if (ddlCriterio.Items.Count > 0)
                ddlCriterio.SelectedIndex = 0;
        }


        private void AplicarFiltros()
        {
            string textoBusqueda = txtBuscar.Text.Trim();
            string campo = ddlCampo.SelectedValue;
            string criterio = ddlCriterio.SelectedValue;

            // guardar en sesion los campos
            Session[KEY_MEDICO_BUSQUEDA] = string.IsNullOrEmpty(textoBusqueda) ? null : textoBusqueda;
            Session[KEY_MEDICO_CAMPO] = string.IsNullOrEmpty(campo) ? null : campo;
            Session[KEY_MEDICO_CRITERIO] = string.IsNullOrEmpty(criterio) ? null : criterio;

            List<MedicoListadoDto> listaCompleta = new List<MedicoListadoDto>();

            try
            {
                listaCompleta = _servicioMedico.Listar();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al listar médicos: " + ex.Message);
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "No se pudo cargar la lista de médicos.", "Cerrar", null, "abrirModalResultado");
                return;
            }

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                string texto = ValidadorCampos.NormalizarTexto(textoBusqueda);
                List<MedicoListadoDto> filtrada = new List<MedicoListadoDto>();

                foreach (MedicoListadoDto m in listaCompleta)
                {
                    bool coincideNombre = !string.IsNullOrEmpty(m.NombreCompleto) && ValidadorCampos.NormalizarTexto(m.NombreCompleto).Contains(texto);
                    bool coincideDni = !string.IsNullOrEmpty(m.Dni) && m.Dni.Contains(texto);
                    bool coincideMatricula = !string.IsNullOrEmpty(m.Matricula) && m.Matricula.Contains(texto);

                    if (coincideNombre || coincideDni || coincideMatricula)
                    {
                        filtrada.Add(m);
                    }
                }
                listaCompleta = filtrada;
            }

            if (!string.IsNullOrEmpty(campo) && !string.IsNullOrEmpty(criterio))
            {
                List<MedicoListadoDto> filtrada = new List<MedicoListadoDto>();

                if (campo == "Estado")
                {
                    foreach (MedicoListadoDto m in listaCompleta)
                    {
                        string estadoDto = m.Estado.ToLower().StartsWith("act") ? "A" : "I";
                        if (estadoDto == criterio)
                        {
                            filtrada.Add(m);
                        }
                    }
                }
                else if (campo == "Especialidad")
                {
                    foreach (MedicoListadoDto m in listaCompleta)
                    {
                        bool tieneEspecialidad = false;
                        foreach (string esp in m.NombresEspecialidades)
                        {
                            if (esp.Equals(criterio, StringComparison.OrdinalIgnoreCase))
                            {
                                tieneEspecialidad = true;
                                break;
                            }
                        }

                        if (tieneEspecialidad)
                        {
                            filtrada.Add(m);
                        }
                    }
                }
                listaCompleta = filtrada;
            }

            gvMedicos.DataSource = listaCompleta;
            gvMedicos.DataBind();
        }

        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string campo = ddlCampo.SelectedValue;
            Session[KEY_MEDICO_CAMPO] = string.IsNullOrEmpty(campo) ? null : campo;

            CargarCriterios(campo);

            Session[KEY_MEDICO_CRITERIO] = null;
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }


        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Session[KEY_MEDICO_BUSQUEDA] = null;
            Session[KEY_MEDICO_CAMPO] = null;
            Session[KEY_MEDICO_CRITERIO] = null;

            txtBuscar.Text = string.Empty;
            ddlCampo.SelectedIndex = 0;
            ddlCriterio.Items.Clear();
            ddlCriterio.Items.Add(new ListItem("Seleccione un criterio", ""));
            ddlCriterio.Enabled = false;

            AplicarFiltros();
        }

        protected void gvMedicos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMedicos.PageIndex = e.NewPageIndex;
            AplicarFiltros();
        }

        protected void gvMedicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Ver")
            {
                if (int.TryParse(e.CommandArgument.ToString(), out int idMedico))
                {
                    Response.Redirect($"~/Pages/Medicos/Detalle.aspx?id-medico={idMedico}", false);
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