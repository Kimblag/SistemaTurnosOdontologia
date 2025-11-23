using SGTO.Negocio.Seguridad;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Seguridad;
using System;
using System.Web.UI;

namespace SGTO.UI.Webforms.Pages.Medicos
{
    public partial class Detalle : Page
    {
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();
        private readonly MedicoService _medicoService = new MedicoService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.EstaLogueado())
            {
                Response.Redirect("~/Pages/Login/Index.aspx");
                return;
            }

            if (!_servicioAutorizacion.TienePermiso(SessionManager.Usuario, "MEDICOS", "VER"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

            if (Master is SiteMaster master)
            {
                master.ConfigurarBotonVolver(true, "~/Pages/Medicos/Index.aspx");
                master.EstablecerOpcionMenuActiva("Medicos");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Información de matrícula, especialidades habilitadas y grilla horaria semanal.");
            }
            if (!IsPostBack)
            {
                CargarDatos();
            }
        }

        private void CargarDatos()
        {
            string idStr = Request.QueryString["id-medico"];
            if (int.TryParse(idStr, out int id))
            {
                try
                {
                    var medico = _medicoService.ObtenerDetalle(id);

                    if (medico != null)
                    {
                        lblNombre.Text = medico.NombreCompleto;
                        lblDni.Text = medico.NumeroDocumento;
                        lblNacimiento.Text = medico.FechaNacimiento.ToShortDateString();
                        lblGenero.Text = medico.Genero;
                        lblEmail.Text = medico.Email;
                        lblTelefono.Text = medico.Telefono;

                        bool activo = medico.Estado == "Activo" || medico.Estado == "A";
                        lblEstado.Text = activo ? "ACTIVO" : "INACTIVO";
                        lblEstado.CssClass = activo ? "badge bg-success" : "badge bg-danger";

                        lblMatricula.Text = medico.Matricula;
                        lblUsuario.Text = medico.NombreUsuario;

                        if (medico.FechaIncorporacion == DateTime.MinValue)
                        {
                            lblFechaAlta.Text = "-";
                        }
                        else
                        {
                            lblFechaAlta.Text = medico.FechaIncorporacion.ToShortDateString();
                        }
                        lblEspecialidades.Text = (medico.Especialidades != null && medico.Especialidades.Count > 0)
                            ? string.Join(", ", medico.Especialidades)
                            : "Sin especialidades";

                        lblCoberturas.Text = (medico.CoberturasAceptadas != null && medico.CoberturasAceptadas.Count > 0)
                            ? string.Join(", ", medico.CoberturasAceptadas)
                            : "Sin registros de atención";

                        lblTotalPacientes.Text = medico.CantidadPacientesAtendidos.ToString();

                        gvHistorial.DataSource = medico.HistorialTurnos;
                        gvHistorial.DataBind();


                    }
                    else
                    {
                        Response.Redirect("~/Pages/Medicos/Index.aspx");
                    }
                }
                catch (Exception)
                {
                    Response.Redirect("~/Pages/Errores/Error.aspx");
                }
            }
            else
            {
                Response.Redirect("~/Pages/Medicos/Index.aspx");
            }
        }
    }
}