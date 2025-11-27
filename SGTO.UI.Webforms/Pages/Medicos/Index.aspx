<%@ Page Title="Staff Médico" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="SGTO.UI.Webforms.Pages.Medicos.Medicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic">

        <%--filtros--%>
        <div class="container-fluid px-0 mb-4">
            <div class="bg-white p-3 rounded shadow-sm border w-100">

                <div class="row g-3 align-items-end">

                    <div class="col-12 col-md-6 col-xl-4">
                        <label class="form-label small text-muted">Buscar Médico</label>
                        <div class="input-group">
                            <span class="input-group-text bg-white border-end-0 text-muted"><i class="bi bi-search"></i></span>
                            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control border-start-0"
                                placeholder="Buscar Médico por Nombre, DNI, Matrícula..." />
                        </div>
                    </div>

                    <div class="col-12 col-md-6 col-xl-3">
                        <label class="form-label small text-muted">Especialidad</label>
                        <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                            <asp:ListItem Text="Todas las especialidades" Value="" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-12 col-md-6 col-xl-3">
                        <label class="form-label small text-muted">Estado</label>
                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Activos" Value="Activo" Selected="True" />
                            <asp:ListItem Text="Inactivos" Value="Inactivo" />
                            <asp:ListItem Text="Todos" Value="" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-12 col-md-6 col-xl-2 d-flex gap-2">
                        <asp:Button ID="btnBuscar" runat="server" Text="Filtrar" CssClass="btn btn-primary w-50" OnClick="btnBuscar_Click" />
                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary w-50" OnClick="btnLimpiar_Click" />
                    </div>

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

                            <asp:LinkButton ID="btnDetalle" runat="server"
                                CssClass="btn btn-outline-primary"
                                CommandName="Ver"
                                CommandArgument='<%# Eval("IdMedico") %>'
                                ToolTip="Ver Detalle">
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
