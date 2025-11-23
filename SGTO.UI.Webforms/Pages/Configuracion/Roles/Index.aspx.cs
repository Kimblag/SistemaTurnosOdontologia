using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs.Roles;
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

namespace SGTO.UI.Webforms.Pages.Configuracion.Roles
{
    public partial class Index : System.Web.UI.Page
    {
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();
        private readonly RolService _servicioRol = new RolService();

        private const string KEY_ROL_BUSQUEDA = "FiltroRolBusqueda";
        private const string KEY_ROL_ESTADO = "FiltroRolEstado";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!SessionManager.EstaLogueado())
            {
                Response.Redirect("~/Pages/Login/Index.aspx");
                return;
            }

            if (!_servicioAutorizacion.TienePermiso(SessionManager.Usuario, "ROLES", "VER"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

            if (Master is SiteMaster master)
            {
                master.ConfigurarBotonVolver(true, "~/Pages/Configuracion/Index.aspx");
                master.EstablecerOpcionMenuActiva("Configuracion");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Configure los niveles de autorización y permisos por grupo.");
            }

            if (!IsPostBack)
            {
                txtBuscarRol.Text = Session[KEY_ROL_BUSQUEDA] as string ?? string.Empty;
                string estado = Session[KEY_ROL_ESTADO] as string;
                if (!string.IsNullOrEmpty(estado) && ddlEstado.Items.FindByValue(estado) != null)
                {
                    ddlEstado.SelectedValue = estado;
                }

                AplicarFiltros();
            }
        }

        private void CargarRoles(string estado = null)
        {
            List<RolListadoDto> lista = new List<RolListadoDto>();

            try
            {
                lista = _servicioRol.Listar();
                gvRoles.DataSource = lista;
                gvRoles.DataBind();
            }
            catch (Exception ex)
            {
                gvRoles.DataSource = lista;
                gvRoles.DataBind();

                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error al cargar roles",
                    "Ocurrió un error inesperado al intentar obtener la lista de roles. " + ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado"
                );
            }
        }

        private void AplicarFiltros()
        {
            string textoBusqueda = txtBuscarRol.Text.Trim();
            string estadoSeleccionado = ddlEstado.SelectedValue;

            Session[KEY_ROL_BUSQUEDA] = string.IsNullOrEmpty(textoBusqueda) ? null : textoBusqueda;
            Session[KEY_ROL_ESTADO] = estadoSeleccionado != "todos" ? estadoSeleccionado : null;

            string estadoFiltro = estadoSeleccionado != "todos" ? estadoSeleccionado : null;

            List<RolListadoDto> lista = new List<RolListadoDto>();
            try
            {
                lista = _servicioRol.Listar(estadoFiltro);
            }
            catch (Exception ex)
            {
                gvRoles.DataSource = lista;
                gvRoles.DataBind();
                MensajeUiHelper.SetearYMostrar(
                   this.Page, "Error al cargar roles",
                   "Ocurrió un error inesperado. " + ex.Message, "Resultado",
                   null, "abrirModalResultado"
               );
                return;
            }

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                string textoNormalizado = ValidadorCampos.NormalizarTexto(textoBusqueda);
                List<RolListadoDto> filtrada = new List<RolListadoDto>();

                foreach (var rol in lista)
                {
                    string nombreNormalizado = ValidadorCampos.NormalizarTexto(rol.Nombre);
                    string descNormalizada = ValidadorCampos.NormalizarTexto(rol.Descripcion);

                    bool coincide =
                        (!string.IsNullOrEmpty(nombreNormalizado) && nombreNormalizado.Contains(textoNormalizado)) ||
                        (!string.IsNullOrEmpty(descNormalizada) && descNormalizada.Contains(textoNormalizado));

                    if (coincide)
                    {
                        filtrada.Add(rol);
                    }
                }
                lista = filtrada;
            }

            gvRoles.DataSource = lista;
            gvRoles.DataBind();
        }

        protected void btnNuevoRol_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Configuracion/Roles/Nuevo.aspx", false);
        }

        protected void gvRoles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RolListadoDto rolDto = (RolListadoDto)e.Row.DataItem;

                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");
                var btnEditar = (LinkButton)e.Row.FindControl("btnEditar");
                var btnEliminar = (HtmlButton)e.Row.FindControl("btnEliminar");
                var btnDetalle = (LinkButton)e.Row.FindControl("btnDetalle");

                if (rolDto != null)
                {
                    string estado = rolDto.Estado.ToLower();
                    if (lblEstado != null)
                    {
                        lblEstado.InnerText = rolDto.Estado;

                        if (estado == "activo")
                            lblEstado.Attributes["class"] = "badge badge-success";
                        else
                            lblEstado.Attributes["class"] = "badge badge-warning";
                    }

                    string nombreRol = rolDto.Nombre.Trim();
                    bool esAdmin = nombreRol.Equals("Administrador", StringComparison.OrdinalIgnoreCase);
                    bool esSistema = nombreRol.Equals("Médico", StringComparison.OrdinalIgnoreCase) ||
                                     nombreRol.Equals("Recepcionista", StringComparison.OrdinalIgnoreCase) ||
                                     esAdmin;

                    if (esSistema)
                    {
                        // nadie puede borrar roles de sistema
                        if (btnEliminar != null) btnEliminar.Visible = false;

                        // el admin no se edita
                        if (esAdmin && btnEditar != null)
                        {
                            btnEditar.Visible = false;
                        }
                    }
                }
            }
        }

        protected void gvRoles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRoles.PageIndex = e.NewPageIndex;
            AplicarFiltros();
        }

        protected void gvRoles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null)
                return;

            int idRol = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Pages/Configuracion/Roles/Editar?id-rol={idRol}", false);
            }
            else if (e.CommandName == "Eliminar")
            {
                hdnIdEliminar.Value = idRol.ToString();
            }
            else if (e.CommandName == "Ver")
            {

                Response.Redirect($"~/Pages/Configuracion/Roles/Detalle?id-rol={idRol}", false);
            }
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hdnIdEliminar.Value))
                return;

            int idRol = int.Parse(hdnIdEliminar.Value);

            try
            {
                _servicioRol.DarDeBaja(idRol);

                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Rol dado de baja",
                    "El rol fue dado de baja correctamente.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Configuracion/Roles/Index"),
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
                    "Ocurrió un error al intentar dar de baja el rol. " + ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado"
                );
            }

            CargarRoles();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Session[KEY_ROL_BUSQUEDA] = null;
            Session[KEY_ROL_ESTADO] = null;

            txtBuscarRol.Text = string.Empty;
            ddlEstado.SelectedIndex = 0;

            CargarRoles();
        }
    }
}