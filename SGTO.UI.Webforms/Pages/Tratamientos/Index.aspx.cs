using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private const string KEY_ESTADO_TRATAMIENTOS = "FiltroEstadoTratamientos";
        private const string KEY_ESPECIALIDAD_TRATAMIENTOS = "FiltroEspecialidadTratamientos";
        private const string KEY_BUSQUEDA_TRATAMIENTOS = "FiltroBusquedaTratamientos";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Tratamientos");
                master.EstablecerTituloSeccion(this.Page.Title);
            }
            if (!IsPostBack)
            {
                CargarEspecialidades();
                RestaurarFiltros();
                AplicarFiltros();

                MensajeUiHelper.MostrarModal(this.Page);
            }
        }

        private void CargarEspecialidades()
        {
            try
            {
                var especialidades = _especialidadService.Listar("activo");
                ddlEspecialidad.DataSource = especialidades;
                ddlEspecialidad.DataTextField = "Nombre";
                ddlEspecialidad.DataValueField = "IdEspecialidad";
                ddlEspecialidad.DataBind();
                ddlEspecialidad.Items.Insert(0, new ListItem("Todas las especialidades", "0"));
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "No se pudieron cargar las especialidades. " + ex.Message);
            }
        }

        private void RestaurarFiltros()
        {
            if (Session[KEY_ESTADO_TRATAMIENTOS] != null)
            {
                ddlEstado.SelectedValue = Session[KEY_ESTADO_TRATAMIENTOS].ToString();
            }
            if (Session[KEY_ESPECIALIDAD_TRATAMIENTOS] != null)
            {
                ddlEspecialidad.SelectedValue = Session[KEY_ESPECIALIDAD_TRATAMIENTOS].ToString();
            }
            if (Session[KEY_BUSQUEDA_TRATAMIENTOS] != null)
            {
                txtBuscar.Text = Session[KEY_BUSQUEDA_TRATAMIENTOS].ToString();
            }
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

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        // Botones de filtro
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Session[KEY_ESTADO_TRATAMIENTOS] = null;
            Session[KEY_ESPECIALIDAD_TRATAMIENTOS] = null;
            Session[KEY_BUSQUEDA_TRATAMIENTOS] = null;

            ddlEstado.SelectedValue = "todos";
            ddlEspecialidad.SelectedValue = "0";
            txtBuscar.Text = string.Empty;

            AplicarFiltros();
        }

        private void CargarGrilla(List<TratamientoDto> lista)
        {
            gvTratamientos.DataSource = lista;
            gvTratamientos.DataBind();
        }

        private void AplicarFiltros()
        {
            string estadoSeleccionado = ddlEstado.SelectedValue;
            string idEspecialidad = ddlEspecialidad.SelectedValue;
            string textoBusqueda = txtBuscar.Text.ToLower().Trim();

            Session[KEY_ESTADO_TRATAMIENTOS] = estadoSeleccionado;
            Session[KEY_ESPECIALIDAD_TRATAMIENTOS] = idEspecialidad;
            Session[KEY_BUSQUEDA_TRATAMIENTOS] = textoBusqueda;

            string filtroEstado = (estadoSeleccionado == "todos") ? null : estadoSeleccionado;
            List<TratamientoDto> lista;
            try
            {
                lista = _tratamientoService.Listar(filtroEstado);
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error al cargar", "No se pudo obtener la lista de tratamientos. " + ex.Message);
                CargarGrilla(new List<TratamientoDto>());
                return;
            }

            int especialidadIdFiltro = 0;
            int.TryParse(idEspecialidad, out especialidadIdFiltro);

            if (especialidadIdFiltro > 0)
            {
                lista = lista.Where(t => t.IdEspecialidad == especialidadIdFiltro).ToList();
            }

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                lista = lista.Where(dto =>
                    (dto.Nombre != null && dto.Nombre.ToLower().Contains(textoBusqueda)) ||
                    (dto.Descripcion != null && dto.Descripcion.ToLower().Contains(textoBusqueda))
                ).ToList();
            }

            CargarGrilla(lista);
        }
    }
}