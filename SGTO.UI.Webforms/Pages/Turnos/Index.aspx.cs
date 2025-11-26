using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Turnos;
using SGTO.Negocio.Seguridad;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Seguridad;
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
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();
        private readonly TurnoService _servicioTurno = new TurnoService();
        private readonly MedicoService _servicioMedico = new MedicoService();
        private readonly CoberturaService _servicioCobertura = new CoberturaService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.EstaLogueado())
            {
                Response.Redirect("~/Pages/Login/Index.aspx");
                return;
            }

            if (!_servicioAutorizacion.TienePermiso(SessionManager.Usuario, "TURNOS", "VER"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("turnos");
                master.EstablecerTituloSeccion("Gestión de Turnos");
                master.EstablecerSubtituloSeccion("Visualice y administre la agenda diaria. Utilice los filtros para encontrar turnos específicos.");
            }

            if (!IsPostBack)
            {

                ConfigurarVistaPorRol();

                if (pnlFiltroMedico.Visible)
                {
                    // solo se carag el dropdown si el usuario no es médico
                    CargarMedicosDropdown();
                }
                CargarCoberturasDropdown();

                txtFecha.Text = string.Empty;

                CargarTurnosConFiltros();
            }
        }

        private void ConfigurarVistaPorRol()
        {
            // si usuario es medico, entonces ocultar el filtro, porque sólo puede ver sus turnos
            if (_servicioAutorizacion.EsMedico(SessionManager.Usuario))
            {
                pnlFiltroMedico.Visible = false;
            }
        }


        private void CargarMedicosDropdown()
        {
            try
            {
                ddlMedico.Items.Clear();

                ddlMedico.Items.Add(new ListItem("Todos los médicos", "-1"));

                var medicos = _servicioMedico.Listar("activos");

                foreach (var m in medicos)
                {
                    ddlMedico.Items.Add(new ListItem(m.NombreCompleto, m.IdMedico.ToString()));
                }
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "No se pudieron cargar los médicos: " + ex.Message);
            }
        }

        private void CargarCoberturasDropdown()
        {
            try
            {
                ddlCobertura.Items.Clear();
                ddlCobertura.Items.Add(new ListItem("Todas", "-1"));

                var coberturas = _servicioCobertura.Listar("activas");

                foreach (var c in coberturas)
                {
                    ddlCobertura.Items.Add(new ListItem(c.Nombre, c.IdCobertura.ToString()));
                }
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "Error cargando coberturas: " + ex.Message);
            }
        }

        private void CargarTurnosConFiltros()
        {
            try
            {
                // este dto es para ayudarnos a armar los filtros necesarios para la bd
                FiltroTurnoDto filtros = new FiltroTurnoDto();


                // verificar si hay filtro por fecha
                if (DateTime.TryParse(txtFecha.Text, out DateTime fechaSeleccionada))
                {
                    filtros.FechaInicio = fechaSeleccionada.Date;
                    filtros.FechaFin = fechaSeleccionada.Date.AddDays(1).AddSeconds(-1);
                }

                // verificar filtro de medicos, sólo estara visible para recepcionista o admin
                if (pnlFiltroMedico.Visible && ddlMedico.SelectedValue != "-1")
                {
                    if (int.TryParse(ddlMedico.SelectedValue, out int idMedicoSeleccionado))
                    {
                        filtros.IdMedico = idMedicoSeleccionado;
                    }
                }

                if (ddlCobertura.SelectedValue != "-1")
                {
                    if (int.TryParse(ddlCobertura.SelectedValue, out int idCoberturaSeleccionado))
                    {
                        filtros.IdCobertura = idCoberturaSeleccionado;
                    }
                }

                List<TurnoListadoDto> listaDesdeBase = _servicioTurno.ListarConFiltros(filtros, SessionManager.Usuario);
                List<TurnoListadoDto> listaFinal = new List<TurnoListadoDto>();

                string textoBuscar = ValidadorCampos.NormalizarTexto(txtBuscar.Text.Trim());
                string[] palabrasClave = string.IsNullOrEmpty(textoBuscar) 
                    ? new string[0]
                    : textoBuscar.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                string estadoSeleccionado = ddlEstado.SelectedValue;

                foreach (TurnoListadoDto turno in listaDesdeBase)
                {
                    bool coincideTexto = EsCoincidenciaDeBusqueda(turno, palabrasClave);

                    bool coincideEstado = true;
                    if (!string.IsNullOrEmpty(estadoSeleccionado))
                    {
                        if (!turno.Estado.Equals(estadoSeleccionado, StringComparison.OrdinalIgnoreCase))
                        {
                            coincideEstado = false;
                        }
                    }

                    if (coincideTexto && coincideEstado)
                    {
                        listaFinal.Add(turno);
                    }
                }

                listaFinal.Sort((x, y) =>
                {
                    int comparacionFecha = x.Fecha.CompareTo(y.Fecha);
                    if (comparacionFecha == 0)
                    {
                        return string.Compare(x.Hora, y.Hora);
                    }
                    return comparacionFecha;
                });

                gvTurnos.DataSource = listaFinal;
                gvTurnos.DataBind();

            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", "Ocurrió un error al cargar los turnos: " + ex.Message);
            }

        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            gvTurnos.PageIndex = 0;
            CargarTurnosConFiltros();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = string.Empty;

            txtFecha.Text = string.Empty;

            if (ddlMedico.Items.Count > 0) ddlMedico.SelectedIndex = 0;
            if (ddlEstado.Items.Count > 0) ddlEstado.SelectedIndex = 0;
            if (ddlCobertura.Items.Count > 0) ddlCobertura.SelectedIndex = 0;

            gvTurnos.PageIndex = 0;
            CargarTurnosConFiltros();
        }


        protected void gvTurnos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTurnos.PageIndex = e.NewPageIndex;
            CargarTurnosConFiltros();
        }

        protected void gvTurnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TurnoListadoDto turnoDto = (TurnoListadoDto)e.Row.DataItem;

                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");
                var btnEditar = (LinkButton)e.Row.FindControl("btnEditar");
                var btnAtender = (LinkButton)e.Row.FindControl("btnAtender");
                var btnDetalle = (LinkButton)e.Row.FindControl("btnDetalle");

                if (turnoDto != null)
                {
                    string estadoTurno = turnoDto.Estado != null ? turnoDto.Estado.ToLower() : "";

                    if (lblEstado != null)
                    {
                        lblEstado.Attributes["class"] = TurnoUiHelper.ObtenerCssEstadoTurnoBadge(estadoTurno);
                        lblEstado.InnerText = TurnoUiHelper.ObtenerTextoEstado(estadoTurno);
                    }

                    if (btnEditar != null)
                    {
                        // validar que el estado del turno permita editarlo
                        bool estadoPermiteEdicion = TurnoUiHelper.EsEditable(estadoTurno);

                        // verificar que el usuario actual tenga el permiso para editar
                        bool tienePermisoEditar = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "TURNOS", "EDITAR");

                        btnEditar.Visible = estadoPermiteEdicion && tienePermisoEditar;
                    }

                    if (btnAtender != null)
                    {
                        // validar que su estado permita atender un turno
                        bool esAtendible = (estadoTurno == "nuevo" || estadoTurno == "reprogramado" ||
                                             estadoTurno == "n" || estadoTurno == "r");

                        // validar que sean turnos del día actual, esto evita que atienda turnos del futuro.
                        bool esFechaHoy = turnoDto.Fecha.Date == DateTime.Today;

                        // validar que el usuario es un médico
                        bool esMedico = _servicioAutorizacion.EsMedico(SessionManager.Usuario);
                        bool puedeAtender = _servicioAutorizacion.TienePermiso(SessionManager.Usuario, "ATENCION", "VER");

                        btnAtender.Visible = esAtendible && esFechaHoy && esMedico && puedeAtender;
                    }

                    if (btnDetalle != null)
                    {
                        btnDetalle.Visible = true;
                    }
                }
            }
        }

        protected void gvTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument != null && !string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                if (int.TryParse(e.CommandArgument.ToString(), out int idTurno))
                {
                    if (e.CommandName == "Editar")
                    {
                        Response.Redirect($"~/Pages/Turnos/Editar?id-turno={idTurno}", false);
                    }
                    else if (e.CommandName == "Atender")
                    {
                        Response.Redirect($"~/Pages/Medicos/Atencion?id={idTurno}", false);
                    }
                    else if (e.CommandName == "Ver")
                    {
                        Response.Redirect($"~/Pages/Turnos/Detalle?id-turno={idTurno}", false);
                    }
                }
            }
        }

        private bool EsCoincidenciaDeBusqueda(TurnoListadoDto turno, string[] palabrasClave)
        {
            // creé este método para hacer búsquedas por tokens eb kugar de usar un simple contains
            // ya que había una inconsistencia al buscar.
            // Por ejemplo en el listado se ve Blandon Kim, pero si buscamos "kim blandon"
            // el contains no lo ubica porque compara siguiendo el orden exacto de los caracteres.

            if (palabrasClave == null || palabrasClave.Length == 0)
                return true;

            string nombrePac = ValidadorCampos.NormalizarTexto(turno.NombrePaciente) ?? "";
            string dniPac = turno.DniPaciente ?? "";
            string matricula = turno.Matricula ?? "";
            string nombreMed = ValidadorCampos.NormalizarTexto(turno.NombreMedico) ?? "";

            // agregar espacios entre cada token para evitar errores de palabras pegadas a la siguiente
            string datosTurnoConcatenados = string.Format("{0} {1} {2} {3}", nombrePac, dniPac, matricula, nombreMed);

            // se verifica que todos los tokens existan
            foreach (string palabra in palabrasClave)
            {
                //si falta al menos una, ya no es coincidencia
                if (!datosTurnoConcatenados.Contains(palabra))
                {
                    return false;
                }
            }
            return true;
        }
    }
}