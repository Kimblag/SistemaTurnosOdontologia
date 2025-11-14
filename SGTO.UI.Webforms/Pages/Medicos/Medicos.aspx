<%@ Page Title="Médicos" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="Medicos.aspx.cs"
    Inherits="SGTO.UI.Webforms.Pages.Medicos.Medicos" %>

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
                                placeholder="Buscar por nombre, DNI o matrícula..." />
                        </div>
                    </div>

                    <%-- Selector del campo a filtrar --%>
                    <div class="col-auto">
                        <asp:DropDownList
                            ID="ddlCampo"
                            runat="server"
                            CssClass="form-select"
                            Width="170"
                            AutoPostBack="true">
                            <asp:ListItem Text="Filtrar por..." Value="" />
                            <asp:ListItem Text="Especialidad" Value="Especialidad" />
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
