<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Home.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <div class="container-fluid">

        <div class="row g-3 text-center mb-4">

            <div class="col-12 col-sm-6 col-lg-3">
                <div class="card border-primary h-100 shadow-sm card-hover">
                    <div class="card-body d-flex flex-column justify-content-center">
                        <h5 class="card-title kpi-title">Turnos del día</h5>
                        <div class="mt-2">
                            <h2 class="kpi-number text-primary"><%= KpiTurnosDia %></h2>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-12 col-sm-6 col-lg-3">
                <div class="card border-success h-100 shadow-sm card-hover">
                    <div class="card-body d-flex flex-column justify-content-center">
                        <h5 class="card-title kpi-title">Pacientes atendidos</h5>
                        <div class="mt-2">
                            <h2 class="kpi-number text-success"><%= KpiPacientesAtendidos %></h2>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-12 col-sm-6 col-lg-3">
                <div class="card border-warning h-100 shadow-sm card-hover">
                    <div class="card-body d-flex flex-column justify-content-center">
                        <h5 class="card-title kpi-title">Reprogramados</h5>
                        <div class="mt-2">
                            <h2 class="kpi-number text-warning"><%= KpiReprogramados %></h2>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-12 col-sm-6 col-lg-3">
                <div class="card border-danger h-100 shadow-sm card-hover">
                    <div class="card-body d-flex flex-column justify-content-center">
                        <h5 class="card-title kpi-title">Cancelados</h5>
                        <div class="mt-2">
                            <h2 class="kpi-number text-danger"><%= KpiCancelados %></h2>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-12">
                <div class="card shadow-sm border-0">
                    <div class="card-body">
                        <h5 class="card-title mb-4 text-muted fw-bold">Actividad Semanal</h5>

                        <div class="chart-container">
                            <canvas id="graficoActividad"></canvas>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            const ctx = document.getElementById('graficoActividad').getContext('2d');
            const chart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: [<%= CategoriasCsv %>],
                    datasets: [
                        {
                            label: 'Nuevos',
                            data: [<%= DataNuevos %>],
                            backgroundColor: '#0d6efd',
                        },
                        {
                            label: 'Reprogramados',
                            data: [<%= DataReprogramados %>],
                            backgroundColor: '#ffc107',
                        },
                        {
                            label: 'Atendidos/Cerrados',
                            data: [<%= DataCerrados %>],
                            backgroundColor: '#198754',
                        },
                        {
                            label: 'Cancelados',
                            data: [<%= DataCancelados %>],
                            backgroundColor: '#dc3545',
                        }
                    ]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        x: {
                            stacked: true
                        },
                        y: {
                            stacked: true,
                            beginAtZero: true
                        }
                    }
                }
            });
        });
    </script>

</asp:Content>
