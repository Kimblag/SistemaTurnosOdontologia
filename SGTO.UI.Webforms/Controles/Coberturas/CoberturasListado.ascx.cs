using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private const string KEY_ESTADO_COBERTURAS = "FiltroEstadoCoberturas";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                if (lblEstado != null && coberturaDto != null)
                {
                    if (coberturaDto.Estado == "Activo")
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
                List<CoberturaDto> listaFiltrada = new List<CoberturaDto>();

                foreach (CoberturaDto dto in lista)
                {
                    if ((dto.Nombre != null && dto.Nombre.ToLower().Contains(textoBusqueda))
                       || (dto.Descripcion != null && dto.Descripcion.ToLower().Contains(textoBusqueda)))
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


    }
}