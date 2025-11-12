<%@ Page Title="Reportes - Pacientes" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Pacientes.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Reportes.Pacientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic reportes-page pacientes-reporte">

        <%--Filtros--%>
        <small class="text-muted">Las fechas aplican sobre la última atención registrada de cada paciente.</small>
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
                        <label for="ddlCobertura" class="form-label fw-semibold">Cobertura</label>
                        <asp:DropDownList ID="ddlCobertura" runat="server" CssClass="form-select w-100"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlCobertura_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>

                    <div class="filtro flex-grow-1">
                        <label for="ddlPlan" class="form-label fw-semibold">Plan</label>
                        <asp:DropDownList ID="ddlPlan" runat="server" CssClass="form-select w-100"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlPlan_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="d-flex align-items-end gap-2">
                    <asp:Button ID="btnAplicarFiltros" runat="server" Text="Ejecutar"
                        CssClass="btn btn-primary d-flex align-items-center gap-2 px-3"
                        OnClick="btnAplicarFiltros_Click" />
                    <asp:Button ID="btnLimpiarFiltros" runat="server" Text="Limpiar"
                        CssClass="btn btn-outline-secondary d-flex align-items-center gap-2 px-3"
                        OnClick="btnLimpiarFiltros_Click" />
                </div>

            </div>
        </div>




        <%--KPIs--%>
        <div class="row g-3 mb-4">

            <div class="col-6 col-md-4 col-lg">
                <div class="kpi-card text-center py-3 px-2 bg-light border rounded shadow-sm">
                    <h6 class="text-muted mb-1">Total Pacientes</h6>
                    <p class="text-theme-primary fw-bold fs-4 mb-0">
                        <asp:Label ID="lblTotalPacientes" runat="server" Text="-" />
                    </p>
                </div>
            </div>

            <div class="col-6 col-md-4 col-lg">
                <div class="kpi-card text-center py-3 px-2 bg-light border rounded shadow-sm">
                    <h6 class="text-muted mb-1">Atendidos</h6>
                    <p class="text-theme-primary fw-bold fs-4 mb-0">
                        <asp:Label ID="lblAtendidos" runat="server" Text="-" />
                    </p>
                </div>
            </div>

            <div class="col-6 col-md-4 col-lg">
                <div class="kpi-card text-center py-3 px-2 bg-light border rounded shadow-sm">
                    <h6 class="text-muted mb-1">Nuevos en período</h6>
                    <p class="text-theme-primary fw-bold fs-4 mb-0">
                        <asp:Label ID="lblNuevos" runat="server" Text="-" />
                    </p>
                </div>
            </div>

            <div class="col-6 col-md-4 col-lg">
                <div class="kpi-card text-center py-3 px-2 bg-light border rounded shadow-sm">
                    <h6 class="text-muted mb-1">Con Cobertura</h6>
                    <p class="text-theme-primary fw-bold fs-4 mb-0">
                        <asp:Label ID="lblConCobertura" runat="server" Text="-" />
                    </p>
                </div>
            </div>

            <div class="col-6 col-md-4 col-lg">
                <div class="kpi-card text-center py-3 px-2 bg-light border rounded shadow-sm">
                    <h6 class="text-muted mb-1">Particulares</h6>
                    <p class="text-theme-primary fw-bold fs-4 mb-0">
                        <asp:Label ID="lblParticulares" runat="server" Text="-" />
                    </p>
                </div>
            </div>

        </div>

        <%--botones de exportación--%>
        <div class="d-flex justify-content-end flex-wrap gap-2 mb-3 w-100">
            <asp:Button ID="btnExportarPdf" runat="server"
                Text="Exportar PDF"
                CssClass="btn btn-outline-danger btn-sm px-3 d-flex align-items-center gap-2"
                OnClick="btnExportarPdf_Click" />

            <asp:Button ID="btnExportarExcel" runat="server"
                Text="Exportar Excel"
                CssClass="btn btn-outline-success btn-sm px-3 d-flex align-items-center gap-2"
                OnClick="btnExportarExcel_Click" />
        </div>

        <%--Tabla--%>
        <div class="content-wrapper">
            <asp:GridView ID="gvPacientes"
                runat="server"
                CssClass="table gridview mb-0"
                AutoGenerateColumns="false"
                EmptyDataText="No se encontraron resultados para los filtros aplicados."
                AllowPaging="true"
                PageSize="8"
                OnPageIndexChanging="gvPacientes_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre" />
                    <asp:BoundField DataField="NumeroDocumento" HeaderText="DNI" />
                    <asp:BoundField DataField="Cobertura" HeaderText="Cobertura" />
                    <asp:BoundField DataField="Plan" HeaderText="Plan" />
                    <asp:BoundField DataField="TotalTurnos" HeaderText="Total Turnos" />
                    <asp:BoundField DataField="UltimaAtencion" HeaderText="Última Atención" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="MedicoFrecuente" HeaderText="Médico más Frecuente" />
                </Columns>

                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="bi bi-shield-check fs-5"></i>
                        No hay datos para mostrar.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
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
