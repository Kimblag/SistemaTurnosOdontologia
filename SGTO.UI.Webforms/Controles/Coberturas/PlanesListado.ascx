<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanesListado.ascx.cs" Inherits="SGTO.UI.Webforms.Controles.Coberturas.PlanesListado" %>


<div class="d-flex flex-column gap-3 pt-2">

    <%--Filtros--%>
    <div class="d-flex gap-2 align-items-center my-3 mb-3 justify-content-between">

        <div class="d-flex gap-2 align-items-center w-50">
            <asp:TextBox ID="txtBuscarPlanes" runat="server" CssClass="form-control" placeholder="Buscar Planes..."></asp:TextBox>

            <asp:DropDownList ID="ddlCoberturas" runat="server" CssClass="form-select">
                <asp:ListItem Text="Todas" />
                <asp:ListItem Text="OSDE" />
                <asp:ListItem Text="OSECAC" />
                <asp:ListItem Text="Swiss Medical" />
            </asp:DropDownList>

            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                <asp:ListItem Text="Todos" />
                <asp:ListItem Text="Activo" />
                <asp:ListItem Text="Inactivo" />
            </asp:DropDownList>
        </div>

        <asp:Button ID="btnNuevaPlanes" runat="server" Text="+ Nuevo Plan"
            CssClass="btn btn-primary btn-sm me-1" OnClick="btnNuevoPlan_Click" />
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
                <asp:TemplateField HeaderText="Cobertura">
                    <ItemTemplate>
                        <%# Eval("Cobertura.Nombre") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Nombre" HeaderText="Nombre del Plan" />
                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                <asp:BoundField DataField="PorcentajeCobertura" HeaderText="% de Cobertura" />

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
                            CommandArgument='<%# Eval("IdPlan") %>'>
                             <i class="bi bi-pencil"></i>
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnEliminar" runat="server"
                            ToolTip="Eliminar"
                            CssClass="btn btn-outline-danger btn-sm me-1"
                            CommandName="Eliminar" CommandArgument='<%# Eval("IdPlan") %>'>
                             <i class="bi bi-x"></i>
                        </asp:LinkButton>
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

