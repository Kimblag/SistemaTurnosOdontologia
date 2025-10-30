<%@ Page Title="Especialidades" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="SGTO.UI.Webforms.Pages.Especialidades.Especialidades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic">

        <%-- Filtros --%>
        <div class="container-fluid px-0 mb-3">
            <div class="row g-2 align-items-center">

                <%-- Izquierda: buscador + estado --%>
                <div class="col-12 col-lg-9 d-flex flex-wrap align-items-center gap-2">

                    <%-- Buscador --%>
                    <div class="col-auto flex-grow-1" style="min-width:260px; max-width:400px;">
                        <div class="input-group">
                            <span class="input-group-text bg-white border-end-0">
                                <i class="bi bi-search text-muted"></i>
                            </span>
                            <asp:TextBox
                                ID="txtBuscar"
                                runat="server"
                                CssClass="form-control border-start-0"
                                placeholder="Buscar especialidades..." />
                        </div>
                    </div>

                    <%-- Filtro por Estado --%>
                    <div class="col-auto">
                        <asp:DropDownList
                            ID="ddlEstado"
                            runat="server"
                            CssClass="form-select"
                            Width="170"
                            AutoPostBack="true">
                            <asp:ListItem Text="Estado" Value="" />
                            <asp:ListItem Text="Activo" Value="Activo" />
                            <asp:ListItem Text="Inactivo" Value="Inactivo" />
                        </asp:DropDownList>
                    </div>

                </div>

                <%-- Derecha: botón nueva especialidad --%>
                <div class="col-12 col-lg-3 text-lg-end">
                    <asp:Button
                        ID="btnNuevaEspecialidad"
                        runat="server"
                        Text="+ Nueva Especialidad"
                        OnClick="btnNuevaEspecialidad_Click"
                        CssClass="btn btn-primary fw-semibold px-3 py-2 d-flex d-lg-inline-flex align-items-center gap-1 mx-auto mx-lg-0" />
                </div>

            </div>
        </div>

        <%-- Tabla --%>
        <div class="content-wrapper">

            <asp:GridView ID="gvEspecialidades" runat="server"
                AutoGenerateColumns="false"
                CssClass="table gridview mb-0"
                DataKeyNames="IdEspecialidad"
                AllowPaging="True" PageSize="7"
                OnRowDataBound="gvEspecialidades_RowDataBound"
                OnPageIndexChanging="gvEspecialidades_PageIndexChanging"
                OnRowCommand="gvEspecialidades_RowCommand">

                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre de Especialidad" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <span id="lblEstado" runat="server" class="badge"><%# Eval("Estado") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server"
                                CssClass="btn btn-outline-secondary btn-sm me-1"
                                CommandName="Editar"
                                CommandArgument='<%# Eval("IdEspecialidad") %>'>
                                <i class="bi bi-pencil"></i>
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnDetalle" runat="server"
                                CssClass="btn btn-outline-primary btn-sm me-1"
                                CommandName="Ver"
                                CommandArgument='<%# Eval("IdEspecialidad") %>'>
                                <i class="bi bi-eye"></i>
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnEliminar" runat="server"
                                CssClass="btn btn-outline-danger btn-sm"
                                CommandName="Eliminar"
                                CommandArgument='<%# Eval("IdEspecialidad") %>'>
                                <i class="bi bi-x"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="bi bi-x-octagon"></i>
                        No hay especialidades para mostrar.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>

        </div>
    </div>

</asp:Content>
