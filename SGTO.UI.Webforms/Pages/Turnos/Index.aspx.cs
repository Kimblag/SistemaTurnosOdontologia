using SGTO.Dominio.Enums;
using SGTO.UI.Webforms.MasterPages;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
                    Estado = EstadoTurno.Nuevo
                },
                new {
                    IdTurno = 2,
                    FechaHora = new DateTime(2025, 10, 28, 10, 30, 0),
                    Paciente = new { Nombre = "Lucia", Apellido = "Fernandez" },
                    Medico = new { Nombre = "Juan", Apellido = "Perez" },
                    Estado = EstadoTurno.Cerrado
                },
                new {
                    IdTurno = 3,
                    FechaHora = new DateTime(2025, 10, 29, 11, 0, 0),
                    Paciente = new { Nombre = "Miguel", Apellido = "Suarez" },
                    Medico = new { Nombre = "Ana", Apellido = "Martinez" },
                    Estado = EstadoTurno.Cancelado
                },
                new {
                    IdTurno = 4,
                    FechaHora = new DateTime(2025, 10, 29, 15, 0, 0),
                    Paciente = new { Nombre = "Elena", Apellido = "Ruiz" },
                    Medico = new { Nombre = "Carlos", Apellido = "Sanchez" },
                    Estado = EstadoTurno.Reprogramado
                },
                new {
                    IdTurno = 5,
                    FechaHora = new DateTime(2025, 10, 30, 8, 30, 0),
                    Paciente = new { Nombre = "Javier", Apellido = "Lopez" },
                    Medico = new { Nombre = "Laura", Apellido = "Garcia" },
                    Estado = EstadoTurno.Nuevo
                },
                new {
                    IdTurno = 6,
                    FechaHora = new DateTime(2025, 10, 30, 14, 0, 0),
                    Paciente = new { Nombre = "Sofia", Apellido = "Diaz" },
                    Medico = new { Nombre = "Juan", Apellido = "Perez" },
                    Estado = EstadoTurno.Cerrado
                },
                new {
                    IdTurno = 7,
                    FechaHora = new DateTime(2025, 10, 31, 16, 45, 0),
                    Paciente = new { Nombre = "Pedro", Apellido = "Alvarez" },
                    Medico = new { Nombre = "Carlos", Apellido = "Sanchez" },
                    Estado = EstadoTurno.NoAsistio
                },
                new {
                    IdTurno = 8,
                    FechaHora = new DateTime(2025, 11, 1, 17, 45, 0),
                    Paciente = new { Nombre = "Mariano", Apellido = "Cremone" },
                    Medico = new { Nombre = "Carles", Apellido = "Pericles" },
                    Estado = EstadoTurno.NoAsistio
                }

            };


            gvTurnos.DataSource = listaDeEjemplo;
            gvTurnos.DataBind();
        }


        protected void txtBuscar_TextChanged(object sender, EventArgs e) { }
        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e) { }
        protected void ddlCriterio_SelectedIndexChanged(object sender, EventArgs e) { }

        protected void gvTurnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var estado = (EstadoTurno)DataBinder.Eval(e.Row.DataItem, "Estado");

                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");

                if (lblEstado != null)
                {
                    string textoEstado = "";
                    string cssClass = "badge ";

                    switch (estado)
                    {
                        case EstadoTurno.Nuevo:
                            textoEstado = "Nuevo";
                            cssClass += "badge-primary";
                            break;
                        case EstadoTurno.Cancelado:
                            textoEstado = "Cancelado";
                            cssClass += "badge-danger";
                            break;
                        case EstadoTurno.Reprogramado:
                            textoEstado = "Reprogramado";
                            cssClass += "badge-warning";
                            break;
                        case EstadoTurno.Cerrado:
                            textoEstado = "Cerrado";
                            cssClass += "badge-success";
                            break;
                        case EstadoTurno.NoAsistio:
                            textoEstado = "No asistió";
                            cssClass += "badge-dark";
                            break;
                        default:
                            textoEstado = "Indefinido";
                            cssClass += "badge-secondary";
                            break;
                    }


                    lblEstado.Attributes["class"] = cssClass;
                    lblEstado.InnerText = textoEstado;
                }
            }
        }

        protected void gvTurnos_PageIndexChanging(object sender, GridViewPageEventArgs e) { }

        protected void gvTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument != null && !string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                int idTurno = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName == "Editar")
                {
                    Response.Redirect($"~/Pages/Turnos/Editar?id-turno={idTurno}", false);
                }
                else if (e.CommandName == "Ver")
                {
                    //Response.Redirect($"~/Pages/Turnos/Detalle?id-turno={idTurno}", false);
                }
            }
        }

        protected void btnNuevoTurno_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Turnos/Nuevo", false);
        }

    }
}