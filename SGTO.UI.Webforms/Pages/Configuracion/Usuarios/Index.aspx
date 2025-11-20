<%@ Page Title="Cuentas de Usuario" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Configuracion.Usuarios.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>

        <%--Filtros--%>
        <div class="d-flex gap-2 align-items-center my-3 mb-3 justify-content-between">

            <div class="d-flex gap-2 align-items-center w-75">

                <div class="col-auto flex-grow-1" style="min-width: 260px; max-width: 400px;">
                    <div class="input-group">
                        <span class="input-group-text bg-white border-end-0">
                            <i class="bi bi-search text-muted"></i>
                        </span>
                        <asp:TextBox
                            ID="txtBuscarUsuario"
                            runat="server"
                            CssClass="form-control border-start-0"
                            placeholder="Buscar usuario..." />
                    </div>
                </div>


                <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select" AutoPostBack="true">
                </asp:DropDownList>

                <asp:DropDownList ID="ddlEstado" runat="server"
                    AutoPostBack="true"
                    CssClass="form-select"
                    OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                    <asp:ListItem Text="Todos los estados" Value="todos" />
                    <asp:ListItem Text="Activo" Value="activo" />
                    <asp:ListItem Text="Inactivo" Value="inactivo" />
                </asp:DropDownList>


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

            <asp:Button ID="btnNuevoUsuario" runat="server" Text="+ Nuevo Usuario"
                CssClass="btn btn-primary btn-sm me-1" OnClick="btnNuevoUsuario_Click" />
        </div>


        <%--tabla listado--%>
        <div class="content-wrapper">
            <asp:GridView ID="gvUsuarios" runat="server"
                AutoGenerateColumns="false"
                OnRowDataBound="gvUsuarios_RowDataBound"
                OnPageIndexChanging="gvUsuarios_PageIndexChanging"
                OnRowCommand="gvUsuarios_RowCommand"
                DataKeyNames="IdUsuario"
                CssClass="table gridview mb-0"
                AllowPaging="True" PageSize="8">

                <Columns>
                    <asp:BoundField DataField="NombreCompleto" HeaderText="Apellido y Nombre" />
                    <asp:BoundField DataField="NombreUsuario" HeaderText="Nombre de Usuario" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="NombreRol" HeaderText="Rol" />


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
                                CommandArgument='<%# Eval("IdUsuario") %>'>
                          <i class="bi bi-pencil"></i>
                            </asp:LinkButton>

                            <%--                <button type="button"
                                class="btn btn-outline-danger btn-sm me-1"
                                data-id='<%# Eval("IdUsuario") %>'
                                onclick="abrirModalConfirmacion('<%# Eval("IdUsuario") %>', 'usuario')">
                                <i class="bi bi-x"></i>
                            </button>--%>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="bi bi-shield-check fs-5"></i>
                        No hay usuarios para mostrar.
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
