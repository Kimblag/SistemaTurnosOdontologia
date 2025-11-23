<%@ Page Title="Gestión de Planes" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.CoberturasPlanes.Planes.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="page-generic">

        <%--Filtros--%>
        <div class="container-fluid px-0 mb-4">
            <div class="bg-white p-3 rounded shadow-sm border w-100">

                <div class="row g-2 align-items-end">

                    <div id="pnlBuscador" runat="server" class="col-md-4 col-lg-4">
                        <label class="form-label small text-muted">Buscar Plan</label>
                        <div class="input-group">
                            <span class="input-group-text bg-white border-end-0 text-muted"><i class="bi bi-search"></i></span>
                            <asp:TextBox ID="txtBuscarPlanes" runat="server" CssClass="form-control border-start-0"
                                placeholder="Buscar Plan por Nombre, Cobertura..." />
                        </div>
                    </div>

                    <div class="col-md-3 col-lg-3">
                        <label class="form-label small text-muted">Cobertura</label>
                        <asp:DropDownList ID="ddlCobertura" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                            <asp:ListItem Text="Todas las coberturas" Value="-1" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2 col-lg-2">
                        <label class="form-label small text-muted">Estado</label>
                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Activos" Value="Activo" Selected="True" />
                            <asp:ListItem Text="Inactivos" Value="Inactivo" />
                            <asp:ListItem Text="Todos" Value="" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-3 col-lg-2 d-flex gap-1">
                        <asp:Button ID="btnBuscar" runat="server" Text="Filtrar" CssClass="btn btn-primary w-50" OnClick="btnBuscar_Click" />
                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary w-50" OnClick="btnLimpiar_Click" />
                    </div>

                    <div id="pnlNuevoPlan" runat="server" class="col-md-12 col-lg-1 text-end border-start ps-3">
                        <label class="form-label d-none d-lg-block">&nbsp;</label>
                        <asp:Button ID="btnNuevaPlanes" runat="server" Text="+ Nuevo" OnClick="btnNuevoPlan_Click" CssClass="btn btn-success w-100" />
                    </div>

                </div>
            </div>
        </div>

        <%--tabla listado--%>
        <div class="content-wrapper">
            <asp:GridView ID="gvPlanes" runat="server"
                AutoGenerateColumns="false"
                OnRowDataBound="gvPlanes_RowDataBound"
                OnPageIndexChanging="gvPlanes_PageIndexChanging"
                OnRowCommand="gvPlanes_RowCommand"
                DataKeyNames="IdPlan"
                CssClass="table gridview mb-0"
                AllowPaging="True" PageSize="7">

                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre del Plan" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="PorcentajeCobertura" HeaderText="% de Cobertura" />
                    <asp:BoundField DataField="NombreCobertura" HeaderText="Cobertura Asociada" />

                    <%--columna estado--%>
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <div id="lblEstado" runat="server" class="badge"><%# Eval("Estado") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <%--columna acciones--%>
                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="240px" ItemStyle-CssClass="text-start">
                        <ItemTemplate>

                            <div class="btn-group btn-group-sm" role="group">

                                <asp:LinkButton ID="btnEditar"
                                    runat="server"
                                    ToolTip="Editar Datos"
                                    CssClass="btn btn-outline-secondary"
                                    CommandName="Editar"
                                    CommandArgument='<%# Eval("IdPlan") %>'>
                             <i class="bi bi-pencil"></i>
                                </asp:LinkButton>

                                <button id="btnEliminar" runat="server" type="button"
                                    class="btn btn-outline-danger"
                                    title="Dar de baja">
                                    <i class="bi bi-trash"></i>
                                </button>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="bi bi-shield-check fs-5"></i>
                        No hay Planes para mostrar.
                    </div>
                </EmptyDataTemplate>

            </asp:GridView>
        </div>

    </div>

    <%--modal de confirmación--%>
    <div class="modal fade" id="modalConfirmar" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 id="modalConfirmarTitulo" class="modal-title">Confirmar acción</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <p id="modalConfirmarTexto"></p>
                </div>
                <div class="modal-footer">
                    <asp:HiddenField ID="hdnIdEliminar" runat="server" />
                    <asp:HiddenField ID="hdnTipoEliminar" runat="server" />
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnConfirmarEliminar" runat="server"
                        CssClass="btn btn-danger"
                        Text="Confirmar"
                        OnClick="btnConfirmarEliminar_Click" />
                </div>
            </div>
        </div>
    </div>

    <%-- modal resultado --%>
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
            // modal de confirmación    
            window.abrirModalConfirmacion = function (id, tipo) {
                try {
                    document.getElementById('<%= hdnIdEliminar.ClientID %>').value = id;
                    document.getElementById('<%= hdnTipoEliminar.ClientID %>').value = tipo;

                    let titulo = "Confirmar acción";
                    let texto = "¿Está seguro que desea continuar?";

                    switch (tipo) {
                        case "paciente":
                            titulo = "Confirmar baja de paciente";
                            texto = "¿Está seguro de que desea dar de baja este paciente?";
                            break;
                        case "plan":
                            titulo = "Confirmar baja de plan";
                            texto = "¿Está seguro de que desea dar de baja este plan?";
                            break;
                        case "cobertura":
                            titulo = "Confirmar baja de cobertura";
                            texto = "¿Está seguro de que desea dar de baja esta cobertura?";
                            break;
                    }

                    document.getElementById('modalConfirmarTitulo').textContent = titulo;
                    document.getElementById('modalConfirmarTexto').textContent = texto;

                    new bootstrap.Modal(document.getElementById('modalConfirmar')).show();
                } catch (err) {
                    console.error("Error al abrir modal de confirmación:", err);
                }
            };


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
