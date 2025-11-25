<%@ Page Title="Roles y Seguridad" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Configuracion.Roles.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>

        <%--Filtros--%>
        <div class="container-fluid px-0 mb-4">
            <div class="d-flex flex-wrap align-items-center gap-3 bg-white p-3 rounded shadow-sm border w-100">

                <div class="flex-grow-1" style="min-width: 250px;">
                    <div class="input-group">
                        <span class="input-group-text bg-white border-end-0 text-muted">
                            <i class="bi bi-search"></i>
                        </span>
                        <asp:TextBox ID="txtBuscarRol" runat="server"
                            CssClass="form-control border-start-0"
                            placeholder="Buscar Rol..." />
                    </div>
                </div>

                <div style="min-width: 180px;">
                    <asp:DropDownList ID="ddlEstado" runat="server"
                        CssClass="form-select w-100"
                        AutoPostBack="true">
                        <asp:ListItem Text="Todos los estados" Value="todos" Selected="True" />
                        <asp:ListItem Text="Activo" Value="activo" />
                        <asp:ListItem Text="Inactivo" Value="inactivo" />
                    </asp:DropDownList>
                </div>

                <div class="d-flex gap-2 border-start ps-3">
                    <asp:Button ID="btnBuscar" runat="server" Text="Aplicar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click" />
                </div>

              
                <div id="divBtnNuevo" runat="server" class="border-start ps-3">
                    <asp:Button ID="btnNuevoRol" runat="server"
                        Text="+ Nuevo"
                        OnClick="btnNuevoRol_Click"
                        CssClass="btn btn-success text-nowrap" />
                </div>

            </div>
        </div>

        <%--tabla listado--%>
        <div class="content-wrapper">
            <asp:GridView ID="gvRoles" runat="server"
                AutoGenerateColumns="false"
                OnRowDataBound="gvRoles_RowDataBound"
                OnPageIndexChanging="gvRoles_PageIndexChanging"
                OnRowCommand="gvRoles_RowCommand"
                DataKeyNames="IdRol"
                CssClass="table gridview mb-0"
                AllowPaging="True" PageSize="8">

                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />

                    <asp:BoundField DataField="CantidadPermisos" HeaderText="Permisos Asignados" />

                    <%--columna estado--%>
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <div id="lblEstado" runat="server" class="badge"><%# Eval("Estado") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--columna acciones--%>
                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="240px" ItemStyle-CssClass="text-end">
                        <ItemTemplate>
                            <div class="d-flex justify-content-start gap-2">
                                <div class="btn-group btn-group-sm" role="group">

                                    <asp:LinkButton ID="btnEditar"
                                        runat="server"
                                        ToolTip="Editar"
                                        CssClass="btn btn-outline-secondary"
                                        CommandName="Editar"
                                        CommandArgument='<%# Eval("IdRol") %>'>
                                <i class="bi bi-pencil"></i>
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnDetalle" runat="server"
                                        CssClass="btn btn-outline-primary"
                                        CommandName="Ver"
                                        ToolTip="Ver Detalle"
                                        CommandArgument='<%# Eval("IdRol") %>'>
                                    <i class="bi bi-eye"></i>
                                    </asp:LinkButton>

                                    <button type="button"
                                        id="btnEliminar"
                                        class="btn btn-outline-danger"
                                        data-id='<%# Eval("IdRol") %>'
                                        title="Dar de baja"
                                        onclick="abrirModalConfirmacion('<%# Eval("IdRol") %>', 'rol')">
                                        <i class="bi bi-x"></i>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="bi bi-shield-check fs-5"></i>
                        No hay roles para mostrar.
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

                    const titulo = "Confirmar baja de rol";
                    const texto = "¿Está seguro de que desea dar de baja este rol?";

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
