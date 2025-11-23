using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Pacientes;
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

namespace SGTO.UI.Webforms.Pages.Pacientes
{
    public partial class Pacientes : System.Web.UI.Page
    {
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();
        private readonly PacienteService _servicioPaciente = new PacienteService();
        private readonly CoberturaService _servicioCobertura = new CoberturaService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.EstaLogueado())
            {
                Response.Redirect("~/Pages/Login/Index.aspx");
                return;
            }

            if (!_servicioAutorizacion.TienePermiso(SessionManager.Usuario, "PACIENTES", "VER"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Pacientes");
                master.EstablecerTituloSeccion("Directorio de Pacientes");
                master.EstablecerSubtituloSeccion("Gestione el padrón, consulte historias clínicas o asigne turnos.");
            }

            if (!IsPostBack)
            {
                bool puedeCrear = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "PACIENTES", "CREAR");

                pnlNuevoPaciente.Visible = puedeCrear;

                if (!puedeCrear)
                {

                    pnlBuscador.Attributes["class"] = "col-md-6 col-lg-5";
                }
                else
                {
                    pnlBuscador.Attributes["class"] = "col-md-4 col-lg-3";
                }

                CargarCombos();
                CargarPacientesConFiltros();
            }
        }


        private void CargarCombos()
        {
            try
            {
                ddlCobertura.Items.Clear();
                ddlCobertura.Items.Add(new ListItem("Todas las coberturas", "-1"));

                var coberturas = _servicioCobertura.Listar("activo");
                foreach (var c in coberturas)
                {
                    ddlCobertura.Items.Add(new ListItem(c.Nombre, c.IdCobertura.ToString()));
                }
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "No se pudieron cargar las listas desplegables: " + ex.Message);
            }
        }



        private void CargarPacientesConFiltros(string estado = null)
        {
            List<PacienteListadoDto> todosLosPacientes = new List<PacienteListadoDto>();
            List<PacienteListadoDto> listaFiltrada = new List<PacienteListadoDto>();

            string textoBuscar = txtBuscar.Text.Trim().ToUpper();
            string idCoberturaSeleccionada = ddlCobertura.SelectedValue;
            string estadoSeleccionado = ddlEstado.SelectedValue;

            try
            {
                string estadoParaServicio = string.IsNullOrEmpty(estadoSeleccionado) ? null : estadoSeleccionado;
                todosLosPacientes = _servicioPaciente.Listar(estadoParaServicio);

            }
            catch (Exception ex)
            {
                gvPacientes.DataSource = null;
                gvPacientes.DataBind();
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "Error al obtener pacientes: " + ex.Message);
                return;
            }

            foreach (PacienteListadoDto p in todosLosPacientes)
            {
                bool cumple = true;

                if (!string.IsNullOrEmpty(textoBuscar))
                {
                    string nombre = p.NombreCompleto != null ? ValidadorCampos.NormalizarTexto(p.NombreCompleto) : "";
                    string dni = p.Dni != null ? p.Dni : "";
                    string email = p.Email != null ? p.Email.ToUpper() : "";

                    if (!nombre.Contains(textoBuscar) &&
                        !dni.Contains(textoBuscar) &&
                        !email.Contains(textoBuscar))
                    {
                        cumple = false;
                    }
                }

                if (cumple && idCoberturaSeleccionada != "-1")
                {
                    int idCobFiltro = int.Parse(idCoberturaSeleccionada);

                    if (p.IdCobertura == null || p.IdCobertura != idCobFiltro)
                    {
                        cumple = false;
                    }
                }


                if (cumple)
                {
                    listaFiltrada.Add(p);
                }
            }

            gvPacientes.DataSource = listaFiltrada;
            gvPacientes.DataBind();
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            gvPacientes.PageIndex = 0;
            CargarPacientesConFiltros();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = string.Empty;
            ddlCobertura.SelectedIndex = 0;
            ddlEstado.SelectedValue = "Activo";

            gvPacientes.PageIndex = 0;
            CargarPacientesConFiltros();
        }


        protected void btnNuevoPaciente_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Pacientes/Nuevo", false);
        }


        protected void gvPacientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPacientes.PageIndex = e.NewPageIndex;
            CargarPacientesConFiltros();
        }


        protected void gvPacientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PacienteListadoDto pacienteDto = (PacienteListadoDto)e.Row.DataItem;

                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");
                var btnAgendar = (LinkButton)e.Row.FindControl("btnAgendarTurno");
                var btnEditar = (LinkButton)e.Row.FindControl("btnEditar");
                var btnEliminar = (HtmlControl)e.Row.FindControl("btnEliminar");

                if (pacienteDto != null)
                {
                    bool esActivo = string.Equals(pacienteDto.Estado.ToString(), "Activo", StringComparison.OrdinalIgnoreCase)
                                 || string.Equals(pacienteDto.Estado.ToString(), "A", StringComparison.OrdinalIgnoreCase);

                    if (lblEstado != null)
                    {
                        lblEstado.Attributes["class"] = esActivo ? "badge badge-success" : "badge badge-warning";
                        lblEstado.InnerText = esActivo ? "Activo" : "Inactivo";
                    }

                    if (btnAgendar != null)
                    {
                        bool permisoAgendar = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "TURNOS", "CREAR");
                        if (!permisoAgendar)
                        {
                            btnAgendar.Visible = false;
                        }
                        if (!esActivo)
                        {
                            btnAgendar.Enabled = false;
                            btnAgendar.CssClass += " disabled border-0 opacity-50";
                            btnAgendar.ToolTip = "No se puede agendar a un paciente inactivo";
                        }
                    }
                    if (btnEditar != null)
                    {
                        btnEditar.Visible = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "PACIENTES", "EDITAR");
                    }

                    if (btnEliminar != null)
                    {
                        bool permisoEliminar = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "PACIENTES", "ELIMINAR");

                        btnEliminar.Visible = permisoEliminar && esActivo;
                        if (btnEliminar.Visible)
                        {
                            btnEliminar.Attributes["onclick"] = $"abrirModalConfirmacion('{pacienteDto.IdPaciente}', 'paciente');";
                        }
                    }
                }
            }
        }



        protected void gvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idPaciente = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Pages/Pacientes/Editar?id-paciente={idPaciente}", false);
            }
            else if (e.CommandName == "Ver")
            {
                Response.Redirect($"~/Pages/Pacientes/Detalle?id-paciente={idPaciente}", false);
            }
            else if (e.CommandName == "Agendar")
            {
                Response.Redirect($"~/Pages/Turnos/Nuevo?id-paciente={idPaciente}", false);
            }
        }



        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            int idPaciente = int.Parse(hdnIdEliminar.Value);
            try
            {
                _servicioPaciente.DarDeBaja(idPaciente);

                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Paciente dado de baja",
                    "El paciente fue dado de baja correctamente.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Pacientes/Index"),
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
                    "Ocurrió un error al intentar dar de baja el paciente. " + ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado"
                );
            }

        }
    }
}