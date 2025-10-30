<%@ Page Title="Agregar Nueva Especialidad" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" 
CodeBehind="Nuevo.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Especialidades.Nuevo" %>

<%--registrar el user control--%>
<%@ Register Src="~/Controles/Especialidades/EspecialidadForm.ascx" TagPrefix="uc1" TagName="EspecialidadForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <uc1:EspecialidadForm ID="EspecialidadFormControl" runat="server" />

</asp:Content>