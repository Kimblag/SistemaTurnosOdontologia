<%@ Page Title="Configuración - Parámetros del Sistema" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Configuracion.Parametros.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="card shadow-sm px-3 py-5 gap-2 justify-content-between">

        <div class="col justify-content-between">

            <div class="mb-3">

                <div class="row g-3 mb-3 align-items-center">
                    <div class="col-md-4">
                        <label for="txtNombreClinica" class="form-label fw-semibold">Nombre de la Clínica</label>
                    </div>
                    <div class="col-md-8">
                        <asp:TextBox ID="txtNombreClinica" runat="server" CssClass="form-control" placeholder="Ingrese el nombre de la clínica" />
                    </div>
                </div>

              <%--  <div class="row g-3 mb-3 align-items-center">
                    <div class="col-md-4">
                        <label for="ddlDuracionTurno" class="form-label fw-semibold">Duración Estándar del Turno</label>
                    </div>
                    <div class="col-md-8">
                        <asp:DropDownList ID="ddlDuracionTurno" runat="server" CssClass="form-select">
                            <asp:ListItem Text="30 minutos" Value="30" />
                            <asp:ListItem Text="45 minutos" Value="45" />
                            <asp:ListItem Text="1 hora" Value="60" />
                        </asp:DropDownList>
                    </div>
                </div>--%>

              <%--  <div class="row g-3 mb-3 align-items-center">
                    <div class="col-md-4">
                        <label for="txtHorarioInicio" class="form-label fw-semibold">Horario de Inicio</label>
                    </div>
                    <div class="col-md-8">
                        <asp:TextBox ID="txtHorarioInicio" runat="server" CssClass="form-control" TextMode="Time" />
                    </div>
                </div>

                <div class="row g-3 mb-3 align-items-center">
                    <div class="col-md-4">
                        <label for="txtHorarioCierre" class="form-label fw-semibold">Horario de Cierre</label>
                    </div>
                    <div class="col-md-8">
                        <asp:TextBox ID="txtHorarioCierre" runat="server" CssClass="form-control" TextMode="Time" />
                    </div>
                </div>--%>

                <div class="row g-3 mb-3 align-items-center">
                    <div class="col-md-4">
                        <label for="txtServidorCorreo" class="form-label fw-semibold">Servidor SMTP</label>
                    </div>
                    <div class="col-md-8">
                        <asp:TextBox ID="txtServidorCorreo" runat="server" CssClass="form-control" placeholder="smtp.clinica.com" />
                    </div>
                </div>

                <div class="row g-3 mb-3 align-items-center">
                    <div class="col-md-4">
                        <label for="txtPuertoCorreo" class="form-label fw-semibold">Puerto SMTP</label>
                    </div>
                    <div class="col-md-8">
                        <asp:TextBox ID="txtPuertoCorreo" runat="server" CssClass="form-control" TextMode="Number" />
                    </div>
                </div>

                <div class="row g-3 mb-3 align-items-center">
                    <div class="col-md-4">
                        <label for="txtEmailRemitente" class="form-label fw-semibold">Email Remitente</label>
                    </div>
                    <div class="col-md-8">
                        <asp:TextBox ID="txtEmailRemitente" runat="server" CssClass="form-control" />
                    </div>
                </div>

                <div class="row g-3 mb-4 align-items-center">
                    <div class="col-md-4">
                        <label for="txtReintentosEmail" class="form-label fw-semibold">Reintentos de Envío de Email</label>
                    </div>
                    <div class="col-md-8">
                        <asp:TextBox ID="txtReintentosEmail" runat="server" CssClass="form-control" TextMode="Number" min="1" max="10" />
                    </div>
                </div>


            </div>

        </div>

        <%--botones--%>
        <div class="col align-content-end">
            <div class="row gx-2 gy-2 align-items-center justify-content-end">


                <div class="col-auto d-flex flex-wrap justify-content-end gap-2">

                    <asp:Button ID="btnCancelar" runat="server"
                        Text="Cancelar"
                        CssClass="btn btn-outline-secondary btn-sm"
                        OnClick="btnCancelar_Click" />

                    <asp:Button ID="btnGuardar" runat="server"
                        Text="Guardar"
                        CssClass="btn btn-primary btn-sm"
                        OnClick="btnGuardar_Click" />
                </div>
            </div>
        </div>

    </div>


    <%-- modal resultado --%>
    <div class="modal fade" id="modalResultado" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 id="modalResultadoTitulo" class="modal-title">Resultado</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <p id="modalResultadoDesc"></p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" id="btnModalCerrar" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>


    <script>

        document.addEventListener("DOMContentLoaded", () => {

            // modal resultado
            window.abrirModalResultado = function (titulo, descripcion) {
                try {
                    document.getElementById('modalResultadoTitulo').textContent = titulo || "Resultado";
                    document.getElementById('modalResultadoDesc').textContent = descripcion || "";
                    new bootstrap.Modal(document.getElementById('modalResultado')).show();
                } catch (err) {
                    console.error("Error al abrir modal de resultado:", err);
                }
            };
        });
    </script>

</asp:Content>
