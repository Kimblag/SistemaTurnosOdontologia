using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Configuracion.Roles
{
    public partial class RolesListado : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) CargarRoles();
        }

        protected void btnNuevoRol_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/Configuracion/Roles/Nuevo", false);
        }

        public void gvRoles_RowDataBound(object sender, GridViewRowEventArgs e) { }
        public void gvRoles_PageIndexChanging(object sender, GridViewPageEventArgs e) { }
        public void gvRoles_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int idRol = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Editar")
            {
                Response.Redirect($"~/Pages/Configuracion/Roles/Editar?id-rol={idRol}", false);
            }

        }

        private void CargarRoles()
        {
            // metodo test carga pruebas
            var permisosAdmin = new List<Permiso>
            {
                new Permiso(Modulo.Turnos, TipoAccion.Ver, "Puede ver los turnos"),
                new Permiso(Modulo.Turnos, TipoAccion.Crear, "Puede crear turnos"),
                new Permiso(Modulo.Turnos, TipoAccion.Editar, "Puede editar turnos"),
                new Permiso(Modulo.Pacientes, TipoAccion.Ver, "Puede ver pacientes"),
                new Permiso(Modulo.Pacientes, TipoAccion.Crear, "Puede agregar pacientes"),
                new Permiso(Modulo.Configuracion, TipoAccion.Editar, "Puede editar configuración del sistema"),
                new Permiso(Modulo.Roles, TipoAccion.Ver, "Puede ver y administrar roles"),
                new Permiso(Modulo.Usuarios, TipoAccion.Ver, "Puede ver y administrar usuarios")
            };

            var permisosOdontologo = new List<Permiso>
            {
                new Permiso(Modulo.Turnos, TipoAccion.Ver, "Puede ver sus turnos asignados"),
                new Permiso(Modulo.Turnos, TipoAccion.Editar, "Puede registrar observaciones o tratamientos"),
                new Permiso(Modulo.Tratamientos, TipoAccion.Ver, "Puede ver tratamientos disponibles")
            };

            var permisosRecepcionista = new List<Permiso>
            {
                new Permiso(Modulo.Turnos, TipoAccion.Ver, "Puede ver los turnos"),
                new Permiso(Modulo.Turnos, TipoAccion.Crear, "Puede agendar nuevos turnos"),
                new Permiso(Modulo.Turnos, TipoAccion.Editar, "Puede reprogramar o confirmar turnos"),
                new Permiso(Modulo.Pacientes, TipoAccion.Ver, "Puede ver los pacientes"),
                new Permiso(Modulo.Pacientes, TipoAccion.Crear, "Puede registrar nuevos pacientes")
            };

            var permisosReportes = new List<Permiso>
            {
                new Permiso(Modulo.Reportes, TipoAccion.Ver, "Puede consultar reportes de gestión"),
            };

            var roles = new List<Rol>
            {
                new Rol(1, "Administrador", "Acceso total al sistema", permisosAdmin, EstadoEntidad.Activo),
                new Rol(2, "Odontólogo", "Acceso a turnos y registro de atención clínica", permisosOdontologo, EstadoEntidad.Activo),
                new Rol(3, "Recepcionista", "Gestión de turnos y pacientes", permisosRecepcionista, EstadoEntidad.Activo),
                new Rol(4, "Consultor de Reportes", "Solo acceso a reportes del sistema", permisosReportes, EstadoEntidad.Inactivo)
            };

            gvRoles.DataSource = roles;
            gvRoles.DataBind();
        }

    }
}