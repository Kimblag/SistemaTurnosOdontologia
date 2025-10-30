<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReporteTurnos.ascx.cs" Inherits="SGTO.UI.Webforms.Controles.Reportes.Turnos.ReporteTurnos" %>

<div class="d-flex flex-column gap-3 pt-2">

    <%-- Filtros superiores --%>
    <div class="filters w-100 flex-column">
        <div class="filter-group w-100">
            <div class="row g-3 align-items-end w-100">

                <div class="col-xl-2 col-lg-3 col-md-4 col-sm-6">
                    <label for="txtFechaDesde" class="form-label fw-semibold">Desde</label>
                    <asp:TextBox ID="txtFechaDesde" runat="server" TextMode="Date"
                        CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-xl-2 col-lg-3 col-md-4 col-sm-6">
                    <label for="txtFechaHasta" class="form-label fw-semibold">Hasta</label>
                    <asp:TextBox ID="txtFechaHasta" runat="server" TextMode="Date"
                        CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-xl-2 col-lg-3 col-md-4 col-sm-6">
                    <label for="ddlMedico" class="form-label fw-semibold">Médico</label>
                    <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select w-100">
                        <asp:ListItem Text="Todos" Value="0" />
                        <asp:ListItem Text="Dr. Juan Pérez" Value="1" />
                        <asp:ListItem Text="Dra. Ana Gómez" Value="2" />
                    </asp:DropDownList>
                </div>

                <div class="col-xl-2 col-lg-3 col-md-4 col-sm-6">
                    <label for="ddlEstado" class="form-label fw-semibold">Estado</label>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select w-100">
                        <asp:ListItem Text="Todos" Value="" />
                        <asp:ListItem Text="Atendido" Value="Atendido" />
                        <asp:ListItem Text="Cancelado" Value="Cancelado" />
                        <asp:ListItem Text="No Asistió" Value="NoAsistio" />
                        <asp:ListItem Text="Reprogramado" Value="Reprogramado" />
                    </asp:DropDownList>
                </div>

                <div class="col-xl-3 col-lg-3 col-md-4 col-sm-6">
                    <label for="ddlCobertura" class="form-label fw-semibold">Cobertura</label>
                    <asp:DropDownList ID="ddlCobertura" runat="server" CssClass="form-select w-100">
                        <asp:ListItem Text="Todas" Value="" />
                        <asp:ListItem Text="OSDE" Value="OSDE" />
                        <asp:ListItem Text="Swiss Medical" Value="Swiss" />
                    </asp:DropDownList>
                </div>

                <div class="col-xl-1 col-lg-3 col-md-4 col-sm-6 d-flex justify-content-end">
                    <asp:Button ID="btnGenerarReporte" runat="server"
                        Text="Aplicar filtros"
                        CssClass="btn btn-primary btn-sm w-100 w-md-auto" />
                </div>

            </div>
        </div>
    </div>

    <%-- Botones de exportación --%>
    <div class="d-flex justify-content-end flex-wrap gap-2 mb-3 w-100">
        <asp:Button ID="Button1" runat="server"
            Text="Exportar a PDF"
            CssClass="btn btn-outline-danger btn-sm px-3 d-flex align-items-center gap-2" />

        <asp:Button ID="Button2" runat="server"
            Text="Exportar a Excel"
            CssClass="btn btn-outline-success btn-sm px-3 d-flex align-items-center gap-2" />
    </div>

    <%--Tabla de resultados --%>
    <div class="content-wrapper">
        <asp:GridView ID="gvTurnos" runat="server"
            AutoGenerateColumns="false"
            CssClass="table gridview mb-0"
            AllowPaging="true" PageSize="6"
            OnPageIndexChanging="gvTurnos_PageIndexChanging"
            EmptyDataText="No se encontraron resultados para los filtros aplicados.">

            <Columns>
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                <asp:BoundField DataField="Paciente" HeaderText="Paciente" />
                <asp:BoundField DataField="Medico" HeaderText="Médico" />
                <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                <asp:BoundField DataField="Cobertura" HeaderText="Cobertura" />
                <asp:BoundField DataField="Estado" HeaderText="Estado" />
            </Columns>
        </asp:GridView>
    </div>


</div>
