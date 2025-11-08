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
        public bool ModoEdicion { get; set; } = false;
        private readonly EspecialidadService _especialidadService;

        public EspecialidadForm()
        {
            _especialidadService = new EspecialidadService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int idEspecialidad = ExtraerIdEspecialidad();

            if (idEspecialidad != 0)
            {
                ModoEdicion = true;
                chkActivo.Enabled = true;
            }

            if (!IsPostBack)
            {
                if (idEspecialidad != 0)
                {
                    CargarDetalleEspecialidad(idEspecialidad);
                }
            }
        }

        public int ExtraerIdEspecialidad()
        {
            string idString = Request.QueryString["id-especialidad"] ?? string.Empty;
            if (!string.IsNullOrEmpty(idString) && int.TryParse(idString, out int id))
            {
                return id;
            }
            return 0;
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
                    Response.Redirect("~/Pages/Especialidades/Index.aspx", false);
                }
            }
            catch (Exception)
            {
                Response.Redirect("~/Pages/Especialidades/Index.aspx", false);
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
            string nombre = txtNombre.Text;
            string descripcion = txtDescripcion.Text;
            string estado = chkActivo.Checked ? "activo" : "inactivo";
            EspecialidadDto especialidadDto = EspecialidadMapper.MapearADto(0, nombre, descripcion, estado);

            try
            {
                ValidarCampos();

                _especialidadService.GuardarNuevaEspecialidad(especialidadDto);
                Session["EspecialidadMensajeTitulo"] = "Especialidad creada";
                Session["EspecialidadMensajeDesc"] = "La especialidad se ha creado correctamente.";
                ModalHelper.MostrarModalDesdeSession(this.Page,
                    "EspecialidadMensajeTitulo",
                    "EspecialidadMensajeDesc",
                    VirtualPathUtility.ToAbsolute("~/Pages/Especialidades/Index"),
                    "abrirModalResultado");
            }
            catch (ExcepcionReglaNegocio ex)
            {
                Session["EspecialidadMensajeTitulo"] = "Operación no permitida";
                Session["EspecialidadMensajeDesc"] = ex.Message;
                Session["ModalTipo"] = "Resultado";
                ModalHelper.MostrarModalDesdeSession(this.Page, "EspecialidadMensajeTitulo", "EspecialidadMensajeDesc", null, "abrirModalResultado");
                return;
            }
            catch (Exception ex)
            {
                Session["EspecialidadMensajeTitulo"] = "Error inesperado";
                Session["EspecialidadMensajeDesc"] = "Ocurrió un error al intentar crear la especialidad." + ex.Message;
                Session["ModalTipo"] = "Resultado";
                ModalHelper.MostrarModalDesdeSession(this.Page, "EspecialidadMensajeTitulo", "EspecialidadMensajeDesc", null, "abrirModalResultado");
                return;
            }
        }

        public void ModificarEspecialidad()
        {
            int idEspecialidad = ExtraerIdEspecialidad();
            string nombre = txtNombre.Text;
            string descripcion = txtDescripcion.Text;
            string estado = chkActivo.Checked ? "activo" : "inactivo";

            try
            {
                EspecialidadDto especialidadDto = EspecialidadMapper.MapearADto(idEspecialidad, nombre, descripcion, estado);
                _especialidadService.ModificarEspecialidad(especialidadDto);
                Session["CoberturaMensajeTitulo"] = "Especialidad modificada con éxito.";
                Session["CoberturaMensajeDesc"] = "La especialidad ha sido actualizada correctamente.";
                Session["ModalTipo"] = "Resultado";
                Response.Redirect($"~/Pages/Especialidades/Index.aspx", false);
            }
            catch (ExcepcionReglaNegocio ex)
            {
                Session["CoberturaMensajeTitulo"] = "Operación no permitida";
                Session["CoberturaMensajeDesc"] = ex.Message;
                Session["ModalTipo"] = "Resultado";
                ModalHelper.MostrarModalDesdeSession(this.Page, "CoberturaMensajeTitulo", "CoberturaMensajeDesc", null, "abrirModalResultado");
            }
            catch (Exception)
            {
                Session["CoberturaMensajeTitulo"] = "Error inesperado";
                Session["CoberturaMensajeDesc"] = "Ocurrió un error al intentar dar de baja la cobertura.";
                Session["ModalTipo"] = "Resultado";
                ModalHelper.MostrarModalDesdeSession(this.Page, "CoberturaMensajeTitulo", "CoberturaMensajeDesc", null, "abrirModalResultado");
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/Especialidades/Index.aspx", false);
        }

        private bool ValidarCampos()
        {
            string nombre = txtNombre.Text;
            string descripcion = txtDescripcion.Text;

            if (!ValidadorCampos.EsTextoValido(nombre, 3, 50))
                throw new ExcepcionReglaNegocio("El nombre debe tener entre 3 y 50 caracteres y no puede estar vacío.");

            if (!string.IsNullOrWhiteSpace(descripcion) && !ValidadorCampos.TieneLongitudMinima(descripcion, 10))
                throw new ExcepcionReglaNegocio("La descripción debe tener al menos 10 caracteres si se completa.");

            return true;
        }
    }
}