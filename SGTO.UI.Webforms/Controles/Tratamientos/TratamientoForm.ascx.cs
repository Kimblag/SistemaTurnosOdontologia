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

        public TratamientoForm()
        {
            _tratamientoService = new TratamientoService();
            _especialidadService = new EspecialidadService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                litIdTratamiento.Text = "Nuevo";
                ddlEstado.SelectedValue = "1";
                ddlEstado.Enabled = false;

                CargarEspecialidades();
            }
        }

        private void CargarEspecialidades()
        {
            try
            {
                var especialidades = _especialidadService.Listar("activo");

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

        private void ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                throw new ExcepcionReglaNegocio("El nombre del tratamiento es obligatorio.");

            if (string.IsNullOrWhiteSpace(txtCostoBase.Text))
                throw new ExcepcionReglaNegocio("El costo base es obligatorio.");

            var costoTxt = (txtCostoBase.Text ?? "").Trim().Replace(',', '.');
            if (!decimal.TryParse(costoTxt, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                throw new ExcepcionReglaNegocio("El costo base debe ser un número válido.");

            if (ddlEspecialidad.SelectedValue == "0" || string.IsNullOrEmpty(ddlEspecialidad.SelectedValue))
                throw new ExcepcionReglaNegocio("Debe seleccionar una especialidad.");
        }

        private void CrearTratamiento()
        {
            try
            {
                ValidarCampos();

                var costoTxt = (txtCostoBase.Text ?? "").Trim().Replace(',', '.');
                var costo = decimal.Parse(costoTxt, CultureInfo.InvariantCulture);

                var dto = new TratamientoDto
                {
                    IdTratamiento = 0,
                    Nombre = (txtNombre.Text ?? "").Trim(),
                    Descripcion = (txtDescripcion.Text ?? "").Trim(),
                    CostoBase = costo,
                    IdEspecialidad = int.Parse(ddlEspecialidad.SelectedValue),
                    Estado = ddlEstado.SelectedValue == "1" ? "Activo" : "Inactivo"
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

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            CrearTratamiento();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Tratamientos/Index.aspx", false);
        }
    }
}