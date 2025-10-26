<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PacienteForm.ascx.cs" Inherits="SGTO.UI.Webforms.Controles.Pacientes.PacienteForm" %>

<div class="card shadow-sm p-5 w-100 gap-2">

    <%--campos--%>
    <div>
        <%--DNI y fecha de nacimiento--%>
        <div class="row mb-3">
            <%--DNI--%>
            <div class="col-12 col-md-6">
                <label for="txtDni" class="form-label">DNI</label>
                <asp:TextBox ID="txtDni" runat="server" placeholder="Ej.: 11234567" CssClass="form-control"></asp:TextBox>
            </div>

            <%--fecha nacimiento--%>
            <div class="col-12 col-md-6">
                <label for="txtFechaNacimiento" class="form-label">Fecha de Nacimiento</label>
                <asp:TextBox ID="txtFechaNacimiento" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <%--Nombre y apellido--%>
        <div class="row g-3 mb-3">

            <%--Nombre--%>
            <div class="col-12 col-md-6">
                <label for="txtNombre" class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server" placeholder="Ej.: Juan" CssClass="form-control"></asp:TextBox>
            </div>


            <%--Apellido--%>
            <div class="col-12 col-md-6">
                <label for="txtApellido" class="form-label">Apellido</label>
                <asp:TextBox ID="txtApellido" runat="server" placeholder="Ej.: López" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <%--Genero y telefono--%>
        <div class="row g-3 mb-3">

            <%--Genero--%>
            <div class="col-12 col-md-6">

                <label for="ddlGenero" class="form-label">Género</label>
                <asp:DropDownList CssClass="form-select" ID="ddlGenero" runat="server">
                    <asp:ListItem>Masculino</asp:ListItem>
                    <asp:ListItem>Femenino</asp:ListItem>
                    <asp:ListItem>Otro</asp:ListItem>
                    <asp:ListItem>Prefiere no decir</asp:ListItem>
                </asp:DropDownList>
            </div>

            <%--telefono--%>
            <div class="col-12 col-md-6">
                <label for="txtTelefono" class="form-label">Teléfono</label>
                <asp:TextBox ID="txtTelefono" runat="server" placeholder="Ej.: 1134123456" TextMode="Phone" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <%--email y estado--%>
        <div class="row g-3 mb-3">

            <%--email--%>
            <div class="col-12 col-md-6">
                <label for="txtEmail" class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" placeholder="Ej.: juanperez@mail.com" TextMode="Email" CssClass="form-control"></asp:TextBox>
            </div>

            <%--Estado--%>
            <div class="col-12 col-md-6">

                <label for="ddlEstado" class="form-label">Estado</label>
                <asp:DropDownList CssClass="form-select" ID="DropDownList1" runat="server">
                    <asp:ListItem Selected="True">Activo</asp:ListItem>
                    <asp:ListItem>Inactivo</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <%--cobertura y plan--%>
        <div class="row g-3 mb-3">

            <%--Cobertura--%>
            <div class="col-12 col-md-6">

                <label for="ddlCobertura" class="form-label">Estado</label>
                <asp:DropDownList CssClass="form-select" ID="ddlCobertura" runat="server">
                    <asp:ListItem Selected="True">Particular</asp:ListItem>
                    <asp:ListItem>OSDE</asp:ListItem>
                    <asp:ListItem>OSECAC</asp:ListItem>
                    <asp:ListItem>OSPIN</asp:ListItem>
                    <asp:ListItem>Swiss Medical</asp:ListItem>
                </asp:DropDownList>
            </div>

            <%--plan--%>
            <div class="col-12 col-md-6">

                <label for="ddlPlan" class="form-label">Estado</label>
                <asp:DropDownList CssClass="form-select" ID="ddlPlan" runat="server">
                    <asp:ListItem>310</asp:ListItem>
                    <asp:ListItem>200</asp:ListItem>
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
                    OnClick="btnCancelar_Click" />
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
