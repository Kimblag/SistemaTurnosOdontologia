<%@ Page Title="Detalle de Rol" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Detalle.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Configuracion.Roles.Detalle" %>

<%@ Register Src="~/Controles/Configuracion/Roles/RolesForm.ascx" TagPrefix="uc" TagName="RolesForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <uc:RolesForm runat="server" ID="ucRolesForm" ModoLectura="true" />

</asp:Content>
