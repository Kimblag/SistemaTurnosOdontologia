using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Pacientes
{
    public partial class Pacientes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) CargarPacientes();
        }

        private void CargarPacientes()
        {
            // metodo para testear lista de pacientes
            var osde = new Cobertura("OSDE", "Cobertura médica prepaga");
            var galeno = new Cobertura("Galeno", "Cobertura médica integral");
            var swiss = new Cobertura("Swiss Medical", "Cobertura premium");
            var medicus = new Cobertura("Medicus", "Cobertura urbana");
            var particular = new Cobertura("Particular", "Paciente particular");


            List<Paciente> lista = new List<Paciente>
            {
                new Paciente(1, "Carla", "Ramírez", new DocumentoIdentidad("38555666"),
                    new DateTime(1995, 3, 14), Genero.Femenino,
                    new Telefono("11-6543-2211"), new Email("carla.ramirez@example.com"),
                    osde, new Plan("210", 0.80m, osde, "Plan clásico 80% cobertura"),
                    new List<Turno>(), new List<HistoriaClinicaRegistro>()),

                new Paciente(2, "Luciano", "Pérez", new DocumentoIdentidad("37222444"),
                    new DateTime(1988, 9, 2), Genero.Masculino,
                    new Telefono("11-6001-2233"), new Email("luciano.perez@example.com"),
                    swiss, new Plan("SMG20", 0.90m, swiss, "Plan 90% cobertura en odontología"),
                    new List<Turno>(), new List<HistoriaClinicaRegistro>()),

                new Paciente(3, "Sofía", "Fernández", new DocumentoIdentidad("42111888"),
                    new DateTime(2000, 7, 25), Genero.Femenino,
                    new Telefono("11-7033-5599"), new Email("sofia.fernandez@example.com"),
                    galeno, new Plan("400", 0.85m, galeno, "Plan integral 85%"),
                    new List<Turno>(), new List<HistoriaClinicaRegistro>()),

                new Paciente(4, "Martín", "Domínguez", new DocumentoIdentidad("39544222"),
                    new DateTime(1992, 11, 10), Genero.Masculino,
                    new Telefono("11-8855-2231"), new Email("martin.dominguez@example.com"),
                    particular, null,
                    new List<Turno>(), new List<HistoriaClinicaRegistro>()),

                new Paciente(5, "Julieta", "Morales", new DocumentoIdentidad("43444555"),
                    new DateTime(1998, 1, 5), Genero.Femenino,
                    new Telefono("11-7774-9912"), new Email("julieta.morales@example.com"),
                    medicus, new Plan("Medicus Azul", 0.75m, medicus, "Plan Azul 75% cobertura"),
                    new List<Turno>(), new List<HistoriaClinicaRegistro>()),

                new Paciente(6, "Tomás", "Gómez", new DocumentoIdentidad("40223311"),
                    new DateTime(1990, 4, 20), Genero.Masculino,
                    new Telefono("11-9123-4412"), new Email("tomas.gomez@example.com"),
                    particular, null,
                    new List<Turno>(), new List<HistoriaClinicaRegistro>()),

                new Paciente(7, "Camila", "López", new DocumentoIdentidad("42899877"),
                    new DateTime(1997, 6, 18), Genero.Femenino,
                    new Telefono("11-6342-7888"), new Email("camila.lopez@example.com"),
                    osde, new Plan("310", 0.90m, osde, "Plan 310 odontológico"),
                    new List<Turno>(), new List<HistoriaClinicaRegistro>()),

                new Paciente(8, "Ezequiel", "Martínez", new DocumentoIdentidad("41022764"),
                    new DateTime(1985, 12, 4), Genero.Masculino,
                    new Telefono("11-5556-4432"), new Email("ezequiel.martinez@example.com"),
                    galeno, new Plan("220", 0.70m, galeno, "Plan básico 70%"),
                    new List<Turno>(), new List<HistoriaClinicaRegistro>()),

                new Paciente(9, "Agustina", "Ruiz", new DocumentoIdentidad("43777888"),
                    new DateTime(1999, 10, 11), Genero.Femenino,
                    new Telefono("11-6112-2398"), new Email("agustina.ruiz@example.com"),
                    particular, null,
                    new List<Turno>(), new List<HistoriaClinicaRegistro>()),

                new Paciente(10, "Federico", "Torres", new DocumentoIdentidad("39666123"),
                    new DateTime(1991, 2, 17), Genero.Masculino,
                    new Telefono("11-7234-5589"), new Email("federico.torres@example.com"),
                    swiss, new Plan("SMG10", 0.75m, swiss, "Plan 75% cobertura básica"),
                    new List<Turno>(), new List<HistoriaClinicaRegistro>()),

                new Paciente(11, "Valentina", "Sosa", new DocumentoIdentidad("44455999"),
                    new DateTime(1996, 5, 23), Genero.Femenino,
                    new Telefono("11-9087-6631"), new Email("valentina.sosa@example.com"),
                    osde, new Plan("410", 0.95m, osde, "Plan 410 Premium"),
                    new List<Turno>(), new List<HistoriaClinicaRegistro>()),

                new Paciente(12, "Diego", "Navarro", new DocumentoIdentidad("38999111"),
                    new DateTime(1983, 8, 12), Genero.Masculino,
                    new Telefono("11-5433-2299"), new Email("diego.navarro@example.com"),
                    particular, null,
                    new List<Turno>(), new List<HistoriaClinicaRegistro>()),

                new Paciente(13, "Florencia", "Vega", new DocumentoIdentidad("42666555"),
                    new DateTime(1994, 9, 30), Genero.Femenino,
                    new Telefono("11-7344-8800"), new Email("florencia.vega@example.com"),
                    medicus, new Plan("Medicus Rojo", 0.85m, medicus, "Plan Rojo 85%"),
                    new List<Turno>(), new List<HistoriaClinicaRegistro>()),

                new Paciente(14, "Nicolás", "Castro", new DocumentoIdentidad("40111222"),
                    new DateTime(1989, 11, 8), Genero.Masculino,
                    new Telefono("11-6122-4467"), new Email("nicolas.castro@example.com"),
                    osde, new Plan("210", 0.80m, osde, "Plan clásico 80% cobertura"),
                    new List<Turno>(), new List<HistoriaClinicaRegistro>()),

                new Paciente(15, "Milagros", "Ponce", new DocumentoIdentidad("44555123"),
                    new DateTime(1997, 3, 5), Genero.Femenino,
                    new Telefono("11-8809-7710"), new Email("milagros.ponce@example.com"),
                    particular, null,
                    new List<Turno>(), new List<HistoriaClinicaRegistro>())
            };

            gvPacientes.DataSource = lista;
            gvPacientes.DataBind();
        }

        protected void gvTurnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var paciente = (Paciente)e.Row.DataItem;
                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");

                if (lblEstado != null)
                {
                    string cssClass = "badge "; // clase base

                    switch (paciente.Estado)
                    {
                        case EstadoEntidad.Activo:
                            cssClass += "badge-primary";
                            break;
                        case EstadoEntidad.Inactivo:
                            cssClass += "badge-secondary";
                            break;
                        default:
                            cssClass += "badge-secondary";
                            break;
                    }
                    lblEstado.Attributes["class"] = cssClass;
                }
            }
        }

        protected void gvPacientes_PageIndexChanging(object sender, GridViewPageEventArgs e) { }

        protected void gvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int idPaciente = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"~/Pages/Pacientes/Editar?id-paciente={idPaciente}", false);
            }
            else if (e.CommandName == "Ver")
            {
                int idPaciente = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"~/Pages/Pacientes/Detalle?id-paciente={idPaciente}", false);
            }
        }

        protected void btnNuevoPaciente_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Pacientes/Nuevo", false);
        }
    }
}