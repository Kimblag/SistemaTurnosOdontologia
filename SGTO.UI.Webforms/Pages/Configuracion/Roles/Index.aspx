<%@ Page Title="Configuración - Roles" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Configuracion.Roles.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>

        <%--Filtros--%>
        <div class="d-flex gap-2 align-items-center my-3 mb-3 justify-content-between">

            <div class="d-flex gap-2 align-items-center w-50">
                <asp:TextBox ID="txtBuscarRol" runat="server"
                    CssClass="form-control"
                    placeholder="Buscar rol..."></asp:TextBox>
            </div>

            <asp:Button ID="btnNuevoRol" runat="server" Text="+ Nuevo Rol"
                CssClass="btn btn-primary btn-sm me-1" OnClick="btnNuevoRol_Click" />
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
                AllowPaging="True" PageSize="5">

                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />

                    <asp:TemplateField HeaderText="Permisos Asignados">
                        <ItemTemplate>
                            <%# Eval("Permisos.Count") %>
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
                                CommandArgument='<%# Eval("IdRol") %>'>
                          <i class="bi bi-pencil"></i>
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnEliminar" runat="server"
                                ToolTip="Eliminar"
                                CssClass="btn btn-outline-danger btn-sm me-1"
                                CommandName="Eliminar" CommandArgument='<%# Eval("IdRol") %>'>
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
