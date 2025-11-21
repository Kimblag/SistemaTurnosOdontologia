<%@ Page Title="Reporte de Médicos" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Medicos.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Reportes.Medicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-generic reportes-page medicos-reporte">
        
        <small class="text-muted">Métricas basadas en turnos asignados y realizados.</small>
        
        <%-- Filtros --%>
        <div class="filters card shadow-sm p-4 mb-4 border-0">
            <div class="d-flex flex-wrap align-items-end justify-content-between gap-3 w-100">
                <div class="d-flex flex-wrap gap-3 flex-grow-1">
                    <div class="filtro">
                        <label for="txtFechaDesde" class="form-label fw-semibold">Desde</label>
                        <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="filtro">
                        <label for="txtFechaHasta" class="form-label fw-semibold">Hasta</label>
                        <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="filtro flex-grow-1">
                        <label for="ddlEspecialidad" class="form-label fw-semibold">Especialidad</label>
                        <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select w-100"></asp:DropDownList>
                    </div>
                </div>
                <div class="d-flex align-items-end gap-2">
                    <asp:Button ID="btnAplicarFiltros" runat="server" Text="Ejecutar" CssClass="btn btn-primary px-3" OnClick="btnAplicarFiltros_Click" />
                    <asp:Button ID="btnLimpiarFiltros" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary px-3" OnClick="btnLimpiarFiltros_Click" />
                </div>
            </div>
        </div>

        <%-- KPIs --%>
        <div class="row g-3 mb-4">
            <div class="col-6 col-md-3">
                <div class="kpi-card text-center py-3 bg-light border rounded shadow-sm">
                    <h6 class="text-muted mb-1">Total Médicos</h6>
                    <p class="text-theme-primary fw-bold fs-4 mb-0"><asp:Label ID="lblTotalMedicos" runat="server" Text="-" /></p>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="kpi-card text-center py-3 bg-light border rounded shadow-sm">
                    <h6 class="text-muted mb-1">Activos</h6>
                    <p class="text-theme-primary fw-bold fs-4 mb-0"><asp:Label ID="lblActivos" runat="server" Text="-" /></p>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="kpi-card text-center py-3 bg-light border rounded shadow-sm">
                    <h6 class="text-muted mb-1">Turnos en Período</h6>
                    <p class="text-theme-primary fw-bold fs-4 mb-0"><asp:Label ID="lblTotalTurnos" runat="server" Text="-" /></p>
                </div>
            </div>
            <div class="col-6 col-md-3">
                <div class="kpi-card text-center py-3 bg-light border rounded shadow-sm">
                    <h6 class="text-muted mb-1">Especialidades</h6>
                    <p class="text-theme-primary fw-bold fs-4 mb-0"><asp:Label ID="lblEspecialidades" runat="server" Text="-" /></p>
                </div>
            </div>
        </div>

        <%-- Botones Exportar --%>
        <div class="d-flex justify-content-end flex-wrap gap-2 mb-3 w-100">
            <asp:Button ID="btnExportarPdf" runat="server" Text="Exportar PDF" CssClass="btn btn-outline-danger btn-sm px-3" OnClick="btnExportarPdf_Click" />
            <asp:Button ID="btnExportarExcel" runat="server" Text="Exportar Excel" CssClass="btn btn-outline-success btn-sm px-3" OnClick="btnExportarExcel_Click" />
        </div>

        <%-- GridView --%>
        <div class="content-wrapper">
            <asp:GridView ID="gvMedicos" runat="server" CssClass="table gridview mb-0" AutoGenerateColumns="false" 
                EmptyDataText="No se encontraron resultados." AllowPaging="true" PageSize="10" OnPageIndexChanging="gvMedicos_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="Matricula" HeaderText="Matrícula" />
                    <asp:BoundField DataField="NombreCompleto" HeaderText="Médico" />
                    <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                    <asp:BoundField DataField="TotalTurnos" HeaderText="Turnos Totales" />
                    <asp:BoundField DataField="PacientesAtendidos" HeaderText="Pacientes Únicos" />
                    <asp:BoundField DataField="UltimoTurno" HeaderText="Último Turno" DataFormatString="{0:dd/MM/yyyy}" />
                </Columns>
                <EmptyDataTemplate>
                    <div class="empty-state">No hay datos para mostrar.</div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
    
    <%-- Modal Error --%>
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
            window.abrirModalResultado = function (titulo, descripcion) {
                document.getElementById('modalResultadoTitulo').textContent = titulo || "Resultado";
                document.getElementById('modalResultadoDesc').textContent = descripcion || "";
                new bootstrap.Modal(document.getElementById('modalResultado')).show();
            };
        });
    </script>
</asp:Content>