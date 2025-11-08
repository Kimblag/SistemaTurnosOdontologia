using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using SGTO.Negocio.DTOs.Pacientes;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
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

        protected void gvTurnosPaciente_RowCommand(object sender, GridViewCommandEventArgs e) { }


    }
}