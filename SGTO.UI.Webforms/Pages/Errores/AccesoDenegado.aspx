<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="AccesoDenegado.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Errores.AccesoDenegado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex flex-column align-items-center justify-content-center text-center mt-5">

        <div class="mb-4">
            <i class="bi bi-shield-lock-fill text-danger" style="font-size: 5rem;"></i>
        </div>

        <h1 class="display-5 fw-bold text-dark mb-3">Acceso Restringido</h1>

        <p class="lead text-muted mb-4" style="max-width: 600px;">
            Lo sentimos, tu perfil de usuario no cuenta con los permisos necesarios para acceder a esta sección del sistema.
       
        </p>

        <div class="d-flex gap-3">
            <a href="<%= ResolveUrl("~/Pages/Home/Dashboard.aspx") %>" class="btn btn-primary btn-lg px-4">
                <i class="bi bi-house-door-fill me-2"></i>Ir al Inicio
            </a>
        </div>

    </div>
</asp:Content>
