using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace SGTO.UI.Webforms.Utils
{
    public static class MensajeUiHelper
    {
        private const string SESSION_TITULO = "CoberturaMensajeTitulo";
        private const string SESSION_DESCRIPCION = "CoberturaMensajeDesc";
        private const string SESSION_TIPO = "ModalTipo";

        public static void SetearMensaje(string titulo, string descripcion, string tipo = "Resultado")
        {
            HttpContext.Current.Session[SESSION_TITULO] = titulo;
            HttpContext.Current.Session[SESSION_DESCRIPCION] = descripcion;
            HttpContext.Current.Session[SESSION_TIPO] = tipo;
        }

        public static void MostrarModal(Page page, string redirectUrl = null, string funcionJsPersonalizada = null)
        {
            ModalHelper.MostrarModalDesdeSession(
                page,
                SESSION_TITULO,
                SESSION_DESCRIPCION,
                redirectUrl,
                funcionJsPersonalizada
            );
        }

        public static void SetearYMostrar(
            Page page,
            string titulo,
            string descripcion,
            string tipo = "Resultado",
            string redirectUrl = null,
            string funcionJsPersonalizada = null)
        {
            SetearMensaje(titulo, descripcion, tipo);
            MostrarModal(page, redirectUrl, funcionJsPersonalizada);
        }
    }
}