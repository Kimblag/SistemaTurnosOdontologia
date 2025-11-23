using SGTO.Comun.Validacion;
using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Mappers;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.Pages.Turnos;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SGTO.UI.Webforms.Controles.Especialidades
{
    public partial class EspecialidadForm : System.Web.UI.UserControl
    {
        private readonly EspecialidadService _especialidadService;
        private readonly TratamientoService _tratamientoService;

        public bool ModoEdicion { get; set; } = false;

        public EspecialidadForm()
        {
            _especialidadService = new EspecialidadService();
            _tratamientoService = new TratamientoService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int idEspecialidad = ValidarModoEdicion();

            if (!IsPostBack)
            {
                if (ModoEdicion)
                {
                    CargarDetalleEspecialidad(idEspecialidad);
                    CargarTratamientosAsociados(idEspecialidad);
                }

                ModalHelper.MostrarModalDesdeSession(this.Page, "EspecialidadMensajeTitulo", "EspecialidadMensajeDesc", "/Pages/Especialidades/Index");
            }
        }

        private int ValidarModoEdicion()
        {
            int idEspecialidad = ExtraerIdEspecialidad();

            if (idEspecialidad != 0)
            {
                ModoEdicion = true;
                chkActivo.Enabled = true;
                panelTratamientos.Visible = true;
            }
            else
            {
                panelTratamientos.Visible = false;
                chkActivo.Checked = true;
                chkActivo.Enabled = false;
            }
            return idEspecialidad;
        }



        private int ExtraerIdEspecialidad()
        {
            string idString = Request.QueryString["id-especialidad"] ?? string.Empty;
            return int.TryParse(idString, out int id) ? id : 0;
        }

        public void CargarDetalleEspecialidad(int idEspecialidad)
        {
            try
            {
                EspecialidadDto dto = _especialidadService.ObtenerEspecialidadPorId(idEspecialidad);
                if (dto != null)
                {
                    txtNombre.Text = dto.Nombre;
                    txtDescripcion.Text = dto.Descripcion;
                    chkActivo.Checked = dto.Estado.ToLower() == "activo";
                }
                else
                {
                    MensajeUiHelper.SetearYMostrar(this.Page,
                         "Especialidad no encontrada",
                         "No se encontró la especialidad solicitada.",
                         "Resultado",
                         VirtualPathUtility.ToAbsolute("~/Pages/Especialidades/Index"),
                         "abrirModalResultado");
                }
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page,
                     "Error inesperado",
                     "Ocurrió un error al cargar la especialidad. " + ex.Message,
                     "Resultado",
                     VirtualPathUtility.ToAbsolute("~/Pages/Especialidades/Index"),
                     "abrirModalResultado");
            }
        }



        private void CargarTratamientosAsociados(int idEspecialidad)
        {
            try
            {
                List<TratamientoDto> lista = _tratamientoService.ListarPorEspecialidad(idEspecialidad);

                gvTratamientos.DataSource = lista;
                gvTratamientos.DataBind();
            }
            catch (Exception)
            {
            }
        }

        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ModoEdicion)
            {
                ModificarEspecialidad();
            }
            else
            {
                CrearEspecialidad();
            }
        }

        private void CrearEspecialidad()
        {
            try
            {
                ValidarCamposFormulario();

                string nombre = txtNombre.Text.Trim();
                string descripcion = txtDescripcion.Text.Trim();
                string estado = chkActivo.Checked ? "activo" : "inactivo";

                EspecialidadDto especialidadDto = EspecialidadMapper.MapearADto(0, nombre, descripcion, estado);

                _especialidadService.GuardarNuevaEspecialidad(especialidadDto);

                MensajeUiHelper.SetearYMostrar(
                     this.Page,
                     "Especialidad creada",
                     "La especialidad se ha creado correctamente.",
                     "Resultado",
                     VirtualPathUtility.ToAbsolute("~/Pages/Especialidades/Index"),
                     "abrirModalResultado"
                 );
            }
            catch (ArgumentException ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Dato inválido", ex.Message, "Resultado", null, "abrirModalResultado");
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Operación no permitida", ex.Message, "Resultado", null, "abrirModalResultado");
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error inesperado", "Error al crear: " + ex.Message, "Resultado", null, "abrirModalResultado");
            }
        }

        public void ModificarEspecialidad()
        {
            int idEspecialidad = ExtraerIdEspecialidad();

            try
            {
                ValidarCamposFormulario();

                string nombre = txtNombre.Text.Trim();
                string descripcion = txtDescripcion.Text.Trim();
                string estado = chkActivo.Checked ? "activo" : "inactivo";

                EspecialidadDto especialidadDto = EspecialidadMapper.MapearADto(idEspecialidad, nombre, descripcion, estado);

                _especialidadService.ModificarEspecialidad(especialidadDto);

                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Especialidad modificada",
                    "La especialidad ha sido actualizada correctamente.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Especialidades/Index"),
                    "abrirModalResultado"
                );
            }
            catch (ArgumentException ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Dato inválido", ex.Message, "Resultado", null, "abrirModalResultado");
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Operación no permitida", ex.Message, "Resultado", null, "abrirModalResultado");
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error inesperado", "Error al modificar: " + ex.Message, "Resultado", null, "abrirModalResultado");
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/Especialidades/Index.aspx", false);
        }

        private void ValidarCamposFormulario()
        {
            string nombre = txtNombre.Text;
            string descripcion = txtDescripcion.Text;

            if (!ValidadorCampos.EsTextoValido(nombre, 3, 50))
                throw new ArgumentException("El nombre debe tener entre 3 y 50 caracteres y no puede estar vacío.");

            if (!string.IsNullOrWhiteSpace(descripcion) && !ValidadorCampos.EsTextoValido(descripcion, 10, 200))
                throw new ArgumentException("La descripción debe tener al menos 10 caracteres si se completa.");
        }
    }
}