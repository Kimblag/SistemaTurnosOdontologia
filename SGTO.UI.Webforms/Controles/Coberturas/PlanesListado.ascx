<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanesListado.ascx.cs" Inherits="SGTO.UI.Webforms.Controles.Coberturas.PlanesListado" %>


<div class="d-flex flex-column gap-3 pt-2">

    <%--Filtros--%>
    <div class="d-flex gap-2 align-items-center my-3 mb-3 justify-content-between">

        <div class="d-flex gap-2 align-items-center w-50">
            <asp:TextBox ID="txtBuscarPlanes" runat="server" CssClass="form-control" placeholder="Buscar Planes..."></asp:TextBox>

            <asp:DropDownList ID="ddlCoberturas" runat="server" 
                CssClass="form-select"
                 AutoPostBack="True" 
                OnSelectedIndexChanged="ddlCoberturas_SelectedIndexChanged">
            </asp:DropDownList>

            <asp:DropDownList ID="ddlEstado" runat="server" 
                CssClass="form-select" 
                AutoPostBack="True" 
                OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                <asp:ListItem Text="Todos" Value="todos" />
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
                <asp:BoundField DataField="NombreCobertura" HeaderText="Cobertura" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre del Plan" />
                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                <asp:BoundField DataField="PorcentajeCobertura" HeaderText="% de Cobertura" />

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
                            CommandArgument='<%# Eval("IdPlan") %>'>
                             <i class="bi bi-pencil"></i>
                        </asp:LinkButton>

                        <button type="button"
                            class="btn btn-outline-danger btn-sm me-1"
                            data-id='<%# Eval("IdPlan") %>'
                            onclick="abrirModalConfirmacion('<%# Eval("IdPlan") %>', 'plan')">
                            <i class="bi bi-x"></i>
                        </button>
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

