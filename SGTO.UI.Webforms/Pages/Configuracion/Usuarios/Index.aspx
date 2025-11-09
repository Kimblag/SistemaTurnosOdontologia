<%@ Page Title="Configuración - Gestión de Usuarios" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Configuracion.Usuarios.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>

        <%--Filtros--%>
        <div class="d-flex gap-2 align-items-center my-3 mb-3 justify-content-between">

            <div class="d-flex gap-2 align-items-center w-50">
                <asp:TextBox ID="txtBuscarUsuario" runat="server" CssClass="form-control" placeholder="Buscar usuario..."></asp:TextBox>

                <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select">
                    <asp:ListItem Text="Administrador" />
                    <asp:ListItem Text="Odontólogo" />
                    <asp:ListItem Text="Recepcionista" />
                </asp:DropDownList>

                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                    <asp:ListItem Text="Todos" />
                    <asp:ListItem Text="Activo" />
                    <asp:ListItem Text="Inactivo" />
                </asp:DropDownList>
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
                AllowPaging="True" PageSize="5">

                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                    <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:TemplateField HeaderText="Rol">
                        <ItemTemplate>
                            <%# Eval("Rol.Nombre") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--columna estado--%>
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <span id="lblEstado" runat="server" class="badge"><%# Eval("Estado") %></span>
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
                            <asp:LinkButton ID="btnEliminar" runat="server"
                                ToolTip="Eliminar"
                                CssClass="btn btn-outline-danger btn-sm me-1"
                                CommandName="Eliminar" CommandArgument='<%# Eval("IdUsuario") %>'>
                            <i class="bi bi-x"></i>
                            </asp:LinkButton>
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


</asp:Content>
