using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs.ParametroSistema;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Seguridad;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Seguridad;
using SGTO.UI.Webforms.Utils;
using System;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Configuracion.Parametros
{
    public partial class Index : System.Web.UI.Page
    {
        private readonly ServicioAutorizacion _servicioAutorizacion = new ServicioAutorizacion();
        private readonly ParametroService _servicioParametros = new ParametroService();

        private bool _puedeEditar = false;

        protected void Page_Load(object sender, EventArgs e)
        {

            var usuario = SessionManager.Usuario;

            if (!_servicioAutorizacion.TienePermiso(usuario, "PARAMETROSISTEMA", "VER"))
            {
                Response.Redirect("~/Pages/Errores/AccesoDenegado.aspx");
                return;
            }

            _puedeEditar = _servicioAutorizacion.TienePermiso(usuario, "PARAMETROSISTEMA", "EDITAR");

            if (Master is SiteMaster master)
            {
                master.ConfigurarBotonVolver(true, "~/Pages/Configuracion/Index.aspx");
                master.EstablecerOpcionMenuActiva("Configuracion");
                master.EstablecerTituloSeccion(this.Page.Title);
                master.EstablecerSubtituloSeccion("Defina las variables institucionales y técnicas de la aplicación.");
            }

            if (!IsPostBack)
            {
                CargarParametros();

                if (!_puedeEditar)
                {
                    BloquearFormulario();
                }

                ModalHelper.MostrarModalDesdeSession(this.Page, "ConfigMensajeTitulo", "ConfigMensajeDesc", "/Pages/Configuracion/Index");
            }
        }

        private void BloquearFormulario()
        {
            txtNombreClinica.Enabled = false;
            txtServidorCorreo.Enabled = false;
            txtPuertoCorreo.Enabled = false;
            txtUsuarioCorreo.Enabled = false;
            txtEmailRemitente.Enabled = false;
            txtReintentosEmail.Enabled = false;

            btnGuardar.Visible = false;

            btnCancelar.Text = "Volver";
            btnCancelar.CssClass = "btn btn-primary btn-sm";
        }


        private void CargarParametros()
        {
            try
            {
                ParametroSistemaDto dto = _servicioParametros.Obtener();
                if (dto == null)
                {
                    MensajeUiHelper.SetearYMostrar(
                        this.Page,
                        "Parámetros no encontrados",
                        "No se pudieron obtener los parámetros del sistema.",
                        "Resultado",
                        null,
                        "abrirModalResultado"
                    );
                    return;
                }

                txtNombreClinica.Text = dto.NombreClinica ?? string.Empty;
                txtUsuarioCorreo.Text = dto.UsuarioCorreo ?? string.Empty;
                txtServidorCorreo.Text = dto.ServidorCorreo ?? string.Empty;
                txtPuertoCorreo.Text = dto.PuertoCorreo > 0 ? dto.PuertoCorreo.ToString() : string.Empty;
                txtEmailRemitente.Text = dto.EmailRemitente ?? string.Empty;
                txtReintentosEmail.Text = dto.ReintentosEmail > 0 ? dto.ReintentosEmail.ToString() : "3";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al cargar parámetros: " + ex.Message);
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error inesperado",
                    "Ocurrió un error al intentar cargar los parámetros del sistema.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Configuracion/Index"),
                    "abrirModalResultado"
                );
            }
        }

        private static void ValidarCampos(string nombreClinica, string duracionTurnoStr,
            string servidorCorreo, string puertoStr,
            string remitente, string reintentosStr)
        {
            if (!ValidadorCampos.EsTextoValido(nombreClinica, 3, 100))
                throw new ArgumentException("El nombre de la clínica debe tener entre 3 y 100 caracteres.");

            if (!ValidadorCampos.EsEnteroPositivo(duracionTurnoStr))
                throw new ArgumentException("La duración del turno debe ser un número entero positivo.");

            if (!ValidadorCampos.EsEnteroPositivo(puertoStr))
                throw new ArgumentException("El puerto SMTP debe ser un número entero positivo.");

            if (!ValidadorCampos.EsEnteroPositivo(reintentosStr))
                throw new ArgumentException("La cantidad de reintentos debe ser un número entero positivo.");

            if (!ValidadorCampos.EsTextoObligatorio(servidorCorreo))
                throw new ArgumentException("Debe indicar un servidor SMTP.");

            if (!ValidadorCampos.EsEmailValido(remitente))
                throw new ArgumentException("El correo remitente no tiene un formato válido.");
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Configuracion/Index.aspx", false);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!_servicioAutorizacion.TienePermiso(SessionManager.Usuario, "PARAMETROSISTEMA", "EDITAR"))
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Acceso Denegado", "No tiene permisos para modificar la configuración.", "Error", null, "abrirModalResultado");
                return;
            }
            try
            {
                string nombreClinica = txtNombreClinica.Text.Trim();
                string duracionTurnoStr = "60";
                string horaInicio = "08:00";
                string horaCierre = "18:00";
                string servidorCorreo = txtServidorCorreo.Text.Trim();
                string puertoStr = txtPuertoCorreo.Text.Trim();
                string remitente = txtEmailRemitente.Text.Trim();
                string reintentosStr = txtReintentosEmail.Text.Trim();

                ValidarCampos(nombreClinica, duracionTurnoStr, servidorCorreo, puertoStr, remitente, reintentosStr);

                ParametroSistemaDto dto = new ParametroSistemaDto
                {
                    NombreClinica = nombreClinica,
                    DuracionTurnoMinutos = int.Parse(duracionTurnoStr),
                    HoraInicio = horaInicio,
                    HoraCierre = horaCierre,
                    ServidorCorreo = servidorCorreo,
                    PuertoCorreo = int.Parse(puertoStr),
                    EmailRemitente = remitente,
                    ReintentosEmail = int.Parse(reintentosStr)
                };

                _servicioParametros.Guardar(dto);

                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Parámetros guardados",
                    "Los parámetros del sistema se actualizaron correctamente.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Configuracion/Index"),
                    "abrirModalResultado"
                );

            }
            catch (ArgumentException ex)
            {
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Dato inválido",
                    ex.Message,
                    "Resultado",
                    null,
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
                    "Ocurrió un error al guardar los parámetros: " + ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado"
                );
            }
        }


    }
}