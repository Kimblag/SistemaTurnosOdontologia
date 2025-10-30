<%@ Page Title="Editar Tratamiento" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" 
    CodeBehind="Editar.aspx.cs" 
    Inherits="SGTO.UI.Webforms.Pages.Tratamientos.Editar" %> 

<%-- Registrar el user control de Tratamiento --%>
<%@ Register Src="~/Controles/Tratamientos/TratamientoForm.ascx" TagPrefix="uc1" TagName="TratamientoForm" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        <uc1:TratamientoForm ID="TratamientoFormControl" runat="server" /> 


</asp:Content>