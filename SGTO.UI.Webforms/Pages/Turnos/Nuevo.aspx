<%@ Page Title="Agregar Nuevo Turno" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Nuevo.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Turnos.Nuevo" %>

<%--registrar el user control--%>
<%@ Register Src="~/Controles/Turnos/TurnoForm.ascx" TagPrefix="uc1" TagName="TurnoForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <uc1:TurnoForm ID="TurnoFormControl" runat="server" />

</asp:Content>