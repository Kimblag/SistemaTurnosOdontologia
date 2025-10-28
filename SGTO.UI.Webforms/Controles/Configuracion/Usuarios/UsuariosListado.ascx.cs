using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Configuracion.Usuarios
{
    public partial class UsuariosListado : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarUsuarios();
            }
        }

        protected void btnNuevoUsuario_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/Configuracion/Usuarios/Nuevo", false);
        }

        public void gvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e) { }
        public void gvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e) { }
        public void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int idUsuario = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Pages/Configuracion/Usuarios/Editar?id-usuario={idUsuario}", false);
            }
       
        }


        private void CargarUsuarios()
        {
            //metodo de test
            var rolAdmin = new Rol(1, "Administrador", "Acceso total al sistema", new List<Permiso>(), EstadoEntidad.Activo);
            var rolMedico = new Rol(2, "Odontólogo", "Gestión de turnos y pacientes", new List<Permiso>(), EstadoEntidad.Activo);
            var rolRecep = new Rol(3, "Recepcionista", "Agenda y atención al paciente", new List<Permiso>(), EstadoEntidad.Activo);
            var usuarios = new List<Usuario>
            {
                new Usuario(1, "Laura", "Martínez", new Email("laura.martinez@clinica.com"), "lauram", "hash123", rolAdmin, EstadoEntidad.Activo, DateTime.Now.AddMonths(-2), DateTime.Now),
                new Usuario(2, "Carlos", "Pérez", new Email("carlos.perez@clinica.com"), "cperez", "hash456", rolMedico, EstadoEntidad.Activo, DateTime.Now.AddMonths(-1), DateTime.Now),
                new Usuario(3, "María", "Gómez", new Email("maria.gomez@clinica.com"), "mgomez", "hash789", rolRecep, EstadoEntidad.Inactivo, DateTime.Now.AddMonths(-3), DateTime.Now),
                new Usuario(4, "Ricardo", "Sosa", new Email("ricardo.sosa@clinica.com"), "rsosa", "hashabc", rolMedico, EstadoEntidad.Activo, DateTime.Now.AddMonths(-6), DateTime.Now),
                new Usuario(5, "Ana", "López", new Email("ana.lopez@clinica.com"), "alopez", "hashxyz", rolRecep, EstadoEntidad.Activo, DateTime.Now.AddMonths(-4), DateTime.Now)
            };

            gvUsuarios.DataSource = usuarios;
            gvUsuarios.DataBind();
        }
    }
}