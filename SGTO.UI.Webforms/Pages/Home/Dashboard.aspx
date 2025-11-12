<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Home.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <div class="container py-4">

        <div class="row text-center mb-4">
            <div class="col-md-3">
                <div class="card border-primary">
                    <div class="card-body">
                        <h5 class="card-title">Turnos del día</h5>
                        <h2 class="text-primary"><%= KpiTurnosDia %></h2>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card border-success">
                    <div class="card-body">
                        <h5 class="card-title">Pacientes atendidos</h5>
                        <h2 class="text-success"><%= KpiPacientesAtendidos %></h2>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card border-warning">
                    <div class="card-body">
                        <h5 class="card-title">Reprogramados</h5>
                        <h2 class="text-warning"><%= KpiReprogramados %></h2>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card border-danger">
                    <div class="card-body">
                        <h5 class="card-title">Cancelados</h5>
                        <h2 class="text-danger"><%= KpiCancelados %></h2>
                    </div>
                </div>
            </div>
        </div>


        <div class="card">
            <div class="card-body">
                <h5 class="card-title mb-3">Actividad semanal</h5>
                <canvas id="graficoActividad"></canvas>
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
                    datasets: [{
                        label: 'Turnos',
                        data: [<%= ValoresCsv %>],
                        backgroundColor: [
                            '#0d6efd', '#198754', '#ffc107', '#dc3545', '#6f42c1', '#20c997'
                        ]
                    }]
                },
                options: {
                    responsive: true,
                    scales: {
                        y: { beginAtZero: true }
                    }
                }
            });
        });
    </script>

</asp:Content>
