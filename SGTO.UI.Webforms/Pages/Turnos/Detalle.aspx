<%@ Page Title="Detalle del Turno" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Detalle.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Turnos.Detalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">



    <div class="container-fluid">

        <div class="card shadow-sm p-4 w-100 mb-4 border-0">

            <div>
                <div class="row mb-3">
                    <h2 class="fs-4">Datos personales</h2>
                </div>
                <div class="row g-3 mb-3">
                    <div class="col-12 col-md-4">
                        <label for="lblNombrePaciente" class="form-label d-block">Paciente</label>
                        <asp:Label ID="lblNombrePaciente" runat="server" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="lblNombreMedico" class="form-label d-block">Médico</label>
                        <asp:Label ID="lblNombreMedico" runat="server" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="lblFechaHora" class="form-label d-block">Fecha y Hora</label>
                        <asp:Label ID="lblFechaHora" runat="server" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                </div>


                <div class="row g-3">
                    <div class="col-12 col-md-4">
                        <label for="lblEspecialidad" class="form-label d-block">Especialidad</label>
                        <asp:Label ID="lblEspecialidad" runat="server" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="lblCoberturaPlan" class="form-label d-block">Cobertura / Plan</label>
                        <asp:Label ID="lblCoberturaPlan" runat="server" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="lblEstado" class="form-label d-block">Estado actual</label>
                        <div>
                            <asp:Label ID="lblEstado" runat="server" CssClass="badge"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:PlaceHolder ID="phDetalleClinico" runat="server" Visible="false">
            <div class="card shadow-sm border-0 mb-4">
                <div class="card-header bg-white border-bottom py-3">
                    <h4 class="fs-5 fw-bold text-dark m-0">
                        <i class="bi bi-clipboard-pulse me-2 text-danger"></i>Historia Clínica (Registro Médico)
                    </h4>
                </div>
                <div class="card-body p-4">
                    <div class="row g-4">
                        <div class="col-md-6">
                            <label class="form-label text-muted small mb-1">Tratamiento Realizado</label>
                            <div class="p-3 bg-light rounded border-start border-3 border-success">
                                <span class="fw-bold text-dark fs-5">
                                    <asp:Literal ID="litTratamiento" runat="server"></asp:Literal>
                                </span>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <label class="form-label text-muted small mb-1">Diagnóstico</label>
                            <div class="p-3 bg-light rounded border">
                                <span class="text-dark">
                                    <asp:Literal ID="litDiagnostico" runat="server"></asp:Literal>
                                </span>
                            </div>
                        </div>

                        <div class="col-12">
                            <label class="form-label text-muted small mb-1">Observaciones Médicas</label>
                            <div class="p-3 bg-light rounded border">
                                <p class="mb-0 fst-italic text-secondary">
                                    <asp:Literal ID="litObservacionesClinicas" runat="server"></asp:Literal>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:PlaceHolder>

        <div class="card shadow-sm border-0">
            <div class="card-header bg-white border-bottom py-3">
                <h4 class="fs-5 fw-bold text-secondary m-0">
                    <i class="bi bi-journal-text me-2"></i>Observaciones Administrativas
                </h4>
            </div>
            <div class="card-body p-4">
                <p class="mb-0 text-secondary">
                    <asp:Literal ID="litObservaciones" runat="server"></asp:Literal>
                </p>
            </div>

            <div class="card-footer bg-white border-top py-3 d-flex justify-content-end gap-2">
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
