<%@ Page Title="Configuración - Editar Rol" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Editar.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Configuracion.Roles.Editar" %>

<%--registrar el user control--%>
<%@ Register Src="~/Controles/Configuracion/Roles/RolesForm.ascx" TagPrefix="uc1" TagName="RolesForm" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:RolesForm ID="RolesFormControl" runat="server" />
</asp:Content>
