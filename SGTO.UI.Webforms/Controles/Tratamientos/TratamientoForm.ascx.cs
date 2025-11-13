using SGTO.Comun.Validacion;
using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.Utils;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Tratamientos
{
    public partial class TratamientoForm : UserControl
    {
        private readonly TratamientoService _tratamientoService;
        private readonly EspecialidadService _especialidadService;

        public bool ModoEdicion { get; set; } = false;

        public TratamientoForm()
        {
            _tratamientoService = new TratamientoService();
            _especialidadService = new EspecialidadService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int idTratamiento = ExtraerIdTratamiento();
            if (idTratamiento != 0)
            {
                ModoEdicion = true;
                chkEstado.Enabled = true;
            }

            if (!IsPostBack)
            {
                CargarEspecialidades();

                if (ModoEdicion)
                {
                    CargarDatosTratamiento(idTratamiento);
                }
                else
                {
                    litIdTratamiento.Text = "Nuevo";
                    chkEstado.Checked = true;
                    chkEstado.Enabled = false;
                }
            }
        }

        private void CargarDatosTratamiento(int id)
        {
            try
            {
                TratamientoDto dto = _tratamientoService.ObtenerTratamientoPorId(id);
                if (dto == null)
                {
                    Response.Redirect("~/Pages/Tratamientos/Index.aspx", false);
                    return;
                }

                litIdTratamiento.Text = dto.IdTratamiento.ToString();
                txtNombre.Text = dto.Nombre;
                txtDescripcion.Text = dto.Descripcion;
                txtCostoBase.Text = dto.CostoBase.ToString(CultureInfo.InvariantCulture);

                chkEstado.Checked = (dto.Estado.ToLower() == "activo");

                if (ddlEspecialidad.Items.FindByValue(dto.IdEspecialidad.ToString()) != null)
                {
                    ddlEspecialidad.SelectedValue = dto.IdEspecialidad.ToString();
                }
            }
            catch (Exception)
            {
                Response.Redirect("~/Pages/Tratamientos/Index.aspx", false);
            }
        }

        private void CargarEspecialidades()
        {
            try
            {
                var especialidades = _especialidadService.Listar(null);
                ddlEspecialidad.DataSource = especialidades;
                ddlEspecialidad.DataTextField = "Nombre";
                ddlEspecialidad.DataValueField = "IdEspecialidad";
                ddlEspecialidad.DataBind();
                ddlEspecialidad.Items.Insert(0, new ListItem("-- Seleccione Especialidad --", "0"));
            }
            catch (Exception)
            {
                ddlEspecialidad.Items.Clear();
                ddlEspecialidad.Items.Add(new ListItem("[Error al cargar especialidades]", "0"));
                ddlEspecialidad.Enabled = false;
            }
        }

        private decimal ValidarCampos()
        {
            if (!ValidadorCampos.EsTextoValido(txtNombre.Text, 3, 100))
                throw new ExcepcionReglaNegocio("El nombre del tratamiento es obligatorio y debe tener entre 3 y 100 caracteres.");

            if (!ValidadorCampos.EsTextoValido(txtDescripcion.Text, 10, 200))
                throw new ExcepcionReglaNegocio("La descripción debe tener al menos 10 caracteres si se completa.");

            if (!ValidadorCampos.EsDecimalValido(txtCostoBase.Text, out decimal costo))
                throw new ExcepcionReglaNegocio("El costo base es obligatorio y debe ser un número válido.");

            if (costo <= 0)
                throw new ExcepcionReglaNegocio("El costo base debe ser un número mayor a cero.");

            if (ddlEspecialidad.SelectedValue == "0" || string.IsNullOrEmpty(ddlEspecialidad.SelectedValue))
                throw new ExcepcionReglaNegocio("Debe seleccionar una especialidad.");

            return costo;
        }

        private void CrearTratamiento()
        {
            try
            {
                var costoValidado = ValidarCampos();
                var dto = new TratamientoDto
                {
                    IdTratamiento = 0,
                    Nombre = ValidadorCampos.CapitalizarTexto(txtNombre.Text),
                    Descripcion = (txtDescripcion.Text ?? "").Trim(),
                    CostoBase = costoValidado,
                    IdEspecialidad = int.Parse(ddlEspecialidad.SelectedValue),
                    Estado = chkEstado.Checked ? "Activo" : "Inactivo"
                };
                _tratamientoService.GuardarNuevoTratamiento(dto);
                Session["TratamientoMensajeTitulo"] = "Tratamiento creado";
                Session["TratamientoMensajeDesc"] = $"El tratamiento \"{dto.Nombre}\" se creó correctamente.";
                ModalHelper.MostrarModalDesdeSession(
                    this.Page,
                    "TratamientoMensajeTitulo",
                    "TratamientoMensajeDesc",
                    VirtualPathUtility.ToAbsolute("~/Pages/Tratamientos/Index"),
                    "abrirModalResultado"
                );
            }
            catch (ExcepcionReglaNegocio ex)
            {
                Session["TratamientoMensajeTitulo"] = "Operación no permitida";
                Session["TratamientoMensajeDesc"] = ex.Message;
                ModalHelper.MostrarModalDesdeSession(this.Page, "TratamientoMensajeTitulo", "TratamientoMensajeDesc", null, "abrirModalResultado");
            }
            catch (Exception ex)
            {
                Session["TratamientoMensajeTitulo"] = "Error inesperado";
                Session["TratamientoMensajeDesc"] = "Ocurrió un error al crear el tratamiento. " + ex.Message;
                ModalHelper.MostrarModalDesdeSession(this.Page, "TratamientoMensajeTitulo", "TratamientoMensajeDesc", null, "abrirModalResultado");
            }
        }

        public void ModificarTratamiento()
        {
            try
            {
                var costoValidado = ValidarCampos();
                int idTratamiento = ExtraerIdTratamiento();
                var dto = new TratamientoDto
                {
                    IdTratamiento = idTratamiento,
                    Nombre = ValidadorCampos.CapitalizarTexto(txtNombre.Text),
                    Descripcion = (txtDescripcion.Text ?? "").Trim(),
                    CostoBase = costoValidado,
                    IdEspecialidad = int.Parse(ddlEspecialidad.SelectedValue),
                    Estado = chkEstado.Checked ? "Activo" : "Inactivo"
                };
                _tratamientoService.ModificarTratamiento(dto);
                Session["TratamientoMensajeTitulo"] = "Tratamiento modificado";
                Session["TratamientoMensajeDesc"] = $"El tratamiento \"{dto.Nombre}\" se actualizó correctamente.";
                Session["ModalTipo"] = "Resultado";
                Response.Redirect("~/Pages/Tratamientos/Index.aspx", false);
            }
            catch (ExcepcionReglaNegocio ex)
            {
                Session["TratamientoMensajeTitulo"] = "Operación no permitida";
                Session["TratamientoMensajeDesc"] = ex.Message;
                ModalHelper.MostrarModalDesdeSession(this.Page, "TratamientoMensajeTitulo", "TratamientoMensajeDesc", null, "abrirModalResultado");
            }
            catch (Exception ex)
            {
                Session["TratamientoMensajeTitulo"] = "Error inesperado";
                Session["TratamientoMensajeDesc"] = "Ocurrió un error al modificar el tratamiento. " + ex.Message;
                ModalHelper.MostrarModalDesdeSession(this.Page, "TratamientoMensajeTitulo", "TratamientoMensajeDesc", null, "abrirModalResultado");
            }
        }

        public int ExtraerIdTratamiento()
        {
            string idString = Request.QueryString["id-tratamiento"] ?? string.Empty;
            if (!string.IsNullOrEmpty(idString) && int.TryParse(idString, out int id))
            {
                return id;
            }
            return 0;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ModoEdicion)
            {
                ModificarTratamiento();
            }
            else
            {
                CrearTratamiento();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Tratamientos/Index.aspx", false);
        }
    }
}