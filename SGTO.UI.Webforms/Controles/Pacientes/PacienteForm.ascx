<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PacienteForm.ascx.cs" Inherits="SGTO.UI.Webforms.Controles.Pacientes.PacienteForm" %>

<div class="card shadow-sm p-5 gap-2 justify-content-between">

    <%--campos--%>
    <div>
        <%--DNI y fecha de nacimiento--%>
        <div class="row mb-3">
            <%--DNI--%>
            <div class="col-12 col-md-6">
                <label for="txtDni" class="form-label">DNI <span class="text-danger">*</span></label>
                <asp:TextBox ID="txtDni" runat="server" placeholder="Ej.: 11234567" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDni" runat="server"
                    ControlToValidate="txtDni"
                    ErrorMessage="El DNI es obligatorio."
                    CssClass="text-danger small"
                    Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revDni" runat="server"
                    ControlToValidate="txtDni"
                    ValidationExpression="^\d{7,9}$"
                    ErrorMessage="El DNI debe contener solo números (7 a 9 dígitos)."
                    CssClass="text-danger small"
                    Display="Dynamic" />
            </div>

            <%--fecha nacimiento--%>
            <div class="col-12 col-md-6">
                <label for="txtFechaNacimiento" class="form-label">Fecha de Nacimiento <span class="text-danger">*</span></label>
                <asp:TextBox ID="txtFechaNacimiento" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFechaNacimiento" runat="server"
                    ControlToValidate="txtFechaNacimiento"
                    ErrorMessage="La fecha de nacimiento es obligatoria."
                    CssClass="text-danger small"
                    Display="Dynamic" />
                <asp:RangeValidator ID="rvFechaNacimiento" runat="server"
                    ControlToValidate="txtFechaNacimiento"
                    Type="Date"
                    MinimumValue="1900-01-01"
                    MaximumValue="9999-12-31"
                    ErrorMessage="Ingrese una fecha de nacimiento válida."
                    CssClass="text-danger small"
                    Display="Dynamic" />
            </div>
        </div>

        <%--Nombre y apellido--%>
        <div class="row g-3 mb-3">

            <%--Nombre--%>
            <div class="col-12 col-md-6">
                <label for="txtNombre" class="form-label">Nombre <span class="text-danger">*</span></label>
                <asp:TextBox ID="txtNombre" runat="server" placeholder="Ej.: Juan" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                    ControlToValidate="txtNombre"
                    ErrorMessage="El nombre es obligatorio."
                    CssClass="text-danger small"
                    Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revNombre" runat="server"
                    ControlToValidate="txtNombre"
                    ValidationExpression="^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]{2,50}$"
                    ErrorMessage="El nombre solo puede contener letras y espacios."
                    CssClass="text-danger small"
                    Display="Dynamic" />
            </div>


            <%--Apellido--%>
            <div class="col-12 col-md-6">
                <label for="txtApellido" class="form-label">Apellido <span class="text-danger">*</span></label>
                <asp:TextBox ID="txtApellido" runat="server" placeholder="Ej.: López" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                    ControlToValidate="txtApellido"
                    ErrorMessage="El apellido es obligatorio."
                    CssClass="text-danger small"
                    Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revApellido" runat="server"
                    ControlToValidate="txtApellido"
                    ValidationExpression="^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]{2,50}$"
                    ErrorMessage="El apellido solo puede contener letras y espacios."
                    CssClass="text-danger small"
                    Display="Dynamic" />
            </div>
        </div>

        <%--Genero y telefono--%>
        <div class="row g-3 mb-3">

            <%--Genero--%>
            <div class="col-12 col-md-6">
                <label for="ddlGenero" class="form-label">Género <span class="text-danger">*</span></label>
                <asp:DropDownList CssClass="form-select" ID="ddlGenero" runat="server">
                    <asp:ListItem Text="Seleccione un género..." Value="" />
                    <asp:ListItem Text="Masculino" Value="M" />
                    <asp:ListItem Text="Femenino" Value="F" />
                    <asp:ListItem Text="Otro" Value="O" />
                    <asp:ListItem Text="Prefiere no decir" Value="N" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvGenero" runat="server"
                    ControlToValidate="ddlGenero"
                    InitialValue=""
                    ErrorMessage="Debe seleccionar un género."
                    CssClass="text-danger small"
                    Display="Dynamic" />
            </div>

            <%--telefono--%>
            <div class="col-12 col-md-6">
                <label for="txtTelefono" class="form-label">Teléfono <span class="text-danger">*</span></label>
                <asp:TextBox ID="txtTelefono" runat="server" placeholder="Ej.: 1134123456" TextMode="Phone" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvTelefono" runat="server"
                    ControlToValidate="txtTelefono"
                    ErrorMessage="El teléfono es obligatorio."
                    CssClass="text-danger small"
                    Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revTelefono" runat="server"
                    ControlToValidate="txtTelefono"
                    ValidationExpression="^\d{6,15}$"
                    ErrorMessage="El teléfono debe contener solo números (6 a 15 dígitos)."
                    CssClass="text-danger small"
                    Display="Dynamic" />
            </div>
        </div>

        <%--email y estado--%>
        <div class="row g-3 mb-3">

            <%--email--%>
            <div class="col-12 col-md-6">
                <label for="txtEmail" class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" placeholder="Ej.: juanperez@mail.com" TextMode="Email" CssClass="form-control"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                    ControlToValidate="txtEmail"
                    ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w{2,}$"
                    ErrorMessage="Ingrese un correo electrónico válido."
                    CssClass="text-danger small"
                    Display="Dynamic" />
            </div>

            <%--Estado--%>
            <div class="col-12 col-md-6">
                <label for="ddlEstado" class="form-label">Estado</label>
                <asp:DropDownList CssClass="form-select" ID="ddlEstado" runat="server">
                    <asp:ListItem Text="Activo" Value="A" Selected="True" />
                    <asp:ListItem Text="Inactivo" Value="I" />
                </asp:DropDownList>
            </div>
        </div>

        <%--cobertura y plan--%>
        <div class="row g-3 mb-3">

            <%--Cobertura--%>
            <div class="col-12 col-md-6">
                <label for="ddlCobertura" class="form-label">Cobertura <span class="text-danger">*</span></label>
                <asp:DropDownList CssClass="form-select" ID="ddlCobertura" runat="server">
                    <asp:ListItem Text="Seleccione una cobertura..." Value="" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvCobertura" runat="server"
                    ControlToValidate="ddlCobertura"
                    InitialValue=""
                    ErrorMessage="Debe seleccionar una cobertura."
                    CssClass="text-danger small"
                    Display="Dynamic" />
            </div>

            <%--plan--%>
            <div class="col-12 col-md-6">
                <label for="ddlPlan" class="form-label">Plan</label>
                <asp:DropDownList CssClass="form-select" ID="ddlPlan" runat="server">
                    <asp:ListItem Text="Seleccione un plan..." Value="" />
                </asp:DropDownList>
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
                    OnClick="btnCancelar_Click" CausesValidation="false" />
            </div>

            <%-- boton guardar --%>
            <div class="col-6 col-sm-4 col-md-2 d-grid">
                <asp:Button ID="btnGuardar" runat="server"
                    Text="Guardar"
                    CssClass="btn btn-primary btn-sm" />
            </div>
        </div>
    </div>

</div>
