<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoberturasListado.ascx.cs" Inherits="SGTO.UI.Webforms.Controles.Coberturas.CoberturasListado" %>


<div class="d-flex flex-column gap-3 pt-2">

    <%--Filtros--%>
    <div class="d-flex gap-2 align-items-center my-3 mb-3 justify-content-between">

        <div class="d-flex gap-2 align-items-center w-50">
            <asp:TextBox ID="txtBuscarCobertura" runat="server" CssClass="form-control" placeholder="Buscar cobertura..."></asp:TextBox>

            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                <asp:ListItem Text="Todos" />
                <asp:ListItem Text="Activo" />
                <asp:ListItem Text="Inactivo" />
            </asp:DropDownList>
        </div>

        <asp:Button ID="btnNuevaCobertura" runat="server" Text="+ Nueva Cobertura"
            CssClass="btn btn-primary btn-sm me-1" OnClick="btnNuevaCobertura_Click" />
    </div>

    <%--tabla listado--%>
    <div class="content-wrapper">
        <asp:GridView ID="gvCoberturas" runat="server"
            AutoGenerateColumns="false"
            OnRowDataBound="gvCoberturas_RowDataBound"
            OnPageIndexChanging="gvCoberturas_PageIndexChanging"
            OnRowCommand="gvCoberturas_RowCommand"
            DataKeyNames="IdCobertura"
            CssClass="table gridview mb-0"
            AllowPaging="True" PageSize="5">

            <Columns>
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                <asp:BoundField DataField="CantidadPlanes" HeaderText="Cantidad de Planes" />


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
                            CommandArgument='<%# Eval("IdCobertura") %>'>
                             <i class="bi bi-pencil"></i>
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnVerPlanes" runat="server"
                            ToolTip="Ver Planes"
                            CommandName="VerPlanes"
                            CommandArgument='<%# Eval("IdCobertura") %>'>
                         <i class="bi bi-link me-1"></i>Ver Planes
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnEliminar" runat="server" ToolTip="Eliminar"
                            CssClass="btn btn-outline-danger btn-sm me-1"
                            CommandName="Eliminar" CommandArgument='<%# Eval("IdCobertura") %>'>
                             <i class="bi bi-x"></i>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>

            <EmptyDataTemplate>
                <div class="empty-state">
                    <i class="bi bi-shield-check fs-5"></i>
                    No hay coberturas para mostrar.
                </div>
            </EmptyDataTemplate>

        </asp:GridView>
    </div>

</div>

