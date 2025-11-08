<%@ Page Title="Editar Paciente" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Editar.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Pacientes.Editar" %>

<%--registrar el user control--%>
<%@ Register Src="~/Controles/Pacientes/PacienteForm.ascx" TagPrefix="uc1" TagName="PacienteForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <uc1:PacienteForm ID="PacienteFormControl" runat="server" />


    <%-- modal resultado de la ejecucion del servicio --%>
    <div class="modal fade" id="modalResultado" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 id="modalResultadoTitulo" class="modal-title">Acción completada</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <p id="modalResultadoDesc"></p>
                </div>
                <div class="modal-footer">
                    <button id="btnModalCerrar" type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>


    <script>

        document.addEventListener("DOMContentLoaded", () => {
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
