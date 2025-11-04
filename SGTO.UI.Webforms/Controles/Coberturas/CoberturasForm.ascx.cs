using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Coberturas
{
    public partial class CoberturasForm : System.Web.UI.UserControl
    {
        public bool ModoEdicion { get; set; } = false;
        private readonly CoberturaService _servicioCobertura = new CoberturaService();

        protected void Page_Load(object sender, EventArgs e)
        {
            int idCobertura = ExtraerIdCobertura();
            if (idCobertura != 0) ModoEdicion = true;

            if (!IsPostBack)
            {
                if (idCobertura != 0)
                {
                    CargarDetalleCobertura(idCobertura);
                }
                else
                {
                    List<Plan> planes = new List<Plan>();
                    gvPlanes.DataSource = planes;
                    gvPlanes.DataBind();
                }
                panelPlanes.Visible = !ModoEdicion;

                ModalHelper.MostrarModalDesdeSession(this.Page, "CoberturaMensajeTitulo", "CoberturaMensajeDesc", "/Pages/CoberturasPlanes/Index");
            }
        }

        protected void btnNuevoPlan_Click(object sender, EventArgs e)
        {

        }

        public void gvPlanes_RowDataBound(object sender, GridViewRowEventArgs e) { }
        public void gvPlanes_PageIndexChanging(object sender, GridViewPageEventArgs e) { }
        public void gvPlanes_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int idPlan = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Pages/CoberturasPlanes/EditarPlan?id-plan={idPlan}", false);
            }
            else if (e.CommandName == "Eliminar")
            {

            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/CoberturasPlanes/Index", false);
        }


        public void CargarDetalleCobertura(int idCobertura)
        {
            try
            {
                CoberturaDto coberturaDto = _servicioCobertura.ObtenerCoberturaPorId(idCobertura);
                txtNombreCobertura.Text = coberturaDto.Nombre;
                txtDescripcionCobertura.Text = coberturaDto.Descripcion;
                ddlEstado.SelectedValue = coberturaDto.Estado.ToLower();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ExtraerIdCobertura()
        {
            string idCoberturaString = Request.QueryString["id-cobertura"] ?? string.Empty;
            if (!string.IsNullOrEmpty(idCoberturaString) && int.TryParse(idCoberturaString, out int idCobertura))
            {
                return idCobertura;
            }
            return 0;
        }

        public void ModificarCobertura()
        {
            int idCobertura = ExtraerIdCobertura();

            try
            {
                CoberturaDto coberturaDto = new CoberturaDto
                {
                    IdCobertura = idCobertura,
                    Nombre = txtNombreCobertura.Text,
                    Descripcion = txtDescripcionCobertura.Text,
                    Estado = ddlEstado.SelectedValue
                };
                _servicioCobertura.ModificarCobertura(coberturaDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CrearCobertura()
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ModoEdicion)
                {
                    ModificarCobertura();
                    Session["CoberturaMensajeTitulo"] = "Cobertura actualizada";
                    Session["CoberturaMensajeDesc"] = "La cobertura se ha modificado correctamente.";
                }
                else
                {
                    CrearCobertura();
                    Session["CoberturaMensajeTitulo"] = "Cobertura creada";
                    Session["CoberturaMensajeDesc"] = "La cobertura se ha guardado correctamente.";
                }

                Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception)
            {
                Session["CoberturaMensajeTitulo"] = "Error";
                Session["CoberturaMensajeDesc"] = "Ocurrió un error al guardar la cobertura.";
                Response.Redirect(Request.RawUrl, false);
            }
        }



    }
}