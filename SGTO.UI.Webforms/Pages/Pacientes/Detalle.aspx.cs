using SGTO.Negocio.DTOs.Pacientes;
using SGTO.Negocio.DTOs.Turnos;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.Negocio.Servicios.Exportacion;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Pacientes
{
    public partial class Detalle : System.Web.UI.Page
    {
        private readonly PacienteService _servicioPaciente = new PacienteService();
        private readonly ParametroService _servicioParametro = new ParametroService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Pacientes");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Vista consolidada de datos filiatorios, historial de atenciones y agenda futura.");
            }
            if (!IsPostBack)
            {
                int idPaciente = ExtraerIdPaciente();
                if (idPaciente == 0)
                {
                    MensajeUiHelper.SetearYMostrar(
                        this.Page,
                        "Paciente no encontrado",
                        "No se especificó un paciente válido.",
                        "Resultado",
                        VirtualPathUtility.ToAbsolute("~/Pages/Pacientes/Index"),
                        "abrirModalResultado"
                    );
                    return;
                }
                CargarTodo(idPaciente);
                ModalHelper.MostrarModalDesdeSession(this.Page, "PacienteMensajeTitulo", "PacienteMensajeDesc", "/Pages/Pacientes/Index");
            }
        }

        private int ExtraerIdPaciente()
        {
            string idStr = Request.QueryString["id-paciente"] ?? string.Empty;
            return int.TryParse(idStr, out int id) ? id : 0;
        }


        private void CargarTodo(int idPaciente)
        {
            try
            {
                PacienteDetalleDto dto = _servicioPaciente.ObtenerDetalle(idPaciente);

                if (dto == null)
                {
                    MensajeUiHelper.SetearYMostrar(
                        this.Page,
                        "Paciente no encontrado",
                        "No se encontró el paciente solicitado.",
                        "Resultado",
                        VirtualPathUtility.ToAbsolute("~/Pages/Pacientes/Index.aspx"),
                        "abrirModalResultado"
                    );
                    return;
                }

                CargarDatosPersonalesYAgenda(dto);

                CargarHistoriaClinica(idPaciente);
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Atención", ex.Message, "Resultado", VirtualPathUtility.ToAbsolute("~/Pages/Pacientes/Index.aspx"), "abrirModalResultado");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al cargar detalle: " + ex.Message);
                MensajeUiHelper.SetearYMostrar(this.Page, "Error inesperado", "Ocurrió un error al cargar los datos.", "Resultado", VirtualPathUtility.ToAbsolute("~/Pages/Pacientes/Index.aspx"), "abrirModalResultado");
            }
        }

        private void CargarDatosPersonalesYAgenda(PacienteDetalleDto dto)
        {
            lblNombreCompleto.Text = dto.NombreCompleto;
            lblDni.Text = dto.Dni;
            lblFechaNacimiento.Text = dto.FechaNacimiento;
            lblGenero.Text = dto.Genero;
            lblTelefono.Text = dto.Telefono;
            lblEmail.Text = dto.Email;
            lblCobertura.Text = dto.Cobertura;
            lblPlan.Text = dto.Plan;

            lblEstado.Text = dto.Estado;
            lblEstado.CssClass = dto.Estado == "Activo" ? "badge bg-success" : "badge bg-secondary";

            gvTurnosPaciente.DataSource = dto.Turnos;
            gvTurnosPaciente.DataBind();
        }

        private void CargarHistoriaClinica(int idPaciente)
        {
            try
            {
                List<HistoriaClinicaResumenDto> historia = _servicioPaciente.ObtenerHistoriaClinica(idPaciente);

                gvHistoriaClinica.DataSource = historia;
                gvHistoriaClinica.DataBind();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error cargando historia clínica: " + ex.Message);
            }
        }

        protected void gvTurnosPaciente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTurnosPaciente.PageIndex = e.NewPageIndex;
                CargarTodo(ExtraerIdPaciente());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en paginación de turnos: " + ex.Message);
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error de paginación",
                    "Ocurrió un error al intentar mostrar los turnos.",
                    "Resultado",
                    null,
                    "abrirModalResultado"
                );
            }
        }

        protected void gvTurnosPaciente_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void gvTurnosPaciente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TurnoPacienteDto turnoDto = (TurnoPacienteDto)e.Row.DataItem;

                var divEstadoTurno = (HtmlGenericControl)e.Row.FindControl("divEstadoTurno");

                if (divEstadoTurno != null && turnoDto != null)
                {
                    divEstadoTurno.InnerText = TurnoUiHelper.ObtenerTextoEstado(turnoDto.Estado);
                    divEstadoTurno.Attributes["class"] = TurnoUiHelper.ObtenerCssEstadoTurnoBadge(turnoDto.Estado);
                }
            }
        }

        protected void gvHistoriaClinica_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHistoriaClinica.PageIndex = e.NewPageIndex;
            CargarHistoriaClinica(ExtraerIdPaciente());
        }


        protected void btnExportarHistoria_Click(object sender, EventArgs e)
        {
            int idPaciente = ExtraerIdPaciente();
            if (idPaciente == 0) return;

            try
            {
                var pacienteDto = _servicioPaciente.ObtenerDetalle(idPaciente);
                var historial = _servicioPaciente.ObtenerHistoriaClinica(idPaciente);

                if (historial == null || historial.Count == 0)
                {
                    MensajeUiHelper.SetearYMostrar(this.Page, "Sin datos", "El paciente no tiene historia clínica para exportar.", "Cerrar", null, "abrirModalResultado");
                    return;
                }
                string nombreClinica = _servicioParametro.ObtenerValor("NombreClinica");

                byte[] pdfBytes = GeneradorPdf.GenerarHistoriaClinicaPdf(pacienteDto, historial, nombreClinica);

                Response.Clear();
                Response.ContentType = "application/pdf";
                string fileName = string.Format("HistoriaClinica_{0}.pdf", pacienteDto.Dni.Trim());
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.Buffer = true;
                Response.BinaryWrite(pdfBytes);
                Response.End();
            }
            catch (Exception ex)
            {
                // agrego esto porque estaba dando error por la excepción del redirect
                if (!(ex is System.Threading.ThreadAbortException))
                {
                    Debug.WriteLine("Error generando PDF: " + ex.Message);
                    MensajeUiHelper.SetearYMostrar(this.Page, "Error", "No se pudo generar el reporte PDF.", "Cerrar", null, "abrirModalResultado");
                }
            }
        }


    }
}