using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.UI.Webforms.MasterPages;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Tratamientos
{
    public partial class Tratamientos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Tratamientos");
                master.EstablecerTituloSeccion(this.Page.Title);
            }

            if (!IsPostBack)
                CargarTratamientos();
        }

        private void CargarTratamientos()
        {
            // Especialidades de ejemplo
            var espGeneral = new Especialidad("Odontología General", "Tratamientos generales y preventivos.");
            var espEstetica = new Especialidad("Estética Dental", "Tratamientos estéticos para mejorar la sonrisa.");
            var espOrtodoncia = new Especialidad("Ortodoncia", "Corrección de dientes y mandíbulas.");
            var espEndodoncia = new Especialidad("Endodoncia", "Tratamiento de conducto radicular.");

            
            var listaTratamientos = new List<Tratamiento>
            {
                new Tratamiento("Limpieza Dental", "Eliminación de placa y sarro.", 1500, espGeneral)
                {
                    IdTratamiento = 1,
                    Estado = EstadoEntidad.Activo
                },
                new Tratamiento("Blanqueamiento Dental", "Tratamiento estético para aclarar el color de los dientes.", 4000, espEstetica)
                {
                    IdTratamiento = 2,
                    Estado = EstadoEntidad.Activo
                },
                new Tratamiento("Ortodoncia (Brackets)", "Corrección de la posición de los dientes mediante aparatos.", 25000, espOrtodoncia)
                {
                    IdTratamiento = 3,
                    Estado = EstadoEntidad.Activo
                },
                new Tratamiento("Tratamiento de Caries", "Remoción de caries y restauración del diente afectado.", 2000, espGeneral)
                {
                    IdTratamiento = 4,
                    Estado = EstadoEntidad.Inactivo
                },
                new Tratamiento("Endodoncia", "Tratamiento de conducto radicular.", 5500, espEndodoncia)
                {
                    IdTratamiento = 5,
                    Estado = EstadoEntidad.Activo
                },
                new Tratamiento("Extracción Simple", "Extracción de pieza dental no compleja.", 3000, espGeneral)
                {
                    IdTratamiento = 6,
                    Estado = EstadoEntidad.Activo
                },
                new Tratamiento("Consulta Ortodoncia", "Evaluación y seguimiento de ortodoncia.", 2500, espOrtodoncia)
                {
                    IdTratamiento = 7,
                    Estado = EstadoEntidad.Activo
                },
                new Tratamiento("Carillas de Resina", "Láminas finas de resina para cubrir el diente.", 8000, espEstetica)
                {
                    IdTratamiento = 8,
                    Estado = EstadoEntidad.Activo
                }

            };

            gvTratamientos.DataSource = listaTratamientos;
            gvTratamientos.DataBind();
        }

        protected void btnNuevoTratamiento_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Tratamientos/Nuevo.aspx", false);
        }

        protected void gvTratamientos_PageIndexChanging(object sender, GridViewPageEventArgs e) {}

        protected void gvTratamientos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int idTratamiento = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"~/Pages/Tratamientos/Editar?id-tratamiento={idTratamiento}", false);
            }
            else if (e.CommandName == "Ver")
            {
                int idTratamiento = Convert.ToInt32(e.CommandArgument);
                //Response.Redirect($"~/Pages/Tratamientos/Detalle?id-tratamiento={idTratamiento}", false);
            }

        }

        protected void gvTratamientos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                var tratamiento = (Tratamiento)e.Row.DataItem;
                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");

                if (lblEstado != null && tratamiento != null)
                {
                    string cssClass = "badge "; // clase base


                    switch (tratamiento.Estado)
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
                    lblEstado.InnerText = tratamiento.Estado.ToString(); // Mostrar el texto del estado
                }
            }
        }
    }
}
