<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="TratamientoForm.ascx.cs"
    Inherits="SGTO.UI.Webforms.Controles.Tratamientos.TratamientoForm" %>

<div class="card shadow-sm p-5 w-100 gap-2">

    <%-- Encabezado con ID --%>
    <div class="d-flex justify-content-end mb-3">
        <small class="text-muted">ID Tratamiento:
           
            <asp:Literal ID="litIdTratamiento" runat="server" />
        </small>
    </div>
    <div class="row justify-content-center">
        <div class="col-12 col-lg-9 col-xl-8">

            <%-- Nombre --%>
            <div class="mb-3">
                <label for="txtNombre" class="form-label">Nombre <span class="text-danger">*</span></label>
                <asp:TextBox ID="txtNombre" runat="server"
                    CssClass="form-control"
                    placeholder="Ej.: Blanqueamiento Dental"></asp:TextBox>
                <div class="mt-1">
                    <asp:RequiredFieldValidator runat="server"
                        ControlToValidate="txtNombre"
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ErrorMessage="El nombre es obligatorio."
                        ValidationGroup="vgTratamiento" />
                </div>
            </div>

            <%-- Costo Base --%>
            <div class="mb-3">
                <label for="txtCostoBase" class="form-label">Costo Base <span class="text-danger">*</span></label>
                <div class="input-group">
                    <span class="input-group-text">$</span>
                    <asp:TextBox ID="txtCostoBase" runat="server"
                        CssClass="form-control"
                        placeholder="Ej.: 15000"
                        TextMode="Number"></asp:TextBox>
                </div>
                <div class="mt-1">
                    <small class="text-muted">Ingresá el costo en moneda local.</small>
                </div>
                <div class="mt-1">
                    <asp:RequiredFieldValidator runat="server"
                        ControlToValidate="txtCostoBase"
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ErrorMessage="El costo base es obligatorio."
                        ValidationGroup="vgTratamiento" />
                </div>
                <div class="mt-1">
                    <asp:RegularExpressionValidator runat="server"
                        ControlToValidate="txtCostoBase"
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationExpression="^\d+([,\.]\d{1,2})?$"
                        ErrorMessage="Formato inválido. Usá números con hasta 2 decimales."
                        ValidationGroup="vgTratamiento" />
                </div>
            </div>

            <%-- Descripción --%>
            <div class="mb-3">
                <label for="txtDescripcion" class="form-label">Descripción</label>
                <asp:TextBox ID="txtDescripcion" runat="server"
                    CssClass="form-control"
                    TextMode="MultiLine"
                    Rows="3"
                    placeholder="Ej.: Procedimiento para aclarar el color de los dientes, eliminando manchas y decoloraciones."></asp:TextBox>
            </div>

            <%-- Especialidad Asociada --%>
            <div class="mb-3">
                <label for="ddlEspecialidad" class="form-label">Especialidad Asociada <span class="text-danger">*</span></label>
                <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select">
                </asp:DropDownList>
                <div class="mt-1">
                    <asp:RequiredFieldValidator runat="server"
                        ControlToValidate="ddlEspecialidad"
                        InitialValue="0"
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ErrorMessage="Seleccioná una especialidad."
                        ValidationGroup="vgTratamiento" />
                </div>
            </div>

            <%-- Estado --%>
            <div class="mb-3">
                <label class="form-label">Estado</label>
                <div class="form-check p-0 mt-1">
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
        <div class="modal-content border-0 shadow">
            <div class="modal-header bg-white border-bottom-0">
                <h5 id="modalResultadoTitulo" class="modal-title fw-bold">Mensaje</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body text-center py-4">
                <div class="mb-3">
                    <i class="bi bi-info-circle text-primary" style="font-size: 3rem;"></i>
                </div>
                <p id="modalResultadoDesc" class="lead fs-6"></p>
            </div>
            <div class="modal-footer border-top-0 justify-content-center pb-4">
                <button id="btnModalCerrar" type="button" class="btn btn-primary px-4" data-bs-dismiss="modal">Aceptar</button>
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
