<%@ Page Title="Detalle de Paciente" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Detalle.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Pacientes.Detalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">
        <div class="card shadow-sm p-5 w-100">

            <%--datos personales--%>
            <div>
                <div class="row mb-3">
                    <h2 class="fs-4">Datos personales</h2>
                </div>

                <div class="row g-3 mb-3">
                    <div class="col-12 col-md-4">
                        <label for="lblNombreCompleto" class="form-label d-block">Nombre completo</label>
                        <asp:Label ID="lblNombreCompleto" runat="server" CssClass="fw-semibold text-dark d-block" Text="Juan Pérez"></asp:Label>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="lblDni" class="form-label d-block">DNI</label>
                        <asp:Label ID="lblDni" runat="server" Text="12345678" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="lblFechaNacimiento" class="form-label d-block">Fecha de Nacimiento</label>
                        <asp:Label ID="lblFechaNacimiento" runat="server" Text="15/05/1987" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                </div>

                <div class="row g-3 mb-3">
                    <div class="col-12 col-md-4">
                        <label for="lblGenero" class="form-label d-block">Género</label>
                        <asp:Label ID="lblGenero" runat="server" Text="Masculino" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="lblTelefono" class="form-label d-block">Teléfono</label>
                        <asp:Label ID="lblTelefono" runat="server" Text="1134710523" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="lblEmail" class="form-label d-block">Email</label>
                        <asp:Label ID="lblEmail" runat="server" Text="juanperez@gmail.com" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                </div>

                <div class="row g-3 mb-3">
                    <div class="col-12 col-md-4">
                        <label for="lblCobertura" class="form-label d-block">Cobertura</label>
                        <asp:Label ID="lblCobertura" runat="server" Text="OSDE" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="lblPlan" class="form-label d-block">Plan</label>
                        <asp:Label ID="lblPlan" runat="server" Text="310" CssClass="fw-semibold text-dark d-block"></asp:Label>
                    </div>
                    <div class="col-12 col-md-4">
                        <label for="lblEstado" class="form-label d-block">Estado</label>
                        <asp:Label ID="lblEstado" runat="server" CssClass="badge badge-success">Activo</asp:Label>
                    </div>
                </div>

            </div>

            <hr />
            <%--historial de turnos del paciente--%>
            <div>
                <div class="row mb-3">
                    <h2 class="fs-4">Historial de Turnos</h2>
                </div>

                <div class="row">
                    <asp:GridView
                        ID="gvTurnosPaciente" runat="server" AutoGenerateColumns="False"
                        CssClass="table gridview mb-0"
                        AllowPaging="True" PageSize="5"
                        OnPageIndexChanging="gvTurnosPaciente_PageIndexChanging"
                        OnRowCommand="gvTurnosPaciente_RowCommand">

                        <Columns>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                            <asp:BoundField DataField="Hora" HeaderText="Hora" />
                            <asp:BoundField DataField="Profesional" HeaderText="Profesional" />
                            <asp:BoundField DataField="Motivo" HeaderText="Motivo" />

                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <span id="lblEstado" runat="server" class="badge"><%# Eval("EstadoTexto") %></span>
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

        </div>
    </div>

</asp:Content>
