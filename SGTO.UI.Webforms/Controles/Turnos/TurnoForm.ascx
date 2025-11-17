<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TurnoForm.ascx.cs" Inherits="SGTO.UI.Webforms.Controles.Turnos.TurnoForm" %>

<div class="card shadow-sm p-5 gap-2 justify-content-between">
    <div id="alertPacienteInactivo" runat="server" class="alert alert-warning py-1 px-2 d-none"></div>
    <div>

        <div class="row mb-3 g-3">
            <div class="col-12 col-md-6">
                <label for="txtPaciente" class="form-label">Paciente</label>
                <asp:HiddenField ID="hdnIdPaciente" runat="server" Value="0" />
                <asp:TextBox ID="txtPaciente" runat="server" CssClass="form-control" Enabled="false" />
            </div>


            <div class="col-12 col-md-6">
                <label for="ddlEspecialidad" class="form-label">Especialidad</label>
                <asp:DropDownList ID="ddlEspecialidad" runat="server"
                    CssClass="form-select"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlEspecialidad_SelectedIndexChanged">
                    <asp:ListItem Text="Seleccione una especialidad" Value="" />
                </asp:DropDownList>
            </div>
        </div>

        <div class="row mb-3 g-3">
            <div class="col-12 col-md-6">
                <label for="ddlMedico" class="form-label">Médico</label>
                <asp:DropDownList ID="ddlMedico" runat="server"
                    CssClass="form-select"
                    Enabled="false"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlMedico_SelectedIndexChanged">
                    <asp:ListItem Text="Seleccione un médico" Value="" />
                </asp:DropDownList>
            </div>

            <div class="col-12 col-md-6">
                <label for="ddlFecha" class="form-label">Fecha disponible</label>
                <asp:DropDownList ID="ddlFecha" runat="server"
                    CssClass="form-select"
                    Enabled="false"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlFecha_SelectedIndexChanged">
                    <asp:ListItem Text="Seleccione una fecha" Value="" />
                </asp:DropDownList>
            </div>
        </div>

        <div class="row mb-3 g-3">
            <div class="col-12 col-md-6">
                <label for="ddlHora" class="form-label">Hora disponible</label>
                <asp:DropDownList ID="ddlHora" runat="server"
                    CssClass="form-select"
                    Enabled="false">
                    <asp:ListItem Text="Seleccione una hora" Value="" />
                </asp:DropDownList>
            </div>

            <div class="col-12 col-md-6" id="divEstado" runat="server">
                <label for="ddlEstadoTurno" class="form-label">Estado del turno</label>
                <asp:DropDownList ID="ddlEstadoTurno" runat="server" Enabled="false" CssClass="form-select"></asp:DropDownList>
            </div>
        </div>

        <div class="row mb-3 g-3">
            <div class="col-12 col-md-6">
                <label for="ddlCobertura" class="form-label">Cobertura</label>
                <asp:DropDownList ID="ddlCobertura" runat="server"
                    CssClass="form-select"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlCobertura_SelectedIndexChanged">
                    <asp:ListItem Text="Seleccione una cobertura" Value="" />
                </asp:DropDownList>
            </div>

            <div class="col-12 col-md-6">
                <label for="ddlPlan" class="form-label">Plan</label>
                <asp:DropDownList ID="ddlPlan" runat="server" CssClass="form-select">
                    <asp:ListItem Text="Seleccione un plan" Value="" />
                </asp:DropDownList>
            </div>
        </div>
        <div id="alertCobertura" runat="server" class="alert alert-warning py-1 px-2 d-none"></div>
        <div id="alertPlan" runat="server" class="alert alert-warning py-1 px-2 d-none"></div>


        <div class="row mb-3">
            <div class="col-12">
                <label for="txtObservaciones" class="form-label">Observaciones (opcional)</label>
                <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" placeholder="Notas adicionales..."></asp:TextBox>
            </div>
        </div>

    </div>

    <div class="h-100 w-100 align-content-end">
        <div class="row justify-content-end gx-2 gy-2">

            <div class="col-6 col-sm-4 col-md-2 d-grid">
                <asp:Button ID="btnCancelarTurno" runat="server"
                    Text="Cancelar"
                    CssClass="btn btn-outline-secondary btn-sm"
                    OnClick="btnCancelarTurno_Click" />
            </div>

            <div class="col-6 col-sm-4 col-md-2 d-grid">
                <asp:Button ID="btnGuardar" runat="server"
                    Text="Guardar"
                    CssClass="btn btn-primary btn-sm"
                    OnClick="btnGuardar_Click"
                    />
            </div>

        </div>
    </div>

</div>
