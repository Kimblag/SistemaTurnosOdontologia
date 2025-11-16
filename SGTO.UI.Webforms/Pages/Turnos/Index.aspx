<%@ Page Title="Gestión de Turnos" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="SGTO.UI.Webforms.Pages.Turnos.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="page-generic">

        <%-- Filtros --%>
        <div class="container-fluid px-0 mb-3">
            <div class="filtros-turnos">


                <div class="filtro-item filtro-busqueda">
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


                <div class="filtro-item">
                    <asp:DropDownList
                        ID="ddlCampo"
                        runat="server"
                        CssClass="form-select"
                        Width="170"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged">
                        <asp:ListItem Text="Filtrar por..." Value="" />
                        <asp:ListItem Text="Médico" Value="Medico" />
                        <asp:ListItem Text="Estado" Value="Estado" />
                        <asp:ListItem Text="Especialidad" Value="Especialidad" />
                        <asp:ListItem Text="Cobertura" Value="Cobertura" />
                    </asp:DropDownList>
                </div>


                <div class="filtro-item">
                    <asp:DropDownList
                        ID="ddlCriterio"
                        runat="server"
                        CssClass="form-select"
                        Width="300"
                        Enabled="false">
                        <asp:ListItem Text="Seleccione un criterio" Value="" />
                    </asp:DropDownList>
                </div>


                <div class="filtro-item">
                    <asp:Button ID="btnBuscar" runat="server"
                        Text="Aplicar Filtro"
                        CssClass="btn btn-outline-primary"
                        OnClick="btnBuscar_Click" />
                </div>

                <div class="filtro-item">
                    <asp:Button ID="btnLimpiar" runat="server"
                        Text="Limpiar"
                        CssClass="btn btn-outline-secondary"
                        OnClick="btnLimpiar_Click" />
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
                    <asp:BoundField DataField="NombrePaciente" HeaderText="Paciente" />

                    <%-- Medico --%>
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
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server"
                                CssClass="btn btn-outline-secondary btn-sm me-1"
                                CommandName="Editar" CommandArgument='<%# Eval("IdTurno") %>'>
                                <i class="bi bi-pencil"></i>
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnDetalle" runat="server"
                                CssClass="btn btn-outline-primary btn-sm me-1"
                                CommandName="Ver" CommandArgument='<%# Eval("IdTurno") %>'>
                                <i class="bi bi-eye"></i>
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
