<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Home.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%--librerias de highcharts--%>
    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/highcharts-more.js"></script>
    <script src="https://code.highcharts.com/modules/accessibility.js"></script>
    <script src="https://code.highcharts.com/modules/solid-gauge.js"></script>
    <script src="https://code.highcharts.com/dashboards/dashboards.js"></script>
    <script src="https://code.highcharts.com/dashboards/modules/layout.js"></script>
    <link rel="stylesheet" href="https://code.highcharts.com/dashboards/css/dashboards.css">

    <div class="p-4">
        <%--actividad semanal--%>
        <div id="dashboard-container" class="w-100"></div>
    </div>


    <script type="text/javascript">

        document.addEventListener("DOMContentLoaded", function () {

            Highcharts.setOptions({
                chart: { styledMode: true }
            });

            const board = Dashboards.board('dashboard-container', {
                components: [
                    // ==== KPIs (Cards) ====
                    {
                        renderTo: 'kpi-turnos-dia',
                        type: 'KPI',
                        title: 'Turnos del día',
                        value: 12,
                        subtitle: '+20%',
                        className: 'text-dia'

                    },
                    {
                        renderTo: 'kpi-pacientes',
                        type: 'KPI',
                        title: 'Pacientes atendidos',
                        value: 8,
                        subtitle: '-10%',
                        className: 'text-pacientes'
                    },
                    {
                        renderTo: 'kpi-reprogramados',
                        type: 'KPI',
                        title: 'Reprogramados',
                        value: 2,
                        subtitle: '+5%',
                        className: 'text-reprogramados'
                    },
                    {
                        renderTo: 'kpi-cancelados',
                        type: 'KPI',
                        title: 'Cancelados',
                        value: 1,
                        subtitle: '-15%',
                        className: 'text-cancelados'
                    },

                    // ==== Gráfico de Actividad Semanal ====
                    {
                        renderTo: 'grafico-actividad',
                        type: 'Highcharts',
                        chartOptions: {
                            chart: { styledMode: true, type: "column" },
                            title: { text: 'Actividad semanal' },
                            xAxis: {
                                categories: ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado']
                            },
                            yAxis: {
                                title: { text: 'Cantidad de turnos' }
                            },
                            series: [{
                                name: 'Turnos',
                                data: [10, 20, 15, 25, 18, 7],
                                colorByPoint: true
                            }]
                        }
                    }
                ],

                // ==== Layout del Dashboard ====
                gui: {
                    layouts: [{
                        id: 'layout-1',
                        rows: [
                            {
                                id: "row-1", // selector id para poder aplicar estilos de css
                                cells: [
                                    { id: 'kpi-turnos-dia' },
                                    { id: 'kpi-pacientes' },
                                    { id: 'kpi-reprogramados' },
                                    { id: 'kpi-cancelados' }
                                ]
                            },
                            {
                                cells: [{ id: 'grafico-actividad' }]
                            }
                        ]
                    }]
                }
            });

        });
    </script>
</asp:Content>
