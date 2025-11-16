using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace SGTO.UI.Webforms.Pages.Turnos
{
    public partial class Index : System.Web.UI.Page
    {

        private readonly TurnoService _servicioTurno = new TurnoService();
        private readonly MedicoService _servicioMedico = new MedicoService();
        private readonly EspecialidadService _servicioEspecialidad = new EspecialidadService();
        private readonly CoberturaService _servicioCobertura = new CoberturaService();

        private const string KEY_TURNO_BUSQUEDA = "FiltroTurnoBusqueda";
        private const string KEY_TURNO_CAMPO = "FiltroTurnoCampo";
        private const string KEY_TURNO_CRITERIO = "FiltroTurnoCriterio";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("turnos");
                master.EstablecerTituloSeccion(this.Page.Title);
            }
            if (!IsPostBack)
            {
                txtBuscar.Text = Session[KEY_TURNO_BUSQUEDA] as string ?? string.Empty;
                string campo = Session[KEY_TURNO_CAMPO] as string;
                if (!string.IsNullOrEmpty(campo))
                {
                    if (ddlCampo.Items.FindByValue(campo) != null)
                    {
                        ddlCampo.SelectedValue = campo;
                        CargarCriterios(campo);
                    }
                }

                string criterio = Session[KEY_TURNO_CRITERIO] as string;
                if (!string.IsNullOrEmpty(criterio) && ddlCriterio.Items.FindByValue(criterio) != null)
                {
                    ddlCriterio.SelectedValue = criterio;
                    ddlCriterio.Enabled = true;
                }
                AplicarFiltros();
            }
        }


        private void CargarTurnos()
        {
            List<TurnoListadoDto> turnos = new List<TurnoListadoDto>();
            try
            {
                turnos = _servicioTurno.Listar();

                gvTurnos.DataSource = turnos;
                gvTurnos.DataBind();
            }
            catch (Exception ex)
            {
                gvTurnos.DataSource = turnos;
                gvTurnos.DataBind();
                MensajeUiHelper.SetearYMostrar(
                   this.Page,
                   "Error al cargar los turnos",
                   "Ocurrió un error inesperado al intentar obtener la lista de turnos." + ex.Message
               );
            }
        }

        private void CargarMedicosDropdown()
        {
            try
            {
                var medicos = _servicioMedico.Listar("activos");
                foreach (var m in medicos)
                {
                    ddlCriterio.Items.Add(new ListItem(m.NombreCompleto, m.IdMedico.ToString()));
                }
            }
            catch (Exception) { throw; }
        }

        private void CargarEspecialidadesDropdown()
        {
            try
            {
                var especialidades = _servicioEspecialidad.Listar("activo");
                foreach (var e in especialidades)
                {
                    ddlCriterio.Items.Add(new ListItem(e.Nombre, e.IdEspecialidad.ToString()));
                }
            }
            catch (Exception) { throw; }
        }

        private void CargarCoberturasDropdown()
        {
            try
            {
                var coberturas = _servicioCobertura.Listar("activo");
                foreach (var c in coberturas)
                {
                    ddlCriterio.Items.Add(new ListItem(c.Nombre, c.IdCobertura.ToString()));
                }
            }
            catch (Exception) { throw; }
        }


        private void CargarCriterios(string campo)
        {
            ddlCriterio.Items.Clear();
            ddlCriterio.Enabled = false;

            if (string.IsNullOrEmpty(campo))
            {
                ddlCriterio.Items.Add(new ListItem("Seleccione un criterio", ""));
                return;
            }

            campo = campo.ToLower();
            ddlCriterio.Items.Add(new ListItem("Seleccione un criterio", ""));
            ddlCriterio.Enabled = true;

            if (campo == "estado")
            {
                ddlCriterio.Items.Add(new ListItem("Nuevo", "Nuevo"));
                ddlCriterio.Items.Add(new ListItem("Pendiente Reprogramación", "PendienteReprogramacion"));
                ddlCriterio.Items.Add(new ListItem("Reprogramado", "Reprogramado"));
                ddlCriterio.Items.Add(new ListItem("No asistió", "NoAsistio"));
                ddlCriterio.Items.Add(new ListItem("Cancelado", "Cancelado"));
                ddlCriterio.Items.Add(new ListItem("Cerrado", "Cerrado"));
            }
            else if (campo == "medico")
            {
                CargarMedicosDropdown();
            }
            else if (campo == "especialidad")
            {
                CargarEspecialidadesDropdown();
            }
            else if (campo == "cobertura")
            {
                CargarCoberturasDropdown();
            }

            if (ddlCriterio.Items.Count > 0)
                ddlCriterio.SelectedIndex = 0;
        }

        private void AplicarFiltros()
        {
            string textoBusqueda = txtBuscar.Text.Trim();
            string campo = ddlCampo.SelectedValue;
            string criterio = ddlCriterio.SelectedValue;

            Session[KEY_TURNO_BUSQUEDA] = string.IsNullOrEmpty(textoBusqueda) ? null : textoBusqueda;
            Session[KEY_TURNO_CAMPO] = string.IsNullOrEmpty(campo) ? null : campo;
            Session[KEY_TURNO_CRITERIO] = string.IsNullOrEmpty(criterio) ? null : criterio;

            List<TurnoListadoDto> lista = new List<TurnoListadoDto>();
            try
            {
                lista = _servicioTurno.Listar();
            }
            catch (Exception ex)
            {
                gvTurnos.DataSource = lista;
                gvTurnos.DataBind();
                MensajeUiHelper.SetearYMostrar(
                   this.Page,
                   "Error al cargar los turnos",
                   "Ocurrió un error inesperado al intentar obtener la lista de turnos." + ex.Message
               );
                return;
            }

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                string texto = ValidadorCampos.NormalizarTexto(textoBusqueda);
                List<TurnoListadoDto> filtrada = new List<TurnoListadoDto>();

                foreach (TurnoListadoDto t in lista)
                {
                    string nombrePaciente = ValidadorCampos.NormalizarTexto(t.NombrePaciente);
                    string nombreMedico = ValidadorCampos.NormalizarTexto(t.NombreMedico);
                    bool coincide =
                        (!string.IsNullOrEmpty(nombrePaciente) && nombrePaciente.Contains(texto)) ||
                        (!string.IsNullOrEmpty(nombreMedico) && nombreMedico.Contains(texto));

                    if (coincide)
                        filtrada.Add(t);
                }
                lista = filtrada;
            }

            if (!string.IsNullOrEmpty(campo) && !string.IsNullOrEmpty(criterio))
            {
                List<TurnoListadoDto> filtrada = new List<TurnoListadoDto>();

                if (campo == "Estado")
                {
                    foreach (var t in lista)
                    {
                        if (!string.IsNullOrEmpty(t.Estado) &&
                            t.Estado.Equals(criterio, StringComparison.OrdinalIgnoreCase))
                        {
                            filtrada.Add(t);
                        }
                    }
                }
                else if (campo == "Medico")
                {
                    if (int.TryParse(criterio, out int idMedicoSeleccionado))
                    {
                        foreach (var t in lista)
                        {
                            if (t.IdMedico == idMedicoSeleccionado)
                                filtrada.Add(t);
                        }
                    }
                }
                else if (campo == "Especialidad")
                {
                    if (int.TryParse(criterio, out int idEspecialidadSeleccionada))
                    {
                        foreach (var t in lista)
                        {
                            if (t.IdEspecialidad == idEspecialidadSeleccionada)
                                filtrada.Add(t);
                        }
                    }
                }
                else if (campo == "Cobertura")
                {
                    if (int.TryParse(criterio, out int idCoberturaSeleccionada))
                    {
                        foreach (var t in lista)
                        {
                            if (t.IdCobertura == idCoberturaSeleccionada)
                                filtrada.Add(t);
                        }
                    }
                }
                lista = filtrada;
            }
            gvTurnos.DataSource = lista;
            gvTurnos.DataBind();
        }


        protected void txtBuscar_TextChanged(object sender, EventArgs e) { }

        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string campo = ddlCampo.SelectedValue;
            Session[KEY_TURNO_CAMPO] = string.IsNullOrEmpty(campo) ? null : campo;
            CargarCriterios(campo);
            Session[KEY_TURNO_CRITERIO] = null;
        }

        protected void ddlCriterio_SelectedIndexChanged(object sender, EventArgs e) { }

        protected void gvTurnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TurnoListadoDto turnoDto = (TurnoListadoDto)e.Row.DataItem;

                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");

                if (lblEstado != null && turnoDto != null)
                {
                    string estadoTurno = turnoDto.Estado.ToLower();

                    switch (estadoTurno)
                    {
                        case "nuevo":
                            lblEstado.InnerText = "Nuevo";
                            lblEstado.Attributes["class"] = "badge badge-primary";
                            break;
                        case "cancelado":
                            lblEstado.InnerText = "Cancelado";
                            lblEstado.Attributes["class"] = "badge badge-danger";
                            break;
                        case "pendientereprogramacion":
                            lblEstado.InnerText = "Pendiente Reprogramación";
                            lblEstado.Attributes["class"] = "badge badge-pending";
                            break;
                        case "reprogramado":
                            lblEstado.InnerText = "Reprogramado";
                            lblEstado.Attributes["class"] = "badge badge-info";
                            break;
                        case "noasistio":
                            lblEstado.InnerText = "No asistió";
                            lblEstado.Attributes["class"] = "badge badge-dark";
                            break;
                        case "cerrado":
                            lblEstado.InnerText = "Cerrado";
                            lblEstado.Attributes["class"] = "badge badge-completed";
                            break;
                        default:
                            lblEstado.InnerText = "Indefinido";
                            lblEstado.Attributes["class"] = "badge badge-secondary";
                            break;
                    }
                }
            }
        }

        protected void gvTurnos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTurnos.PageIndex = e.NewPageIndex;
            AplicarFiltros();
        }

        protected void gvTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument != null && !string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                int idTurno = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName == "Editar")
                {
                    Response.Redirect($"~/Pages/Turnos/Editar?id-turno={idTurno}", false);
                }
                else if (e.CommandName == "Ver")
                {
                    Response.Redirect($"~/Pages/Turnos/Detalle?id-turno={idTurno}", false);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Session[KEY_TURNO_BUSQUEDA] = null;
            Session[KEY_TURNO_CAMPO] = null;
            Session[KEY_TURNO_CRITERIO] = null;

            txtBuscar.Text = string.Empty;
            ddlCampo.SelectedIndex = 0;
            ddlCriterio.Items.Clear();
            ddlCriterio.Items.Add(new ListItem("Seleccione un criterio", ""));
            ddlCriterio.Enabled = false;

            CargarTurnos();
        }
    }
}