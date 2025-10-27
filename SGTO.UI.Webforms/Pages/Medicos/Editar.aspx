<%@ Page Title="Editar Médico" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" 
    CodeBehind="Editar.aspx.cs" 
    Inherits="SGTO.UI.Webforms.Pages.Medicos.Editar" %> 

<%-- Registrar el user control de Médico --%>
<%@ Register Src="~/Controles/Medicos/MedicoForm.ascx" TagPrefix="uc1" TagName="MedicoForm" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%-- Título de la página --%>
    <h4 class="mb-4">Editar Médico</h4>

    <div class="h-100 w-100 d-flex justify-content-center"> 
        <%-- Usar el control de Médico --%>
        <uc1:MedicoForm ID="MedicoFormControl" runat="server" /> 
    </div> 

</asp:Content>