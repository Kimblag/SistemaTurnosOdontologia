<%@ Page Title="Reporte de Coberturas" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Coberturas.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Reportes.Coberturas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic reportes-page">

        <%-- Título --%>
        <div class="mb-3">
            <h4>Reporte de Coberturas y Planes</h4>
            <small class="text-muted">Catálogo y rendimiento histórico.</small>
        </div>

        <%-- Filtros --%>
        <div class="filters card shadow-sm p-4 mb-4 border-0">
            <div class="d-flex flex-wrap align-items-end justify-content-between gap-3 w-100">

                <div class="d-flex flex-wrap gap-3 flex-grow-1">

                    <%-- Estado --%>
                    <div class="filtro">
                        <label for="ddlEstado" class="form-label fw-semibold">Estado</label>
                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Todos" Value="" />
                            <asp:ListItem Text="Activos" Value="A" Selected="True" />
                            <asp:ListItem Text="Inactivos" Value="I" />
                        </asp:DropDownList>
                    </div>

                    <%-- Orden --%>
                    <div class="filtro">
                        <label for="ddlOrden" class="form-label fw-semibold">Orden (% Cob)</label>
                        <asp:DropDownList ID="ddlOrden" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Nombre (A-Z)" Value="nombre" Selected="True" />
                            <asp:ListItem Text="Mayor % primero" Value="mayor" />
                            <asp:ListItem Text="Menor % primero" Value="menor" />
                        </asp:DropDownList>
                    </div>

                    <%-- Cobertura --%>
                    <div class="filtro flex-grow-1">
                        <label for="ddlCoberturaFiltro" class="form-label fw-semibold">Cobertura</label>
                        <asp:DropDownList ID="ddlCoberturaFiltro" runat="server" CssClass="form-select w-100" />
                    </div>

                </div>

                <div class="d-flex align-items-end gap-2">
                    <asp:Button ID="btnEjecutar" runat="server"
                        Text="Ejecutar"
                        CssClass="btn btn-primary px-3"
                        OnClick="btnEjecutar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server"
                        Text="Limpiar"
                        CssClass="btn btn-outline-secondary px-3"
                        OnClick="btnLimpiar_Click" />
                </div>

            </div>
        </div>

        <%-- Tabs --%>
        <ul class="nav nav-tabs mb-3">
            <li class="nav-item">
                <asp:LinkButton ID="tabCoberturas" runat="server"
                    CssClass="nav-link active"
                    OnClick="tabCoberturas_Click">
                    <i class="bi bi-building"></i> Coberturas
                </asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="tabPlanes" runat="server"
                    CssClass="nav-link"
                    OnClick="tabPlanes_Click">
                    <i class="bi bi-list-check"></i> Planes
                </asp:LinkButton>
            </li>
        </ul>

        <%-- Vistas --%>
        <asp:MultiView ID="mvReportes" runat="server" ActiveViewIndex="0">

            <%-- VISTA 1: COBERTURAS --%>
            <asp:View ID="vCoberturas" runat="server">
                <div class="card border-0 shadow-sm">
                    <div class="card-body p-0 table-responsive">
                        <asp:GridView ID="gvCoberturas" runat="server"
                            CssClass="table table-hover gridview mb-0"
                            AutoGenerateColumns="false"
                            EmptyDataText="Sin datos."
                            GridLines="None">

                            <Columns>
                                <asp:BoundField DataField="Cobertura" HeaderText="Obra Social" />

                                <asp:BoundField DataField="Estado" HeaderText="Estado"
                                    HeaderStyle-CssClass="text-center"
                                    ItemStyle-CssClass="text-center" />

                                <asp:BoundField DataField="CantidadPlanes" HeaderText="Planes"
                                    HeaderStyle-CssClass="text-center"
                                    ItemStyle-CssClass="text-center" />

                                <asp:BoundField DataField="TotalTurnos" HeaderText="Total Turnos"
                                    HeaderStyle-CssClass="text-center"
                                    ItemStyle-CssClass="text-center" />

                                <asp:BoundField DataField="PacientesAtendidos" HeaderText="Pacientes"
                                    HeaderStyle-CssClass="text-center"
                                    ItemStyle-CssClass="text-center" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:View>

            <%-- VISTA 2: PLANES --%>
            <asp:View ID="vPlanes" runat="server">
                <div class="card border-0 shadow-sm">
                    <div class="card-body p-0 table-responsive">
                        <asp:GridView ID="gvPlanes" runat="server"
                            CssClass="table table-hover gridview mb-0"
                            AutoGenerateColumns="false"
                            EmptyDataText="Sin datos."
                            GridLines="None">

                            <Columns>
                                <asp:BoundField DataField="Cobertura" HeaderText="Obra Social" />

                                <asp:BoundField DataField="Plan" HeaderText="Plan" />

                                <asp:BoundField DataField="Estado" HeaderText="Estado"
                                    HeaderStyle-CssClass="text-center"
                                    ItemStyle-CssClass="text-center" />

                                <asp:BoundField DataField="PorcentajeCubierto"
                                    HeaderText="% Cobertura"
                                    DataFormatString="{0:N0}%"
                                    HeaderStyle-CssClass="text-end"
                                    ItemStyle-CssClass="text-end" />

                                <asp:BoundField DataField="TotalTurnos" HeaderText="Total Turnos"
                                    HeaderStyle-CssClass="text-center"
                                    ItemStyle-CssClass="text-center" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:View>

        </asp:MultiView>

        <%-- Exportar --%>
        <div class="mt-3 text-end">
            <asp:Button ID="btnExportarPdf" runat="server"
                Text="Exportar PDF"
                CssClass="btn btn-danger btn-sm"
                OnClick="btnExportarPdf_Click" />
            <asp:Button ID="btnExportarExcel" runat="server"
                Text="Exportar Excel"
                CssClass="btn btn-success btn-sm"
                OnClick="btnExportarExcel_Click" />
        </div>

        <%-- Modal Mensajes --%>
        <div class="modal fade" id="modalResultado" tabindex="-1">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 id="modalResultadoTitulo" class="modal-title">Mensaje</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body">
                        <p id="modalResultadoDesc"></p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>

        <script>
            document.addEventListener("DOMContentLoaded", () => {
                window.abrirModalResultado = function (titulo, descripcion) {
                    document.getElementById('modalResultadoTitulo').textContent = titulo || "Información";
                    document.getElementById('modalResultadoDesc').textContent = descripcion || "";
                    new bootstrap.Modal(document.getElementById('modalResultado')).show();
                };
            });
        </script>

    </div>

</asp:Content>
