<%@ Page Title="Gestión de Plan" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EditarPlan.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.CoberturasPlanes.EditarPlan" %>

<%--registrar el user control--%>
<%@ Register Src="~/Controles/Coberturas/PlanesForm.ascx" TagPrefix="uc1" TagName="PlanesForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:PlanesForm ID="PlanesFormControl" runat="server" />

    <%-- modal resultado de la ejecucion del servicio --%>
     <div class="modal fade" id="modalResultado" tabindex="-1" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0 shadow">
              <div class="modal-header bg-white border-bottom-0">
                  <h5 id="modalResultadoTitulo" class="modal-title fw-bold">Mensaje</h5>
                  <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
              </div>
              <div class="modal-body text-center py-4">
                  <div class="mb-3">
                      <i class="bi bi-info-circle text-primary" style="font-size: 3rem;"></i>
                  </div>
                  <p id="modalResultadoDesc" class="lead fs-6"></p>
              </div>
              <div class="modal-footer border-top-0 justify-content-center pb-4">
                  <button id="btnModalCerrar" type="button" class="btn btn-primary px-4" data-bs-dismiss="modal">Aceptar</button>
              </div>
          </div>
      </div>
  </div>


    <script>

        document.addEventListener("DOMContentLoaded", () => {
            console.log("NuevoPlan")
            window.abrirModalResultado = function (titulo, descripcion) {
                try {
                    document.getElementById('modalResultadoTitulo').textContent = titulo || "Resultado";
                    document.getElementById('modalResultadoDesc').textContent = descripcion || "";
                    new bootstrap.Modal(document.getElementById('modalResultado')).show();
                } catch (err) {
                    console.error("Error al abrir modal de resultado:", err);
                }
            };
        });
    </script>
</asp:Content>
