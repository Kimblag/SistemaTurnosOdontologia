<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Reportes.Reportes" %>

<%--registro de controles--%>
<%@ Register Src="~/Controles/Reportes/Turnos/ReporteTurnos.ascx" TagPrefix="uc1" TagName="ReporteTurnos" %>
<%@ Register Src="~/Controles/Reportes/Pacientes/ReportePacientes.ascx" TagPrefix="uc1" TagName="ReportePacientes" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="page-generic">

        <%--Tabs de navegación--%>
        <ul class="nav nav-tabs" id="ReportesTabs" role="tablist">

            <li class="nav-item" role="presentation">
                <button class="nav-link active"
                    id="tab-turnos"
                    data-bs-toggle="tab"
                    data-bs-target="#pane-turnos"
                    type="button"
                    role="tab"
                    aria-controls="pane-turnos"
                    aria-selected="true">
                    Turnos
                </button>
            </li>

            <li class="nav-item" role="presentation">
                <button class="nav-link "
                    id="tab-pacientes"
                    data-bs-toggle="tab"
                    data-bs-target="#pane-pacientes"
                    type="button"
                    role="tab"
                    aria-controls="pane-pacientes"
                    aria-selected="false">
                    Pacientes
                </button>
            </li>

        </ul>

        <%--Contenido de las pestañas--%>
        <div class="tab-content" id="reporteTabsContent">

            <div class="tab-pane fade show active"
                id="pane-turnos"
                role="tabpanel"
                aria-labelledby="tab-turnos">
                <uc1:ReporteTurnos ID="reporteTurnosControl" runat="server" />
            </div>

            <div class="tab-pane fade"
                id="pane-pacientes"
                role="tabpanel"
                aria-labelledby="tab-pacientes">
                <uc1:ReportePacientes ID="reportePacientesControl" runat="server" />
            </div>

        </div>

    </div>
</asp:Content>
