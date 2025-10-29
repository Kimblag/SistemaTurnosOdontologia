<%@ Page Title="Coberturas y Planes" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.CoberturasPlanes.CoberturasPlanes" %>

<%--registro de controles--%>
<%@ Register Src="~/Controles/Coberturas/CoberturasListado.ascx" TagPrefix="uc1" TagName="CoberturasListado" %>
<%@ Register Src="~/Controles/Coberturas/PlanesListado.ascx" TagPrefix="uc1" TagName="PlanesListado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic">

        <%--Tabs de navegación--%>
        <ul class="nav nav-tabs" id="coberturasPlanesTabs" role="tablist">
            <li class="nav-item" role="presentation">
                <button class="nav-link active"
                    id="tab-coberturas"
                    data-bs-toggle="tab"
                    data-bs-target="#pane-coberturas"
                    type="button"
                    role="tab"
                    aria-controls="pane-coberturas"
                    aria-selected="true">
                    Coberturas
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link"
                    id="tab-planes"
                    data-bs-toggle="tab"
                    data-bs-target="#pane-planes"
                    type="button"
                    role="tab"
                    aria-controls="pane-planes"
                    aria-selected="false">
                    Planes
                </button>
            </li>
        </ul>

        <%--Contenido de las pestañas--%>
        <div class="tab-content" id="coberturasPlanesTabsContent">
            <div class="tab-pane fade show active"
                id="pane-coberturas"
                role="tabpanel"
                aria-labelledby="tab-coberturas">
                <uc1:CoberturasListado ID="CoberturasListadoControl" runat="server" />
            </div>

            <div class="tab-pane fade"
                id="pane-planes"
                role="tabpanel"
                aria-labelledby="tab-planes">
                <uc1:PlanesListado ID="PlanesListadoControl" runat="server" />
            </div>
        </div>

    </div>

</asp:Content>
