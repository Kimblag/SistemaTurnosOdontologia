<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportePacientes.ascx.cs" Inherits="SGTO.UI.Webforms.Controles.Reportes.Pacientes.ReportePacientes" %>

<div class="card shadow-sm p-4 d-flex flex-column gap-3">

    <%-- Filtros superiores --%>
    <div class="filters w-100 flex-column">
        <div class="filter-group w-100">
            <div class="row g-3 align-items-end w-100">

                <div class="col-xl-2 col-lg-3 col-md-4 col-sm-6">
                    <label for="txtFechaDesde" class="form-label fw-semibold">Desde</label>
                    <asp:TextBox ID="txtFechaDesde" runat="server"
                        CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>

                <div class="col-xl-2 col-lg-3 col-md-4 col-sm-6">
                    <label for="txtFechaHasta" class="form-label fw-semibold">Hasta</label>
                    <asp:TextBox ID="txtFechaHasta" runat="server"
                        CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>

                <div class="col-xl-3 col-lg-4 col-md-6 col-sm-12">
                    <label for="ddlCobertura" class="form-label fw-semibold">Cobertura</label>
                    <asp:DropDownList ID="ddlCobertura" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Todas" Value="" />
                        <asp:ListItem Text="Particular" Value="Particular" />
                        <asp:ListItem Text="OSDE" Value="OSDE" />
                        <asp:ListItem Text="Swiss Medical" Value="Swiss" />
                    </asp:DropDownList>
                </div>

                <div class="col-xl-3 col-lg-4 col-md-6 col-sm-12">
                    <label for="ddlPlanes" class="form-label fw-semibold">Plan</label>
                    <asp:DropDownList ID="ddlPlanes" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Todos" Value="0" />
                        <asp:ListItem Text="210" Value="210" />
                        <asp:ListItem Text="310" Value="310" />
                        <asp:ListItem Text="SMG20" Value="SMG20" />
                    </asp:DropDownList>
                </div>

                <div class="col-xl-2 col-lg-3 col-md-6 col-sm-12 d-flex justify-content-end">
                    <asp:Button ID="btnGenerarReporte" runat="server"
                        Text="Aplicar Filtros"
                        CssClass="btn btn-primary w-100 w-md-auto px-3 d-flex align-items-center justify-content-center gap-2" />
                </div>
            </div>
        </div>
    </div>




    <%--kpis--%>
    <div class="d-flex flex-wrap gap-3 justify-content-between mt-1">

        <div class="kpi-card flex-grow-1 text-center py-3 px-2">
            <h6 class="text-muted mb-1">Total Pacientes</h6>
            <p class="text-theme-primary fw-bold fs-3 mb-0">850</p>
        </div>

        <div class="kpi-card flex-grow-1 text-center py-3 px-2">
            <h6 class="text-muted mb-1">Pacientes Atendidos</h6>
            <p class="text-theme-primary fw-bold fs-3 mb-0">320</p>
        </div>

        <div class="kpi-card flex-grow-1 text-center py-3 px-2">
            <h6 class="text-muted mb-1">Nuevos en el período</h6>
            <p class="text-theme-primary fw-bold fs-3 mb-0">45</p>
        </div>

        <div class="kpi-card flex-grow-1 text-center py-3 px-2">
            <h6 class="text-muted mb-1">Con Cobertura</h6>
            <p class="text-theme-primary fw-bold fs-3 mb-0">650</p>
        </div>

        <div class="kpi-card flex-grow-1 text-center py-3 px-2">
            <h6 class="text-muted mb-1">Particulares</h6>
            <p class="text-theme-primary fw-bold fs-3 mb-0">200</p>
        </div>
    </div>

    <%-- Botones de exportación en su propia fila --%>
    <div class="d-flex justify-content-end flex-wrap gap-2 mb-3 w-100">
        <asp:Button ID="btnExportarPdf" runat="server"
            Text="Exportar a PDF"
            CssClass="btn btn-outline-danger btn-sm px-3 d-flex align-items-center gap-2" />

        <asp:Button ID="btnExportarExcel" runat="server"
            Text="Exportar a Excel"
            CssClass="btn btn-outline-success btn-sm px-3 d-flex align-items-center gap-2" />
    </div>



    <%--tabla--%>
    <div class="content-wrapper">
        <asp:GridView ID="gvPacientes"
            runat="server"
            CssClass="table gridview mb-0"
            AutoGenerateColumns="false"
            EmptyDataText="No se encontraron resultados para los filtros aplicados."
            AllowPaging="true"
            PageSize="5"
            OnPageIndexChanging="gvPacientes_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Dni" HeaderText="DNI" />
                <asp:BoundField DataField="Cobertura" HeaderText="Cobertura" />
                <asp:BoundField DataField="Plan" HeaderText="Plan" />
                <asp:BoundField DataField="TotalTurnos" HeaderText="Total de Turnos" />
                <asp:BoundField DataField="UltimaAtencion" HeaderText="Última Atención" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="MedicoMasFrecuente" HeaderText="Médico más Frecuente" />
            </Columns>

        </asp:GridView>
    </div>


</div>
