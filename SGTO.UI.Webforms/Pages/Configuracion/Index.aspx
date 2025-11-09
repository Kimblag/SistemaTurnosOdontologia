<%@ Page Title="Configuración" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Configuracion.Configuracion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic">

        <div class="row g-4">

            <!-- Usuarios -->
            <div class="col-12 col-md-4">
                <div class="card card-config h-100 shadow-sm border-0">
                    <div class="card-body d-flex flex-column justify-content-between text-center">
                        <div>
                            <div class="icon-wrapper bg-primary bg-opacity-10 text-primary mb-3">
                                <i class="bi bi-person-gear"></i>
                            </div>
                            <h5 class="card-title fw-semibold">Gestión de Usuarios</h5>
                            <p class="card-text text-muted">
                                Cree, edite y administre los usuarios del sistema, así como sus accesos.
                            </p>
                        </div>
                        <a href='<%= ResolveUrl("~/Pages/Configuracion/Usuarios/Index.aspx") %>' class="btn btn-outline-primary mt-3">
                            <i class="bi bi-arrow-right-circle me-1"></i>Ir a Usuarios
                        </a>
                    </div>
                </div>
            </div>

            <!-- Roles -->
            <div class="col-12 col-md-4">
                <div class="card card-config h-100 shadow-sm border-0">
                    <div class="card-body d-flex flex-column justify-content-between text-center">
                        <div>
                            <div class="icon-wrapper bg-success bg-opacity-10 text-success mb-3">
                                <i class="bi bi-shield-lock"></i>
                            </div>
                            <h5 class="card-title fw-semibold">Gestión de Roles</h5>
                            <p class="card-text text-muted">
                                Defina los roles y asigne permisos a los distintos tipos de usuarios.
                            </p>
                        </div>
                        <a href='<%= ResolveUrl("~/Pages/Configuracion/Roles/Index.aspx") %>' class="btn btn-outline-success mt-3">
                            <i class="bi bi-arrow-right-circle me-1"></i>Ir a Roles
                        </a>
                    </div>
                </div>
            </div>

            <!-- Parámetros del sistema -->
            <div class="col-12 col-md-4">
                <div class="card card-config h-100 shadow-sm border-0">
                    <div class="card-body d-flex flex-column justify-content-between text-center">
                        <div>
                            <div class="icon-wrapper bg-warning bg-opacity-10 text-warning mb-3">
                                <i class="bi bi-gear-fill"></i>
                            </div>
                            <h5 class="card-title fw-semibold">Parámetros del Sistema</h5>
                            <p class="card-text text-muted">
                                Configure valores globales como duración de turnos o reglas generales.
                            </p>
                        </div>
                        <a href='<%= ResolveUrl("~/Pages/Configuracion/Parametros/Index.aspx") %>' class="btn btn-outline-warning mt-3">
                            <i class="bi bi-arrow-right-circle me-1"></i>Ir a Parámetros
                        </a>
                    </div>
                </div>
            </div>

        </div>


    </div>
</asp:Content>
