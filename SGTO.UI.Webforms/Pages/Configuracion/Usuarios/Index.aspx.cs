using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Roles;
using SGTO.Negocio.Excepciones;
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

namespace SGTO.UI.Webforms.Pages.Configuracion.Usuarios
{
    public partial class Index : System.Web.UI.Page
    {

        private readonly UsuarioService _servicioUsuario = new UsuarioService();
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();

        private const string KEY_USUARIO_BUSQUEDA = "FiltroUsuarioBusqueda";
        private const string KEY_USUARIO_ROL = "FiltroUsuarioRol";
        private const string KEY_USUARIO_ESTADO = "FiltroUsuarioEstado";


        private bool _puedeCrear = false;
        private bool _puedeEditar = false;
        private bool _puedeEliminar = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.EstaLogueado())
            {
                Response.Redirect("~/Pages/Login/Index.aspx");
                return;
            }
            var usuarioActual = SessionManager.Usuario;

            if (!_servicioAutorizacion.TienePermiso(usuarioActual, "USUARIOS", "VER"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

            _puedeCrear = _servicioAutorizacion.TienePermiso(usuarioActual, "USUARIOS", "CREAR");
            _puedeEditar = _servicioAutorizacion.TienePermiso(usuarioActual, "USUARIOS", "EDITAR");
            _puedeEliminar = _servicioAutorizacion.TienePermiso(usuarioActual, "USUARIOS", "ELIMINAR");

            if (Master is SiteMaster master)
            {
                master.ConfigurarBotonVolver(true, "~/Pages/Configuracion/Index.aspx");
                master.EstablecerOpcionMenuActiva("Configuracion");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Administre el acceso del personal y los perfiles profesionales.");
            }


            if (!IsPostBack)
            {
                divBtnNuevo.Visible = _puedeCrear;

                CargarRolesDropDown();
                RestaurarFiltrosDesdeSession();
                AplicarFiltros();
            }
            else
            {
                divBtnNuevo.Visible = _puedeCrear;
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

                ddlRol.Items.Insert(0, new ListItem("Todos los roles", "todos"));
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
                string textoNormalizado = ValidadorCampos.NormalizarTexto(textoSession);

                lista = lista.FindAll(dto =>
                    (ValidadorCampos.NormalizarTexto(dto.NombreCompleto).Contains(textoNormalizado)) ||
                    (ValidadorCampos.NormalizarTexto(dto.NombreUsuario).Contains(textoNormalizado)) ||
                    (ValidadorCampos.NormalizarTexto(dto.Email).Contains(textoNormalizado))
                );
            }

            gvUsuarios.DataSource = lista;
            gvUsuarios.DataBind();
            if (!_puedeEditar && !_puedeEliminar)
            {
                if (gvUsuarios.Columns.Count > 5)
                {
                    gvUsuarios.Columns[5].Visible = false;
                }
            }
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

                var btnEditar = (LinkButton)e.Row.FindControl("btnEditar");

                if (btnEditar != null)
                {
                    btnEditar.Visible = _puedeEditar;
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