<%@ Page Title="Detalle del Turno" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Detalle.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Turnos.Detalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">

        <div class="card shadow-sm p-5 w-100 mb-4 border-0">

            <div class="d-flex align-items-center mb-4 border-bottom pb-3">
                <div class="bg-primary bg-opacity-10 rounded-circle text-primary me-3 d-flex align-items-center justify-content-center"
                    style="width: 60px; height: 60px;">
                    <i class="bi bi-calendar-check fs-3"></i>
                </div>
                <h2 class="fs-4 fw-bold text-primary m-0">Datos Generales</h2>
            </div>

            <%-- Fila 1 --%>
            <div class="row g-3 mb-3">
                <div class="col-12 col-md-4">
                    <label class="form-label d-block">Paciente</label>
                    <asp:Label ID="lblNombrePaciente" runat="server" CssClass="fw-semibold text-dark d-block"></asp:Label>
                </div>
                <div class="col-12 col-md-4">
                    <label class="form-label d-block">Médico</label>
                    <asp:Label ID="lblNombreMedico" runat="server" CssClass="fw-semibold text-dark d-block"></asp:Label>
                </div>
                <div class="col-12 col-md-4">
                    <label class="form-label d-block">Fecha y Hora</label>
                    <asp:Label ID="lblFechaHora" runat="server" CssClass="fw-semibold text-dark d-block"></asp:Label>
                </div>
            </div>

            <%-- Fila 2 --%>
            <div class="row g-3 mb-3">
                <div class="col-12 col-md-4">
                    <label class="form-label d-block">Especialidad</label>
                    <asp:Label ID="lblEspecialidad" runat="server" CssClass="fw-semibold text-dark d-block"></asp:Label>
                </div>
                <div class="col-12 col-md-4">
                    <label class="form-label d-block">Cobertura / Plan</label>
                    <asp:Label ID="lblCoberturaPlan" runat="server" CssClass="fw-semibold text-dark d-block"></asp:Label>
                </div>
                <div class="col-12 col-md-4">
                    <label class="form-label d-block">Estado actual</label>
                    <div>
                        <asp:Label ID="lblEstado" runat="server" CssClass="badge"></asp:Label>
                    </div>
                </div>
            </div>

        </div>


        <asp:PlaceHolder ID="phDetalleClinico" runat="server" Visible="false">
            <div class="card shadow-sm p-5 w-100 mb-4 border-0">

                <div class="d-flex align-items-center mb-4 border-bottom pb-3">
                    <div class="bg-danger bg-opacity-10 rounded-circle text-danger me-3 d-flex align-items-center justify-content-center"
                        style="width: 60px; height: 60px;">
                        <i class="bi bi-clipboard-pulse fs-3"></i>
                    </div>
                    <h2 class="fs-4 fw-bold text-danger m-0">Registro Clínico de Atención</h2>
                </div>

                <div class="row g-4">
                    <div class="col-md-6">
                        <label class="form-label text-muted small mb-1">Tratamiento Realizado</label>
                        <div class="p-3 bg-light rounded border-start border-3 border-success">
                            <asp:Label ID="lblTratamiento" runat="server" CssClass="fw-bold text-dark fs-5 d-block"></asp:Label>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label text-muted small mb-1">Diagnóstico</label>
                        <div class="p-3 bg-light rounded border">
                            <asp:Label ID="lblDiagnostico" runat="server" CssClass="text-dark d-block"></asp:Label>
                        </div>
                    </div>

                    <div class="col-12">
                        <label class="form-label text-muted small mb-1">Observaciones Médicas</label>
                        <div class="p-3 bg-light rounded border">
                            <asp:Label ID="lblObservacionesClinicas" runat="server" CssClass="mb-0 fst-italic text-secondary d-block"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </asp:PlaceHolder>


        <div class="card shadow-sm p-5 w-100 border-0">


            <div class="d-flex align-items-center mb-4 border-bottom pb-3">
                <div class="bg-secondary bg-opacity-10 rounded-circle text-secondary me-3 d-flex align-items-center justify-content-center"
                    style="width: 60px; height: 60px;">
                    <i class="bi bi-journal-text fs-3"></i>
                </div>
                <h2 class="fs-4 fw-bold text-secondary m-0">Observaciones Administrativas</h2>
            </div>

            <div class="mb-4">
                <asp:Label ID="lblObservaciones" runat="server" CssClass="mb-0 text-secondary d-block"></asp:Label>
            </div>

            <div class="d-flex justify-content-end gap-2 pt-3 border-top">
                <asp:HyperLink ID="lnkVolver" runat="server" CssClass="btn btn-outline-secondary" NavigateUrl="~/Pages/Turnos/Index.aspx">
                    Volver
                </asp:HyperLink>

                <asp:Button ID="btnEditar" runat="server" Text="Editar Turno" CssClass="btn btn-primary" OnClick="btnEditar_Click" />
            </div>
        </div>

    </div>


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
