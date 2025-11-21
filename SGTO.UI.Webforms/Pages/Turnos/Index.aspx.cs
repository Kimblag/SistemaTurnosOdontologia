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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("turnos");
                master.EstablecerTituloSeccion("Gestión de Turnos");
                master.EstablecerSubtituloSeccion("Visualice y administre la agenda diaria. Utilice los filtros para encontrar turnos específicos.");
            }

            if (!IsPostBack)
            {
                CargarMedicosDropdown();
                txtFecha.Text = string.Empty;
                CargarTurnosConFiltros();
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

        private void CargarTurnosConFiltros()
        {
            List<TurnoListadoDto> todosLosTurnos = new List<TurnoListadoDto>();

            List<TurnoListadoDto> listaFiltrada = new List<TurnoListadoDto>();

            try
            {
                todosLosTurnos = _servicioTurno.Listar();
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error", ex.Message);
                return;
            }

            string textoBuscar = ValidadorCampos.NormalizarTexto(txtBuscar.Text.Trim());
            string fechaTexto = txtFecha.Text;
            string idMedicoSeleccionado = ddlMedico.SelectedValue;
            string estadoSeleccionado = ddlEstado.SelectedValue;

            foreach (TurnoListadoDto turno in todosLosTurnos)
            {
                bool cumple = true;

                if (!string.IsNullOrEmpty(textoBuscar))
                {
                    string nombre = ValidadorCampos.NormalizarTexto(turno.NombrePaciente) ?? "";
                    string dni = turno.DniPaciente ?? "";
                    string matricula = turno.Matricula ?? "";
                    string nombreMedico = ValidadorCampos.NormalizarTexto(turno.NombreMedico) ?? "";
                    if (!nombre.ToUpper().Contains(textoBuscar) &&
                        !dni.Contains(textoBuscar) &&
                        !nombreMedico.Contains(textoBuscar) &&
                        !matricula.Contains(textoBuscar))
                    {
                        cumple = false;
                    }
                }

                if (cumple && !string.IsNullOrEmpty(fechaTexto))
                {
                    DateTime fechaFiltro;
                    if (DateTime.TryParse(fechaTexto, out fechaFiltro))
                    {
                        if (turno.Fecha.Date != fechaFiltro.Date) cumple = false;
                    }
                }

                if (cumple && idMedicoSeleccionado != "-1")
                {
                    if (turno.IdMedico != int.Parse(idMedicoSeleccionado)) cumple = false;
                }

                if (cumple && !string.IsNullOrEmpty(estadoSeleccionado))
                {
                    if (!turno.Estado.Equals(estadoSeleccionado, StringComparison.OrdinalIgnoreCase)) cumple = false;
                }

                if (cumple) listaFiltrada.Add(turno);
            }

            // se ordena la lista por fechas
            // si no seleccionó fechas entonces se ordenan para mostrar la lista ordenada desde el día actual
            if (string.IsNullOrEmpty(fechaTexto))
            {
                List<TurnoListadoDto> listaFuturos = new List<TurnoListadoDto>();
                List<TurnoListadoDto> listaPasados = new List<TurnoListadoDto>();
                DateTime hoy = DateTime.Today;

                // separar en dos listas para tener los turnos del futuro y los qu eya pasaron.
                foreach (TurnoListadoDto t in listaFiltrada)
                {
                    if (t.Fecha.Date >= hoy)
                        listaFuturos.Add(t);
                    else
                        listaPasados.Add(t);
                }

                //se ordenen los turnos futuros de menor a mayor
                listaFuturos.Sort((x, y) => x.Fecha.CompareTo(y.Fecha));

                // ordenado descendentemente los pasados
                listaPasados.Sort((x, y) => y.Fecha.CompareTo(x.Fecha));

                listaFiltrada.Clear();
                listaFiltrada.AddRange(listaFuturos);
                listaFiltrada.AddRange(listaPasados);
            }
            else
            {
                listaFiltrada.Sort((x, y) => string.Compare(x.Hora, y.Hora));
            }

            gvTurnos.DataSource = listaFiltrada;
            gvTurnos.DataBind();
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
                        btnEditar.Visible = TurnoUiHelper.EsEditable(estadoTurno);
                    }

                    if (btnAtender != null)
                    {
                        bool esAtendible = estadoTurno == "nuevo" || estadoTurno == "reprogramado" ||
                                           estadoTurno == "n" || estadoTurno == "r";

                        btnAtender.Visible = esAtendible;
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
    }
}