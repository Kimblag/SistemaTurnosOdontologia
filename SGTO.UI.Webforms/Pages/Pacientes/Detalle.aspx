<%@ Page Title="Legajo del Paciente" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Detalle.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Pacientes.Detalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">
        <div class="card shadow-sm p-5 w-100">

            <%--datos personales--%>
            <div>
                <div class="row mb-3">
                    <h2 class="fs-4">Datos personales</h2>
                </div>

                <div class="row g-3 mb-3">
                    <div class="col-12 col-md-4">
                        <label for="lblNombreCompleto" class="form-label d-block">Nombre completo</label>
                        <asp:Label ID="lblNombreCompleto" runat="server" CssClass="fw-semibold text-dark d-block" Text="Juan Pérez"></asp:Label>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="lblDni" class="form-label d-block">DNI</label>
                        <asp:Label ID="lblDni" runat="server" Text="12345678" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="lblFechaNacimiento" class="form-label d-block">Fecha de Nacimiento</label>
                        <asp:Label ID="lblFechaNacimiento" runat="server" Text="15/05/1987" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                </div>

                <div class="row g-3 mb-3">
                    <div class="col-12 col-md-4">
                        <label for="lblGenero" class="form-label d-block">Género</label>
                        <asp:Label ID="lblGenero" runat="server" Text="Masculino" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="lblTelefono" class="form-label d-block">Teléfono</label>
                        <asp:Label ID="lblTelefono" runat="server" Text="1134710523" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="lblEmail" class="form-label d-block">Email</label>
                        <asp:Label ID="lblEmail" runat="server" Text="juanperez@gmail.com" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                </div>

                <div class="row g-3 mb-3">
                    <div class="col-12 col-md-4">
                        <label for="lblCobertura" class="form-label d-block">Cobertura</label>
                        <asp:Label ID="lblCobertura" runat="server" Text="OSDE" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="lblPlan" class="form-label d-block">Plan</label>
                        <asp:Label ID="lblPlan" runat="server" Text="310" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="lblEstado" class="form-label d-block">Estado</label>
                        <asp:Label ID="lblEstado" runat="server" CssClass="badge badge-success">Activo</asp:Label>
                    </div>
                </div>

            </div>

            <hr />

            <div class="card shadow-sm border-0 mb-4">
                <div class="card-header bg-white border-bottom py-3 d-flex justify-content-between align-items-center">
                    <h4 class="fs-5 fw-bold text-dark m-0">
                        <i class="bi bi-clipboard-pulse me-2 text-danger"></i>Historia Clínica (Atenciones)
                    </h4>

                    <asp:LinkButton ID="btnExportarHistoria" runat="server"
                        CssClass="btn btn-outline-danger btn-sm fw-bold"
                        OnClick="btnExportarHistoria_Click"
                        ToolTip="Descargar historial completo en PDF">
                        <i class="bi bi-file-earmark-pdf me-2"></i>Descargar PDF
                    </asp:LinkButton>
                </div>
                <div class="card-body p-0">
                    <asp:GridView ID="gvHistoriaClinica" runat="server" AutoGenerateColumns="False"
                        CssClass="table table-hover align-middle mb-0" GridLines="None"
                        AllowPaging="True" PageSize="5"
                        OnPageIndexChanging="gvHistoriaClinica_PageIndexChanging">

                        <HeaderStyle CssClass="table-light text-secondary small text-uppercase" />

                        <Columns>
                            <%-- Fecha --%>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />

                            <%-- Tratamiento --%>
                            <asp:TemplateField HeaderText="Tratamiento">
                                <ItemTemplate>
                                    <span class="fw-bold text-dark"><%# Eval("Tratamiento") %></span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- Diagnóstico --%>
                            <asp:BoundField DataField="Diagnostico" HeaderText="Diagnóstico" />

                            <asp:TemplateField HeaderText="Profesional">
                                <ItemTemplate>
                                    <div class="d-flex flex-column">
                                        <span class="fw-medium"><%# Eval("Profesional") %></span>
                                        <small class="text-muted"><%# Eval("Especialidad") %></small>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-end">
                                <ItemTemplate>
                                    <a href='<%# ResolveUrl("~/Pages/Turnos/Detalle.aspx?id-turno=" + Eval("IdTurno")) %>'
                                        class="btn btn-sm btn-outline-primary" title="Ver detalle del turno">
                                        <i class="bi bi-eye me-1"></i>Ver Turno
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                        <EmptyDataTemplate>
                            <div class="text-center p-5 text-muted">
                                <i class="bi bi-journal-medical fs-1 d-block mb-2"></i>
                                <p class="mb-0">Este paciente no posee registros clínicos de atenciones finalizadas.</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>

            <hr />
            <%--historial de turnos del paciente--%>
            <div class="card shadow-sm border-0">
                <div class="card-header bg-white border-bottom py-3">
                    <h4 class="fs-5 fw-bold text-secondary m-0">
                        <i class="bi bi-calendar-week me-2"></i>Historial de Turnos (Agenda)
                </h4>
                </div>
                <div class="card-body p-0">
                    <asp:GridView ID="gvTurnosPaciente" runat="server" AutoGenerateColumns="False"
                        CssClass="table table-hover align-middle mb-0" GridLines="None"
                        AllowPaging="True" PageSize="10"
                        OnPageIndexChanging="gvTurnosPaciente_PageIndexChanging"
                        OnRowDataBound="gvTurnosPaciente_RowDataBound">

                        <HeaderStyle CssClass="table-light text-secondary small text-uppercase" />

                        <Columns>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                            <asp:BoundField DataField="Hora" HeaderText="Hora" />
                            <asp:BoundField DataField="Medico" HeaderText="Médico" />
                            <asp:BoundField DataField="Observaciones" HeaderText="Obs. Administrativas" />

                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <div id="divEstadoTurno" runat="server" class="badge">
                                        <%# Eval("Estado") %>
                                    </div>
                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="" ItemStyle-CssClass="text-end">
                                <ItemTemplate>
                                    <a href='<%# ResolveUrl("~/Pages/Turnos/Detalle.aspx?id-turno=" + Eval("IdTurnoPaciente")) %>'
                                        class="btn btn-sm btn-outline-primary" title="Ver detalle del turno">
                                        <i class="bi bi-eye me-1"></i>Ver Turno
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                        <EmptyDataTemplate>
                            <div class="text-center p-5 text-muted">
                                <i class="bi bi-calendar-x fs-1 d-block mb-2"></i>
                                <p>No hay turnos registrados en la agenda.</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>

        </div>
    </div>


    <%-- modal resultado --%>
    <div class="modal fade" id="modalResultado" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 id="modalResultadoTitulo" class="modal-title">Resultado</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <p id="modalResultadoDesc"></p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" id="btnModalCerrar" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>


    <script>

        document.addEventListener("DOMContentLoaded", () => {

            // modal resultado
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
