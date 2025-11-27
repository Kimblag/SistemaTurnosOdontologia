<%@ Page Title="Reporte de Tratamientos" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Tratamientos.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Reportes.Tratamientos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-generic reportes-page tratamientos-reporte">

        <small class="text-muted">Análisis de demanda y facturación estimada por tratamiento.</small>

        <%-- Bloque de filtros  --%>
        <div class="filters card shadow-sm p-4 mb-4 border-0">
            <div class="d-flex flex-wrap align-items-end justify-content-between gap-3 w-100">
                <div class="d-flex flex-wrap gap-3 flex-grow-1">

                    <%-- Filtro Estado  --%>
                    <div class="filtro">
                        <label for="ddlEstado" class="form-label small text-muted">Estado</label>
                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select" Width="200">
                            <asp:ListItem Text="Todos" Value="" />
                            <asp:ListItem Text="Activos" Value="A" Selected="True" />
                            <asp:ListItem Text="Inactivos" Value="I" />
                        </asp:DropDownList>
                    </div>

                    <div class="filtro flex-grow-1">
                        <label for="ddlEspecialidad" class="form-label small text-muted">Especialidad</label>
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
                <div class="kpi-card text-center py-3 px-2 bg-white border rounded shadow-sm h-100">
                   <h6 class="text-muted mb-1 small text-uppercase">Realizados</h6>
                    <p class="text-dark fw-bold fs-4 mb-0">
                        <asp:Label ID="lblTotalRealizados" runat="server" Text="-" />
                        <span class="fs-6 text-muted fw-normal">/
                            <asp:Label ID="lblTotalCatalogo" runat="server" Text="-" />
                            cat.</span>
                    </p>
                </div>
            </div>

            <div class="col-6 col-md-3">
                <div class="kpi-card text-center py-3 px-2 bg-white border rounded shadow-sm h-100">
                    <h6 class="text-muted mb-1 small text-uppercase">Facturación Total</h6>
                    <p class="text-primary fw-bold fs-5 mb-0">
                        <asp:Label ID="lblIngresosBrutos" runat="server" Text="-" />
                    </p>
                </div>
            </div>

            <div class="col-6 col-md-3">
                <div class="kpi-card text-center py-3 px-2 bg-white border rounded shadow-sm h-100 border-success">
                    <h6 class="text-muted mb-1 small text-uppercase">Cobertura O.S.</h6>
                    <p class="text-success fw-bold fs-5 mb-0">
                        <asp:Label ID="lblIngresosOS" runat="server" Text="-" />
                    </p>
                </div>
            </div>

            <div class="col-6 col-md-3">
                <div class="kpi-card text-center py-3 px-2 bg-white border rounded shadow-sm h-100 border-info">
                    <h6 class="text-muted mb-1 small text-uppercase">Pago Pacientes</h6>
                    <p class="text-info fw-bold fs-5 mb-0" style="color: #0dcaf0;">
                        <asp:Label ID="lblIngresosPac" runat="server" Text="-" />
                    </p>
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
            <asp:GridView ID="gvTratamientos" runat="server" CssClass="table gridview mb-0" AutoGenerateColumns="false"
                EmptyDataText="No se encontraron resultados." AllowPaging="true" PageSize="10" OnPageIndexChanging="gvTratamientos_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Tratamiento" />
                    <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                    <asp:BoundField DataField="CostoBase" HeaderText="Costo Unit." DataFormatString="{0:C}" />
                    <asp:BoundField DataField="CantidadRealizados" HeaderText="Cant." ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="TotalFacturado" HeaderText="Total Bruto" DataFormatString="{0:C}" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="TotalCobradoObraSocial" HeaderText="Cobertura (O.S.)" DataFormatString="{0:C}" ItemStyle-ForeColor="Green" />
                    <asp:BoundField DataField="TotalCobradoPaciente" HeaderText="Pago Paciente" DataFormatString="{0:C}" ItemStyle-ForeColor="Blue" />
                </Columns>
                <EmptyDataTemplate>
                    <div class="empty-state">No hay datos para mostrar.</div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>

    <%-- Modal Error --%>
    <div class="modal fade" id="modalResultado" tabindex="-1">
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
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
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
