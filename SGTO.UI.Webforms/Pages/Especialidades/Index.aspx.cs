using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.UI.Webforms.MasterPages;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Especialidades
{
    public partial class Especialidades : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Especialidades");
                master.EstablecerTituloSeccion(this.Page.Title);
            }

            if (!IsPostBack)
                CargarEspecialidades();
        }

        private void CargarEspecialidades()
        {
           
            var listaEspecialidades = new List<Especialidad>
            {
                new Especialidad("Ortodoncia", "Corrección de dientes y mandíbulas.") { Estado = EstadoEntidad.Activo },
                new Especialidad("Endodoncia", "Tratamiento de conducto radicular.") { Estado = EstadoEntidad.Activo },
                new Especialidad("Implantología", "Colocación de implantes dentales.") { Estado = EstadoEntidad.Activo },
                new Especialidad("Periodoncia", "Tratamiento de las encías y tejidos que rodean los dientes.") { Estado = EstadoEntidad.Activo },
                new Especialidad("Odontopediatría", "Atención dental especializada en niños.") { Estado = EstadoEntidad.Activo },
                new Especialidad("Cirugía Bucal", "Extracción de piezas dentarias y cirugía de tejidos orales.") { Estado = EstadoEntidad.Activo },
                new Especialidad("Estética Dental", "Blanqueamientos, carillas y tratamientos estéticos del diente.") { Estado = EstadoEntidad.Inactivo }
            };


            gvEspecialidades.DataSource = listaEspecialidades;
            gvEspecialidades.DataBind();
        }

        protected void btnNuevaEspecialidad_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Especialidades/Nuevo.aspx", false);
        }

        protected void gvEspecialidades_PageIndexChanging(object sender, GridViewPageEventArgs e){}

        protected void gvEspecialidades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int idEspecialidad = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"~/Pages/Especialidades/Editar?id-medico={idEspecialidad}", false);
            }
            else if (e.CommandName == "Ver")
            {
                int idEspecialidad = Convert.ToInt32(e.CommandArgument);
                //Response.Redirect($"~/Pages/Especialidades/Detalle?id-especialidad={idEspecialidad}", false);
            }

        }

        protected void gvEspecialidades_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                var especialidad = (Especialidad)e.Row.DataItem;
                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");

                if (lblEstado != null && especialidad != null)
                {
                    string cssClass = "badge "; // clase base


                    switch (especialidad.Estado)
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
                    lblEstado.InnerText = especialidad.Estado.ToString(); // Mostrar el texto del estado
                }
            }
        }
    }
}
