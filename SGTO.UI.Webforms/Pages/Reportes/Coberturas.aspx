<%@ Page Title="Reporte de Coberturas" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Coberturas.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Reportes.Coberturas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-generic reportes-page">

        <!-- Filtros -->
        <div class="filters card shadow-sm p-4 mb-4 border-0">
            <div class="d-flex flex-wrap align-items-end justify-content-between gap-3 w-100">
                <div class="d-flex flex-wrap gap-3 flex-grow-1">

                    <div class="filtro">
                        <label for="ddlEstado" class="form-label small text-muted">Estado</label>
                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select" Width="160">
                            <asp:ListItem Text="Todos" Value="" />
                            <asp:ListItem Text="Activos" Value="A" Selected="True" />
                            <asp:ListItem Text="Inactivos" Value="I" />
                        </asp:DropDownList>
                    </div>

                    <div class="filtro">
                        <label for="ddlOrden" class="form-label small text-muted">Orden (% Cob)</label>
                        <asp:DropDownList ID="ddlOrden" runat="server" CssClass="form-select" Width="200">
                            <asp:ListItem Text="Nombre (A-Z)" Value="nombre" Selected="True" />
                            <asp:ListItem Text="Mayor % primero" Value="mayor" />
                            <asp:ListItem Text="Menor % primero" Value="menor" />
                        </asp:DropDownList>
                    </div>

                    <div class="filtro flex-grow-1">
                        <label for="ddlCoberturaFiltro" class="form-label small text-muted">Cobertura</label>
                        <asp:DropDownList ID="ddlCoberturaFiltro" runat="server" CssClass="form-select w-100"></asp:DropDownList>
                    </div>

                </div>
                <div class="d-flex align-items-end gap-2">
                    <asp:Button ID="btnEjecutar" runat="server" Text="Ejecutar" CssClass="btn btn-primary px-3" OnClick="btnEjecutar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary px-3" OnClick="btnLimpiar_Click" />
                </div>
            </div>

        </div>

        <!-- KPIs -->
        <div class="row g-3 mb-3">
            <div class="col-12 col-md-4">
                <div class="kpi-card text-center py-3 bg-white border rounded shadow-sm h-100">
                    <h6 class="text-muted mb-1 small text-uppercase">Catálogo</h6>
                    <p class="text-dark fw-bold fs-5 mb-0">
                        <asp:Label ID="lblTotalCoberturas" runat="server" Text="-" />
                        O.S. / 
                       
                        <asp:Label ID="lblTotalPlanes" runat="server" Text="-" />
                        Planes
                   
                    </p>
                </div>
            </div>
            <div class="col-12 col-md-4">
                <div class="kpi-card text-center py-3 bg-white border rounded shadow-sm h-100">
                    <h6 class="text-muted mb-1 small text-uppercase">Más Solicitada</h6>
                    <p class="text-primary fw-bold fs-5 mb-0 text-truncate px-2">
                        <asp:Label ID="lblMasUsada" runat="server" Text="-" />
                    </p>
                </div>
            </div>
            <div class="col-12 col-md-4">
                <div class="kpi-card text-center py-3 bg-white border rounded shadow-sm h-100">
                    <h6 class="text-muted mb-1 small text-uppercase">Estado Sistema</h6>
                    <p class="text-success fw-bold fs-5 mb-0">Activo</p>
                </div>
            </div>
        </div>

        <div class="row g-3 mb-4">
            <div class="col-12 col-md-4">
                <div class="kpi-card text-center py-3 bg-white border border-primary rounded shadow-sm h-100">
                    <h6 class="text-primary small mb-1 text-uppercase">Facturación Total</h6>
                    <p class="text-primary fw-bold fs-4 mb-0">
                        <asp:Label ID="lblTotalFacturado" runat="server" Text="-" />
                    </p>
                </div>
            </div>

            <div class="col-12 col-md-4">
                <div class="kpi-card text-center py-3 bg-white border border-success rounded shadow-sm h-100">
                    <h6 class="text-success small mb-1 text-uppercase">Crédito a Cobrar (O.S.)</h6>
                    <p class="text-success fw-bold fs-4 mb-0">
                        <asp:Label ID="lblTurnosOS" runat="server" Text="-" />
                    </p>
                </div>
            </div>

            <div class="col-12 col-md-4">
                <div class="kpi-card text-center py-3 bg-white border border-info rounded shadow-sm h-100">
                    <h6 class="text-info small mb-1 text-uppercase">Copagos (Caja)</h6>
                    <p class="text-info fw-bold fs-4 mb-0">
                        <asp:Label ID="lblTotalCopagos" runat="server" Text="-" />
                    </p>
                </div>
            </div>
        </div>

        <!-- Export buttons -->
        <div class="text-end">
            <asp:Button ID="btnExportarPdf" runat="server" Text="Exportar PDF" CssClass="btn btn-outline-danger btn-sm" OnClick="btnExportarPdf_Click" />
            <asp:Button ID="btnExportarExcel" runat="server" Text="Exportar Excel" CssClass="btn btn-outline-success btn-sm" OnClick="btnExportarExcel_Click" />
        </div>


        <!-- Tabs -->
        <ul class="nav nav-tabs mb-3">
            <li class="nav-item">
                <asp:LinkButton ID="tabCoberturas" runat="server" CssClass="nav-link active" OnClick="tabCoberturas_Click">
                    <i class="bi bi-building"></i> Coberturas
                </asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="tabPlanes" runat="server" CssClass="nav-link" OnClick="tabPlanes_Click">
                    <i class="bi bi-list-check"></i> Planes
                </asp:LinkButton>
            </li>
        </ul>

        <!-- Contenido -->
        <asp:MultiView ID="mvReportes" runat="server" ActiveViewIndex="0">

            <!-- VISTA 1: COBERTURAS -->
            <asp:View ID="vCoberturas" runat="server">
                <div class="card border-0 shadow-sm">
                    <div class="card-body p-0 table-responsive">
                        <asp:GridView ID="gvCoberturas" runat="server" CssClass="table table-hover gridview mb-0" AutoGenerateColumns="false" EmptyDataText="Sin datos." GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="Cobertura" HeaderText="Obra Social" />
                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                <asp:BoundField DataField="CantidadPlanes" HeaderText="Planes" ItemStyle-HorizontalAlign="Center" />

                                <asp:TemplateField HeaderText="Turnos (Realiz./Total)">
                                    <ItemTemplate>
                                        <%# Eval("TurnosRealizados") %> / <%# Eval("TurnosAgendados") %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="TotalFacturado" HeaderText="Total Bruto" DataFormatString="{0:C0}" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="A_Cargo_OS" HeaderText="A Facturar (O.S.)" DataFormatString="{0:C0}" ItemStyle-HorizontalAlign="Right" ItemStyle-ForeColor="Green" ItemStyle-Font-Bold="true" />
                                <asp:BoundField DataField="A_Cargo_Paciente" HeaderText="Copagos" DataFormatString="{0:C0}" ItemStyle-HorizontalAlign="Right" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:View>

            <!-- VISTA 2: PLANES -->
            <asp:View ID="vPlanes" runat="server">
                <div class="card border-0 shadow-sm">
                    <div class="card-body p-0 table-responsive">
                        <asp:GridView ID="gvPlanes" runat="server" CssClass="table table-hover gridview mb-0" AutoGenerateColumns="false" EmptyDataText="Sin datos." GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="Cobertura" HeaderText="Obra Social" />
                                <asp:BoundField DataField="Plan" HeaderText="Plan" />
                                <asp:BoundField DataField="Estado" HeaderText="Est." />
                                <asp:BoundField DataField="PorcentajeCubierto" HeaderText="% Cob." DataFormatString="{0:N0}%" ItemStyle-HorizontalAlign="Center" />

                                <asp:BoundField DataField="TurnosRealizados" HeaderText="Realizados" ItemStyle-HorizontalAlign="Center" />

                                <asp:BoundField DataField="TotalFacturado" HeaderText="Total Bruto" DataFormatString="{0:C0}" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="A_Cargo_OS" HeaderText="A Cargo O.S." DataFormatString="{0:C0}" ItemStyle-HorizontalAlign="Right" ItemStyle-ForeColor="Green" />
                                <asp:BoundField DataField="A_Cargo_Paciente" HeaderText="Copagos" DataFormatString="{0:C0}" ItemStyle-HorizontalAlign="Right" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:View>

        </asp:MultiView>



        <!-- Modal -->
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
