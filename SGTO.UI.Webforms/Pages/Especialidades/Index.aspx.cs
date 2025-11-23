using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Seguridad;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Seguridad;
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
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();
        private readonly EspecialidadService _especialidadService = new EspecialidadService();
        private readonly TurnoService _turnoService = new TurnoService();
        private const string KEY_ESTADO_ESPECIALIDADES = "FiltroEstadoEspecialidades";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.EstaLogueado())
            {
                Response.Redirect("~/Pages/Login/Index.aspx");
                return;
            }

            if (!_servicioAutorizacion.TienePermiso(SessionManager.Usuario, "ESPECIALIDADES", "VER"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Especialidades");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Catálogo de las áreas de práctica odontológica habilitadas.");
            }

            if (!IsPostBack)
            {
                bool puedeCrear = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "ESPECIALIDADES", "CREAR");
                pnlNuevaEspecialidad.Visible = puedeCrear;

                if (!puedeCrear)
                {
                    pnlBuscador.Attributes["class"] = "col-md-4 col-lg-6";
                }
                else
                {
                    pnlBuscador.Attributes["class"] = "col-md-4 col-lg-5";
                }

                bool puedeEditar = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "ESPECIALIDADES", "EDITAR");
                bool puedeEliminar = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "ESPECIALIDADES", "ELIMINAR");

                if (!puedeEditar && !puedeEliminar)
                {
                    gvEspecialidades.Columns[3].Visible = false;
                }
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
                var btnEliminar = (HtmlControl)e.Row.FindControl("btnEliminar");
                var btnEditar = (LinkButton)e.Row.FindControl("btnEditar");

                if (lblEstado != null && especialidadDto != null)
                {
                    if (especialidadDto.Estado.ToLower() == "Activo".ToLower())
                    {
                        lblEstado.Attributes["class"] = "badge badge-success";
                    }
                    else
                    {
                        lblEstado.Attributes["class"] = "badge badge-warning";
                    }

                }
                if (btnEliminar != null && btnEliminar.Visible)
                {
                    bool puedeEliminar = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "ESPECIALIDADES", "ELIMINAR");
                    btnEliminar.Visible = puedeEliminar;

                    if (puedeEliminar)
                    {
                        btnEliminar.Attributes["onclick"] = $"abrirModalConfirmacion('{especialidadDto.IdEspecialidad}')";
                    }
                }

                if (btnEditar != null)
                {
                    bool puedeEditar = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "ESPECIALIDADES", "EDITAR");
                    btnEditar.Visible = puedeEditar;
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
            ddlEstado.SelectedValue = "todos";
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
                    "Especialidad dada de baja",
                    "La especialidad fue dada de baja correctamente.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Especialidades/Index"),
                    "abrirModalResultado"
                );
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

                string texto = ValidadorCampos.NormalizarTexto(textoBusqueda);
                foreach (EspecialidadDto dto in lista)
                {
                    if ((dto.Nombre != null && ValidadorCampos.NormalizarTexto(dto.Nombre).Contains(texto))
                         || (dto.Descripcion != null && ValidadorCampos.NormalizarTexto(dto.Descripcion).Contains(texto)))
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