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

                <div class="row g-3 mb-3 align-items-center">
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
                </div>

                <div class="row g-3 mb-3 align-items-center">
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
                </div>

                <div class="row g-3 mb-3 align-items-center">
                    <div class="col-md-4">
                        <label for="txtServidorCorreo" class="form-label fw-semibold">Servidor SMTP</label>
                    </div>
                    <div class="col-md-8">
                        <asp:TextBox ID="txtServidorCorreo" runat="server" CssClass="form-control" placeholder="smtp.clinica.com" />
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
            <div class="row justify-content-end gx-2 gy-2">
                <%-- boton cancelar --%>
                <div class="col-6 col-sm-4 col-md-2 d-grid">
                    <asp:Button ID="btnCancelar" runat="server"
                        Text="Cancelar"
                        CssClass="btn btn-outline-secondary btn-sm"
                        OnClick="btnCancelar_Click" />
                </div>

                <%-- boton guardar --%>
                <div class="col-6 col-sm-4 col-md-2 d-grid">
                    <asp:Button ID="btnGuardar" runat="server"
                        Text="Guardar"
                        CssClass="btn btn-primary btn-sm" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
