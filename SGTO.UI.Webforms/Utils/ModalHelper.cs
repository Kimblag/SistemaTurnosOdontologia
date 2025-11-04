using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;

namespace SGTO.UI.Webforms.Utils
{
    // esta clase es para reutilizarla y poder mostrar modales en toda la UI sin necesidad de repetir codigo,
    // ya que estaba repitiendo lo mismo en coberturas form y listado. Ahora se podrá reutilizar en cualquier lugar.
    public static class ModalHelper
    {
        public static void MostrarModalDesdeSession(Page page, string sessionTituloKey,
            string sessionDescKey,
            string redirectUrl = null)
        {
            // validar primero porque si es null no se puede avanzar (no debería ser null, pero agrego por si acaso)
            if (page == null) throw new ArgumentNullException(nameof(page));

            // aca se obtiene el objeto Session que maneja la página actual en ese momento.
            HttpSessionState session = page.Session;
            if (session[sessionTituloKey] == null)
                return;

            string titulo = session[sessionTituloKey].ToString();
            string descripcion = session[sessionDescKey] != null
                ? session[sessionDescKey].ToString()
                : string.Empty;
            string tipoModal = session["ModalTipo"] != null ? session["ModalTipo"].ToString().ToLower() : "confirmacion";
            string funcionJs = tipoModal == "resultado" ? "abrirModalResultado" : "abrirModalConfirmacion";

            // se reemplazan posibles comillas, ya que sino se rompe el script y es un error comprobado que puede ocurrir
            titulo = titulo.Replace("'", "\\'");
            descripcion = descripcion.Replace("'", "\\'");

            string redirect = !string.IsNullOrEmpty(redirectUrl)
                ? $"document.getElementById('btnModalOk').addEventListener('click', function() " +
                $"{{ window.location.href = '{redirectUrl}'; }});"
                : string.Empty;

            string script = $@"
                document.addEventListener('DOMContentLoaded', function() {{
                    if (typeof bootstrap !== 'undefined') {{
                        {funcionJs}('{titulo}', '{descripcion}');
                        {redirect}
                    }} else {{
                        console.error('Bootstrap aún no está disponible.');
                    }}
                }});
            ";

            ScriptManager.RegisterStartupScript(page, page.GetType(), "MostrarModal", script, true);

            // se limpia la sesión para que no queden residuos luego de mostrar el modal.
            session[sessionTituloKey] = null;
            session[sessionDescKey] = null;
            session["ModalTipo"] = null;
        }
    }
}