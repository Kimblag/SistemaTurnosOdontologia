<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Configuracion.Configuracion" %>

<%--registro de controles--%>
<%@ Register Src="~/Controles/Configuracion/Usuarios/UsuariosListado.ascx" TagPrefix="uc1" TagName="UsuariosListado" %>
<%@ Register Src="~/Controles/Configuracion/Roles/RolesListado.ascx" TagPrefix="uc1" TagName="RolesListado" %>
<%@ Register Src="~/Controles/Configuracion/ParametrosSistema/ParametrosForm.ascx" TagPrefix="uc1" TagName="ParametrosSistemaForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic">

        <%--Tabs de navegación--%>
        <ul class="nav nav-tabs" id="configuracionTabs" role="tablist">
            <li class="nav-item" role="presentation">
                <button class="nav-link active"
                    id="tab-usuarios"
                    data-bs-toggle="tab"
                    data-bs-target="#pane-usuarios"
                    type="button"
                    role="tab"
                    aria-controls="pane-usuarios"
                    aria-selected="true">
                    Usuarios
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link"
                    id="tab-roles"
                    data-bs-toggle="tab"
                    data-bs-target="#pane-roles"
                    type="button"
                    role="tab"
                    aria-controls="pane-roles"
                    aria-selected="false">
                    Roles
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link"
                    id="tab-parametros"
                    data-bs-toggle="tab"
                    data-bs-target="#pane-parametros"
                    type="button"
                    role="tab"
                    aria-controls="pane-parametros"
                    aria-selected="false">
                    Parámetros del Sistema
                </button>
            </li>
        </ul>

        <%--Contenido de las pestañas--%>
        <div class="tab-content" id="configuracionTabsContent">
            <div class="tab-pane fade show active"
                id="pane-usuarios"
                role="tabpanel"
                aria-labelledby="tab-usuarios">
                <uc1:UsuariosListado ID="usuariosListadoControl" runat="server" />
            </div>

            <div class="tab-pane fade"
                id="pane-roles"
                role="tabpanel"
                aria-labelledby="tab-roles">
                <uc1:RolesListado ID="rolesListadoControl" runat="server" />
            </div>

            <div class="tab-pane fade"
                id="pane-parametros"
                role="tabpanel"
                aria-labelledby="tab-parametros">
                <uc1:ParametrosSistemaForm ID="ParametrosSistemaForm" runat="server" />
            </div>
        </div>

    </div>
</asp:Content>
