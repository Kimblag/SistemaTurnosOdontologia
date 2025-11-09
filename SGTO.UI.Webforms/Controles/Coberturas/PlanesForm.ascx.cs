using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Mappers;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Coberturas
{
    public partial class PlanesForm : System.Web.UI.UserControl
    {
        public bool ModoEdicion { get; set; } = false;
        private readonly PlanService _servicioPlan = new PlanService();
        private readonly CoberturaService _servicioCobertura = new CoberturaService();

        protected void Page_Load(object sender, EventArgs e)
        {
            int idPlan = ExtraerIdPlan();
            if (idPlan != 0)
            {
                ModoEdicion = true;
                chkActivo.Enabled = true;
            }

            if (!IsPostBack)
            {
                if (idPlan != 0)
                {
                    CargarDetallePlan(idPlan);
                }

                CargarCoberturasDropdown();
                ModalHelper.MostrarModalDesdeSession(this.Page, "CoberturaMensajeTitulo", "CoberturaMensajeDesc", "/Pages/CoberturasPlanes/Index");
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/CoberturasPlanes/Index", false);
        }

        private void CargarDetallePlan(int idPlan)
        {
            try
            {
                PlanDto planDto = _servicioPlan.ObtenerPorId(idPlan);
                ddlCobertura.SelectedValue = planDto.IdCobertura.ToString();
                txtNombrePlan.Text = planDto.Nombre;
                txtDescripcionPlan.Text = planDto.Descripcion;
                txtPorcentajeCobertura.Text = planDto.PorcentajeCobertura.ToString();
                chkActivo.Checked = planDto.Estado.ToLower() == "activo";

                CoberturaService coberturaService = new CoberturaService();
                if (coberturaService.EstaInactiva(planDto.IdCobertura))
                {
                    DeshabilitarFormularioPorCoberturaInactiva();
                }
            }
            catch (Exception)
            {
                MensajeUiHelper.SetearYMostrar(this.Page,
                    "Error inesperado",
                    "Ocurrió un error al cargar los datos del plan seleccionado.",
                    "Resultado",
                    null,
                    "abrirModalResultado");
            }
        }


        private void CargarCoberturasDropdown()
        {
            try
            {
                List<CoberturaDto> coberturas;

                if (ModoEdicion)
                {
                    // si estoy editando, cargo todas las coberturas (activas e inactivas)
                    coberturas = _servicioCobertura.Listar();
                }
                else
                {
                    coberturas = _servicioCobertura.Listar("activo");
                }

                if (coberturas == null || coberturas.Count == 0)
                {
                    ddlCobertura.Items.Clear();
                    ddlCobertura.Items.Add(new ListItem("No hay coberturas disponibles", ""));
                    ddlCobertura.Enabled = false;
                    return;
                }

                ddlCobertura.DataSource = coberturas;
                ddlCobertura.DataTextField = "Nombre";
                ddlCobertura.DataValueField = "IdCobertura";
                ddlCobertura.DataBind();

                if (ModoEdicion)
                    ddlCobertura.Enabled = !ModoEdicion;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                ddlCobertura.Items.Clear();
                ddlCobertura.Items.Add(new ListItem("Error al cargar", ""));
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ModoEdicion)
            {
                ModificarPlan();
            }
            else
            {
                CrearPlan();
            }
        }


        public void CrearPlan()
        {
            try
            {
                int.TryParse(ddlCobertura.SelectedValue, out int idCobertura);
                string nombre = txtNombrePlan.Text.Trim();
                string descripcion = txtDescripcionPlan.Text.Trim();
                string estado = chkActivo.Checked ? "activo" : "inactivo";

                if (!decimal.TryParse(txtPorcentajeCobertura.Text, out decimal porcentajeCobertura))
                    throw new ArgumentException("El porcentaje de cobertura debe ser un número válido.");

                ValidarCamposPlan(nombre, descripcion, porcentajeCobertura);

                PlanDto nuevoPlanDto = PlanMapper.MapearADto(0, nombre, descripcion, porcentajeCobertura, estado, idCobertura);

                CoberturaService servicioCobertura = new CoberturaService();
                _servicioPlan.Crear(nuevoPlanDto, servicioCobertura);

                MensajeUiHelper.SetearYMostrar(
                     this.Page,
                     "Plan creado",
                     "El plan se ha creado correctamente.",
                     "Resultado",
                     VirtualPathUtility.ToAbsolute("~/Pages/CoberturasPlanes/Index"),
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
                    "Ocurrió un error al intentar crear el plan. Detalle: " + ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado"
                );
            }
        }


        private void ModificarPlan()
        {
            try
            {
                int idPlan = ExtraerIdPlan();
                int.TryParse(ddlCobertura.SelectedValue, out int idCobertura);
                string nombre = txtNombrePlan.Text.Trim();
                string descripcion = txtDescripcionPlan.Text.Trim();
                string estado = chkActivo.Checked ? "activo" : "inactivo";
                if (!decimal.TryParse(txtPorcentajeCobertura.Text, out decimal porcentajeCobertura))
                    throw new ArgumentException("El porcentaje de cobertura debe ser un número válido.");

                ValidarCamposPlan(nombre, descripcion, porcentajeCobertura);

                PlanDto planDto = PlanMapper.MapearADto(idPlan, nombre, descripcion, porcentajeCobertura, estado, idCobertura);
                TurnoService servicioTurno = new TurnoService();
                CoberturaService servicioCobertura = new CoberturaService();
                _servicioPlan.Modificar(planDto, servicioTurno, servicioCobertura);

                MensajeUiHelper.SetearYMostrar(this.Page,
                  "Plan modificado",
                  "El plan fue modificado correctamente.",
                  "Resultado",
                   VirtualPathUtility.ToAbsolute("~/Pages/CoberturasPlanes/Index"),
                   "abrirModalResultado");
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
                    "Ocurrió un error al intentar modificar el plan. " + ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado"
                );
            }
        }


        private void ValidarCamposPlan(string nombre, string descripcion, decimal porcentajeCobertura)
        {
            if (!ValidadorCampos.EsTextoValido(nombre, 3, 50))
                throw new ArgumentException("El nombre del plan debe tener entre 3 y 50 caracteres y no puede estar vacío.");

            if (!string.IsNullOrWhiteSpace(descripcion) && !ValidadorCampos.TieneLongitudMinima(descripcion, 10))
                throw new ArgumentException("La descripción del plan debe tener al menos 10 caracteres si se completa.");

            if (!ValidadorCampos.EsPorcentajeCoberturaValido(porcentajeCobertura))
                throw new ArgumentException("El porcentaje de cobertura del plan debe estar entre 0 y 100.");
        }

        private int ExtraerIdPlan()
        {
            string idPlanString = Request.QueryString["id-plan"] ?? string.Empty;
            if (!string.IsNullOrEmpty(idPlanString) && int.TryParse(idPlanString, out int idPlan))
                return idPlan;
            return 0;
        }

        private void DeshabilitarFormularioPorCoberturaInactiva()
        {
            txtNombrePlan.Enabled = false;
            txtDescripcionPlan.Enabled = false;
            txtPorcentajeCobertura.Enabled = false;
            chkActivo.Enabled = false;
            ddlCobertura.Enabled = false;
            btnGuardar.Enabled = false;

            lblEstadoInfo.Visible = true;
            lblEstadoInfo.Text = "La cobertura asociada a este plan está inactiva. No es posible modificar sus datos.";
        }


    }
}