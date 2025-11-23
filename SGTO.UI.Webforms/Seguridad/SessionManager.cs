using SGTO.Negocio.DTOs.Seguridad;
using System.Web;

namespace SGTO.UI.Webforms.Seguridad
{
    public static class SessionManager
    {
        private const string CLAVE_SESION = "UsuarioSesion";

        public static UsuarioSesionDto Usuario
        {
            get
            {
                if (HttpContext.Current.Session[CLAVE_SESION] != null)
                {
                    return (UsuarioSesionDto)HttpContext.Current.Session[CLAVE_SESION];
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session[CLAVE_SESION] = value;
            }
        }


        public static bool EstaLogueado()
        {
            return Usuario != null;
        }

        public static void CerrarSesion()
        {
            HttpContext.Current.Session.Abandon();
        }

    }
}