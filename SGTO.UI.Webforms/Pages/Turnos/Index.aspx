<%@ Page Title="Gestión de Turnos" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="SGTO.UI.Webforms.Pages.Turnos.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="page-generic">

        <%-- Filtros --%>
        <div class="container-fluid px-0 mb-4">
            <div class="bg-white p-3 rounded shadow-sm border w-100">

                <div class="row g-3">
                    <div class="col-md-12">
                        <div class="input-group">
                            <span class="input-group-text bg-white border-end-0 text-muted"><i class="bi bi-search"></i></span>
                            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control border-start-0"
                                placeholder="Buscar rápido por Paciente (Nombre, DNI) o Médico (Nombre, Matrícula)..." />
                        </div>
                    </div>

                    <div class="col-md-3">
                        <label class="form-label small text-muted">Fecha Turno</label>
                        <asp:TextBox ID="txtFecha" runat="server" TextMode="Date" CssClass="form-control" />
                    </div>

                    <div class="col-md-3">
                        <label class="form-label small text-muted">Médico</label>
                        <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                            <asp:ListItem Text="Todos los médicos" Value="-1" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-3">
                        <label class="form-label small text-muted">Estado</label>
                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Todos" Value="" />
                            <asp:ListItem Text="Nuevo" Value="Nuevo" />
                            <asp:ListItem Text="Reprogramado" Value="Reprogramado" />
                            <asp:ListItem Text="No asistió" Value="No asistió" />
                            <asp:ListItem Text="Cancelado" Value="Cancelado" />
                            <asp:ListItem Text="Cerrado" Value="Cerrado" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-3 d-flex align-items-end gap-2">
                        <asp:Button ID="btnBuscar" runat="server" Text="Filtrar" CssClass="btn btn-primary w-100" OnClick="btnBuscar_Click" />
                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click" />
                    </div>
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
                            <div class="d-flex justify-content-end gap-2 align-items-center">

                                <asp:LinkButton ID="btnAtender" runat="server"
                                    CssClass="btn btn-success btn-sm shadow-sm d-flex align-items-center"
                                    CommandName="Atender"
                                    ToolTip="Realizar consulta médica"
                                    CommandArgument='<%# Eval("IdTurno") %>'>
                                    <i class="bi bi-journal-medical me-1"></i> Atender
                                </asp:LinkButton>

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
