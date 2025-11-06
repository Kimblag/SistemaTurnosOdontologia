using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.CoberturasPlanes
{
    public partial class CoberturasPlanes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // se pregunta si se carga la página desde un site master (la pagina actual es hija de site master)
            // y se activa la opcion
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Coberturas");
                master.EstablecerTituloSeccion(this.Page.Title);
            }
            ModalHelper.MostrarModalDesdeSession(this, "ModalTitulo", "ModalDesc");
        }

        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            string tipo = hdnTipoEliminar.Value;
            int id = Convert.ToInt32(hdnIdEliminar.Value);

            try
            {
                TurnoService servicioTurno = new TurnoService();
                if (tipo == "cobertura")
                {
                    CoberturaService servicioCobertura = new CoberturaService();
                    servicioCobertura.DarDeBajaCobertura(id, servicioTurno);

                    MensajeUiHelper.SetearMensaje("Cobertura dada de baja", "La cobertura y sus planes fueron dados de baja correctamente.");
                }
                else if (tipo == "plan")
                {

                    PlanService servicioPlan = new PlanService();
                    servicioPlan.DarDeBajaPlan(id, servicioTurno);

                    MensajeUiHelper.SetearMensaje("Plan dado de baja", "El plan fue dado de baja correctamente.");
                }
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearMensaje("Operación no permitida", ex.Message);
            }
            catch (Exception)
            {
                MensajeUiHelper.SetearMensaje("Error inesperado", "Ocurrió un error al intentar dar de baja el registro.");
            }

            Response.Redirect(Request.RawUrl, false);
        }
    }
}