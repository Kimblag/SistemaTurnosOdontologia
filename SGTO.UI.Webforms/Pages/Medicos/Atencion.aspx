<%@ Page Title="Atención de Paciente" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Atencion.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Medicos.Atencion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">

        <div class="row g-4">

            <div class="col-md-4">
                <div class="card h-100 shadow-sm border-0">

                    <div class="card-header bg-white border-bottom-0 pt-4 pb-0">
                        <div class="d-flex align-items-center">
                            <div class="bg-primary bg-opacity-10 p-3 rounded-circle text-primary me-3 d-flex align-items-center justify-content-center" style="width: 60px; height: 60px;">
                                <i class="bi bi-person-vcard fs-3"></i>
                            </div>
                            <h5 class="mb-0 fw-bold text-primary">Datos del Turno</h5>
                        </div>
                    </div>

                    <div class="card-body">
                        <asp:HiddenField ID="hdnIdTurno" runat="server" />

                        <div class="mb-3">
                            <label class="small text-muted text-uppercase fw-bold">Paciente</label>
                            <asp:TextBox ID="txtPacienteNombre" runat="server" CssClass="form-control-plaintext lead fw-normal p-0" ReadOnly="true" />
                        </div>

                        <div class="mb-3">
                            <label class="small text-muted text-uppercase fw-bold">Especialidad</label>
                            <asp:TextBox ID="txtEspecialidad" runat="server" CssClass="form-control-plaintext p-0" ReadOnly="true" />
                        </div>

                        <div class="row mb-3">
                            <div class="col-6">
                                <label class="small text-muted text-uppercase fw-bold">Fecha</label>
                                <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control-plaintext p-0 fw-bold" ReadOnly="true" />
                            </div>
                            <div class="col-6">
                                <label class="small text-muted text-uppercase fw-bold">Hora</label>
                                <asp:TextBox ID="txtHora" runat="server" CssClass="form-control-plaintext p-0 fw-bold" ReadOnly="true" />
                            </div>
                        </div>

                        <div class="mb-0">
                            <label class="small text-muted text-uppercase fw-bold">Cobertura / Plan</label>
                            <asp:TextBox ID="txtCobertura" runat="server" CssClass="form-control-plaintext p-0" ReadOnly="true" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="card h-100 shadow-sm border-0">

                    <div class="card-header bg-white border-bottom-0 pt-4 pb-0">
                        <div class="d-flex align-items-center">
                            <div class="bg-success bg-opacity-10 p-3 rounded-circle text-success me-3 d-flex align-items-center justify-content-center" style="width: 60px; height: 60px;">
                                <i class="bi bi-journal-medical fs-3"></i>
                            </div>
                            <h5 class="mb-0 fw-bold text-success">Registro Clínico</h5>
                        </div>
                    </div>

                    <div class="card-body pt-4">

                        <div class="mb-4">
                            <label class="small text-muted text-uppercase fw-bold mb-2">Tratamiento Realizado <span class="text-danger">*</span></label>

                            <asp:Panel ID="pnlTratamientoSeleccion" runat="server" Visible="false">
                                <div class="input-group">
                                    <span class="input-group-text bg-light border-end-0"><i class="bi bi-clipboard-check"></i></span>
                                    <asp:DropDownList ID="ddlTratamiento" runat="server" CssClass="form-select border-start-0"></asp:DropDownList>
                                </div>
                                <div class="form-text ms-1">Seleccione de la lista predefinida.</div>
                            </asp:Panel>

                            <asp:Panel ID="pnlTratamientoManual" runat="server" Visible="false">
                                <div class="input-group">
                                    <span class="input-group-text bg-light border-end-0"><i class="bi bi-pencil-square"></i></span>
                                    <asp:TextBox ID="txtTratamientoManual" runat="server" CssClass="form-control border-start-0" placeholder="Especifique el tratamiento realizado..."></asp:TextBox>
                                </div>
                                <div class="form-text text-warning ms-1"><i class="bi bi-info-circle"></i>No hay tratamientos predefinidos. Ingrese manualmente.</div>
                            </asp:Panel>
                        </div>

                        <div class="mb-4">
                            <label for="txtDiagnostico" class="small text-muted text-uppercase fw-bold mb-2">Diagnóstico <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtDiagnostico" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" placeholder="Ingrese el diagnóstico clínico detallado..." />
                        </div>

                        <div class="mb-4">
                            <label for="txtObservaciones" class="small text-muted text-uppercase fw-bold mb-2">Observaciones / Notas</label>
                            <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" placeholder="Detalles del procedimiento, medicación recetada, recomendaciones, etc." />
                        </div>

                        <div class="alert alert-warning d-flex align-items-center border-0 bg-warning bg-opacity-10 text-warning-emphasis" role="alert">
                            <i class="bi bi-exclamation-triangle-fill me-3 fs-4"></i>
                            <div>
                                <strong>Atención:</strong> Al guardar, se generará la historia clínica y el turno quedará <strong>Cerrado</strong> definitivamente.
                           
                            </div>
                        </div>

                    </div>

                    <div class="card-footer bg-white border-top-0 d-flex justify-content-end gap-2 pb-4 pe-4">
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-outline-secondary px-4" OnClick="btnCancelar_Click" />
                        <asp:Button ID="btnGuardar" runat="server" Text="Finalizar Atención" CssClass="btn btn-success px-4" OnClick="btnGuardar_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%-- Modal resultado--%>
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
        function abrirModalResultado(titulo, descripcion) {
            document.getElementById('modalResultadoTitulo').textContent = titulo;
            document.getElementById('modalResultadoDesc').textContent = descripcion;
            new bootstrap.Modal(document.getElementById('modalResultado')).show();
        }
    </script>
</asp:Content>
