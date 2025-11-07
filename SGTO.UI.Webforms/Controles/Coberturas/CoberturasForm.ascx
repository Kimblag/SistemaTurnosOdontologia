<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoberturasForm.ascx.cs" Inherits="SGTO.UI.Webforms.Controles.Coberturas.CoberturasForm" %>

<div class="card shadow-sm p-5 gap-5 cobertura-form">

    <div class="col justify-content-between">
        <div class="mb-3">
            <div class="row gy-4">

                <%-- Nombre --%>
                <div class="col-12">
                    <label for="txtNombreCobertura" class="form-label">
                        Nombre <span class="text-danger">*</span>
                    </label>
                    <asp:TextBox
                        ID="txtNombreCobertura"
                        runat="server"
                        placeholder="Ingrese el nombre..."
                        CssClass="form-control" MaxLength="80">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                        ControlToValidate="txtNombreCobertura"
                        ErrorMessage="El nombre es obligatorio."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="CoberturaGroup" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                        ControlToValidate="txtNombreCobertura"
                        ValidationExpression="^[A-Za-z0-9\s]{3,80}$"
                        ErrorMessage="El nombre debe tener entre 3 y 80 caracteres y solo contener letras, números o espacios."
                        CssClass="text-danger small"
                        Display="Dynamic" ValidationGroup="CoberturaGroup" />
                </div>

                <%-- Descripción --%>
                <div class="col-12">
                    <label for="txtDescripcionCobertura" class="form-label">Descripción</label>
                    <asp:TextBox
                        ID="txtDescripcionCobertura"
                        runat="server"
                        placeholder="Ingrese la descripción..."
                        TextMode="MultiLine"
                        CssClass="form-control descripcion-textarea" MaxLength="200">
                    </asp:TextBox>

                </div>

                <%-- porcentaje cobertura --%>
                <div class="col-12">
                    <label for="txtPorcentaje" class="form-label">Porcentaje de cobertura</label>
                    <div class="input-group mb-3">
                        <asp:TextBox
                            ID="txtPorcentaje"
                            runat="server"
                            placeholder="Ej.: 40..."
                            TextMode="Number"
                            MaxLength="3"
                            CssClass="form-control"
                            step="0.01"
                            min="0"
                            max="100">
                        </asp:TextBox>
                        <span class="input-group-text">%</span>
                    </div>


                    <asp:RangeValidator
                        ID="rvPorcentaje"
                        runat="server"
                        ControlToValidate="txtPorcentaje"
                        MinimumValue="0"
                        MaximumValue="100"
                        Type="Double"
                        ErrorMessage="El porcentaje debe estar entre 0 y 100."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="CoberturaGroup"
                        ValidateEmptyText="false" />

                    <asp:RegularExpressionValidator
                        ID="revPorcentaje"
                        runat="server"
                        ControlToValidate="txtPorcentaje"
                        ValidationExpression="^\d{1,3}(\.\d{1,2})?$"
                        ErrorMessage="Ingrese un número válido (por ejemplo 25 o 75.5)."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="CoberturaGroup"
                        ValidateEmptyText="false" />
                    <small class="text-muted">Este porcentaje solo se aplica a coberturas sin planes asociados. 
                    Si la cobertura tiene planes, use los porcentajes de cada plan.</small>
                </div>

                <%-- Estado --%>
                <div class="col-12 ml-0">
                    <label class="form-label">Estado</label>
                    <div class="form-check p-0">
                        <asp:CheckBox ID="chkActivo" Text="Activo" CssClass="d-flex gap-2" runat="server" Checked="true" Enabled="false" />
                    </div>
                </div>

            </div>
        </div>

        <%--tabla planes--%>
        <div id="panelPlanes" runat="server" class="row gap-2">
            <div class="d-flex gap-2 align-items-center my-3 mb-3 justify-content-between flex-wrap">

                <div class="d-flex gap-2 align-items-center w-50">
                    <h2>Planes asociados</h2>
                </div>

                <asp:Button ID="btnNuevoPlan" runat="server" Text="+ Nuevo Plan"
                    CssClass="btn btn-primary btn-sm me-1"
                    CausesValidation="false"
                    OnClientClick="new bootstrap.Modal(document.getElementById('modalNuevoPlan')).show(); return false;" />

            </div>
            <div class="content-wrapper">
                <asp:GridView ID="gvPlanes" runat="server"
                    AutoGenerateColumns="false"
                    OnRowDataBound="gvPlanes_RowDataBound"
                    OnRowCommand="gvPlanes_RowCommand"
                    DataKeyNames="IdPlan"
                    CssClass="table gridview mb-0">

                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre del Plan" />
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                        <asp:BoundField DataField="PorcentajeCobertura" HeaderText="% de Cobertura" />

                        <%--columna estado--%>
                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <span id="lblEstado" runat="server" class="badge"><%# Eval("Estado") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <%--columna acciones--%>
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEditar"
                                    runat="server"
                                    CssClass="btn btn-outline-secondary btn-sm me-1"
                                    CommandName="Editar"
                                    CommandArgument='<%# Container.DataItemIndex %>'>
                                 <i class="bi bi-pencil"></i>
                                </asp:LinkButton>

                                <asp:LinkButton ID="btnEliminar" runat="server"
                                    CssClass="btn btn-outline-danger btn-sm me-1"
                                    CommandName="Eliminar" CommandArgument='<%# Container.DataItemIndex %>'>
                             <i class="bi bi-x"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                    <EmptyDataTemplate>
                        <div class="empty-state">
                            <i class="bi bi-shield-check fs-5"></i>
                            No hay Planes para mostrar.
                        </div>
                    </EmptyDataTemplate>

                </asp:GridView>
            </div>
        </div>
    </div>

    <%--botones--%>
    <div class="h-100 w-100 align-content-end">
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
                    CssClass="btn btn-primary btn-sm"
                    OnClick="btnGuardar_Click" ValidationGroup="CoberturaGroup" />
            </div>
        </div>
    </div>
</div>




<%--modal confirmacion--%>
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


<%-- modal resultado de la ejecucion del servicio --%>
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

<%--modal nuevo plan--%>
<div class="modal fade" id="modalNuevoPlan" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Nuevo Plan</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body">
                <div class="alert alert-danger small mt-2" id="lblErrorPlan" runat="server" visible="false"></div>
                <div class="mb-3">
                    <label for="txtNombrePlan" class="form-label">Nombre <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtNombrePlan" runat="server" CssClass="form-control" MaxLength="80" />
                    <asp:RequiredFieldValidator ID="rfvNombrePlan" runat="server"
                        ControlToValidate="txtNombrePlan"
                        ErrorMessage="Debe ingresar un nombre para el plan."
                        CssClass="text-danger small"
                        Display="Dynamic" ValidationGroup="PlanCoberturaGroup" />
                    <asp:RegularExpressionValidator ID="revNombrePlan" runat="server"
                        ControlToValidate="txtNombrePlan"
                        ValidationExpression="^[A-Za-z0-9\s]{3,80}$"
                        ErrorMessage="El nombre debe tener entre 3 y 80 caracteres y solo contener letras, números o espacios."
                        CssClass="text-danger small"
                        Display="Dynamic" ValidationGroup="PlanCoberturaGroup" />
                </div>
                <div class="mb-3">
                    <label for="txtDescripcionPlan" class="form-label">Descripción</label>
                    <asp:TextBox ID="txtDescripcionPlan" runat="server" CssClass="form-control" TextMode="MultiLine" MaxLength="200" />
                </div>
                <div class="mb-3">
                    <label for="txtPorcentajeCobertura" class="form-label">% Cobertura <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtPorcentajeCobertura" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvPorcentaje" runat="server"
                        ControlToValidate="txtPorcentajeCobertura"
                        ErrorMessage="Debe ingresar un porcentaje."
                        CssClass="text-danger small"
                        Display="Dynamic" ValidationGroup="PlanCoberturaGroup" />
                    <asp:RangeValidator ID="rngPorcentaje" runat="server"
                        ControlToValidate="txtPorcentajeCobertura"
                        MinimumValue="0" MaximumValue="100"
                        Type="Double"
                        ErrorMessage="El porcentaje debe estar entre 0 y 100."
                        CssClass="text-danger small"
                        Display="Dynamic" ValidationGroup="PlanCoberturaGroup" />
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                <asp:Button ID="btnAgregarPlan" runat="server"
                    CssClass="btn btn-primary"
                    Text="Agregar"
                    OnClick="btnAgregarPlan_Click"
                    ValidationGroup="PlanCoberturaGroup" />
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
