<%@ Control Language="C#" AutoEventWireup="true" 
    CodeBehind="EspecialidadForm.ascx.cs" 
    Inherits="SGTO.UI.Webforms.Controles.Especialidades.EspecialidadForm" %>

<div class="card shadow-sm p-5 w-100 gap-2">

    <%-- Campos principales de la especialidad --%>
    <div>

        <%-- Nombre y Estado --%>
        <div class="row g-3 mb-3">

            <%-- Nombre --%>
            <div class="col-12 col-md-6">
                <label for="<%= txtNombre.ClientID %>" class="form-label">Nombre de la Especialidad</label>
                <asp:TextBox ID="txtNombre" runat="server" 
                    CssClass="form-control" 
                    placeholder="Ej.: Ortodoncia"></asp:TextBox>
            </div>

            <%-- Estado --%>
            <div class="col-12 col-md-6">
                <label for="<%= ddlEstado.ClientID %>" class="form-label">Estado</label>
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                    <asp:ListItem Value="1" Selected="True">Activo</asp:ListItem>
                    <asp:ListItem Value="0">Inactivo</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <%-- Descripción --%>
        <div class="row g-3 mb-3">
            <div class="col-12">
                <label for="<%= txtDescripcion.ClientID %>" class="form-label">Descripción</label>
                <asp:TextBox ID="txtDescripcion" runat="server"
                    CssClass="form-control"
                    TextMode="MultiLine"
                    Rows="3"
                    placeholder="Ej.: Corrección de dientes y mandíbulas, ortodoncia preventiva y estética."></asp:TextBox>
            </div>
        </div>

        <%-- Tratamientos asociados (solo a nivel visual por ahora) --%>
        <div class="row g-3 mb-3">
            <div class="col-12">
                <label for="<%= txtTratamientos.ClientID %>" class="form-label">Tratamientos Asociados (opcional)</label>
                <asp:TextBox ID="txtTratamientos" runat="server"
                    CssClass="form-control"
                    TextMode="MultiLine"
                    Rows="2"
                    placeholder="Ej.: Brackets metálicos, alineadores, limpieza dental..."></asp:TextBox>
                <small class="text-muted">Separá los tratamientos por coma.</small>
            </div>
        </div>

    </div>

    <%-- Botones --%>
    <div class="h-100 w-100 align-content-end">
        <div class="row justify-content-end gx-2 gy-2">
            
            <%-- Botón Cancelar --%>
            <div class="col-6 col-sm-4 col-md-2 d-grid">
                <asp:Button ID="btnCancelar" runat="server"
                    Text="Cancelar"
                    CssClass="btn btn-outline-secondary btn-sm"
                    OnClick="btnCancelar_Click" />
            </div>

            <%-- Botón Guardar --%>
             <div class="col-6 col-sm-4 col-md-2 d-grid">
                <asp:Button ID="btnGuardar" runat="server"
                    Text="Guardar"
                    CssClass="btn btn-primary btn-sm" 
                    OnClick="btnGuardar_Click" />
            </div>

        </div>
    </div>

</div>