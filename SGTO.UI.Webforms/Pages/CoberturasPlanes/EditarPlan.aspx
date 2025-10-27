<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EditarPlan.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.CoberturasPlanes.EditarPlan" %>

<%--registrar el user control--%>
<%@ Register Src="~/Controles/Coberturas/PlanesForm.ascx" TagPrefix="uc1" TagName="PlanesForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:PlanesForm ID="PlanesFormControl" runat="server" />
</asp:Content>
