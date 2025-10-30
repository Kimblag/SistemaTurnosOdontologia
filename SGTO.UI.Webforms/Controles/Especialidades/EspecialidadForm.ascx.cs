using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Especialidades
{
    public partial class EspecialidadForm : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    

    protected void btnCancelar_Click(object sender, EventArgs e)

        {
            Response.Redirect("~/Pages/Especialidades/Index.aspx", false);
        }
    
    }
}