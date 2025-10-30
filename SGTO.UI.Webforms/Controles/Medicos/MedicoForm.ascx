<%@ Control Language="C#" AutoEventWireup="true" 
    CodeBehind="MedicoForm.ascx.cs" 
    Inherits="SGTO.UI.Webforms.Controles.Medicos.MedicoForm" %> <%-- NAMESPACE Y CLASE CAMBIADOS --%>

<div class="card shadow-sm p-5 w-100 gap-2">

    <%--campos--%>
    <div>
        <%--DNI y fecha de nacimiento--%>
        <div class="row mb-3">
            <%--DNI--%>
            <div class="col-12 col-md-6">
                <label for="<%= txtDni.ClientID %>" class="form-label">DNI</label> 
                <asp:TextBox ID="txtDni" runat="server" placeholder="Ej.: 11234567" CssClass="form-control"></asp:TextBox>
            </div>

            <%--fecha nacimiento--%>
            <div class="col-12 col-md-6">
                <label for="<%= txtFechaNacimiento.ClientID %>" class="form-label">Fecha de Nacimiento</label>
                <asp:TextBox ID="txtFechaNacimiento" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <%--Nombre y apellido--%>
        <div class="row g-3 mb-3">
            <%--Nombre--%>
            <div class="col-12 col-md-6">
                <label for="<%= txtNombre.ClientID %>" class="form-label">Nombre</label> 
                <asp:TextBox ID="txtNombre" runat="server" placeholder="Ej.: Julián" CssClass="form-control"></asp:TextBox>
            </div>

            <%--Apellido--%>
            <div class="col-12 col-md-6">
                <label for="<%= txtApellido.ClientID %>" class="form-label">Apellido</label>
                <asp:TextBox ID="txtApellido" runat="server" placeholder="Ej.: Mondillo" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <%--Genero y telefono--%>
        <div class="row g-3 mb-3">
            <%--Genero--%>
            <div class="col-12 col-md-6">
                <label for="<%= ddlGenero.ClientID %>" class="form-label">Género</label> 
                <asp:DropDownList CssClass="form-select" ID="ddlGenero" runat="server">
                    <%-- Options should match the Genero Enum --%>
                    <asp:ListItem Value="0">Masculino</asp:ListItem> 
                    <asp:ListItem Value="1">Femenino</asp:ListItem>
                    <asp:ListItem Value="2">Otro</asp:ListItem>
                    <asp:ListItem Value="3">Prefiere no decir</asp:ListItem> 
                </asp:DropDownList>
            </div>

            <%--telefono--%>
            <div class="col-12 col-md-6">
                <label for="<%= txtTelefono.ClientID %>" class="form-label">Teléfono</label>
                <asp:TextBox ID="txtTelefono" runat="server" placeholder="Ej.: 1134123456" TextMode="Phone" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <%--email y estado--%>
        <div class="row g-3 mb-3">
            <%--email--%>
            <div class="col-12 col-md-6">
                <label for="<%= txtEmail.ClientID %>" class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" placeholder="Ej.: julian.mondillo@mail.com" TextMode="Email" CssClass="form-control"></asp:TextBox>
            </div>

            <%--Estado--%>
            <div class="col-12 col-md-6">
                <label for="<%= ddlEstado.ClientID %>" class="form-label">Estado</label>
                <asp:DropDownList CssClass="form-select" ID="ddlEstado" runat="server"> 
                 
                    <asp:ListItem Value="1" Selected="True">Activo</asp:ListItem> 
                    <asp:ListItem Value="0">Inactivo</asp:ListItem> 
                </asp:DropDownList>
            </div>
        </div>

        <%-- Matrícula y Especialidad  --%>
        <div class="row g-3 mb-3">
            <%--Matrícula--%>
            <div class="col-12 col-md-6">
                <label for="<%= txtMatricula.ClientID %>" class="form-label">Matrícula</label>
                <asp:TextBox ID="txtMatricula" runat="server" placeholder="Ej.: MN-12345" CssClass="form-control"></asp:TextBox>
            </div>

            <%--Especialidad (Simplificado a una sola por ahora) --%>
            <div class="col-12 col-md-6">
                <label for="<%= ddlEspecialidad.ClientID %>" class="form-label">Especialidad Principal</label> 
                <asp:DropDownList CssClass="form-select" ID="ddlEspecialidad" runat="server"> <%-- CAMBIADO --%>
                    <%-- Deberías cargar estas opciones desde la base de datos o servicio --%>
                    <asp:ListItem Value="">Seleccione una especialidad...</asp:ListItem>
                    <asp:ListItem Value="1">Odontología General</asp:ListItem> 
                    <asp:ListItem Value="2">Ortodoncia</asp:ListItem>
                    <asp:ListItem Value="3">Endodoncia</asp:ListItem>
                    <asp:ListItem Value="4">Periodoncia</asp:ListItem>
                    <asp:ListItem Value="5">Odontopediatría</asp:ListItem>
                    <asp:ListItem Value="6">Cirugía Bucal</asp:ListItem>
                    <asp:ListItem Value="7">Implantología</asp:ListItem>
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
                <%-- OnClick="btnGuardar_Click" /> --%>
            </div>
        </div>
    </div>

</div>