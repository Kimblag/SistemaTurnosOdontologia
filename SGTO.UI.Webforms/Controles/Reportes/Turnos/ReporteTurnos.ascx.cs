using SGTO.Dominio.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Reportes.Turnos
{
    public partial class ReporteTurnos : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarReporte();
        }

        private void CargarReporte()
        {
            var lista = new List<ReporteTurnoDto>
            {
                new ReporteTurnoDto { Fecha = new DateTime(2025,10,10), Paciente = "Carlos Sánchez", Medico = "Dr. Juan Pérez", Especialidad = "Odontología General", Cobertura = "OSDE", Plan = "210", Estado = "Atendido" },
                new ReporteTurnoDto { Fecha = new DateTime(2025,10,11), Paciente = "María López", Medico = "Dra. Ana Gómez", Especialidad = "Ortodoncia", Cobertura = "Swiss Medical", Plan = "SMG 300", Estado = "Cancelado" },
                new ReporteTurnoDto { Fecha = new DateTime(2025,10,12), Paciente = "Pedro Gómez", Medico = "Dr. Juan Pérez", Especialidad = "Odontología General", Cobertura = "Galeno", Plan = "220", Estado = "No Asistió" },
                new ReporteTurnoDto { Fecha = new DateTime(2025,10,13), Paciente = "Lucía Fernández", Medico = "Dra. Ana Gómez", Especialidad = "Ortodoncia", Cobertura = "OSDE", Plan = "310", Estado = "Atendido" },
                new ReporteTurnoDto { Fecha = new DateTime(2025,10,14), Paciente = "Ricardo Sosa", Medico = "Dr. Juan Pérez", Especialidad = "Odontología General", Cobertura = "Swiss Medical", Plan = "SMG 300", Estado = "Reprogramado" },
                new ReporteTurnoDto { Fecha = new DateTime(2025,10,15), Paciente = "Laura Martínez", Medico = "Dra. Ana Gómez", Especialidad = "Ortodoncia", Cobertura = "OSDE", Plan = "210", Estado = "Atendido" },
                new ReporteTurnoDto { Fecha = new DateTime(2025,10,16), Paciente = "Pablo Díaz", Medico = "Dr. Juan Pérez", Especialidad = "Odontología General", Cobertura = "Galeno", Plan = "320", Estado = "Atendido" },
                new ReporteTurnoDto { Fecha = new DateTime(2025,10,17), Paciente = "Julieta Romero", Medico = "Dr. Juan Pérez", Especialidad = "Endodoncia", Cobertura = "OSDE", Plan = "210", Estado = "Cancelado" },
                new ReporteTurnoDto { Fecha = new DateTime(2025,10,18), Paciente = "Martín Torres", Medico = "Dra. Ana Gómez", Especialidad = "Ortodoncia", Cobertura = "Swiss Medical", Plan = "SMG 400", Estado = "Atendido" },
                new ReporteTurnoDto { Fecha = new DateTime(2025,10,19), Paciente = "Sofía Ruiz", Medico = "Dr. Juan Pérez", Especialidad = "Odontología General", Cobertura = "OSDE", Plan = "210", Estado = "No Asistió" }
            };

            gvTurnos.DataSource = lista;
            gvTurnos.DataBind();
        }

        protected void gvTurnos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTurnos.PageIndex = e.NewPageIndex;
            CargarReporte();
        }


    }
}