<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TurnoForm.ascx.cs" Inherits="SGTO.UI.Webforms.Controles.Turnos.TurnoForm" %>

<div class="card shadow-sm p-5 gap-2 justify-content-between">

    <%-- Campos del formulario de Turno --%>
    <div>
        <%-- Paciente y Médico --%>
        <div class="row mb-3 g-3">
            <%-- Paciente --%>
            <div class="col-12 col-md-6">
                <label for="ddlPaciente" class="form-label">Paciente</label>
                <asp:DropDownList ID="ddlPaciente" runat="server" CssClass="form-select">
                  
                    <asp:ListItem Text="Seleccione un paciente..." Value="" />
                </asp:DropDownList>
            </div>

            <%-- Médico --%>
            <div class="col-12 col-md-6">
                <label for="ddlMedico" class="form-label">Médico</label>
                <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select">
               
                     <asp:ListItem Text="Seleccione un médico..." Value="" />
                </asp:DropDownList>
            </div>
        </div>

        <%-- Fecha y Hora --%>
        <div class="row mb-3 g-3">
            <%-- Fecha --%>
            <div class="col-12 col-md-6">
                <label for="txtFecha" class="form-label">Fecha</label>
                <asp:TextBox ID="txtFecha" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
            </div>

            <%-- Hora --%>
            <div class="col-12 col-md-6">
                <label for="txtHora" class="form-label">Hora</label>
                <asp:TextBox ID="txtHora" runat="server" TextMode="Time" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <%-- Tratamiento y Estado --%>
        <div class="row mb-3 g-3">
             <%-- Tratamiento --%>
            <div class="col-12 col-md-6">
                <label for="ddlTratamiento" class="form-label">Tratamiento</label>
                <asp:DropDownList ID="ddlTratamiento" runat="server" CssClass="form-select">
                  
                    <asp:ListItem Text="Seleccione un tratamiento..." Value="" />
                </asp:DropDownList>
            </div>

            <%-- Estado --%>
            <div class="col-12 col-md-6">
                <label for="ddlEstadoTurno" class="form-label">Estado del Turno</label>
                <asp:DropDownList ID="ddlEstadoTurno" runat="server" CssClass="form-select">
                   
                </asp:DropDownList>
            </div>
        </div>

        <%-- Cobertura y Plan --%>
        <div class="row mb-3 g-3">
            <%-- Cobertura --%>
            <div class="col-12 col-md-6">
                <label for="ddlCobertura" class="form-label">Cobertura</label>
                <asp:DropDownList ID="ddlCobertura" runat="server" CssClass="form-select">
              
                    <asp:ListItem Text="Seleccione cobertura..." Value="" />
                </asp:DropDownList>
            </div>

            <%-- Plan --%>
            <div class="col-12 col-md-6">
                <label for="ddlPlan" class="form-label">Plan</label>
                <asp:DropDownList ID="ddlPlan" runat="server" CssClass="form-select">
                    
                    <asp:ListItem Text="Seleccione un plan..." Value="" />
                </asp:DropDownList>
            </div>
        </div>

         <%-- Observaciones --%>
        <div class="row mb-3">
            <div class="col-12">
                 <label for="txtObservaciones" class="form-label">Observaciones (Opcional)</label>
                <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" placeholder="Notas adicionales sobre el turno..."></asp:TextBox>
            </div>
        </div>

    </div>

    <%-- Botones --%>
    <div class="h-100 w-100 align-content-end">
        <div class="row justify-content-end gx-2 gy-2">
            <%-- Botón cancelar --%>
            <div class="col-6 col-sm-4 col-md-2 d-grid">
                <asp:Button ID="btnCancelarTurno" runat="server"
                    Text="Cancelar"
                    CssClass="btn btn-outline-secondary btn-sm"
                    OnClick="btnCancelarTurno_Click" /> 
            </div>

            <%-- Botón guardar --%>
           <div class="col-6 col-sm-4 col-md-2 d-grid">
                <asp:Button ID="btnGuardar" runat="server"
                    Text="Guardar"
                    CssClass="btn btn-primary btn-sm" />
           </div>
        </div>
    </div>

</div>