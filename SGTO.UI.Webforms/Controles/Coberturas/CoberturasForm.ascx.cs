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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Coberturas
{
    public partial class CoberturasForm : System.Web.UI.UserControl
    {
        public bool ModoEdicion { get; set; } = false;
        private readonly CoberturaService _servicioCobertura = new CoberturaService();
        private readonly string _keySessionListaPlanes = "PlanesTemporales"; // key para acceder a la lista de planes en session
        private readonly string _keyViewStateEditarPlan = "PlanEditandoIndex"; // key para acceder al View State

        protected void Page_Load(object sender, EventArgs e)
        {
            int idCobertura = ExtraerIdCobertura();
            if (idCobertura != 0)
            {
                ModoEdicion = true;
                chkActivo.Enabled = true;
            }

            if (!IsPostBack)
            {
                if (idCobertura != 0)
                {
                    CargarDetalleCobertura(idCobertura);
                }
                else
                {
                    List<Plan> planes = new List<Plan>();
                    gvPlanes.DataSource = planes;
                    gvPlanes.DataBind();
                }
                panelPlanes.Visible = !ModoEdicion;

                ModalHelper.MostrarModalDesdeSession(this.Page, "CoberturaMensajeTitulo", "CoberturaMensajeDesc", "/Pages/CoberturasPlanes/Index");
            }
        }

        public void gvPlanes_RowDataBound(object sender, GridViewRowEventArgs e) { }
        public void gvPlanes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int indexDtoPlan = Convert.ToInt32(e.CommandArgument);
            List<PlanDto> planes = Session[_keySessionListaPlanes] as List<PlanDto> ?? new List<PlanDto>();

            if (e.CommandName == "Editar")
            {
                if (indexDtoPlan >= 0 && indexDtoPlan < planes.Count)
                {
                    PlanDto planEdicion = planes[indexDtoPlan];

                    // guardar el id del plan que se edita 
                    ViewState[_keyViewStateEditarPlan] = indexDtoPlan;

                    txtNombrePlan.Text = planEdicion.Nombre;
                    txtDescripcionPlan.Text = planEdicion.Descripcion;
                    txtPorcentajeCobertura.Text = planEdicion.PorcentajeCobertura.ToString();

                    btnAgregarPlan.Text = "Guardar cambios";

                    MensajeUiHelper.SetearYMostrar(
                       this.Page,
                       "Cobertura modificada",
                       "La cobertura fue modificada correctamente.",
                       "Resultado",
                       null,
                       "abrirModalNuevoPlan"
                   );

                }
            }
            else if (e.CommandName == "Eliminar")
            {
                if (indexDtoPlan >= 0 && indexDtoPlan < planes.Count)
                {
                    planes.RemoveAt(indexDtoPlan);
                    Session[_keySessionListaPlanes] = planes;

                    gvPlanes.DataSource = planes;
                    gvPlanes.DataBind();
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/CoberturasPlanes/Index", false);
        }


        public void CargarDetalleCobertura(int idCobertura)
        {
            try
            {
                CoberturaDto coberturaDto = _servicioCobertura.ObtenerCoberturaPorId(idCobertura);
                txtNombreCobertura.Text = coberturaDto.Nombre;
                txtDescripcionCobertura.Text = coberturaDto.Descripcion;
                if (coberturaDto.PorcentajeCobertura is null)
                {
                    txtPorcentaje.Enabled = false;
                    txtPorcentaje.Text = string.Empty; // sin valor
                }
                else
                {
                    txtPorcentaje.Enabled = true;
                    txtPorcentaje.Text = coberturaDto.PorcentajeCobertura.Value.ToString("N0"); // muestra "0", "40", "55", etc.
                }
                chkActivo.Checked = coberturaDto.Estado.ToLower() == "activo";
            }
            catch (Exception)
            {
                MensajeUiHelper.SetearYMostrar(this.Page,
                    "Error inesperado",
                    "Ocurrió un error al cargar los datos de la cobertura seleccionada.",
                    "Resultado",
                    null,
                    "abrirModalResultado");
            }
        }

        public int ExtraerIdCobertura()
        {
            string idCoberturaString = Request.QueryString["id-cobertura"] ?? string.Empty;
            if (!string.IsNullOrEmpty(idCoberturaString) && int.TryParse(idCoberturaString, out int idCobertura))
            {
                return idCobertura;
            }
            return 0;
        }

        public void ModificarCobertura()
        {
            int idCobertura = ExtraerIdCobertura();
            string nombre = txtNombreCobertura.Text;
            string descripcion = txtDescripcionCobertura.Text;
            string estado = chkActivo.Checked ? "activo" : "inactivo";
            decimal? porcentajeCobertura = null;
            if (decimal.TryParse(txtPorcentaje.Text, out decimal valor))
            {
                porcentajeCobertura = valor;
            }
            try
            {
                CoberturaDto coberturaDto = CoberturaMapper.MapearADto(idCobertura, nombre, descripcion, estado, porcentajeCobertura);
                TurnoService servicioTurno = new TurnoService();
                _servicioCobertura.ModificarCobertura(coberturaDto, servicioTurno);

                MensajeUiHelper.SetearYMostrar(
                this.Page,
               "Cobertura modificada",
                   "La cobertura fue modificada correctamente.",
                "Resultado",
                VirtualPathUtility.ToAbsolute("~/Pages/CoberturasPlanes/Index"),
                "abrirModalResultado"
            );

            }
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page,
                   "Operación no permitida",
                   ex.Message,
                   "Resultado",
                   null,
                   "abrirModalResultado");
            }
            catch (Exception)
            {
                MensajeUiHelper.SetearYMostrar(this.Page,
                     "Error inesperado",
                     "Ocurrió un error al intentar dar de baja la cobertura.",
                     "Resultado",
                     null,
                     "abrirModalResultado");
            }
        }

        private void CrearCobertura()
        {
            try
            {
                string nombre = txtNombreCobertura.Text;
                string descripcion = txtDescripcionCobertura.Text;
                string estado = chkActivo.Checked ? "activo" : "inactivo";

                decimal? porcentajeCobertura = null;
                if (decimal.TryParse(txtPorcentaje.Text, out decimal valor))
                {
                    porcentajeCobertura = valor;
                }

                CoberturaDto coberturaDto = CoberturaMapper.MapearADto(0, nombre, descripcion, estado, porcentajeCobertura);
                var planes = Session[_keySessionListaPlanes] as List<PlanDto> ?? new List<PlanDto>();
                ValidarCamposCobertura();

                _servicioCobertura.CrearCobertura(coberturaDto, planes);

                MensajeUiHelper.SetearYMostrar(
                   this.Page,
                   "Cobertura creada",
                   "La cobertura se ha creado correctamente.",
                   "Resultado",
                   VirtualPathUtility.ToAbsolute("~/Pages/CoberturasPlanes/Index"),
                   "abrirModalResultado"
               );
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page,
                   "Operación no permitida",
                   ex.Message,
                   "Resultado",
                   null,
                   "abrirModalResultado");
                return;
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page,
                    "Error",
                    "Ocurrió un error al crear la cobertura: " + ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado");
                return;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ModoEdicion)
            {
                ModificarCobertura();
            }
            else
            {
                CrearCobertura();
            }
        }

        protected void btnAgregarPlan_Click(object sender, EventArgs e)
        {
            // limpiar el error
            lblErrorPlan.InnerText = string.Empty;
            lblErrorPlan.Visible = false;

            var planes = Session[_keySessionListaPlanes] as List<PlanDto> ?? new List<PlanDto>();

            if (!decimal.TryParse(txtPorcentajeCobertura.Text, out decimal porcentaje))
            {
                MensajeUiHelper.SetearYMostrar(this.Page,
                   "Operación no permitida",
                   "El porcentaje de cobertura debe ser un número entre 0 y 100.",
                   "Resultado",
                   null,
                   "abrirModalNuevoPlan");

                lblErrorPlan.InnerText = "El porcentaje de cobertura debe ser un número entre 0 y 100.";
                lblErrorPlan.Visible = true;

                return;
            }

            PlanDto nuevoPlanDto = new PlanDto
            {
                Nombre = txtNombrePlan.Text,
                Descripcion = txtDescripcionPlan.Text,
                PorcentajeCobertura = porcentaje,
                Estado = chkActivo.Checked ? "activo" : "inactivo"
            };

            try
            {
                ValidarCamposPlan(nuevoPlanDto);

                // validar si se está editando un plan o agregando
                if (ViewState[_keyViewStateEditarPlan] != null)
                {
                    int edicionIndex = (int)ViewState[_keyViewStateEditarPlan];
                    if (edicionIndex >= 0 && edicionIndex < planes.Count)
                    {
                        planes[edicionIndex].Nombre = nuevoPlanDto.Nombre;
                        planes[edicionIndex].Descripcion = nuevoPlanDto.Descripcion;
                        planes[edicionIndex].PorcentajeCobertura = nuevoPlanDto.PorcentajeCobertura;
                        planes[edicionIndex].Estado = nuevoPlanDto.Estado;
                    }

                    // limpiar modo edición
                    ViewState[_keyViewStateEditarPlan] = null;
                    btnAgregarPlan.Text = "Agregar";
                }
                else
                {
                    planes.Add(nuevoPlanDto);
                }

                Session[_keySessionListaPlanes] = planes;

                // actualizar gv de planes
                gvPlanes.DataSource = planes;
                gvPlanes.DataBind();

                txtNombrePlan.Text = string.Empty;
                txtDescripcionPlan.Text = string.Empty;
                txtPorcentajeCobertura.Text = string.Empty;
            }
            catch (ExcepcionReglaNegocio ex)
            {
                // mantener estos mensajes en session porque son lso que hacen que se abra el modal
                MensajeUiHelper.SetearYMostrar(this.Page,
                    "Operación no permitida",
                    ex.Message,
                    "Resultado",
                    null,
                    "abrirModalNuevoPlan");
                // setear el label de error
                lblErrorPlan.InnerText = ex.Message;
                lblErrorPlan.Visible = true;
            }
        }



        private bool ValidarCamposCobertura()
        {
            string nombre = txtNombreCobertura.Text;
            string descripcion = txtDescripcionCobertura.Text;

            if (!ValidadorCampos.EsTextoValido(nombre, 3, 50))
                throw new ExcepcionReglaNegocio("El nombre debe tener entre 3 y 50 caracteres y no puede estar vacío.");

            if (!string.IsNullOrWhiteSpace(descripcion) && !ValidadorCampos.TieneLongitudMinima(descripcion, 10))
                throw new ExcepcionReglaNegocio("La descripción debe tener al menos 10 caracteres si se completa.");

            return true;
        }


        private bool ValidarCamposPlan(PlanDto planDto)
        {
            if (!ValidadorCampos.EsTextoValido(planDto.Nombre, 3, 50))
                throw new ExcepcionReglaNegocio("El nombre del plan debe tener entre 3 y 50 caracteres y no puede estar vacío.");

            if (!string.IsNullOrWhiteSpace(planDto.Descripcion) && !ValidadorCampos.TieneLongitudMinima(planDto.Descripcion, 10))
                throw new ExcepcionReglaNegocio("La descripción del plan debe tener al menos 10 caracteres si se completa.");

            if (!ValidadorCampos.EsPorcentajeCoberturaValido(planDto.PorcentajeCobertura))
                throw new ExcepcionReglaNegocio("El porcentaje de cobertura del plan no es válido (debe estar entre 0 y 100).");

            return true;
        }
    }
}