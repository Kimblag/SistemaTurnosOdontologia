using SGTO.Negocio.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Reportes.Pacientes
{
    public partial class ReportePacientes : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) CargarReporte();
        }

        protected void gvPacientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPacientes.PageIndex = e.NewPageIndex;
            CargarReporte();
        }


        private void CargarReporte()
        {
            //metodo de prueba
            List<ReportePacienteDto> lista = new List<ReportePacienteDto>
            {
                 new ReportePacienteDto
                {
                    Nombre = "Carlos Sánchez",
                    Dni = "30123456",
                    Cobertura = "OSDE",
                    Plan = "210",
                    TotalTurnos = 12,
                    UltimaAtencion = new DateTime(2025, 10, 20),
                    MedicoMasFrecuente = "Dr. Juan Pérez"
                },
                new ReportePacienteDto
                {
                    Nombre = "María Rodríguez",
                    Dni = "35987654",
                    Cobertura = "Swiss Medical",
                    Plan = "SMG20",
                    TotalTurnos = 8,
                    UltimaAtencion = new DateTime(2025, 10, 18),
                    MedicoMasFrecuente = "Dra. Ana Gómez"
                },
                new ReportePacienteDto
                {
                    Nombre = "Pedro Martínez",
                    Dni = "28456789",
                    Cobertura = "Particular",
                    Plan = "-",
                    TotalTurnos = 5,
                    UltimaAtencion = new DateTime(2025, 10, 17),
                    MedicoMasFrecuente = "Dr. Juan Pérez"
                },
                new ReportePacienteDto
                {
                    Nombre = "Laura Fernández",
                    Dni = "40111222",
                    Cobertura = "OSDE",
                    Plan = "310",
                    TotalTurnos = 2,
                    UltimaAtencion = new DateTime(2025, 10, 15),
                    MedicoMasFrecuente = "Dra. Ana Gómez"
                },
                new ReportePacienteDto
                {
                    Nombre = "Juana Díaz",
                    Dni = "33555888",
                    Cobertura = "Particular",
                    Plan = "-",
                    TotalTurnos = 21,
                    UltimaAtencion = new DateTime(2025, 10, 14),
                    MedicoMasFrecuente = "Dra. Ana Gómez"
                },
                new ReportePacienteDto
                {
                    Nombre = "Martín Torres",
                    Dni = "31222777",
                    Cobertura = "OSDE",
                    Plan = "210",
                    TotalTurnos = 10,
                    UltimaAtencion = new DateTime(2025, 10, 13),
                    MedicoMasFrecuente = "Dr. Juan Pérez"
                },
                new ReportePacienteDto
                {
                    Nombre = "Lucía Gómez",
                    Dni = "27544321",
                    Cobertura = "Swiss Medical",
                    Plan = "SMG20",
                    TotalTurnos = 6,
                    UltimaAtencion = new DateTime(2025, 10, 11),
                    MedicoMasFrecuente = "Dra. Ana Gómez"
                },
                new ReportePacienteDto
                {
                    Nombre = "Pablo Díaz",
                    Dni = "28999111",
                    Cobertura = "Galeno",
                    Plan = "320",
                    TotalTurnos = 4,
                    UltimaAtencion = new DateTime(2025, 10, 10),
                    MedicoMasFrecuente = "Dr. Juan Pérez"
                },
                new ReportePacienteDto
                {
                    Nombre = "Julieta Romero",
                    Dni = "29888222",
                    Cobertura = "OSDE",
                    Plan = "210",
                    TotalTurnos = 7,
                    UltimaAtencion = new DateTime(2025, 10, 9),
                    MedicoMasFrecuente = "Dr. Juan Pérez"
                },
                new ReportePacienteDto
                {
                    Nombre = "Federico Blanco",
                    Dni = "31233444",
                    Cobertura = "Swiss Medical",
                    Plan = "SMG20",
                    TotalTurnos = 3,
                    UltimaAtencion = new DateTime(2025, 10, 8),
                    MedicoMasFrecuente = "Dra. Ana Gómez"
                },
                new ReportePacienteDto
                {
                    Nombre = "Sofía Ruiz",
                    Dni = "27899123",
                    Cobertura = "Particular",
                    Plan = "-",
                    TotalTurnos = 9,
                    UltimaAtencion = new DateTime(2025, 10, 7),
                    MedicoMasFrecuente = "Dr. Juan Pérez"
                }
            };

            gvPacientes.DataSource = lista;
            gvPacientes.DataBind();

        }
    }
}