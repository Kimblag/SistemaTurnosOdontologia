using SGTO.Dominio.Enums;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Especialidades
{
    public partial class Especialidades : System.Web.UI.Page
    {
        private readonly EspecialidadService _especialidadService = new EspecialidadService();
        private readonly TurnoService _turnoService = new TurnoService();
        private const string KEY_ESTADO_ESPECIALIDADES = "FiltroEstadoEspecialidades";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Especialidades");
                master.EstablecerTituloSeccion(this.Page.Title);
            }
            if (!IsPostBack)
            {
                string estadoFiltroGuardado = Session[KEY_ESTADO_ESPECIALIDADES] as string;

                if (estadoFiltroGuardado != null)
                {
                    ddlEstado.SelectedValue = estadoFiltroGuardado;
                }

                CargarEspecialidades(estadoFiltroGuardado);

                MensajeUiHelper.MostrarModal(this.Page);
            }
        }


        protected void gvEspecialidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEspecialidades.PageIndex = e.NewPageIndex;
            string estadoFiltroActual = Session[KEY_ESTADO_ESPECIALIDADES] as string;
            CargarEspecialidades(estadoFiltroActual);
        }

        protected void gvEspecialidades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var especialidadDto = (EspecialidadDto)e.Row.DataItem;
                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");

                if (lblEstado != null)
                {
                    string cssClass = "badge ";

                    if (especialidadDto.Estado == "Activo")
                    {
                        cssClass += "badge-primary";
                    }
                    else
                    {
                        cssClass += "badge-secondary";
                    }
                    lblEstado.Attributes["class"] = cssClass;
                }
            }
        }

        protected void gvEspecialidades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idEspecialidad = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Pages/Especialidades/Editar?id-especialidad={idEspecialidad}", false);
            }
            else if (e.CommandName == "Ver")
            {
                // Response.Redirect($"~/Pages/Especialidades/Detalle?id-especialidad={idEspecialidad}", false);
            }
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Session[KEY_ESTADO_ESPECIALIDADES] = null;
            ddlEstado.SelectedValue = "todos"; // Corregido (era "Todos")
            txtBuscar.Text = string.Empty;
            CargarEspecialidades();
        }

        protected void btnNuevaEspecialidad_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Especialidades/Nuevo", false);
        }

        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            int idEspecialidad = int.Parse(hdnIdEliminar.Value);
            try
            {
                _especialidadService.DarDeBaja(idEspecialidad, _turnoService);

                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Especialidad dado de baja",
                    "La especialidad fue dada de baja correctamente.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Especialidades/Index"),
                    "abrirModalResultado"
                );
                Response.Redirect(Request.RawUrl, false);
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Operación no permitida",
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
                    "Error inesperado",
                    "Ocurrió un error al intentar dar de baja la especialidad. " + ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado"
                );
            }
        }

        private void CargarEspecialidades(string estado = null)
        {
            List<EspecialidadDto> lista = _especialidadService.ObtenerTodasDto();
            try
            {
                lista = _especialidadService.ObtenerTodasDto(estado);
                gvEspecialidades.DataSource = lista;
                gvEspecialidades.DataBind();
            }
            catch (Exception)
            {
                gvEspecialidades.DataSource = lista;
                gvEspecialidades.DataBind();

                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error al cargar especialidades",
                    "Ocurrió un error inesperado al intentar obtener la lista de especialidades"
                );
            }
        }

        private void AplicarFiltros()
        {
            string estadoSeleccionado = ddlEstado.SelectedValue;
            string textoBusqueda = txtBuscar.Text.ToLower();

            Session[KEY_ESTADO_ESPECIALIDADES] = estadoSeleccionado == "todos"
                ? null
                : estadoSeleccionado;

            List<EspecialidadDto> lista = _especialidadService.Listar(Session[KEY_ESTADO_ESPECIALIDADES] as string);

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                List<EspecialidadDto> listaFiltrada = new List<EspecialidadDto>();

                foreach (EspecialidadDto dto in lista)
                {
                    if ((dto.Nombre != null && dto.Nombre.ToLower().Contains(textoBusqueda))
                         || (dto.Descripcion != null && dto.Descripcion.ToLower().Contains(textoBusqueda)))
                    {
                        listaFiltrada.Add(dto);
                    }
                }
                lista = listaFiltrada;
            }
            gvEspecialidades.DataSource = lista;
            gvEspecialidades.DataBind();
        }
    }
}