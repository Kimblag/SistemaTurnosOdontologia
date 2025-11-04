using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Coberturas
{
    public partial class CoberturasListado : System.Web.UI.UserControl
    {

        private readonly CoberturaService _servicioCobertura = new CoberturaService();
        private List<CoberturaDto> _listadoCoberturasDto = new List<CoberturaDto>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string estadoFiltroGuardado = Session["FiltroEstadoCoberturas"] as string;

                /// retomamos el valor del filtro si es que hay algo
                if (estadoFiltroGuardado != null)
                    ddlEstado.SelectedValue = estadoFiltroGuardado;

                CargarCoberturas(estadoFiltroGuardado);

                ModalHelper.MostrarModalDesdeSession(this.Page, "CoberturaMensajeTitulo", "CoberturaMensajeDesc");
            }
        }

        public void gvCoberturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // cambiar los colores de los basges según estado
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CoberturaDto cobertura = (CoberturaDto)e.Row.DataItem;

                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");
                if (lblEstado != null && cobertura != null)
                {
                    if (cobertura.Estado == "Activo")
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

        public void gvCoberturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCoberturas.PageIndex = e.NewPageIndex;
            string estadoFiltroActual = Session["FiltroEstadoCoberturas"] as string;
            CargarCoberturas(estadoFiltroActual);
        }

        public void gvCoberturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idCobertura = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Pages/CoberturasPlanes/EditarCobertura?id-cobertura={idCobertura}", false);
            }
            else if (e.CommandName == "Eliminar")
            {
                try
                {
                    _servicioCobertura.DarDeBajaCobertura(idCobertura);
                    Session["CoberturaMensajeTitulo"] = "Cobertura dada de baja";
                    Session["CoberturaMensajeDesc"] = "La cobertura y sus planes fueron dados de baja correctamente.";
                    Session["ModalTipo"] = "Resultado";
                }
                catch (ExcepcionReglaNegocio ex)
                {
                    Session["CoberturaMensajeTitulo"] = "Operación no permitida";
                    Session["CoberturaMensajeDesc"] = ex.Message;
                    Session["ModalTipo"] = "Resultado";
                }
                catch (Exception)
                {
                    Session["CoberturaMensajeTitulo"] = "Error inesperado";
                    Session["CoberturaMensajeDesc"] = "Ocurrió un error al intentar dar de baja la cobertura.";
                    Session["ModalTipo"] = "Resultado";
                }

                Response.Redirect(Request.RawUrl, false);
            }
        }

        private void CargarCoberturas(string estado = null)
        {
            try
            {
                List<CoberturaDto> listado = _servicioCobertura.Listar(estado);
                _listadoCoberturasDto = listado;
                gvCoberturas.DataSource = _listadoCoberturasDto;
                gvCoberturas.DataBind();
            }
            catch (Exception)
            {
                // TODO: mostrar error en la UI de forma linda con un mensaje
                throw;
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
            Session["FiltroEstadoCoberturas"] = null;
            ddlEstado.SelectedValue = "todos";
            txtBuscarCobertura.Text = string.Empty;
            CargarCoberturas();
        }


        private void AplicarFiltros()
        {
            string estadoSeleccionado = ddlEstado.SelectedValue;
            string textoBusqueda = txtBuscarCobertura.Text.ToLower();

            Session["FiltroEstadoCoberturas"] = estadoSeleccionado == "todos"
                ? null
                : estadoSeleccionado;

            List<CoberturaDto> lista = _servicioCobertura.Listar(Session["FiltroEstadoCoberturas"] as string);

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                List<CoberturaDto> listaFiltrada = new List<CoberturaDto>();

                foreach (CoberturaDto dto in lista)

                    if ((dto.Nombre != null && dto.Nombre.ToLower().Contains(textoBusqueda))
                        || (dto.Descripcion != null && dto.Descripcion.ToLower().Contains(textoBusqueda)))
                    {
                        listaFiltrada.Add(dto);
                    }
                lista = listaFiltrada;
            }
            gvCoberturas.DataSource = lista;
            gvCoberturas.DataBind();
        }

        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            int idCobertura = Convert.ToInt32(hdnIdCoberturaEliminar.Value);

            try
            {
               bool resultado = _servicioCobertura.DarDeBajaCobertura(idCobertura);
                Session["CoberturaMensajeTitulo"] = "Cobertura dada de baja";
                Session["CoberturaMensajeDesc"] = "La cobertura y sus planes fueron dados de baja correctamente.";
                Session["ModalTipo"] = "Resultado";
            }
            catch (ExcepcionReglaNegocio ex)
            {
                Session["CoberturaMensajeTitulo"] = "Operación no permitida";
                Session["CoberturaMensajeDesc"] = ex.Message;
                Session["ModalTipo"] = "Resultado";
            }
            catch (Exception)
            {
                Session["CoberturaMensajeTitulo"] = "Error inesperado";
                Session["CoberturaMensajeDesc"] = "Ocurrió un error al intentar dar de baja la cobertura.";
                Session["ModalTipo"] = "Resultado";
            }

            Response.Redirect(Request.RawUrl, false);

        }
    }
}