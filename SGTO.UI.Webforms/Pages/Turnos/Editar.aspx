<%@ Page Title="Editar Turno" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" 
    CodeBehind="Editar.aspx.cs" 
    Inherits="SGTO.UI.Webforms.Pages.Turnos.Editar" %> 


<%-- Registrar el user control de Turnos --%>
<%@ Register Src="~/Controles/Turnos/TurnoForm.ascx" TagPrefix="uc1" TagName="TurnoForm" %> 

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

        <uc1:TurnoForm ID="TurnoFormControl" runat="server" /> 


</asp:Content>