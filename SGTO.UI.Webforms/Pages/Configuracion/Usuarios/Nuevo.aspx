<%@ Page Title="Configuración - Nuevo Usuario" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Nuevo.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Configuracion.Usuarios.Nuevo" %>

<%--registrar el user control--%>
<%@ Register Src="~/Controles/Configuracion/Usuarios/UsuariosForm.ascx" TagPrefix="uc1" TagName="UsuariosForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <uc1:UsuariosForm ID="UsuariosFormControl" runat="server" />

</asp:Content>
