<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Nuevo.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Configuracion.Roles.Nuevo" %>

<%--registrar el user control--%>
<%@ Register Src="~/Controles/Configuracion/Roles/RolesForm.ascx" TagPrefix="uc1" TagName="RolesForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:RolesForm ID="RolesFormControl" runat="server" />
</asp:Content>
