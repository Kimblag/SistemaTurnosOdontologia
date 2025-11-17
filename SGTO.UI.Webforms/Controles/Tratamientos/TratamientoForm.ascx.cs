using SGTO.Comun.Validacion;
using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Tratamientos
{
    public partial class TratamientoForm : UserControl
    {
        private readonly TratamientoService _servicioTratamiento = new TratamientoService();
        private readonly EspecialidadService _servicioEspecialidad = new EspecialidadService();

        public bool ModoEdicion { get; set; } = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = ExtraerIdTratamiento();

            if (id != 0)
            {
                ModoEdicion = true;
                chkEstado.Enabled = true;
            }

            if (!IsPostBack)
            {
                CargarEspecialidades();

                if (ModoEdicion)
                {
                    CargarDatosTratamiento(id);
                }
                else
                {
                    litIdTratamiento.Text = "Nuevo";
                    chkEstado.Checked = true;
                    chkEstado.Enabled = false;
                }

                ModalHelper.MostrarModalDesdeSession(
                    this.Page,
                    "TratamientoMensajeTitulo",
                    "TratamientoMensajeDesc",
                    "/Pages/Tratamientos/Index",
                    "abrirModalResultado"
                );
            }
        }

        private void CargarDatosTratamiento(int id)
        {
            try
            {
                TratamientoDto dto = _servicioTratamiento.ObtenerTratamientoPorId(id);
                if (dto == null)
                {
                    Response.Redirect("~/Pages/Tratamientos/Index.aspx", false);
                    return;
                }

                litIdTratamiento.Text = dto.IdTratamiento.ToString();
                txtNombre.Text = dto.Nombre;
                txtDescripcion.Text = dto.Descripcion;
                txtCostoBase.Text = dto.CostoBase.ToString(CultureInfo.InvariantCulture);

                chkEstado.Checked = dto.Estado.Equals("activo", StringComparison.OrdinalIgnoreCase);

                if (ddlEspecialidad.Items.FindByValue(dto.IdEspecialidad.ToString()) != null)
                    ddlEspecialidad.SelectedValue = dto.IdEspecialidad.ToString();
            }
            catch
            {
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Error inesperado",
                    "Ocurrió un error al cargar el tratamiento.",
                    "Resultado",
                    null,
                    "abrirModalResultado"
                );
            }
        }

        private void CargarEspecialidades()
        {
            try
            {
                List<EspecialidadDto> especialidades;

                if (ModoEdicion)
                {
                    // Igual que en coberturas/planes: en edición se permiten inactivas
                    especialidades = _servicioEspecialidad.Listar(null); // todas
                }
                else
                {
                    // En creación solo mostrar activas
                    especialidades = _servicioEspecialidad.Listar("activas");
                }

                if (especialidades == null || especialidades.Count == 0)
                {
                    ddlEspecialidad.Items.Clear();
                    ddlEspecialidad.Items.Add(new ListItem("No hay especialidades disponibles", "0"));
                    ddlEspecialidad.Enabled = false;
                    return;
                }

                ddlEspecialidad.DataSource = especialidades;
                ddlEspecialidad.DataTextField = "Nombre";
                ddlEspecialidad.DataValueField = "IdEspecialidad";
                ddlEspecialidad.DataBind();

                ddlEspecialidad.Items.Insert(0, new ListItem("Seleccione una especialidad", "0"));

                if (ModoEdicion)
                {
                    var id = ExtraerIdTratamiento();
                    var dto = _servicioTratamiento.ObtenerTratamientoPorId(id);

                    if (_servicioEspecialidad.EstaInactiva(dto.IdEspecialidad))
                    {
                        DeshabilitarFormularioPorEspecialidadInactiva();
                    }
                }
            }
            catch
            {
                ddlEspecialidad.Items.Clear();
                ddlEspecialidad.Items.Add(new ListItem("[Error al cargar especialidades]", "0"));
                ddlEspecialidad.Enabled = false;
            }
        }


        private decimal ValidarCampos()
        {
            if (!ValidadorCampos.EsTextoValido(txtNombre.Text, 3, 100))
                throw new ArgumentException("El nombre del tratamiento es obligatorio y debe tener entre 3 y 100 caracteres.");

            if (!string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                if (!ValidadorCampos.TieneLongitudMinima(txtDescripcion.Text, 10))
                    throw new ArgumentException("La descripción debe tener al menos 10 caracteres si se completa.");
            }

            if (!ValidadorCampos.EsDecimalValido(txtCostoBase.Text, out decimal costo))
                throw new ArgumentException("El costo base es obligatorio y debe ser un número válido.");

            if (costo <= 0)
                throw new ArgumentException("El costo base debe ser un número mayor a cero.");

            if (ddlEspecialidad.SelectedValue == "0" || string.IsNullOrEmpty(ddlEspecialidad.SelectedValue))
                throw new ArgumentException("Debe seleccionar una especialidad.");

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
                _servicioTratamiento.GuardarNuevoTratamiento(dto);
                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Tratamiento creado",
                    $"El tratamiento \"{dto.Nombre}\" se creó correctamente.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Tratamientos/Index"),
                    "abrirModalResultado"
                );
            }
            catch (ArgumentException ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page,
                   "Dato inválido",
                   ex.Message,
                   "Resultado",
                   null,
                   "abrirModalResultado");
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Operación no permitida", ex.Message, "Resultado", null, "abrirModalResultado");
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error inesperado", "Ocurrió un error. " + ex.Message, "Resultado", null, "abrirModalResultado");
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
                _servicioTratamiento.ModificarTratamiento(dto);
                MensajeUiHelper.SetearYMostrar(
                     this.Page,
                     "Tratamiento modificado",
                     $"El tratamiento \"{dto.Nombre}\" se actualizó correctamente.",
                     "Resultado",
                     VirtualPathUtility.ToAbsolute("~/Pages/Tratamientos/Index"),
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
                MensajeUiHelper.SetearYMostrar(this.Page, "Operación no permitida", ex.Message, "Resultado", null, "abrirModalResultado");
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page, "Error inesperado", "Ocurrió un error. " + ex.Message, "Resultado", null, "abrirModalResultado");
            }
        }

        private int ExtraerIdTratamiento()
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

        private void DeshabilitarFormularioPorEspecialidadInactiva()
        {
            txtNombre.Enabled = false;
            txtDescripcion.Enabled = false;
            txtCostoBase.Enabled = false;
            ddlEspecialidad.Enabled = false;
            chkEstado.Enabled = false;
            btnGuardar.Enabled = false;
        }


    }
}