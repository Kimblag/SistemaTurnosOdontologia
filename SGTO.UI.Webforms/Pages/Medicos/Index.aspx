<%@ Page Title="Staff Médico" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="SGTO.UI.Webforms.Pages.Medicos.Medicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic">

        <%--filtros--%>
        <div class="container-fluid px-0 mb-4">
            <div class="d-flex flex-wrap align-items-center gap-2 bg-white p-3 rounded shadow-sm border w-100">

                <div class="flex-grow-1">
                    <div class="input-group">
                        <span class="input-group-text bg-white border-end-0 text-muted">
                            <i class="bi bi-search"></i>
                        </span>
                        <asp:TextBox ID="txtBuscar" runat="server"
                            CssClass="form-control border-start-0"
                            placeholder="Buscar por nombre, DNI o matrícula..." />
                    </div>
                </div>


                <div>
                    <asp:DropDownList ID="ddlCampo" runat="server" CssClass="form-select" Width="160px"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged">
                        <asp:ListItem Text="Filtrar por..." Value="" />
                        <asp:ListItem Text="Especialidad" Value="Especialidad" />
                        <asp:ListItem Text="Estado" Value="Estado" />
                    </asp:DropDownList>
                </div>

                <div>
                    <asp:DropDownList ID="ddlCriterio" runat="server" CssClass="form-select" Width="220px"
                        Enabled="false">
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

            <asp:GridView ID="gvMedicos" runat="server"
                AutoGenerateColumns="false"
                OnRowDataBound="gvMedicos_RowDataBound"
                OnPageIndexChanging="gvMedicos_PageIndexChanging"
                OnRowCommand="gvMedicos_RowCommand"
                DataKeyNames="IdMedico"
                CssClass="table gridview mb-0"
                AllowPaging="True" PageSize="7">
                <Columns>

                    <asp:BoundField DataField="NombreCompleto" HeaderText="Médico" />

                    <asp:BoundField DataField="Dni" HeaderText="DNI" />

                    <asp:BoundField DataField="Matricula" HeaderText="Matrícula" />

                    <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />

                    <asp:TemplateField HeaderText="Especialidades">
                        <ItemTemplate>
                            <%# string.Join(", ", ((SGTO.Negocio.DTOs.Medicos.MedicoListadoDto)Container.DataItem).NombresEspecialidades) %>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <%--columna estado--%>
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <div id="lblEstado" runat="server" class="badge"><%# Eval("Estado") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Acciones" Visible="true">
                        <ItemTemplate>


                            <asp:LinkButton ID="btnDetalle" runat="server" CssClass="btn btn-outline-primary btn-sm" CommandName="Ver" CommandArgument='<%# Eval("IdMedico") %>'> 
                                <i class="bi bi-eye"></i>
                            </asp:LinkButton>


                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="bi bi-person-x"></i>
                        No hay médicos para mostrar. 
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>

        </div>
    </div>

</asp:Content>
