using System;
using System.Web.SessionState;
using System.Web.UI;

namespace SGTO.UI.Webforms.Utils
{
    // esta clase es para reutilizarla y poder mostrar modales en toda la UI sin necesidad de repetir codigo,
    // ya que estaba repitiendo lo mismo en coberturas form y listado. Ahora se podrá reutilizar en cualquier lugar.
    public static class ModalHelper
    {
        public static void MostrarModalDesdeSession(
                   Page page,
                   string sessionTituloKey,
                   string sessionDescKey,
                   string redirectUrl = null,
                   string funcionJsPersonalizada = null)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            HttpSessionState session = page.Session;
            if (session[sessionTituloKey] == null)
                return;

            string titulo = session[sessionTituloKey]?.ToString() ?? string.Empty;
            string descripcion = session[sessionDescKey]?.ToString() ?? string.Empty;

            string tipoModal = session["ModalTipo"]?.ToString()?.ToLower() ?? "confirmacion";


            string funcionJs = !string.IsNullOrEmpty(funcionJsPersonalizada)
                ? funcionJsPersonalizada
                : (tipoModal == "resultado" ? "abrirModalResultado" : "abrirModalConfirmacion");


            titulo = titulo.Replace("'", "\\'");
            descripcion = descripcion.Replace("'", "\\'");


            string redirect = !string.IsNullOrEmpty(redirectUrl)
                ? $@"
                    ['btnModalOk', 'btnModalCerrar'].forEach(function(id) {{
                        var btn = document.getElementById(id);
                        if (btn) {{
                            btn.addEventListener('click', function() {{
                                window.location.href = '{redirectUrl}';
                            }});
                        }}
                    }});
                    "
                : string.Empty;

            // script final
            string script = $@"
                document.addEventListener('DOMContentLoaded', function() {{
                    if (typeof bootstrap !== 'undefined') {{
                        if (typeof {funcionJs} === 'function') {{
                            {funcionJs}('{titulo}', '{descripcion}');
                            {redirect}
                        }} else {{
                            console.warn('No se encontró la función JS {funcionJs}');
                        }}
                    }} else {{
                        console.error('Bootstrap aún no está disponible.');
                    }}
                }});
            ";

            ScriptManager.RegisterStartupScript(page, page.GetType(), "MostrarModal", script, true);

            // limpiar sesión
            session[sessionTituloKey] = null;
            session[sessionDescKey] = null;
            session["ModalTipo"] = null;
        }
    }
}