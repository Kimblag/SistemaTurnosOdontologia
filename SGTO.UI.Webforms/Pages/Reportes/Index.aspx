<%@ Page Title="Reportes" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Reportes.Reportes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic reportes-page justify-content-center">
        <div class="row g-4">

            <div class="col-12 col-md-6 col-lg-4">
                <a href='<%= ResolveUrl("~/Pages/Reportes/Turnos.aspx") %>'
                    class="card card-report bg-soft-primary shadow-sm border-0 text-decoration-none text-dark h-100">
                    <div class="card-body text-center d-flex flex-column justify-content-between">
                        <div>
                            <div class="icon-wrapper bg-primary bg-opacity-10 text-primary mb-3">
                                <i class="bi bi-calendar-check"></i>
                            </div>
                            <h5 class="card-title fw-semibold">Turnos</h5>
                            <p class="card-text text-muted">
                                Genere reportes por fecha, estado, médico, especialidad o cobertura.
                            </p>
                        </div>
                        <div class="mt-3 fw-semibold text-primary d-inline-flex align-items-center justify-content-center">
                            Ir a Reporte <i class="bi bi-arrow-right-circle ms-2"></i>
                        </div>
                    </div>
                </a>
            </div>

    
            <div class="col-12 col-md-6 col-lg-4">
                <a href='<%= ResolveUrl("~/Pages/Reportes/Pacientes.aspx") %>'
                    class="card card-report bg-soft-success shadow-sm border-0 text-decoration-none text-dark h-100">
                    <div class="card-body text-center d-flex flex-column justify-content-between">
                        <div>
                            <div class="icon-wrapper bg-success bg-opacity-10 text-success mb-3">
                                <i class="bi bi-person-vcard"></i>
                            </div>
                            <h5 class="card-title fw-semibold">Pacientes</h5>
                            <p class="card-text text-muted">
                                Consulte pacientes por cobertura, plan o fecha de alta y analice su distribución.
                            </p>
                        </div>
                        <div class="mt-3 fw-semibold text-success d-inline-flex align-items-center justify-content-center">
                            Ir a Reporte <i class="bi bi-arrow-right-circle ms-2"></i>
                        </div>
                    </div>
                </a>
            </div>


            <div class="col-12 col-md-6 col-lg-4">
                <a href='<%= ResolveUrl("~/Pages/Reportes/Medicos.aspx") %>'
                    class="card card-report bg-soft-info shadow-sm border-0 text-decoration-none text-dark h-100">
                    <div class="card-body text-center d-flex flex-column justify-content-between">
                        <div>
                            <div class="icon-wrapper bg-info bg-opacity-10 text-info mb-3">
                                <i class="bi bi-person-badge"></i>
                            </div>
                            <h5 class="card-title fw-semibold">Médicos</h5>
                            <p class="card-text text-muted">
                                Evalúe productividad, asistencia y carga de trabajo de cada médico.
                            </p>
                        </div>
                        <div class="mt-3 fw-semibold text-info d-inline-flex align-items-center justify-content-center">
                            Ir a Reporte <i class="bi bi-arrow-right-circle ms-2"></i>
                        </div>
                    </div>
                </a>
            </div>


            <div class="col-12 col-md-6 col-lg-4">
                <a href='<%= ResolveUrl("~/Pages/Reportes/Tratamientos.aspx") %>'
                    class="card card-report bg-soft-warning shadow-sm border-0 text-decoration-none text-dark h-100">
                    <div class="card-body text-center d-flex flex-column justify-content-between">
                        <div>
                            <div class="icon-wrapper bg-warning bg-opacity-10 text-warning mb-3">
                                <i class="bi bi-heart-pulse"></i>
                            </div>
                            <h5 class="card-title fw-semibold">Tratamientos</h5>
                            <p class="card-text text-muted">
                                Analice los tratamientos más frecuentes y su relación con coberturas y especialidades.
                            </p>
                        </div>
                        <div class="mt-3 fw-semibold text-warning d-inline-flex align-items-center justify-content-center">
                            Ir a Reporte <i class="bi bi-arrow-right-circle ms-2"></i>
                        </div>
                    </div>
                </a>
            </div>


            <div class="col-12 col-md-6 col-lg-4">
                <a href='<%= ResolveUrl("~/Pages/Reportes/Coberturas.aspx") %>'
                    class="card card-report bg-soft-danger shadow-sm border-0 text-decoration-none text-dark h-100">
                    <div class="card-body text-center d-flex flex-column justify-content-between">
                        <div>
                            <div class="icon-wrapper bg-danger bg-opacity-10 text-danger mb-3">
                                <i class="bi bi-shield-check"></i>
                            </div>
                            <h5 class="card-title fw-semibold">Coberturas y Planes</h5>
                            <p class="card-text text-muted">
                                Consulte la distribución de turnos y pacientes según cobertura o plan de salud.
                            </p>
                        </div>
                        <div class="mt-3 fw-semibold text-danger d-inline-flex align-items-center justify-content-center">
                            Ir a Reporte <i class="bi bi-arrow-right-circle ms-2"></i>
                        </div>
                    </div>
                </a>
            </div>

        </div>
    </div>

</asp:Content>
