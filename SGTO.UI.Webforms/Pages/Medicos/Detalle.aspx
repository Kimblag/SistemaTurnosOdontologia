<%@ Page Title="Detalle de Médico" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Detalle.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Medicos.Detalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <div class="d-flex justify-content-between align-items-center pt-3 pb-2 mb-3 border-bottom">
            <h1 class="h2">Ficha del Médico</h1>
            <div>
                <asp:HyperLink ID="lnkVolver" runat="server" NavigateUrl="~/Pages/Medicos/Medicos.aspx" CssClass="btn btn-outline-secondary me-2">
                    <i class="bi bi-arrow-left"></i> Volver
                </asp:HyperLink>
                <asp:HyperLink ID="lnkEditar" runat="server" CssClass="btn btn-primary">
                    <i class="bi bi-pencil"></i> Editar
                </asp:HyperLink>
            </div>
        </div>

        <div class="row g-4">
            <div class="col-md-6">
                <div class="card h-100 shadow-sm border-0">
                    <div class="card-header bg-white border-bottom-0 pt-4 pb-0">
                        <div class="d-flex align-items-center">
                            <div class="bg-primary bg-opacity-10 p-3 rounded-circle text-primary me-3">
                                <i class="bi bi-person-vcard fs-3"></i>
                            </div>
                            <h5 class="mb-0 fw-bold text-primary">Información Personal</h5>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="mb-3">
                            <label class="small text-muted text-uppercase fw-bold">Nombre Completo</label>
                            <p class="lead fw-normal mb-0"><asp:Label ID="lblNombre" runat="server" /></p>
                        </div>
                        
                        <div class="row">
                            <div class="col-6 mb-3">
                                <label class="small text-muted text-uppercase fw-bold">DNI</label>
                                <p class="mb-0"><asp:Label ID="lblDni" runat="server" /></p>
                            </div>
                            <div class="col-6 mb-3">
                                <label class="small text-muted text-uppercase fw-bold">Fecha Nacimiento</label>
                                <p class="mb-0"><asp:Label ID="lblNacimiento" runat="server" /></p>
                            </div>
                        </div>

                        <div class="mb-3">
                             <label class="small text-muted text-uppercase fw-bold">Género</label>
                             <p class="mb-0"><asp:Label ID="lblGenero" runat="server" /></p>
                        </div>

                        <hr class="text-muted opacity-25">

                        <div class="mb-3">
                            <label class="small text-muted text-uppercase fw-bold">Contacto</label>
                            <p class="mb-1"><i class="bi bi-envelope me-2 text-muted"></i><asp:Label ID="lblEmail" runat="server" /></p>
                            <p class="mb-0"><i class="bi bi-telephone me-2 text-muted"></i><asp:Label ID="lblTelefono" runat="server" /></p>
                        </div>

                        <div class="mb-0">
                            <label class="small text-muted text-uppercase fw-bold">Estado</label>
                            <div><asp:Label ID="lblEstado" runat="server" CssClass="badge" /></div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="card h-100 shadow-sm border-0">
                    <div class="card-header bg-white border-bottom-0 pt-4 pb-0">
                        <div class="d-flex align-items-center">
                            <div class="bg-success bg-opacity-10 p-3 rounded-circle text-success me-3">
                                <i class="bi bi-briefcase fs-3"></i>
                            </div>
                            <h5 class="mb-0 fw-bold text-success">Perfil Profesional</h5>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-6 mb-3">
                                <label class="small text-muted text-uppercase fw-bold">Matrícula</label>
                                <p class="fw-bold mb-0"><asp:Label ID="lblMatricula" runat="server" /></p>
                            </div>
                            <div class="col-6 mb-3">
                                <label class="small text-muted text-uppercase fw-bold">Incorporación</label>
                                <p class="mb-0"><asp:Label ID="lblFechaAlta" runat="server" /></p>
                            </div>
                        </div>

                        <div class="mb-3">
                            <label class="small text-muted text-uppercase fw-bold">Usuario de Sistema</label>
                            <p class="mb-0"><asp:Label ID="lblUsuario" runat="server" CssClass="fst-italic text-dark" /></p>
                        </div>

                        <div class="mb-3">
                            <label class="small text-muted text-uppercase fw-bold">Especialidades</label>
                            <p class="mb-0"><asp:Label ID="lblEspecialidades" runat="server" Text="-" /></p>
                        </div>

                        <div class="mb-3">
                            <label class="small text-muted text-uppercase fw-bold">Coberturas Aceptadas</label>
                            <p class="small text-muted fst-italic mb-1" style="font-size: 0.75rem;">(Basado en historial de atención)</p>
                            <p class="mb-0"><asp:Label ID="lblCoberturas" runat="server" Text="Ninguna registrada" /></p>
                        </div>

                        <div class="p-3 bg-light rounded border mt-4">
                            <div class="d-flex justify-content-between align-items-center">
                                <span class="fw-bold text-secondary">Total Pacientes Atendidos</span>
                                <span class="badge bg-primary rounded-pill fs-6">
                                    <asp:Label ID="lblTotalPacientes" runat="server" Text="0" />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mt-4">
            <div class="col-12">
                <div class="card shadow-sm border-0">
                    <div class="card-header bg-light py-3">
                        <h5 class="mb-0">Historial de Turnos Atendidos</h5>
                    </div>
                    <div class="card-body p-0">
                        <asp:GridView ID="gvHistorial" runat="server" 
                            AutoGenerateColumns="false" 
                            CssClass="table table-hover table-striped mb-0" 
                            GridLines="None" 
                            EmptyDataText="Este médico aún no ha atendido turnos.">
                            <Columns>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                <asp:BoundField DataField="Hora" HeaderText="Hora" />
                                <asp:BoundField DataField="Paciente" HeaderText="Paciente" HeaderStyle-CssClass="fw-bold" />
                                <asp:BoundField DataField="Tratamiento" HeaderText="Tratamiento" />
                                <asp:BoundField DataField="Cobertura" HeaderText="Cobertura" />
                                <asp:TemplateField HeaderText="Estado">
                                    <ItemTemplate>
                                        <span class="badge bg-secondary"><%# Eval("Estado") %></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>