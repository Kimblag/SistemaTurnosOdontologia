<%@ Page Title="Pacientes" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Pacientes.Pacientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic">

        <%--filtros--%>
        <div class="container-fluid px-0 mb-3">
            <div class="row g-2 align-items-center">

                <%-- Columna izquierda: búsqueda + selects --%>
                <div class="col-12 col-lg-9 d-flex flex-wrap align-items-center gap-2">

                    <%-- Campo de búsqueda --%>
                    <div class="col-auto flex-grow-1" style="min-width: 260px; max-width: 400px;">
                        <div class="input-group">
                            <span class="input-group-text bg-white border-end-0">
                                <i class="bi bi-search text-muted"></i>
                            </span>
                            <asp:TextBox
                                ID="txtBuscar"
                                runat="server"
                                CssClass="form-control border-start-0"
                                placeholder="Buscar por nombre, DNI o email..." />
                        </div>
                    </div>

                    <%-- Selector del campo a filtrar --%>
                    <div class="col-auto">
                        <asp:DropDownList
                            ID="ddlCampo"
                            runat="server"
                            CssClass="form-select"
                            Width="170"
                            OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Text="Filtrar por..." Value="" />
                            <asp:ListItem Text="Cobertura" Value="Cobertura" />
                            <asp:ListItem Text="Estado" Value="Estado" />
                        </asp:DropDownList>
                    </div>

                    <%-- Selector del criterio (dependiente del campo) --%>
                    <div class="col-auto">
                        <asp:DropDownList
                            ID="ddlCriterio"
                            runat="server"
                            CssClass="form-select"
                            Width="220"
                            Enabled="false">
                            <asp:ListItem Text="Seleccione un criterio" Value=""></asp:ListItem>
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

                <%-- Columna derecha: botón nuevo paciente --%>
                <div class="col-12 col-lg-3 text-lg-end">
                    <asp:Button
                        ID="btnNuevoPaciente"
                        runat="server"
                        Text="+ Nuevo Paciente"
                        OnClick="btnNuevoPaciente_Click"
                        CssClass="btn btn-primary btn-sm me-1" />
                </div>
            </div>
        </div>


        <%-- Tabla --%>
        <div class="content-wrapper">

            <asp:GridView ID="gvPacientes" runat="server"
                AutoGenerateColumns="false"
                OnRowDataBound="gvPacientes_RowDataBound"
                OnPageIndexChanging="gvPacientes_PageIndexChanging"
                OnRowCommand="gvPacientes_RowCommand"
                DataKeyNames="IdPaciente"
                CssClass="table gridview mb-0"
                AllowPaging="True" PageSize="7">
                <Columns>
                    <asp:BoundField DataField="NombreCompleto" HeaderText="Apellido y Nombre" />
                    <asp:BoundField DataField="Dni" HeaderText="Documento de Identidad" />
                    <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="NombreCobertura" HeaderText="Cobertura" />
                    <asp:BoundField DataField="NombrePlan" HeaderText="Plan" />


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
                                CssClass="btn btn-outline-secondary btn-sm me-1"
                                CommandName="Editar"
                                CommandArgument='<%# Eval("IdPaciente") %>'>
                        <i class="bi bi-pencil"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnDetalle" runat="server" CssClass="btn btn-outline-primary btn-sm" CommandName="Ver" CommandArgument='<%# Eval("IdPaciente") %>'>
                        <i class="bi bi-eye"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-outline-danger btn-sm me-1" CommandName="Eliminar" CommandArgument='<%# Eval("IdPaciente") %>'>
                        <i class="bi bi-x"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="bi bi-person-x"></i>
                        No hay pacientes para mostrar.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>

        </div>
    </div>

</asp:Content>
