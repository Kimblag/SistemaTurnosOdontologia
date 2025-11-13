<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="TratamientoForm.ascx.cs"
    Inherits="SGTO.UI.Webforms.Controles.Tratamientos.TratamientoForm" %>

<div class="card shadow-sm p-5 w-100 gap-2">

    <%-- Encabezado con ID --%>
    <div class="d-flex justify-content-end mb-2">
        <small class="text-muted">
            ID Tratamiento: <asp:Literal ID="litIdTratamiento" runat="server" />
        </small>
    </div>

    <%-- Campos --%>
    <div>

        <%-- Nombre y Costo Base --%>
        <div class="row g-3 mb-3">

            <%-- Nombre --%>
            <div class="col-12 col-md-6">
                <label for="<%= txtNombre.ClientID %>" class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server"
                    CssClass="form-control"
                    placeholder="Ej.: Blanqueamiento Dental"></asp:TextBox>

                <asp:RequiredFieldValidator runat="server"
                    ControlToValidate="txtNombre"
                    CssClass="text-danger small"
                    Display="Dynamic"
                    ErrorMessage="El nombre es obligatorio."
                    ValidationGroup="vgTratamiento" />
            </div>

            <%-- Costo Base --%>
            <div class="col-12 col-md-6">
                <label for="<%= txtCostoBase.ClientID %>" class="form-label">Costo Base</label>
                <div class="input-group">
                    <span class="input-group-text">$</span>
                    <asp:TextBox ID="txtCostoBase" runat="server"
                        CssClass="form-control"
                        placeholder="Ej.: 15000"
                        TextMode="Number"></asp:TextBox>
                </div>
                <small class="text-muted">Ingresá el costo en moneda local.</small>

                <asp:RequiredFieldValidator runat="server"
                    ControlToValidate="txtCostoBase"
                    CssClass="text-danger small d-block"
                    Display="Dynamic"
                    ErrorMessage="El costo base es obligatorio."
                    ValidationGroup="vgTratamiento" />
                <asp:RegularExpressionValidator runat="server"
                    ControlToValidate="txtCostoBase"
                    CssClass="text-danger small d-block"
                    Display="Dynamic"
                    ValidationExpression="^\d+([,\.]\d{1,2})?$"
                    ErrorMessage="Formato inválido. Usá números con hasta 2 decimales."
                    ValidationGroup="vgTratamiento" />
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
                    placeholder="Ej.: Procedimiento para aclarar el color de los dientes, eliminando manchas y decoloraciones."></asp:TextBox>
            </div>
        </div>

        <%-- Especialidad y Estado --%>
        <div class="row g-3 mb-3">

            <%-- Especialidad Asociada --%>
            <div class="col-12 col-md-6">
                <label for="<%= ddlEspecialidad.ClientID %>" class="form-label">Especialidad Asociada</label>

                <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select">
                    
                    <asp:ListItem Text="Seleccione una especialidad" Value="0"></asp:ListItem>
                </asp:DropDownList>

                <asp:RequiredFieldValidator runat="server"
                    ControlToValidate="ddlEspecialidad"
                    InitialValue="0"
                    CssClass="text-danger small d-block"
                    Display="Dynamic"
                    ErrorMessage="Seleccioná una especialidad."
                    ValidationGroup="vgTratamiento" />
            </div>

            <%-- Estado  --%>
            <div class="col-12 ml-0">
                <label class="form-label">Estado</label>
                <div class="form-check p-0">
                    <asp:CheckBox ID="chkEstado" runat="server"
                        Text="Activo"
                        CssClass="d-flex gap-2"
                        Checked="true"
                        Enabled="false" />
                </div>
            </div>

        </div>
    </div>

    <%-- Botones --%>
    <div class="h-100 w-100 align-content-end">
        <div class="row justify-content-end gx-2 gy-2">

            <%-- Cancelar --%>
            <div class="col-6 col-sm-4 col-md-2 d-grid">
                <asp:Button ID="btnCancelar" runat="server"
                    Text="Cancelar"
                    CssClass="btn btn-outline-secondary btn-sm"
                    OnClick="btnCancelar_Click" />
            </div>

            <%-- Guardar --%>
            <div class="col-6 col-sm-4 col-md-2 d-grid">
                <asp:Button ID="btnGuardar" runat="server"
                    Text="Guardar"
                    CssClass="btn btn-primary btn-sm"
                    OnClick="btnGuardar_Click"
                    ValidationGroup="vgTratamiento" />
            </div>
        </div>
    </div>

</div>


<%-- Modal confirmación --%>
<div class="modal" tabindex="-1" id="modalConfirmacion" aria-labelledby="modalConfirmacion" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h5 id="modalTitulo" class="modal-title">Modal título</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <p id="modalDesc">Mensaje confirmación</p>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" id="btnModalOk">Ok</button>
            </div>
        </div>
    </div>
</div>

<%-- Modal resultado --%>
<div class="modal fade" id="modalResultado" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h5 id="modalResultadoTitulo" class="modal-title">Acción completada</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body">
                <p id="modalResultadoDesc"></p>
            </div>
            <div class="modal-footer">
                <button id="btnModalCerrar" type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
            </div>
        </div>
    </div>
</div>

<script>
    function abrirModalConfirmacion(titulo, descripcion) {
        try {
            document.getElementById('modalTitulo').textContent = titulo || 'Acción completada';
            document.getElementById('modalDesc').textContent = descripcion || '';

            const modal = new bootstrap.Modal(document.getElementById('modalConfirmacion'));
            modal.show();

        } catch (err) {
            console.error('Error :', err);
        }
    }

    function abrirModalResultado(titulo, descripcion) {
        document.getElementById('modalResultadoTitulo').textContent = titulo || 'Acción completada';
        document.getElementById('modalResultadoDesc').textContent = descripcion || '';
        new bootstrap.Modal(document.getElementById('modalResultado')).show();
    }

    function abrirModalNuevoPlan() {
        const modal = new bootstrap.Modal(document.getElementById('modalNuevoPlan'));
        modal.show();
    }
</script>
