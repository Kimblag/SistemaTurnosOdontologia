using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Pacientes;
using SGTO.Negocio.DTOs.Turnos;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Pacientes");
                master.EstablecerTituloSeccion(this.Page.Title);
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
                CargarDetallePaciente(idPaciente);
                ModalHelper.MostrarModalDesdeSession(this.Page, "PacienteMensajeTitulo", "PacienteMensajeDesc", "/Pages/Pacientes/Index");
            }
        }

        private int ExtraerIdPaciente()
        {
            string idStr = Request.QueryString["id-paciente"] ?? string.Empty;
            return int.TryParse(idStr, out int id) ? id : 0;
        }

        private void CargarDetallePaciente(int idPaciente)
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
                        VirtualPathUtility.ToAbsolute("~/Pages/Pacientes/Index"),
                        "abrirModalResultado"
                    );
                    return;
                }

                CargarDatosPaciente(dto);
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Operación no permitida",
                    ex.Message,
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Pacientes/Index"),
                    "abrirModalResultado"
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al cargar detalle de paciente: " + ex.Message);
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error inesperado",
                    "Ocurrió un error al intentar cargar el detalle del paciente.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Pacientes/Index"),
                    "abrirModalResultado"
                );
            }
        }


        private void CargarDatosPaciente(PacienteDetalleDto dto)
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

        protected void gvTurnosPaciente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTurnosPaciente.PageIndex = e.NewPageIndex;
                int idPaciente = ExtraerIdPaciente();
                CargarDetallePaciente(idPaciente);
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
                    string estadoTurno = turnoDto.Estado.ToLower();

                    switch (estadoTurno)
                    {
                        case "nuevo":
                            divEstadoTurno.InnerText = "Nuevo";
                            divEstadoTurno.Attributes["class"] = "badge badge-primary";
                            break;
                        case "cancelado":
                            divEstadoTurno.InnerText = "Cancelado";
                            divEstadoTurno.Attributes["class"] = "badge badge-danger";
                            break;
                        case "pendientereprogramacion":
                            divEstadoTurno.InnerText = "Pendiente Reprogramación";
                            divEstadoTurno.Attributes["class"] = "badge badge-pending";
                            break;
                        case "reprogramado":
                            divEstadoTurno.InnerText = "Reprogramado";
                            divEstadoTurno.Attributes["class"] = "badge badge-info";
                            break;
                        case "noasistio":
                            divEstadoTurno.InnerText = "No asistió";
                            divEstadoTurno.Attributes["class"] = "badge badge-dark";
                            break;
                        case "cerrado":
                            divEstadoTurno.InnerText = "Cerrado";
                            divEstadoTurno.Attributes["class"] = "badge badge-completed";
                            break;
                        default:
                            divEstadoTurno.InnerText = "Indefinido";
                            divEstadoTurno.Attributes["class"] = "badge badge-secondary";
                            break;
                    }
                }
            }
        }
    }
}