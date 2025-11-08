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
            <div class="col-12 ml-0">
    <label class="form-label">Estado</label>
    <div class="form-check p-0">
        <asp:CheckBox ID="chkActivo" Text="Activo" CssClass="d-flex gap-2" runat="server" Checked="true" Enabled="false" />
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


<%-- Modal confirmacion--%>
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


<%-- Modal resultado de la ejecucion del servicio --%>
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