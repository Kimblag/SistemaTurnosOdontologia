<%@ Page Title="Gestión de Turnos" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="SGTO.UI.Webforms.Pages.Turnos.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="page-generic">

        <%--filtros --%>
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
                                placeholder="Buscar por paciente o médico..." />
                        </div>
                    </div>

                    <%-- Selector del campo a filtrar --%>
                    <div class="col-auto">
                        <asp:DropDownList
                            ID="ddlCampo"
                            runat="server"
                            CssClass="form-select"
                            Width="170">
                            <asp:ListItem Text="Filtrar por..." Value="" />
                            <asp:ListItem Text="Médico" Value="Medico" />
                            <asp:ListItem Text="Estado" Value="Estado" />
                        </asp:DropDownList>
                    </div>

                    <%-- Selector del criterio  --%>
                    <div class="col-auto">
                        <asp:DropDownList
                            ID="ddlCriterio"
                            runat="server"
                            CssClass="form-select"
                            Width="220"
                            Enabled="false">
                            <asp:ListItem Text="Seleccione un criterio" Value="" />
                        </asp:DropDownList>
                    </div>

                </div>

                <%-- Columna derecha: botón nuevo turno --%>
                <div class="col-12 col-lg-3 text-lg-end">
                    <asp:Button
                        ID="btnNuevoTurno"
                        runat="server"
                        Text="+ Nuevo Turno"
                        OnClick="btnNuevoTurno_Click"
                        CssClass="btn btn-primary fw-semibold px-3 py-2 d-flex d-lg-inline-flex align-items-center gap-1 mx-auto mx-lg-0" />
                </div>
            </div>
        </div>


        <%-- Tabla de Turnos --%>
        <div class="content-wrapper">

            <asp:GridView ID="gvTurnos" runat="server"
                AutoGenerateColumns="false"
                OnRowDataBound="gvTurnos_RowDataBound"
                OnPageIndexChanging="gvTurnos_PageIndexChanging"
                OnRowCommand="gvTurnos_RowCommand"
                DataKeyNames="IdTurno"
                CssClass="table gridview mb-0"
                AllowPaging="True" PageSize="7">
                <Columns>

                    <asp:TemplateField HeaderText="Paciente">
                        <ItemTemplate>
                            <%# Eval("Paciente.Nombre") %> <%# Eval("Paciente.Apellido") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Médico">
                        <ItemTemplate>
                            <%# Eval("Medico.Nombre") %> <%# Eval("Medico.Apellido") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="FechaHora" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="FechaHora" HeaderText="Hora" DataFormatString="{0:HH:mm}" />

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
                                CommandArgument='<%# Eval("IdTurno") %>'>
                        <i class="bi bi-pencil"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnDetalle" runat="server" CssClass="btn btn-outline-primary btn-sm" CommandName="Ver" CommandArgument='<%# Eval("IdTurno") %>'>
                        <i class="bi bi-eye"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-outline-danger btn-sm me-1" CommandName="Eliminar" CommandArgument='<%# Eval("IdTurno") %>'>
                        <i class="bi bi-x"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="bi bi-calendar-x"></i>
                        No hay turnos para mostrar.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>

        </div>

    </div>

</asp:Content>
