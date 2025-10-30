using Microsoft.Ajax.Utilities;
using Microsoft.SqlServer.Server;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using SGTO.UI.Webforms.MasterPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace SGTO.UI.Webforms.Pages.Medicos
{
    public partial class Medicos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Medicos");
                master.EstablecerTituloSeccion(this.Page.Title);
            }
            if (!IsPostBack) CargarMedicos();
        }

        private void CargarMedicos()
        {
            //Metodo Test Medicos

            List<Medico> lista = new List<Medico>
    {
        new Medico(
            1,
            "Julián",
            "Mondillo",
            new DocumentoIdentidad("43669779"),
            new DateTime(2001, 9, 24),
            Genero.Masculino,
            new Telefono("11-2892-5122"),
            new Email("juli.mondi@example.com"),
            "MP-22222",
            new List<Especialidad> { new Especialidad("Ortodoncia", "Descripción de ortodoncia") },
            new List<Turno>(),
            null
        ),

        new Medico(
            2,
            "Ana",
            "García",
            new DocumentoIdentidad("35123456"),
            new DateTime(1985, 5, 15),
            Genero.Femenino,
            new Telefono("11-5555-1234"),
            new Email("ana.garcia@example.com"),
            "MN-54321",
            new List<Especialidad> { new Especialidad("Odontopediatría", "Atención dental para niños") },
            new List<Turno>(),
            null
        ),

        new Medico(
            3,
            "Carlos",
            "Rodríguez",
            new DocumentoIdentidad("38765432"),
            new DateTime(1990, 11, 30),
            Genero.Masculino,
            new Telefono("11-6666-5678"),
            new Email("carlos.rodriguez@example.com"),
            "MP-98765",
            new List<Especialidad> { new Especialidad("Endodoncia", "Tratamientos de conducto") },
            new List<Turno>(),
            null
        ),

        new Medico(
            4,
            "Lucía",
            "Pérez",
            new DocumentoIdentidad("39845217"),
            new DateTime(1992, 3, 12),
            Genero.Femenino,
            new Telefono("11-4728-3344"),
            new Email("lucia.perez@example.com"),
            "MN-12045",
            new List<Especialidad> { new Especialidad("Periodoncia", "Tratamientos de encías y tejidos dentales") },
            new List<Turno>(),
            null
        ),

        new Medico(
            5,
            "Fernando",
            "Sosa",
            new DocumentoIdentidad("37221456"),
            new DateTime(1987, 8, 5),
            Genero.Masculino,
            new Telefono("11-4789-1122"),
            new Email("fernando.sosa@example.com"),
            "MP-33456",
            new List<Especialidad> { new Especialidad("Implantología", "Implantes dentales y rehabilitación oral") },
            new List<Turno>(),
            null
        ),

        new Medico(
            6,
            "Mariana",
            "López",
            new DocumentoIdentidad("40654321"),
            new DateTime(1995, 2, 22),
            Genero.Femenino,
            new Telefono("11-4901-7788"),
            new Email("mariana.lopez@example.com"),
            "MN-88221",
            new List<Especialidad> { new Especialidad("Estética Dental", "Blanqueamientos y restauraciones estéticas") },
            new List<Turno>(),
            null
        ),

        new Medico(
            7,
            "Santiago",
            "Fernández",
            new DocumentoIdentidad("36412890"),
            new DateTime(1989, 6, 18),
            Genero.Masculino,
            new Telefono("11-5123-9944"),
            new Email("santiago.fernandez@example.com"),
            "MP-77002",
            new List<Especialidad> { new Especialidad("Cirugía Oral", "Extracciones y cirugías complejas de boca") },
            new List<Turno>(),
            null
        )
    };

            gvMedicos.DataSource = lista;
            gvMedicos.DataBind();
        }

        protected void gvMedicos_PageIndexChanging(object sender, GridViewPageEventArgs e) { }

        protected void gvMedicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int idMedico = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"~/Pages/Medicos/Editar?id-medico={idMedico}", false);
            }
            else if (e.CommandName == "Ver")
            {
                int idMedico = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"~/Pages/Medicos/Detalle?id-medico={idMedico}", false);
            }

        }


        protected void gvMedicos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                var medico = (Medico)e.Row.DataItem;
                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");

                if (lblEstado != null && medico != null)
                {
                    string cssClass = "badge "; // clase base


                    switch (medico.Estado)
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
                    lblEstado.InnerText = medico.Estado.ToString(); // Mostrar el texto del estado
                }
            }
        }

        protected void btnNuevoMedico_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Medicos/Nuevo", false);
        }



    }


}