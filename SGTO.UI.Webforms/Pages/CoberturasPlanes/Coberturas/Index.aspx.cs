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
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.CoberturasPlanes.Coberturas
{
    public partial class Index : System.Web.UI.Page
    {
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();
        private readonly CoberturaService _servicioCobertura = new CoberturaService();
        private const string KEY_ESTADO_COBERTURAS = "FiltroEstadoCoberturas";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.ConfigurarBotonVolver(true, "~/Pages/CoberturasPlanes/Index.aspx");
                master.EstablecerOpcionMenuActiva("Coberturas");
                master.EstablecerTituloSeccion("Gestión de Coberturas");
                master.EstablecerSubtituloSeccion("Defina las entidades prestadoras de salud y visualice sus planes asociados.");
            }

            if (!_servicioAutorizacion.TienePermiso(SessionManager.Usuario, "COBERTURAS", "VER"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

            if (!IsPostBack)
            {
                bool puedeCrear = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "COBERTURAS", "CREAR");

                pnlNuevaCobertura.Visible = puedeCrear;

                if (!puedeCrear)
                {
                    // Si ocultamos el botón nuevo (col-lg-1), se lo sumamos al buscador
                    // Original: col-xl-3. Nuevo: col-xl-5.
                    // Se agrega: col-12 para indicarle a bootstrap loq ue debe hacer en cada
                    // breakpoint de pantalla. Sin col-12 tiene efectos no deseados (no hace wrap la línea
                    // y se queda visualmente atropellando al otro elemento, haciendo que se vea horrible)
                    pnlBuscador.Attributes["class"] = "col-12 col-md-12 col-xl-6";
                }
                else
                {
                    pnlBuscador.Attributes["class"] = "col-12 col-md-6 col-xl-4";
                }

                string estadoFiltroGuardado = Session[KEY_ESTADO_COBERTURAS] as string;

                /// retomamos el valor del filtro si es que hay algo
                if (estadoFiltroGuardado != null)
                    ddlEstado.SelectedValue = estadoFiltroGuardado;



                CargarCoberturas(estadoFiltroGuardado);

                MensajeUiHelper.MostrarModal(this.Page);
            }
        }

        public void gvCoberturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // cambiar los colores de los basges según estado
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CoberturaDto coberturaDto = (CoberturaDto)e.Row.DataItem;

                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");
                var btnEditar = (LinkButton)e.Row.FindControl("btnEditar");
                var btnEliminar = (HtmlControl)e.Row.FindControl("btnEliminar");

                if (coberturaDto != null)
                {
                    if (lblEstado != null)
                    {
                        bool activo = coberturaDto.Estado.ToLower() == "activo";
                        lblEstado.Attributes["class"] = activo ? "badge badge-success" : "badge badge-warning";
                    }

                    bool puedeEditar = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "COBERTURAS", "EDITAR");
                    bool puedeEliminar = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "COBERTURAS", "ELIMINAR");

                    if (btnEditar != null) btnEditar.Visible = puedeEditar;

                    if (btnEliminar != null)
                    {
                        btnEliminar.Visible = puedeEliminar;
                        if (puedeEliminar)
                        {
                            btnEliminar.Attributes["onclick"] = $"abrirModalConfirmacion('{coberturaDto.IdCobertura}', 'cobertura');";
                        }
                    }
                }
            }
        }

        public void gvCoberturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCoberturas.PageIndex = e.NewPageIndex;
            string estadoFiltroActual = Session[KEY_ESTADO_COBERTURAS] as string;
            CargarCoberturas(estadoFiltroActual);
        }

        public void gvCoberturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sort")
                return;

            int idCobertura = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Pages/CoberturasPlanes/EditarCobertura?id-cobertura={idCobertura}", false);
            }
        }

        private void CargarCoberturas(string estado = null)
        {
            List<CoberturaDto> listado = new List<CoberturaDto>();
            try
            {
                listado = _servicioCobertura.Listar(estado);
                gvCoberturas.DataSource = listado;
                gvCoberturas.DataBind();
            }
            catch (Exception)
            {
                gvCoberturas.DataSource = listado;
                gvCoberturas.DataBind();

                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error al cargar coberturas",
                    "Ocurrió un error inesperado al intentar obtener la lista de coberturas."
                );
            }
        }

        protected void btnNuevaCobertura_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/CoberturasPlanes/NuevaCobertura", false);
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
            // ´método para resetear filtros y limpiar el 
            Session[KEY_ESTADO_COBERTURAS] = null;
            ddlEstado.SelectedValue = "todos";
            txtBuscarCobertura.Text = string.Empty;
            CargarCoberturas();
        }


        private void AplicarFiltros()
        {
            string estadoSeleccionado = ddlEstado.SelectedValue;
            string textoBusqueda = txtBuscarCobertura.Text.ToLower();

            Session[KEY_ESTADO_COBERTURAS] = estadoSeleccionado == "todos"
                ? null
                : estadoSeleccionado;

            List<CoberturaDto> lista = _servicioCobertura.Listar(Session[KEY_ESTADO_COBERTURAS] as string);

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                string texto = ValidadorCampos.NormalizarTexto(textoBusqueda);
                List<CoberturaDto> listaFiltrada = new List<CoberturaDto>();

                foreach (CoberturaDto dto in lista)
                {
                    if ((dto.Nombre != null && ValidadorCampos.NormalizarTexto(dto.Nombre).Contains(texto))
                       || (dto.Descripcion != null && ValidadorCampos.NormalizarTexto(dto.Descripcion).Contains(texto)))
                    {
                        listaFiltrada.Add(dto);
                    }
                }


                lista = listaFiltrada;
            }
            gvCoberturas.DataSource = lista;
            gvCoberturas.DataBind();
        }

        protected void gvCoberturas_Sorting(object sender, GridViewSortEventArgs e)
        {
            string direccionOrdenamiento = ViewState["direccionOrdenamiento"] as string ?? "ASC";

            direccionOrdenamiento = direccionOrdenamiento == "ASC" ? "DESC" : "ASC";
            ViewState["direccionOrdenamiento"] = direccionOrdenamiento;

            List<CoberturaDto> coberturas = _servicioCobertura.Listar();

            if (direccionOrdenamiento == "ASC")
                coberturas.Sort((a, b) => string.Compare(a.Nombre, b.Nombre, StringComparison.OrdinalIgnoreCase));
            else
                coberturas.Sort((a, b) => string.Compare(b.Nombre, a.Nombre, StringComparison.OrdinalIgnoreCase));

            gvCoberturas.DataSource = coberturas;
            gvCoberturas.DataBind();
        }

        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            string tipo = hdnTipoEliminar.Value;
            int id = Convert.ToInt32(hdnIdEliminar.Value);

            try
            {
                TurnoService servicioTurno = new TurnoService();
                if (tipo == "cobertura")
                {
                    CoberturaService servicioCobertura = new CoberturaService();
                    servicioCobertura.DarDeBaja(id, servicioTurno);

                    MensajeUiHelper.SetearMensaje("Cobertura dada de baja", "La cobertura y sus planes fueron dados de baja correctamente.");
                }
                else if (tipo == "plan")
                {

                    PlanService servicioPlan = new PlanService();
                    servicioPlan.DarDeBaja(id, servicioTurno);

                    MensajeUiHelper.SetearMensaje("Plan dado de baja", "El plan fue dado de baja correctamente.");
                }
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearMensaje("Operación no permitida", ex.Message);
            }
            catch (Exception)
            {
                MensajeUiHelper.SetearMensaje("Error inesperado", "Ocurrió un error al intentar dar de baja el registro.");
            }

            Response.Redirect(Request.RawUrl, false);
        }
    }
}