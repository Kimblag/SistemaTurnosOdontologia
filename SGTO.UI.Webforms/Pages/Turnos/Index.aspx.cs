using SGTO.UI.Webforms.MasterPages;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SGTO.UI.Webforms.Pages.Turnos
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("turnos");
                master.EstablecerTituloSeccion(this.Page.Title);
            }
            if (!IsPostBack)
            {
                CargarTurnos();
            }
        }


        private void CargarTurnos()
        {
            
            var listaDeEjemplo = new List<object>
            {
                new {
                    IdTurno = 1,
                    FechaHora = new DateTime(2025, 10, 28, 9, 0, 0),
                    Paciente = new { Nombre = "Carlos", Apellido = "Gomez" },
                    Medico = new { Nombre = "Ana", Apellido = "Martinez" },
                    Estado = "Programado" // Estado como simple texto
                },
                new {
                    IdTurno = 2,
                    FechaHora = new DateTime(2025, 10, 28, 10, 30, 0),
                    Paciente = new { Nombre = "Lucia", Apellido = "Fernandez" },
                    Medico = new { Nombre = "Juan", Apellido = "Perez" },
                    Estado = "Confirmado"
                },
                new {
                    IdTurno = 3,
                    FechaHora = new DateTime(2025, 10, 29, 11, 0, 0),
                    Paciente = new { Nombre = "Miguel", Apellido = "Suarez" },
                    Medico = new { Nombre = "Ana", Apellido = "Martinez" },
                    Estado = "Cancelado"
                },
                new {
                    IdTurno = 4,
                    FechaHora = new DateTime(2025, 10, 29, 15, 0, 0),
                    Paciente = new { Nombre = "Elena", Apellido = "Ruiz" },
                    Medico = new { Nombre = "Carlos", Apellido = "Sanchez" },
                    Estado = "Programado"
                }
            };

           
            gvTurnos.DataSource = listaDeEjemplo;
            gvTurnos.DataBind();
        }

        
        protected void txtBuscar_TextChanged(object sender, EventArgs e) { }
        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e) { }
        protected void ddlCriterio_SelectedIndexChanged(object sender, EventArgs e) { }
        protected void btnNuevoTurno_Click(object sender, EventArgs e) { }
    }
}