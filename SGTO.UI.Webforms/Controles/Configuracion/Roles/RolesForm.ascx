<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RolesForm.ascx.cs" Inherits="SGTO.UI.Webforms.Controles.Configuracion.Roles.RolesForm" %>


<div class="card shadow-sm p-5 gap-5 cobertura-form">

    <div class="col justify-content-between">
        <div class="mb-3">
            <div class="row gy-4 mb-5">

                <%-- Nombre --%>
                <div class="col-12">
                    <label for="txtNombre" class="form-label">Nombre</label>
                    <asp:TextBox
                        ID="txtNombre"
                        runat="server"
                        placeholder="Ingrese el nombre..."
                        CssClass="form-control">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvNombre"
                        runat="server"
                        ControlToValidate="txtNombre"
                        ErrorMessage="El nombre del rol es obligatorio."
                        CssClass="text-danger small"
                        Display="Dynamic" />

                    <asp:RegularExpressionValidator
                        ID="revNombre"
                        runat="server"
                        ControlToValidate="txtNombre"
                        ValidationExpression="^[A-Za-zÁÉÍÓÚáéíóúÑñ ]+$"
                        ErrorMessage="El nombre solo puede contener letras y espacios."
                        CssClass="text-danger small"
                        Display="Dynamic" />
                </div>

                <%-- Descripción --%>
                <div class="col-12">
                    <label for="txtDescripcion" class="form-label fw-semibold">Descripción</label>
                    <asp:TextBox
                        ID="txtDescripcion"
                        runat="server"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="200"
                        placeholder="Ingrese una descripción (opcional, mínimo 10 caracteres si se completa)..."
                        CssClass="form-control descripcion-textarea">
                    </asp:TextBox>

                    <asp:RegularExpressionValidator
                        ID="revDescripcion"
                        runat="server"
                        ControlToValidate="txtDescripcion"
                        ValidationExpression="^.{0,200}$"
                        ErrorMessage="La descripción no puede superar los 200 caracteres."
                        CssClass="text-danger small"
                        Display="Dynamic" />
                </div>


                <%-- Estado --%>
                <div class="col-12">
                    <label for="ddlEstado" class="form-label fw-semibold">Estado</label>
                    <asp:DropDownList
                        CssClass="form-select"
                        ID="ddlEstado"
                        runat="server">
                        <asp:ListItem Selected="True" Value="Activo">Activo</asp:ListItem>
                        <asp:ListItem Value="Inactivo">Inactivo</asp:ListItem>
                    </asp:DropDownList>
                </div>

            </div>

            <%--Permisos del rol--%>
            <div class="row gy-4">
                <h5>Permisos del Rol</h5>
                <div class="alert alert-info small mb-3" role="alert">
                    <i class="bi bi-info-circle"></i>
                    Si un módulo no tiene ningún permiso seleccionado, el rol no podrá acceder ni visualizarlo en la aplicación.
               
                </div>
                <div class="m-0 border border-1"></div>

                <%--Tabla Dinámica con Repeater--%>
                <div class="table-responsive">
                    <table class="table table-borderless align-middle">
                        <thead>
                            <tr class="border-bottom fw-bold">
                                <th class="text-start" style="width: 20%;">Módulo</th>
                                <th class="text-center">Ver</th>
                                <th class="text-center">Crear</th>
                                <th class="text-center">Editar</th>
                                <th class="text-center">Activar</th>
                                <th class="text-center">Desactivar</th>
                                <th class="text-center">Eliminar</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptPermisos" runat="server">
                                <ItemTemplate>
                                    <tr class="border-bottom">
                                        <%-- Nombre del Módulo --%>
                                        <td class="fw-bold text-start">
                                            <%# Eval("NombreModulo") %>
                                        </td>

                                        <%-- el HiddenField sera para guardar el iddel permiso y CheckBox para el estado --%>

                                        <%-- Ver --%>
                                        <td class="text-center">
                                            <asp:HiddenField ID="hdnVer" runat="server" Value='<%# Eval("IdPermisoVer") %>' />
                                            <asp:CheckBox ID="chkVer" runat="server" Checked='<%# Eval("AsignadoVer") %>' Visible='<%# (int)Eval("IdPermisoVer") > 0 %>' />
                                        </td>

                                        <%-- Crear --%>
                                        <td class="text-center">
                                            <asp:HiddenField ID="hdnCrear" runat="server" Value='<%# Eval("IdPermisoCrear") %>' />
                                            <asp:CheckBox ID="chkCrear" runat="server" Checked='<%# Eval("AsignadoCrear") %>' Visible='<%# (int)Eval("IdPermisoCrear") > 0 %>' />
                                        </td>

                                        <%-- Editar --%>
                                        <td class="text-center">
                                            <asp:HiddenField ID="hdnEditar" runat="server" Value='<%# Eval("IdPermisoEditar") %>' />
                                            <asp:CheckBox ID="chkEditar" runat="server" Checked='<%# Eval("AsignadoEditar") %>' Visible='<%# (int)Eval("IdPermisoEditar") > 0 %>' />
                                        </td>

                                        <%-- Activar --%>
                                        <td class="text-center">
                                            <asp:HiddenField ID="hdnActivar" runat="server" Value='<%# Eval("IdPermisoActivar") %>' />
                                            <asp:CheckBox ID="chkActivar" runat="server" Checked='<%# Eval("AsignadoActivar") %>' Visible='<%# (int)Eval("IdPermisoActivar") > 0 %>' />
                                        </td>

                                        <%-- Desactivar --%>
                                        <td class="text-center">
                                            <asp:HiddenField ID="hdnDesactivar" runat="server" Value='<%# Eval("IdPermisoDesactivar") %>' />
                                            <asp:CheckBox ID="chkDesactivar" runat="server" Checked='<%# Eval("AsignadoDesactivar") %>' Visible='<%# (int)Eval("IdPermisoDesactivar") > 0 %>' />
                                        </td>

                                        <%-- Elimianr --%>
                                        <td class="text-center">
                                            <asp:HiddenField ID="hdnEliminar" runat="server" Value='<%# Eval("IdPermisoEliminar") %>' />
                                            <asp:CheckBox ID="chkEliminar" runat="server" Checked='<%# Eval("AsignadoEliminar") %>' Visible='<%# (int)Eval("IdPermisoEliminar") > 0 %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>

        </div>
    </div>

    <asp:ValidationSummary
        ID="ValidationSummaryRol"
        runat="server"
        HeaderText="Por favor corrija los siguientes errores:"
        CssClass="alert alert-danger mt-3"
        DisplayMode="BulletList" />

    <%--botones--%>
    <div class="h-100 w-100 align-content-end">
        <div class="row justify-content-end gx-2 gy-2">
            <div class="col-6 col-sm-4 col-md-2 d-grid">
                <asp:Button
                    ID="btnCancelar"
                    runat="server"
                    Text="Cancelar"
                    CssClass="btn btn-outline-secondary btn-sm"
                    OnClick="btnCancelar_Click"
                    CausesValidation="false" />
            </div>

            <div class="col-6 col-sm-4 col-md-2 d-grid">
                <asp:Button
                    ID="btnGuardar"
                    runat="server"
                    Text="Guardar"
                    CssClass="btn btn-primary btn-sm"
                    OnClick="btnGuardar_Click"
                    CausesValidation="true" />
            </div>
        </div>
    </div>
</div>
