using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
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

namespace SGTO.UI.Webforms.Pages.Tratamientos
{
    public partial class Tratamientos : System.Web.UI.Page
    {
        private readonly TratamientoService _tratamientoService = new TratamientoService();
        private readonly TurnoService _turnoService = new TurnoService();
        private readonly EspecialidadService _especialidadService = new EspecialidadService();

        private const string KEY_BUSQUEDA = "FiltroTratamientoBusqueda";
        private const string KEY_CAMPO = "FiltroTratamientoCampo";
        private const string KEY_CRITERIO = "FiltroTratamientoCriterio";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Tratamientos");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Catálogo de prestaciones odontológicas y costos base por especialidad.");
                if (!IsPostBack)
                {
                    txtBuscar.Text = Session[KEY_BUSQUEDA] as string ?? string.Empty;

                    string campo = Session[KEY_CAMPO] as string;
                    if (!string.IsNullOrEmpty(campo))
                    {
                        if (ddlCampo.Items.FindByValue(campo) != null)
                        {
                            ddlCampo.SelectedValue = campo;
                            CargarCriterios(campo);
                        }
                    }

                    string criterio = Session[KEY_CRITERIO] as string;
                    if (!string.IsNullOrEmpty(criterio) && ddlCriterio.Items.FindByValue(criterio) != null)
                    {
                        ddlCriterio.SelectedValue = criterio;
                        ddlCriterio.Enabled = true;
                    }

                    AplicarFiltros();

                    MensajeUiHelper.MostrarModal(this.Page);
                }
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
                    var especialidades = _especialidadService.Listar();
                    foreach (var esp in especialidades)
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

            Session[KEY_BUSQUEDA] = string.IsNullOrEmpty(textoBusqueda) ? null : textoBusqueda;
            Session[KEY_CAMPO] = string.IsNullOrEmpty(campo) ? null : campo;
            Session[KEY_CRITERIO] = string.IsNullOrEmpty(criterio) ? null : criterio;

            List<TratamientoDto> listaCompleta = new List<TratamientoDto>();

            try
            {
                listaCompleta = _tratamientoService.Listar();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al listar tratamientos: " + ex.Message);
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "No se pudo cargar la lista. " + ex.Message, "Cerrar", null, "abrirModalResultado");
                return;
            }

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                string texto = ValidadorCampos.NormalizarTexto(textoBusqueda);
                List<TratamientoDto> filtrada = new List<TratamientoDto>();

                foreach (var t in listaCompleta)
                {
                    string nombreNorm = ValidadorCampos.NormalizarTexto(t.Nombre);
                    string descNorm = ValidadorCampos.NormalizarTexto(t.Descripcion);

                    if ((!string.IsNullOrEmpty(nombreNorm) && nombreNorm.Contains(texto)) ||
                        (!string.IsNullOrEmpty(descNorm) && descNorm.Contains(texto)))
                    {
                        filtrada.Add(t);
                    }
                }
                listaCompleta = filtrada;
            }

            if (!string.IsNullOrEmpty(campo) && !string.IsNullOrEmpty(criterio))
            {
                List<TratamientoDto> filtrada = new List<TratamientoDto>();

                if (campo == "Estado")
                {
                    foreach (var t in listaCompleta)
                    {
                        string estadoLetra = t.Estado.ToLower().StartsWith("act") ? "A" : "I";
                        if (estadoLetra == criterio)
                        {
                            filtrada.Add(t);
                        }
                    }
                }
                else if (campo == "Especialidad")
                {
                    foreach (var t in listaCompleta)
                    {
                        if (!string.IsNullOrEmpty(t.NombreEspecialidad) &&
                            t.NombreEspecialidad.Equals(criterio, StringComparison.OrdinalIgnoreCase))
                        {
                            filtrada.Add(t);
                        }
                    }
                }
                listaCompleta = filtrada;
            }

            gvTratamientos.DataSource = listaCompleta;
            gvTratamientos.DataBind();
        }


        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string campo = ddlCampo.SelectedValue;
            Session[KEY_CAMPO] = string.IsNullOrEmpty(campo) ? null : campo;
            CargarCriterios(campo);
            Session[KEY_CRITERIO] = null;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Session[KEY_BUSQUEDA] = null;
            Session[KEY_CAMPO] = null;
            Session[KEY_CRITERIO] = null;

            txtBuscar.Text = string.Empty;
            ddlCampo.SelectedIndex = 0;
            ddlCriterio.Items.Clear();
            ddlCriterio.Items.Add(new ListItem("Seleccione un criterio", ""));
            ddlCriterio.Enabled = false;

            AplicarFiltros();
        }

        protected void gvTratamientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTratamientos.PageIndex = e.NewPageIndex;
            AplicarFiltros();
        }

        protected void gvTratamientos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var tratamientoDto = (TratamientoDto)e.Row.DataItem;
                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");

                if (lblEstado != null && tratamientoDto != null)
                {
                    if (tratamientoDto.Estado.ToLower() == "Activo".ToLower())
                    {
                        lblEstado.Attributes["class"] = "badge badge-success";
                    }
                    else
                    {
                        lblEstado.Attributes["class"] = "badge badge-warning";
                    }
                }
            }
        }


        protected void gvTratamientos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idTratamiento = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Pages/Tratamientos/Editar?id-tratamiento={idTratamiento}", false);
            }
        }

        protected void btnNuevoTratamiento_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Tratamientos/Nuevo", false);
        }


        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            int idTratamiento = int.Parse(hdnIdEliminar.Value);

            try
            {
                _tratamientoService.DarDeBaja(idTratamiento, _turnoService);

                AplicarFiltros();

                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Baja exitosa",
                    "El tratamiento ha sido dado de baja correctamente.",
                    "Resultado",
                    null,
                    "abrirModalResultado"
                );
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Operación denegada",
                    ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado"
                );
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error",
                    ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado"
                );
            }
        }

        

    }
}