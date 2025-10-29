<%@ Page Title="Editar Cobertura" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EditarCobertura.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.CoberturasPlanes.EditarCobertura" %>

<%--registrar el user control--%>
<%@ Register Src="~/Controles/Coberturas/CoberturasForm.ascx" TagPrefix="uc1" TagName="CoberturasForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <uc1:CoberturasForm ID="CoberturasFormControl" runat="server" />

</asp:Content>
