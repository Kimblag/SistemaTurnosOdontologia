<%@ Page Title="Agregar Nuevo Medico" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Nuevo.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Medicos.Nuevo" %>

<%--registrar el user control--%>
<%@ Register Src="~/Controles/Medicos/MedicoForm.ascx" TagPrefix="uc1" TagName="MedicoForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <uc1:MedicoForm ID="MedicoFormControl" runat="server" />

</asp:Content>