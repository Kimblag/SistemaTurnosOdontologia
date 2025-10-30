<%@ Page Title="Tratamientos" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="SGTO.UI.Webforms.Pages.Tratamientos.Tratamientos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic">

        <%-- Filtros --%>
        <div class="container-fluid px-0 mb-3">
            <div class="row g-2 align-items-center">

                <%-- Izquierda: buscador + selects --%>
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
                                placeholder="Buscar tratamientos..." />
                        </div>
                    </div>

                    <%-- Filtro por Especialidad --%>
                    <div class="col-auto">
                        <asp:DropDownList
                            ID="ddlEspecialidad"
                            runat="server"
                            CssClass="form-select"
                            Width="190"
                            AutoPostBack="true">
                            <asp:ListItem Text="Especialidad" Value="" />
                            <%-- Cargá dinámicamente --%>
                        </asp:DropDownList>
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

                <%-- Derecha: botón nuevo tratamiento --%>
                <div class="col-12 col-lg-3 text-lg-end">
                    <asp:Button
                        ID="btnNuevoTratamiento"
                        runat="server"
                        Text="+ Nuevo Tratamiento"
                        OnClick="btnNuevoTratamiento_Click"
                        CssClass="btn btn-primary fw-semibold px-3 py-2 d-flex d-lg-inline-flex align-items-center gap-1 mx-auto mx-lg-0" />
                </div>

            </div>
        </div>

        <%-- Tabla --%>
        <div class="content-wrapper">

            <asp:GridView ID="gvTratamientos" runat="server"
                AutoGenerateColumns="false"
                CssClass="table gridview mb-0"
                DataKeyNames="IdTratamiento"
                AllowPaging="True" PageSize="7"
                OnRowDataBound="gvTratamientos_RowDataBound"
                OnPageIndexChanging="gvTratamientos_PageIndexChanging"
                OnRowCommand="gvTratamientos_RowCommand">

                <Columns>
                    <%-- Nombre --%>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre del Tratamiento" />

                    <%-- Descripción --%>
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />

                    <%-- Costo Base (formato moneda) --%>
                    <asp:BoundField DataField="CostoBase" HeaderText="Costo Base"
                        DataFormatString="{0:C}" HtmlEncode="false" />

                    <%-- Especialidad asociada (texto) --%>
                    <asp:TemplateField HeaderText="Especialidad Asociada">
                        <ItemTemplate>
                            <%# Eval("Especialidad.Nombre") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Estado con badge --%>
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <span id="lblEstado" runat="server" class="badge"><%# Eval("Estado") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Acciones --%>
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server"
                                CssClass="btn btn-outline-secondary btn-sm me-1"
                                CommandName="Editar"
                                CommandArgument='<%# Eval("IdTratamiento") %>'>
                                <i class="bi bi-pencil"></i>
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnDetalle" runat="server"
                                CssClass="btn btn-outline-primary btn-sm me-1"
                                CommandName="Ver"
                                CommandArgument='<%# Eval("IdTratamiento") %>'>
                                <i class="bi bi-eye"></i>
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnEliminar" runat="server"
                                CssClass="btn btn-outline-danger btn-sm"
                                CommandName="Eliminar"
                                CommandArgument='<%# Eval("IdTratamiento") %>'>
                                <i class="bi bi-x"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="bi bi-x-octagon"></i>
                        No hay tratamientos para mostrar.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>

        </div>
    </div>

</asp:Content>
