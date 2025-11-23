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
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Tratamientos
{
    public partial class Tratamientos : System.Web.UI.Page
    {
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();
        private readonly TratamientoService _tratamientoService = new TratamientoService();
        private readonly TurnoService _turnoService = new TurnoService();
        private readonly EspecialidadService _especialidadService = new EspecialidadService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.EstaLogueado())
            {
                Response.Redirect("~/Pages/Login/Index.aspx");
                return;
            }

            if (!_servicioAutorizacion.TienePermiso(SessionManager.Usuario, "TRATAMIENTOS", "VER"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Tratamientos");
                master.EstablecerTituloSeccion("Nomenclador de Tratamientos");
                master.EstablecerSubtituloSeccion("Catálogo de prestaciones odontológicas y costos base por especialidad.");
            }

            if (!IsPostBack)
            {
                bool puedeCrear = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "TRATAMIENTOS", "CREAR");
                pnlNuevoTratamiento.Visible = puedeCrear;

                if (!puedeCrear)
                {
                    pnlBuscador.Attributes["class"] = "col-md-4 col-lg-5";
                }
                else
                {
                    pnlBuscador.Attributes["class"] = "col-md-4 col-lg-4";
                }

                bool puedeEditar = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "TRATAMIENTOS", "EDITAR");
                bool puedeEliminar = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "TRATAMIENTOS", "ELIMINAR");

                if (!puedeEditar && !puedeEliminar)
                {
                    gvTratamientos.Columns[5].Visible = false;
                }

                CargarCombos();
                CargarTratamientosConFiltros();

                MensajeUiHelper.MostrarModal(this.Page);
            }
        }

        private void CargarCombos()
        {
            try
            {
                ddlEspecialidad.Items.Clear();
                ddlEspecialidad.Items.Add(new ListItem("Todas las especialidades", ""));

                var especialidades = _especialidadService.Listar();
                foreach (var esp in especialidades)
                {
                    ddlEspecialidad.Items.Add(new ListItem(esp.Nombre, esp.Nombre));
                }
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "No se pudieron cargar las especialidades: " + ex.Message);
            }
        }

        private void CargarTratamientosConFiltros()
        {
            List<TratamientoDto> todosLosTratamientos = new List<TratamientoDto>();
            List<TratamientoDto> listaFiltrada = new List<TratamientoDto>();

            try
            {
                todosLosTratamientos = _tratamientoService.Listar();
            }
            catch (Exception ex)
            {
                gvTratamientos.DataSource = null;
                gvTratamientos.DataBind();
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "No se pudo cargar la lista de tratamientos. " + ex.Message);
                return;
            }

            string textoBuscar = txtBuscar.Text.Trim().ToUpper();
            string especialidadSeleccionada = ddlEspecialidad.SelectedValue;
            string estadoSeleccionado = ddlEstado.SelectedValue;

            foreach (TratamientoDto t in todosLosTratamientos)
            {
                bool cumple = true;

                if (!string.IsNullOrEmpty(textoBuscar))
                {
                    string nombreNorm = ValidadorCampos.NormalizarTexto(t.Nombre);
                    string descNorm = ValidadorCampos.NormalizarTexto(t.Descripcion);
                    string textoNorm = ValidadorCampos.NormalizarTexto(textoBuscar);

                    if (!nombreNorm.Contains(textoNorm) && !descNorm.Contains(textoNorm))
                    {
                        cumple = false;
                    }
                }

                if (cumple && !string.IsNullOrEmpty(especialidadSeleccionada))
                {
                    if (!string.IsNullOrEmpty(t.NombreEspecialidad))
                    {
                        if (!t.NombreEspecialidad.Equals(especialidadSeleccionada, StringComparison.OrdinalIgnoreCase))
                        {
                            cumple = false;
                        }
                    }
                    else
                    {
                        cumple = false;
                    }
                }

                if (cumple && !string.IsNullOrEmpty(estadoSeleccionado))
                {
                    bool esActivoDto = t.Estado != null && t.Estado.Trim().ToLower().StartsWith("act");

                    if (estadoSeleccionado == "Activo" && !esActivoDto) cumple = false;
                    if (estadoSeleccionado == "Inactivo" && esActivoDto) cumple = false;
                }

                if (cumple)
                {
                    listaFiltrada.Add(t);
                }
            }

            gvTratamientos.DataSource = listaFiltrada;
            gvTratamientos.DataBind();
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            gvTratamientos.PageIndex = 0;
            CargarTratamientosConFiltros();
        }


        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = string.Empty;
            ddlEspecialidad.SelectedIndex = 0;
            ddlEstado.SelectedValue = "Activo";

            gvTratamientos.PageIndex = 0;
            CargarTratamientosConFiltros();
        }


        protected void btnNuevoTratamiento_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Tratamientos/Nuevo", false);
        }


        protected void gvTratamientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTratamientos.PageIndex = e.NewPageIndex;
            CargarTratamientosConFiltros();
        }


        protected void gvTratamientos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var tratamientoDto = (TratamientoDto)e.Row.DataItem;
                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");
                var btnEditar = (LinkButton)e.Row.FindControl("btnEditar");
                var btnEliminar = (HtmlControl)e.Row.FindControl("btnEliminar");

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
                if (btnEditar != null)
                    btnEditar.Visible = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "TRATAMIENTOS", "EDITAR");

                if (btnEliminar != null)
                {
                    bool puedeEliminar = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "TRATAMIENTOS", "ELIMINAR");
                    btnEliminar.Visible = puedeEliminar;
                    if (puedeEliminar)
                    {
                        btnEliminar.Attributes["onclick"] = $"abrirModalConfirmacion('{tratamientoDto.IdTratamiento}')";
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




        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            int idTratamiento = int.Parse(hdnIdEliminar.Value);

            try
            {
                _tratamientoService.DarDeBaja(idTratamiento, _turnoService);

                CargarTratamientosConFiltros();

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



    }
}