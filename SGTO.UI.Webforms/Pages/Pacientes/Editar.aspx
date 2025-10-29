<%@ Page Title="Editar Paciente" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Editar.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Pacientes.Editar" %>

<%--registrar el user control--%>
<%@ Register Src="~/Controles/Pacientes/PacienteForm.ascx" TagPrefix="uc1" TagName="PacienteForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <uc1:PacienteForm ID="PacienteFormControl" runat="server" />

</asp:Content>
