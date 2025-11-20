<%@ Page Title="Gestión de Turnos" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="SGTO.UI.Webforms.Pages.Turnos.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="page-generic">

        <%-- Filtros --%>
        <div class="container-fluid px-0 mb-4">
            <div class="d-flex flex-wrap align-items-center gap-2 bg-white p-3 rounded shadow-sm border w-100">


                <div class="flex-grow-1">
                    <div class="input-group">
                        <span class="input-group-text bg-white border-end-0 text-muted">
                            <i class="bi bi-search"></i>
                        </span>
                        <asp:TextBox ID="txtBuscar" runat="server"
                            CssClass="form-control border-start-0"
                            placeholder="Buscar por paciente (DNI, nombre) o médico (matrícula, nombre)..." />
                    </div>
                </div>


                <div>
                    <asp:DropDownList ID="ddlCampo" runat="server" CssClass="form-select" Width="160px"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged">
                        <asp:ListItem Text="Filtrar por..." Value="" />
                        <asp:ListItem Text="Médico" Value="Medico" />
                        <asp:ListItem Text="Estado" Value="Estado" />
                        <asp:ListItem Text="Especialidad" Value="Especialidad" />
                        <asp:ListItem Text="Cobertura" Value="Cobertura" />
                    </asp:DropDownList>
                </div>

                <div>
                    <asp:DropDownList ID="ddlCriterio" runat="server" CssClass="form-select" Width="220px" Enabled="false">
                        <asp:ListItem Text="Seleccione un criterio" Value="" />
                    </asp:DropDownList>
                </div>


                <div class="d-flex gap-2 border-start ps-3 ms-1">
                    <asp:Button ID="btnBuscar" runat="server" Text="Aplicar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click" />
                </div>

            </div>
        </div>


        <%-- Tabla --%>
        <div class="content-wrapper">

            <asp:GridView ID="gvTurnos" runat="server"
                AutoGenerateColumns="false"
                CssClass="table gridview mb-0"
                DataKeyNames="IdTurno"
                OnRowDataBound="gvTurnos_RowDataBound"
                OnRowCommand="gvTurnos_RowCommand"
                AllowPaging="True" PageSize="7"
                OnPageIndexChanging="gvTurnos_PageIndexChanging">

                <Columns>

                    <%-- Paciente --%>
                    <asp:BoundField DataField="DniPaciente" HeaderText="DNI Paciente" />
                    <asp:BoundField DataField="NombrePaciente" HeaderText="Paciente" />

                    <%-- Medico --%>
                    <asp:BoundField DataField="Matricula" HeaderText="Matrícula Médico" />
                    <asp:BoundField DataField="NombreMedico" HeaderText="Médico" />

                    <%-- Especialidad --%>
                    <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />

                    <%-- Fecha --%>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" />

                    <%-- Hora --%>
                    <asp:BoundField DataField="Hora" HeaderText="Hora" />

                    <%-- Cobertura --%>
                    <asp:BoundField DataField="Cobertura" HeaderText="Cobertura" />

                    <%-- Estado --%>
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <div id="lblEstado" runat="server" class="badge">
                                <%# Eval("Estado") %>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Acciones --%>
                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="240px" ItemStyle-CssClass="text-end">
                        <ItemTemplate>
                            <div class="d-flex justify-content-start gap-2">

                                <div class="btn-group btn-group-sm" role="group">

                                    <asp:LinkButton ID="btnEditar" runat="server"
                                        CssClass="btn btn-outline-secondary"
                                        CommandName="Editar"
                                        ToolTip="Editar Datos"
                                        CommandArgument='<%# Eval("IdTurno") %>'>
                                        <i class="bi bi-pencil"></i>
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnDetalle" runat="server"
                                        CssClass="btn btn-outline-primary"
                                        CommandName="Ver"
                                        ToolTip="Ver Detalle"
                                        CommandArgument='<%# Eval("IdTurno") %>'>
                                        <i class="bi bi-eye"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
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
