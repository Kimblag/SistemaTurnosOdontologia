<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanesForm.ascx.cs" Inherits="SGTO.UI.Webforms.Controles.Coberturas.PlanesForm" %>

<div class="card shadow-sm p-5 gap-5 cobertura-form">

    <div class="mb-3">
        <div class="row gy-4">

            <%-- cobertura asociada --%>
            <div class="col-12">
                <label for="ddlCobertura" class="form-label">Cobertura Asociada</label>
                <asp:DropDownList
                    CssClass="form-select"
                    ID="ddlCobertura"
                    runat="server">
                </asp:DropDownList>
            </div>

            <%-- Nombre --%>
            <div class="col-12">
                <label for="txtNombrePlan" class="form-label">Nombre del Plan</label>
                <asp:TextBox
                    ID="txtNombrePlan"
                    runat="server"
                    placeholder="Ingrese el nombre..."
                    CssClass="form-control">
                </asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                    ControlToValidate="txtNombrePlan"
                    ErrorMessage="El nombre es obligatorio."
                    CssClass="text-danger small"
                    Display="Dynamic"
                    ValidationGroup="PlanGroup" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                    ControlToValidate="txtNombrePlan"
                    ValidationExpression="^[A-Za-z0-9\s]{3,80}$"
                    ErrorMessage="El nombre debe tener entre 3 y 80 caracteres y solo contener letras, números o espacios."
                    CssClass="text-danger small"
                    Display="Dynamic" ValidationGroup="PlanGroup" />
            </div>

            <%-- Descripción --%>
            <div class="col-12">
                <label for="txtDescripcionPlan" class="form-label">Descripción</label>
                <asp:TextBox
                    ID="txtDescripcionPlan"
                    runat="server"
                    placeholder="Ingrese la descripción..."
                    TextMode="MultiLine"
                    CssClass="form-control descripcion-textarea"
                    MaxLength="200">
                </asp:TextBox>
            </div>


            <%-- porcentaje cobertura --%>
            <div class="col-12">
                <label for="txtPorcentajeCobertura" class="form-label">Porcentaje de cobertura</label>
                <div class="input-group mb-3">
                    <asp:TextBox
                        ID="txtPorcentajeCobertura"
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
                <asp:RequiredFieldValidator
                    ID="rfvPorcentaje"
                    runat="server"
                    ControlToValidate="txtPorcentajeCobertura"
                    ErrorMessage="El porcentaje es obligatorio."
                    CssClass="text-danger small"
                    Display="Dynamic"
                    ValidationGroup="PlanGroup" />


                <asp:RangeValidator
                    ID="rvPorcentaje"
                    runat="server"
                    ControlToValidate="txtPorcentajeCobertura"
                    MinimumValue="0"
                    MaximumValue="100"
                    Type="Double"
                    ErrorMessage="El porcentaje debe estar entre 0 y 100."
                    CssClass="text-danger small"
                    Display="Dynamic"
                    ValidationGroup="PlanGroup" />


                <asp:RegularExpressionValidator
                    ID="revPorcentaje"
                    runat="server"
                    ControlToValidate="txtPorcentajeCobertura"
                    ValidationExpression="^\d{1,3}(\.\d{1,2})?$"
                    ErrorMessage="Ingrese un número válido (por ejemplo 25 o 75.5)."
                    CssClass="text-danger small"
                    Display="Dynamic"
                    ValidationGroup="PlanGroup" />
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
                    OnClick="btnGuardar_Click"/>
            </div>
        </div>
    </div>

</div>
