using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs.ParametroSistema;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
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
        private readonly ParametroService _servicioParametros = new ParametroService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Configuracion");
                master.EstablecerTituloSeccion(this.Page.Title);
            }

            if (!IsPostBack)
            {
                CargarParametros();

                ModalHelper.MostrarModalDesdeSession(this.Page, "ConfigMensajeTitulo", "ConfigMensajeDesc", "/Pages/Configuracion/Index");
            }
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
                ddlDuracionTurno.SelectedValue = dto.DuracionTurnoMinutos.ToString();
                txtHorarioInicio.Text = dto.HoraInicio ?? string.Empty;
                txtHorarioCierre.Text = dto.HoraCierre ?? string.Empty;
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
            try
            {
                string nombreClinica = txtNombreClinica.Text.Trim();
                string duracionTurnoStr = ddlDuracionTurno.SelectedValue;
                string horaInicio = txtHorarioInicio.Text;
                string horaCierre = txtHorarioCierre.Text;
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