using SGTO.Negocio.DTOs;
using SGTO.Negocio.Servicios;
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
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private readonly EspecialidadService _especialidadService;

        public EspecialidadForm()
        {
            _especialidadService = new EspecialidadService();
        }



        protected void btnCancelar_Click(object sender, EventArgs e)

        {
            Response.Redirect("~/Pages/Especialidades/Index.aspx", false);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            var nuevoDto = new EspecialidadDto
            {
                IdEspecialidad = 0,
                Nombre = txtNombre.Text,
                Descripcion = txtDescripcion.Text,
                Estado = ddlEstado.SelectedValue,
            };
            try
            {
                _especialidadService.GuardarNuevaEspecialidad(nuevoDto);

                Response.Redirect("~/Pages/Especialidades/Index.aspx", false);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    
    }
}