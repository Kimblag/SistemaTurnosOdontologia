using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Turnos
{
    public partial class TurnoForm : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCancelarTurno_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Turnos/Index", false);
        }
    }
}