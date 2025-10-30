<%@ Page Title="Agregar Nuevo Tratamiento" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Nuevo.aspx.cs" 
    Inherits="SGTO.UI.Webforms.Pages.Tratamientos.Nuevo" %>

<%--registrar el user control--%>
<%@ Register Src="~/Controles/Tratamientos/TratamientoForm.ascx" TagPrefix="uc1" TagName="TratamientoForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <uc1:TratamientoForm ID="TratamientoFormControl" runat="server" />

</asp:Content>