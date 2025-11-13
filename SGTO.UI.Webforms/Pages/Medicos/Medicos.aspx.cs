
using Microsoft.Ajax.Utilities;
using Microsoft.SqlServer.Server;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using SGTO.UI.Webforms.MasterPages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SGTO.Negocio.Servicios;
using SGTO.Negocio.DTOs.Medicos;

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
            try
            {
                MedicoService medicoService = new MedicoService();
                List<MedicoListadoDto> listaDto = medicoService.Listar("A");
                gvMedicos.DataSource = listaDto;
                gvMedicos.DataBind();
            }
            catch (Exception ex)
            {
               
                Debug.WriteLine("Error al cargar médicos: " + ex.Message);
            }
        }

        protected void gvMedicos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           
        }

        protected void gvMedicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
            if (e.CommandName == "Editar")
            {
                // int idMedico = Convert.ToInt32(e.CommandArgument);
                // Response.Redirect($"~/Pages/Medicos/Editar?id-medico={idMedico}", false);
            }
            else if (e.CommandName == "Ver")
            {
                // int idMedico = Convert.ToInt32(e.CommandArgument);
                // Response.Redirect($"~/Pages/Medicos/Detalle?id-medico={idMedico}", false);
            }
        }


        protected void gvMedicos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var medicoDto = (MedicoListadoDto)e.Row.DataItem;
                var lblEstado = (HtmlGenericControl)e.Row.FindControl("lblEstado");

                if (lblEstado != null && medicoDto != null)
                {
                    string cssClass = "badge ";

                    switch (medicoDto.Estado)
                    {
                        case "Activo":
                            cssClass += "badge-primary";
                            break;
                        case "Inactivo":
                            cssClass += "badge-secondary";
                            break;
                        default:
                            cssClass += "badge-secondary";
                            break;
                    }
                    lblEstado.Attributes["class"] = cssClass;
                    lblEstado.InnerText = medicoDto.Estado;
                }
            }
        }

        protected void btnNuevoMedico_Click(object sender, EventArgs e)
        {
            
            // Response.Redirect("~/Pages/Medicos/Nuevo", false);
        }
    }
}