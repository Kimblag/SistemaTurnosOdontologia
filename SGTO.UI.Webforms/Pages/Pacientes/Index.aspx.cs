using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Pacientes;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Pacientes
{
    public partial class Pacientes : System.Web.UI.Page
    {
        private readonly PacienteService _servicioPaciente = new PacienteService();
        private const string KEY_PACIENTE_BUSQUEDA = "FiltroPacienteBusqueda";
        private const string KEY_PACIENTE_CAMPO = "FiltroPacienteCampo";
        private const string KEY_PACIENTE_CRITERIO = "FiltroPacienteCriterio";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Pacientes");
                master.EstablecerTituloSeccion(this.Page.Title);
            }
            if (!IsPostBack)
            {
                // verificar si tenemos filtros
                txtBuscar.Text = Session[KEY_PACIENTE_BUSQUEDA] as string ?? string.Empty;
                string campo = Session[KEY_PACIENTE_CAMPO] as string;
                if (!string.IsNullOrEmpty(campo))
                {
                    ddlCampo.SelectedValue = campo;
                    CargarCriterios(campo);
                }

                string criterio = Session[KEY_PACIENTE_CRITERIO] as string;
                if (!string.IsNullOrEmpty(criterio) && ddlCriterio.Items.FindByValue(criterio) != null)
                {
                    ddlCriterio.SelectedValue = criterio;
                    ddlCriterio.Enabled = true;
                }
                AplicarFiltros();
            }
        }


        private void CargarPacientes(string estado = null)
        {
            List<PacienteListadoDto> listado = new List<PacienteListadoDto>();
            try
            {
                listado = _servicioPaciente.ListarPacientes(estado);
                gvPacientes.DataSource = listado;
                gvPacientes.DataBind();


            }
            catch (Exception ex)
            {
                gvPacientes.DataSource = listado;
                gvPacientes.DataBind();
                MensajeUiHelper.SetearYMostrar(
                   this.Page,
                   "Error al cargar pacientes",
                   "Ocurrió un error inesperado al intentar obtener la lista de pacientes." + ex.Message
               );
            }
        }

        private void CargarCoberturasDropdown()
        {
            try
            {
                var servicioCobertura = new CoberturaService();
                var coberturas = servicioCobertura.Listar("activo");

                foreach (var c in coberturas)
                {
                    ddlCriterio.Items.Add(new ListItem(c.Nombre, c.IdCobertura.ToString()));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CargarCriterios(string campo)
        {
            ddlCriterio.Items.Clear();
            ddlCriterio.Enabled = false;

            if (string.IsNullOrEmpty(campo))
            {
                ddlCriterio.Items.Add(new ListItem("Seleccione un criterio", ""));
                return;
            }

            campo = campo.ToLower();

            if (campo == "estado")
            {
                ddlCriterio.Enabled = true;
                ddlCriterio.Items.Add(new ListItem("Seleccione un criterio", ""));
                ddlCriterio.Items.Add(new ListItem("Activo", "A"));
                ddlCriterio.Items.Add(new ListItem("Inactivo", "I"));
            }
            else if (campo == "cobertura")
            {
                ddlCriterio.Enabled = true;
                ddlCriterio.Items.Add(new ListItem("Seleccione un criterio", ""));
                CargarCoberturasDropdown();
            }
            if (ddlCriterio.Items.Count > 0)
                ddlCriterio.SelectedIndex = 0;
        }


        private void AplicarFiltros()
        {
            string textoBusqueda = txtBuscar.Text.Trim();
            string campo = ddlCampo.SelectedValue;
            string criterio = ddlCriterio.SelectedValue;

            Session[KEY_PACIENTE_BUSQUEDA] = string.IsNullOrEmpty(textoBusqueda) ? null : textoBusqueda;
            Session[KEY_PACIENTE_CAMPO] = string.IsNullOrEmpty(campo) ? null : campo;
            Session[KEY_PACIENTE_CRITERIO] = string.IsNullOrEmpty(criterio) ? null : criterio;

            List<PacienteListadoDto> lista = _servicioPaciente.ListarPacientes();

            if (!string.IsNullOrEmpty(textoBusqueda))
            {
                string texto = textoBusqueda.ToLower();
                List<PacienteListadoDto> filtrada = new List<PacienteListadoDto>();

                foreach (var p in lista)
                {
                    bool coincide =
                        (!string.IsNullOrEmpty(p.NombreCompleto) && p.NombreCompleto.ToLower().Contains(texto)) ||
                        (!string.IsNullOrEmpty(p.Dni) && p.Dni.ToLower().Contains(texto)) ||
                        (!string.IsNullOrEmpty(p.Email) && p.Email.ToLower().Contains(texto));

                    if (coincide)
                        filtrada.Add(p);
                }

                lista = filtrada;
            }

            if (!string.IsNullOrEmpty(campo) && !string.IsNullOrEmpty(criterio))
            {
                List<PacienteListadoDto> filtrada = new List<PacienteListadoDto>();

                if (campo == "Estado")
                {
                    foreach (var p in lista)
                    {
                        if (!string.IsNullOrEmpty(p.Estado.ToString()) &&
                            p.Estado.ToString().StartsWith(criterio, StringComparison.OrdinalIgnoreCase))
                        {
                            filtrada.Add(p);
                        }
                    }
                }
                else if (campo == "Cobertura")
                {
                    if (int.TryParse(criterio, out int idCoberturaSeleccionada))
                    {
                        foreach (var p in lista)
                        {
                            if (p.IdCobertura.HasValue && p.IdCobertura.Value == idCoberturaSeleccionada)
                                filtrada.Add(p);
                        }
                    }
                }
                lista = filtrada;
            }

            gvPacientes.DataSource = lista;
            gvPacientes.DataBind();
        }


        protected void gvPacientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PacienteListadoDto pacienteDto = (PacienteListadoDto)e.Row.DataItem;

                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");

                if (lblEstado != null && pacienteDto != null)
                {
                    if (pacienteDto.Estado.ToString().ToLower() == "activo")
                    {
                        lblEstado.Attributes["class"] = "badge badge-success";
                    }
                    else
                    {
                        lblEstado.Attributes["class"] = "badge badge-warning";
                    }
                }
            }
        }

        protected void gvPacientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPacientes.PageIndex = e.NewPageIndex;
            AplicarFiltros();
        }

        protected void gvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idPaciente = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Pages/Pacientes/Editar?id-paciente={idPaciente}", false);
            }
            else if (e.CommandName == "Ver")
            {
                Response.Redirect($"~/Pages/Pacientes/Detalle?id-paciente={idPaciente}", false);
            }
        }

        protected void btnNuevoPaciente_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Pacientes/Nuevo", false);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Session[KEY_PACIENTE_BUSQUEDA] = null;
            Session[KEY_PACIENTE_CAMPO] = null;
            Session[KEY_PACIENTE_CRITERIO] = null;

            txtBuscar.Text = string.Empty;
            ddlCampo.SelectedIndex = 0;
            ddlCriterio.Items.Clear();
            ddlCriterio.Items.Add(new ListItem("Seleccione un criterio", ""));
            ddlCriterio.Enabled = false;

            CargarPacientes();
        }

        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string campo = ddlCampo.SelectedValue;

            Session[KEY_PACIENTE_CAMPO] = string.IsNullOrEmpty(campo) ? null : campo;

            CargarCriterios(campo);

            Session[KEY_PACIENTE_CRITERIO] = null;
        }


    }
}