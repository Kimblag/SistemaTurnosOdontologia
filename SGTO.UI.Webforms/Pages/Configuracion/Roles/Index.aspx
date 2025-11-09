<%@ Page Title="Configuración - Roles" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Configuracion.Roles.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>

        <%--Filtros--%>
        <div class="container-fluid px-0 mb-3">
            <div class="row g-2 align-items-center">

                <div class="col-12 col-lg-9 d-flex flex-wrap align-items-center gap-2">

                    <div class="col-auto flex-grow-1" style="min-width: 260px; max-width: 400px;">
                        <div class="input-group">
                            <span class="input-group-text bg-white border-end-0">
                                <i class="bi bi-search text-muted"></i>
                            </span>
                            <asp:TextBox
                                ID="txtBuscarRol"
                                runat="server"
                                CssClass="form-control border-start-0"
                                placeholder="Buscar por nombre o descripción..." />
                        </div>
                    </div>

                    <div class="col-auto">
                        <asp:DropDownList
                            ID="ddlEstado"
                            runat="server"
                            CssClass="form-select"
                            Width="170">
                            <asp:ListItem Text="Todos" Value="todos" Selected="True" />
                            <asp:ListItem Text="Activo" Value="activo" />
                            <asp:ListItem Text="Inactivo" Value="inactivo" />
                        </asp:DropDownList>
                    </div>

                    <asp:Button
                        ID="btnBuscar"
                        runat="server"
                        Text="Aplicar Filtro"
                        CssClass="btn btn-outline-primary"
                        OnClick="btnBuscar_Click" />

                    <asp:Button
                        ID="btnLimpiar"
                        runat="server"
                        Text="Limpiar"
                        CssClass="btn btn-outline-secondary"
                        OnClick="btnLimpiar_Click" />

                </div>

                <div class="col-12 col-lg-3 text-lg-end">
                    <asp:Button
                        ID="btnNuevoRol"
                        runat="server"
                        Text="+ Nuevo Rol"
                        CssClass="btn btn-primary btn-sm me-1"
                        OnClick="btnNuevoRol_Click" />
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
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar"
                                runat="server"
                                ToolTip="Editar"
                                CssClass="btn btn-outline-secondary btn-sm me-1"
                                CommandName="Editar"
                                CommandArgument='<%# Eval("IdRol") %>'>
                                <i class="bi bi-pencil"></i>
                            </asp:LinkButton>

                            <button type="button"
                                class="btn btn-outline-danger btn-sm me-1"
                                data-id='<%# Eval("IdRol") %>'
                                onclick="abrirModalConfirmacion('<%# Eval("IdRol") %>', 'rol')">
                                <i class="bi bi-x"></i>
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
