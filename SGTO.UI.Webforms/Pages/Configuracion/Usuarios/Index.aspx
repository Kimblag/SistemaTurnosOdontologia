<%@ Page Title="Cuentas de Usuario" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Configuracion.Usuarios.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic">

        <%--Filtros--%>
        <div class="container-fluid px-0 mb-4">
            <div class="d-flex flex-wrap align-items-center gap-2 bg-white p-3 rounded shadow-sm border w-100">

                <div class="flex-grow-1">
                    <div class="input-group">
                        <span class="input-group-text bg-white border-end-0 text-muted">
                            <i class="bi bi-search"></i>
                        </span>
                        <asp:TextBox ID="txtBuscarUsuario" runat="server"
                            CssClass="form-control border-start-0"
                            placeholder="Buscar Usuario por Nombre, Usuario o Email..." />
                    </div>
                </div>

                <div>
                    <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select" Width="190px"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </div>

                <div>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select" Width="200px"
                        AutoPostBack="true">
                        <asp:ListItem Text="Todos los estados" Value="todos" />
                        <asp:ListItem Text="Activo" Value="activo" />
                        <asp:ListItem Text="Inactivo" Value="inactivo" />
                    </asp:DropDownList>
                </div>

                <div class="d-flex gap-2 border-start ps-3 ms-1">
                    <asp:Button ID="btnBuscar" runat="server" Text="Aplicar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click" />
                </div>

                <div class="border-start ps-3 ms-1">
                    <asp:Button ID="btnNuevoUsuario" runat="server"
                        Text="+ Nuevo"
                        OnClick="btnNuevoUsuario_Click"
                        CssClass="btn btn-success text-nowrap" />
                </div>

            </div>
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
