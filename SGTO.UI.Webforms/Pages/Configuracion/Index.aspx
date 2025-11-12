<%@ Page Title="Configuración" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Configuracion.Configuracion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

 <div class="page-generic configuracion-page justify-content-center">
        <div class="row g-4">

            <!-- Usuarios -->
            <div class="col-12 col-md-6 col-lg-4">
                <a href='<%= ResolveUrl("~/Pages/Configuracion/Usuarios/Index.aspx") %>' class="card card-config bg-soft-primary shadow-sm border-0 text-decoration-none text-dark h-100">
                    <div class="card-body text-center d-flex flex-column justify-content-between">
                        <div>
                            <div class="icon-wrapper bg-primary bg-opacity-10 text-primary mb-3">
                                <i class="bi bi-person-gear"></i>
                            </div>
                            <h5 class="card-title fw-semibold">Gestión de Usuarios</h5>
                            <p class="card-text text-muted">Cree, edite y administre los usuarios del sistema.</p>
                        </div>
                        <div class="mt-3 fw-semibold text-primary d-inline-flex align-items-center justify-content-center">
                            Ir a Usuarios <i class="bi bi-arrow-right-circle ms-2"></i>
                        </div>
                    </div>
                </a>
            </div>

            <!-- Roles -->
            <div class="col-12 col-md-6 col-lg-4">
                <a href='<%= ResolveUrl("~/Pages/Configuracion/Roles/Index.aspx") %>' class="card card-config bg-soft-success shadow-sm border-0 text-decoration-none text-dark h-100">
                    <div class="card-body text-center d-flex flex-column justify-content-between">
                        <div>
                            <div class="icon-wrapper bg-success bg-opacity-10 text-success mb-3">
                                <i class="bi bi-shield-lock"></i>
                            </div>
                            <h5 class="card-title fw-semibold">Gestión de Roles</h5>
                            <p class="card-text text-muted">Defina los roles y asigne permisos a los usuarios.</p>
                        </div>
                        <div class="mt-3 fw-semibold text-success d-inline-flex align-items-center justify-content-center">
                            Ir a Roles <i class="bi bi-arrow-right-circle ms-2"></i>
                        </div>
                    </div>
                </a>
            </div>

            <!-- Parámetros -->
            <div class="col-12 col-md-6 col-lg-4">
                <a href='<%= ResolveUrl("~/Pages/Configuracion/Parametros/Index.aspx") %>' class="card card-config bg-soft-warning shadow-sm border-0 text-decoration-none text-dark h-100">
                    <div class="card-body text-center d-flex flex-column justify-content-between">
                        <div>
                            <div class="icon-wrapper bg-warning bg-opacity-10 text-warning mb-3">
                                <i class="bi bi-gear-fill"></i>
                            </div>
                            <h5 class="card-title fw-semibold">Parámetros del Sistema</h5>
                            <p class="card-text text-muted">Configure reglas globales como duración de turnos.</p>
                        </div>
                        <div class="mt-3 fw-semibold text-warning d-inline-flex align-items-center justify-content-center">
                            Ir a Parámetros <i class="bi bi-arrow-right-circle ms-2"></i>
                        </div>
                    </div>
                </a>
            </div>

        </div>
    </div>
</asp:Content>
