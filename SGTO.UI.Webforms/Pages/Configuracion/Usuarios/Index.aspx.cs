using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Roles;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Configuracion.Usuarios
{
    public partial class Index : System.Web.UI.Page
    {

        private readonly UsuarioService _servicioUsuario = new UsuarioService();

        private const string KEY_USUARIO_BUSQUEDA = "FiltroUsuarioBusqueda";
        private const string KEY_USUARIO_ROL = "FiltroUsuarioRol";
        private const string KEY_USUARIO_ESTADO = "FiltroUsuarioEstado";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Configuracion");
                master.EstablecerTituloSeccion(this.Page.Title);
            }

            if (!IsPostBack)
            {
                CargarRolesDropDown();

                // verificar si hay filtros
                RestaurarFiltrosDesdeSession();

                CargarUsuarios();
            }

        }


        private void CargarRolesDropDown()
        {
            try
            {
                RolService servicioRol = new RolService();
                List<RolListadoDto> roles = servicioRol.Listar(); // listar todos activos e inactivos

                ddlRol.DataSource = roles;
                ddlRol.DataTextField = "Nombre";
                ddlRol.DataValueField = "IdRol";
                ddlRol.DataBind();

                ddlRol.Items.Insert(0, new ListItem("Todos", "todos"));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                ddlRol.Items.Clear();
                ddlRol.Items.Add(new ListItem("Error al cargar", ""));
            }
        }

        private void RestaurarFiltrosDesdeSession()
        {
            var estadoGuardado = Session[KEY_USUARIO_ESTADO] as string;
            if (!string.IsNullOrEmpty(estadoGuardado))
            {
                ddlEstado.SelectedValue = estadoGuardado;
            }
            else
            {
                ddlEstado.SelectedValue = "todos";
            }

            var rolGuardado = Session[KEY_USUARIO_ROL] as string;
            if (!string.IsNullOrEmpty(rolGuardado))
            {
                ListItem item = ddlRol.Items.FindByValue(rolGuardado);
                if (item != null)
                    ddlRol.SelectedValue = rolGuardado;
            }
            else
            {
                ddlRol.SelectedValue = "todos";
            }

            var textoGuardado = Session[KEY_USUARIO_BUSQUEDA] as string;
            if (!string.IsNullOrEmpty(textoGuardado))
                txtBuscarUsuario.Text = textoGuardado;
        }

        private void CargarUsuarios()
        {
            try
            {
                List<UsuarioListadoDto> usuarios = _servicioUsuario.Listar();
                gvUsuarios.DataSource = usuarios;
                gvUsuarios.DataBind();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                gvUsuarios.DataSource = new List<PlanDto>();
                gvUsuarios.DataBind();

                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error al cargar usuarios",
                    "Ocurrió un error inesperado al intentar obtener la lista de usuarios.");
            }
        }

        private void AplicarFiltros()
        {
            string estadoSeleccionado = ddlEstado.SelectedValue;
            string textoBusqueda = txtBuscarUsuario.Text.Trim().ToLower();
            string rolSeleccionado = ddlRol.SelectedValue;

            Session[KEY_USUARIO_ESTADO] = estadoSeleccionado == "todos"
                ? null
                : estadoSeleccionado;
            Session[KEY_USUARIO_ROL] = rolSeleccionado == "todos"
                ? null
                : rolSeleccionado;
            Session[KEY_USUARIO_BUSQUEDA] = String.IsNullOrEmpty(textoBusqueda)
                ? null
                : textoBusqueda;

            List<UsuarioListadoDto> lista = _servicioUsuario.Listar(Session[KEY_USUARIO_ESTADO] as string);

            var rolSesion = Session[KEY_USUARIO_ROL] as string;
            if (!string.IsNullOrEmpty(rolSesion))
            {
                int idRol = Convert.ToInt32(rolSesion);
                lista = lista.FindAll(dto => dto.IdRol == idRol);
            }

            var textoSession = Session[KEY_USUARIO_BUSQUEDA] as string;
            if (!string.IsNullOrEmpty(textoSession))
            {
                lista = lista.FindAll(dto =>
                (dto.NombreCompleto != null && dto.NombreCompleto.ToLower().Contains(textoSession)) ||
                (dto.NombreUsuario != null && dto.NombreUsuario.ToLower().Contains(textoSession)) ||
                (dto.Email != null && dto.Email.ToLower().Contains(textoSession))
                );
            }

            gvUsuarios.DataSource = lista;
            gvUsuarios.DataBind();

        }


        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnNuevoUsuario_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/Configuracion/Usuarios/Nuevo", false);
        }

        protected void gvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                UsuarioListadoDto usuarioDto = (UsuarioListadoDto)e.Row.DataItem;

                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");
                if (lblEstado != null && usuarioDto != null)
                {
                    if (usuarioDto.Estado.ToLower() == "Activo".ToLower())
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

        protected void gvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsuarios.PageIndex = e.NewPageIndex;
            AplicarFiltros();
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sort")
                return;

            int idUsuario = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Pages/Configuracion/Usuarios/Editar?id-usuario={idUsuario}", false);
            }
        }

        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            int idUsuario = int.Parse(hdnIdEliminar.Value);
            try
            {
                //_servicioUsuario.DarDeBaja(idUsuario);

                MensajeUiHelper.SetearYMostrar(
                   this.Page,
                   "Usuario dado de baja",
                   "El usuario fue dado de baja correctamente.",
                   "Resultado",
                   VirtualPathUtility.ToAbsolute("~/Pages/Pacientes/Index"),
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
                    "Ocurrió un error al intentar dar de baja el paciente. " + ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado"
                );
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Session[KEY_USUARIO_ESTADO] = null;
            Session[KEY_USUARIO_ROL] = null;
            Session[KEY_USUARIO_BUSQUEDA] = null;

            ddlEstado.SelectedValue = "todos";
            ddlRol.SelectedIndex = 0;
            txtBuscarUsuario.Text = string.Empty;

            CargarUsuarios();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }
    }
}