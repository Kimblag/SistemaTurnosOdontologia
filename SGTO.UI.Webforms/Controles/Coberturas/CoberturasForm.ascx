<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoberturasForm.ascx.cs" Inherits="SGTO.UI.Webforms.Controles.Coberturas.CoberturasForm" %>

<div class="card shadow-sm p-5 gap-5 cobertura-form">

    <div class="col justify-content-between">
        <div class="mb-3">
            <div class="row gy-4">

                <%-- Nombre --%>
                <div class="col-12">
                    <label for="txtNombreCobertura" class="form-label">Nombre</label>
                    <asp:TextBox
                        ID="txtNombreCobertura"
                        runat="server"
                        placeholder="Ingrese el nombre..."
                        CssClass="form-control">
                    </asp:TextBox>
                </div>

                <%-- Descripción --%>
                <div class="col-12">
                    <label for="txtDescripcionCobertura" class="form-label">Descripción</label>
                    <asp:TextBox
                        ID="txtDescripcionCobertura"
                        runat="server"
                        placeholder="Ingrese la descripción..."
                        TextMode="MultiLine"
                        CssClass="form-control descripcion-cobertura">
                    </asp:TextBox>
                </div>

                <%-- Estado --%>
                <div class="col-12">
                    <label for="ddlEstado" class="form-label">Estado</label>
                    <asp:DropDownList
                        CssClass="form-select"
                        ID="ddlEstado"
                        runat="server">
                        <asp:ListItem Selected="True" Value="activo">Activo</asp:ListItem>
                        <asp:ListItem Value="inactivo">Inactivo</asp:ListItem>
                    </asp:DropDownList>
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
                    CssClass="btn btn-primary btn-sm me-1" OnClick="btnNuevoPlan_Click" />
            </div>
            <div class="content-wrapper">
                <asp:GridView ID="gvPlanes" runat="server"
                    AutoGenerateColumns="false"
                    OnRowDataBound="gvPlanes_RowDataBound"
                    OnPageIndexChanging="gvPlanes_PageIndexChanging"
                    OnRowCommand="gvPlanes_RowCommand"
                    DataKeyNames="IdPlan"
                    CssClass="table gridview mb-0"
                    AllowPaging="True" PageSize="7">

                    <Columns>
                        <asp:TemplateField HeaderText="Cobertura">
                            <ItemTemplate>
                                <%# Eval("Cobertura.Nombre") %>
                            </ItemTemplate>
                        </asp:TemplateField>
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
                                    CommandArgument='<%# Eval("IdPlan") %>'>
                                 <i class="bi bi-pencil"></i>
                                </asp:LinkButton>

                                <asp:LinkButton ID="btnEliminar" runat="server"
                                    CssClass="btn btn-outline-danger btn-sm me-1"
                                    CommandName="Eliminar" CommandArgument='<%# Eval("IdPlan") %>'>
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
                    OnClick="btnGuardar_Click" />
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



<script>
    function abrirModalConfirmacion(titulo, descripcion) {
        console.log("HOLA")
        try {
            document.getElementById('modalTitulo').textContent = titulo || 'Acción completada';
            document.getElementById('modalDesc').textContent = descripcion || '';

            const modal = new bootstrap.Modal(document.getElementById('modalConfirmacion'));
            modal.show();

        } catch (err) {
            console.error('Error :', err);
        }
    }
</script>
