<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Perfil.MiPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container py-4">

        <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert alert-dismissible fade show mb-4" role="alert">
            <asp:Label ID="lblMensaje" runat="server"></asp:Label>
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </asp:Panel>

        <div class="row g-4">
            <div class="col-md-5 col-lg-4">
                <div class="card border-0 shadow-sm h-100">
                    <div class="card-body text-center p-4">
                        <div class="mb-3">
                            <div class="d-inline-flex align-items-center justify-content-center bg-light rounded-circle text-primary" style="width: 100px; height: 100px;">
                                <i class="bi bi-person-circle" style="font-size: 3.5rem;"></i>
                            </div>
                        </div>
                        <h4 class="fw-bold mb-1">
                            <asp:Label ID="lblNombreCompleto" runat="server"></asp:Label></h4>
                        <p class="text-muted mb-3">
                            <asp:Label ID="lblRol" runat="server"></asp:Label></p>

                        <hr class="my-4" />

                        <div class="text-start">
                            <div class="mb-3">
                                <label class="small text-muted fw-bold">Usuario</label>
                                <p class="mb-0 fw-medium">
                                    <asp:Label ID="lblUsuario" runat="server"></asp:Label></p>
                            </div>
                            <div class="mb-3">
                                <label class="small text-muted fw-bold">Email</label>
                                <p class="mb-0 fw-medium">
                                    <asp:Label ID="lblEmail" runat="server"></asp:Label></p>
                            </div>
                            <div class="mb-3">
                                <label class="small text-muted fw-bold">Estado</label>
                                <p class="mb-0"><span class="badge bg-success bg-opacity-10 text-success px-3 py-2">Activo</span></p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-7 col-lg-8">
                <div class="card border-0 shadow-sm h-100">
                    <div class="card-header bg-white py-3 border-bottom-0">
                        <h5 class="mb-0 fw-bold text-dark"><i class="bi bi-shield-lock me-2"></i>Seguridad</h5>
                    </div>
                    <div class="card-body p-4">
                        <h6 class="text-primary fw-bold mb-3">Cambiar contraseña</h6>
                        <p class="small text-muted mb-4">Para actualizar tu contraseña, ingresa tu clave actual y la nueva que deseas utilizar.</p>

                        <div class="row g-3">
                            <div class="col-12">
                                <label class="form-label">Contraseña Actual <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtPassActual" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassActual"
                                    ErrorMessage="Requerido" CssClass="text-danger small" ValidationGroup="PassGroup" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-6">
                                <label class="form-label">Nueva Contraseña <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtPassNueva" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassNueva"
                                    ErrorMessage="Requerido" CssClass="text-danger small" ValidationGroup="PassGroup" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-6">
                                <label class="form-label">Confirmar Nueva Contraseña <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtPassConfirmar" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassConfirmar"
                                    ErrorMessage="Requerido" CssClass="text-danger small" ValidationGroup="PassGroup" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CompareValidator runat="server" ControlToValidate="txtPassConfirmar" ControlToCompare="txtPassNueva"
                                    ErrorMessage="Las contraseñas no coinciden" CssClass="text-danger small" ValidationGroup="PassGroup" Display="Dynamic"></asp:CompareValidator>
                            </div>

                            <div class="col-12 mt-4 text-end">
                                <asp:Button ID="btnGuardarPass" runat="server" Text="Actualizar Contraseña"
                                    CssClass="btn btn-primary px-4" OnClick="btnGuardarPass_Click" ValidationGroup="PassGroup" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
