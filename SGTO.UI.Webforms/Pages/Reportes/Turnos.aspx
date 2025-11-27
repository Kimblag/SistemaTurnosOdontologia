<%@ Page Title="Reporte de Turnos" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Turnos.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Reportes.Turnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-generic reportes-page turnos-reporte">

        <%-- Filtros --%>
        <div class="filters card shadow-sm p-4 mb-4 border-0">
            <div class="d-flex flex-wrap align-items-end justify-content-between gap-3 w-100">

                <div class="d-flex flex-wrap gap-3 flex-grow-1">
                    <div class="filtro">
                        <label for="txtFechaDesde" class="form-label small text-muted">Desde</label>
                        <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="filtro">
                        <label for="txtFechaHasta" class="form-label small text-muted">Hasta</label>
                        <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>

                    <div class="filtro">
                        <label for="ddlEstado" class="form-label small text-muted">Estado</label>
                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select" Width="200">
                            <asp:ListItem Text="Todos" Value="" />
                            <asp:ListItem Text="Nuevos (Pendientes)" Value="N" />
                            <asp:ListItem Text="Atendidos (Cerrados)" Value="Z" />
                            <asp:ListItem Text="Cancelados" Value="C" />
                            <asp:ListItem Text="Ausentes" Value="X" />
                            <asp:ListItem Text="Reprogramados" Value="R" />
                        </asp:DropDownList>
                    </div>

                    <div class="filtro flex-grow-1">
                        <label for="ddlMedico" class="form-label small text-muted">Médico</label>
                        <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select w-100"></asp:DropDownList>
                    </div>

                    <div class="filtro flex-grow-1">
                        <label for="ddlEspecialidad" class="form-label small text-muted">Especialidad</label>
                        <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select w-100"></asp:DropDownList>
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

        <div class="row g-3 mb-4">

            <div class="col-12 col-sm-6 col-lg-4 col-xl-2">
                <div class="kpi-card text-center py-3 px-2 bg-white border rounded shadow-sm h-100">
                    <h6 class="text-muted small mb-1 text-uppercase">Total</h6>
                    <p class="text-dark fw-bold fs-4 mb-0">
                        <asp:Label ID="lblTotal" runat="server" Text="0" />
                    </p>
                </div>
            </div>

            <div class="col-12 col-sm-6 col-lg-4 col-xl-2">
                <div class="kpi-card text-center py-3 px-2 bg-white border border-success rounded shadow-sm h-100">
                    <h6 class="text-success small mb-1 text-uppercase">Atendidos</h6>
                    <p class="text-success fw-bold fs-4 mb-0">
                        <asp:Label ID="lblAtendidos" runat="server" Text="0" />
                    </p>
                </div>
            </div>

            <div class="col-12 col-sm-6 col-lg-4 col-xl-2">
                <div class="kpi-card text-center py-3 px-2 bg-white border border-warning rounded shadow-sm h-100">
                    <h6 class="text-warning small mb-1 text-uppercase">Reprogramados</h6>
                    <p class="text-warning fw-bold fs-4 mb-0">
                        <asp:Label ID="lblReprogramados" runat="server" Text="0" />
                    </p>
                </div>
            </div>

            <div class="col-12 col-sm-6 col-lg-4 col-xl-2">
                <div class="kpi-card text-center py-3 px-2 bg-white border border-danger rounded shadow-sm h-100">
                    <h6 class="text-danger small mb-1 text-uppercase">Cancelados</h6>
                    <p class="text-danger fw-bold fs-4 mb-0">
                        <asp:Label ID="lblCancelados" runat="server" Text="0" />
                    </p>
                </div>
            </div>

            <div class="col-12 col-sm-6 col-lg-4 col-xl-2">
                <div class="kpi-card text-center py-3 px-2 bg-white border border-secondary rounded shadow-sm h-100">
                    <h6 class="text-secondary small mb-1 text-uppercase">Ausentes</h6>
                    <p class="text-secondary fw-bold fs-4 mb-0">
                        <asp:Label ID="lblAusentes" runat="server" Text="0" />
                    </p>
                </div>
            </div>

            <div class="col-12 col-sm-6 col-lg-4 col-xl-2">
                <div class="kpi-card text-center py-3 px-2 bg-white border border-primary rounded shadow-sm h-100">
                    <h6 class="text-primary small mb-1 text-uppercase">Pendientes</h6>
                    <p class="text-primary fw-bold fs-4 mb-0">
                        <asp:Label ID="lblPendientes" runat="server" Text="0" />
                    </p>
                </div>
            </div>
        </div>

        <div class="d-flex justify-content-end flex-wrap gap-2 mb-3 w-100">
            <asp:Button ID="btnExportarPdf" runat="server" Text="Exportar PDF" CssClass="btn btn-outline-danger btn-sm px-3" OnClick="btnExportarPdf_Click" />
            <asp:Button ID="btnExportarExcel" runat="server" Text="Exportar Excel" CssClass="btn btn-outline-success btn-sm px-3" OnClick="btnExportarExcel_Click" />
        </div>

        <div class="content-wrapper">
            <asp:GridView ID="gvTurnos" runat="server" CssClass="table gridview mb-0"
                AutoGenerateColumns="false"
                OnRowDataBound="gvTurnos_RowDataBound"
                EmptyDataText="No se encontraron turnos para los filtros seleccionados."
                AllowPaging="true" PageSize="10"
                OnPageIndexChanging="gvTurnos_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Hora" HeaderText="Hora" />
                    <asp:BoundField DataField="Paciente" HeaderText="Paciente" HeaderStyle-CssClass="fw-bold" />
                    <asp:BoundField DataField="DniPaciente" HeaderText="DNI" />
                    <asp:BoundField DataField="Medico" HeaderText="Médico" />
                    <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <div id="lblEstado" runat="server" class="badge">
                                <%# Eval("Estado") %>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Cobertura" HeaderText="Cobertura" />
                </Columns>
                <EmptyDataTemplate>
                    <div class="text-center py-4 text-muted">
                        <i class="bi bi-calendar-x fs-3 d-block mb-2"></i>
                        No hay datos para mostrar.
                   
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>

    </div>

    <div class="modal fade" id="modalResultado" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content border-0 shadow">
                <div class="modal-header bg-white border-bottom-0">
                    <h5 id="modalResultadoTitulo" class="modal-title fw-bold">Mensaje</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body text-center py-4">
                    <p id="modalResultadoDesc" class="lead fs-6"></p>
                </div>
                <div class="modal-footer border-top-0 justify-content-center pb-4">
                    <button type="button" class="btn btn-primary px-4" data-bs-dismiss="modal">Aceptar</button>
                </div>
            </div>
        </div>
    </div>

    <script>
        window.abrirModalResultado = function (titulo, descripcion) {
            document.getElementById('modalResultadoTitulo').textContent = titulo || "Información";
            document.getElementById('modalResultadoDesc').textContent = descripcion || "";
            new bootstrap.Modal(document.getElementById('modalResultado')).show();
        };
    </script>


</asp:Content>
