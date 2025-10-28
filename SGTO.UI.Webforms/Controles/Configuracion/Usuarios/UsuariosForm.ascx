<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UsuariosForm.ascx.cs" Inherits="SGTO.UI.Webforms.Controles.Configuracion.Usuarios.UsuariosForm" %>


<div class="card shadow-sm p-5 gap-5 cobertura-form">

    <div class="col justify-content-between">
        <div class="mb-3">
            <div class="row gy-4">

                <%-- Nombre --%>
                <div class="col-12 col-md-6">
                    <label for="txtNombre" class="form-label">Nombre</label>
                    <asp:TextBox
                        ID="txtNombre"
                        runat="server"
                        placeholder="Ingrese el nombre..."
                        CssClass="form-control">
                    </asp:TextBox>
                </div>

                <%-- Apellido --%>
                <div class="col-12 col-md-6">
                    <label for="txtApellido" class="form-label">Apellido</label>
                    <asp:TextBox
                        ID="txtApellido"
                        runat="server"
                        placeholder="Ingrese el apellido..."
                        CssClass="form-control">
                    </asp:TextBox>
                </div>

                <%-- Email --%>
                <div class="col-12 col-md-6">
                    <label for="txtEmail" class="form-label">Email</label>
                    <asp:TextBox
                        ID="txtEmail"
                        runat="server"
                        placeholder="Ingrese el email..."
                        TextMode="Email"
                        CssClass="form-control">
                    </asp:TextBox>
                </div>

                <%-- Nombre usuario --%>
                <div class="col-12 col-md-6">
                    <label for="txtNombreUsuario" class="form-label">Nombre de Usuario</label>
                    <asp:TextBox
                        ID="txtNombreUsuario"
                        runat="server"
                        placeholder="Ingrese el nombre de usuario..."
                        CssClass="form-control">
                    </asp:TextBox>
                </div>

                <%-- contraseña --%>
                <div class="col-12 col-md-6">
                    <label for="txtPassword" class="form-label">Contraseña</label>
                    <asp:TextBox
                        ID="txtPassword"
                        runat="server"
                        TextMode="Password"
                        placeholder="Ingrese la contraseña..."
                        CssClass="form-control">
                    </asp:TextBox>
                </div>


                <%-- confirmar contraseña --%>
                <div class="col-12 col-md-6">
                    <label for="txtConfirmarPassword" class="form-label">Confirmar Contraseña</label>
                    <asp:TextBox
                        ID="txtConfirmarPassword"
                        runat="server"
                        TextMode="Password"
                        placeholder="Confirme la contraseña..."
                        CssClass="form-control">
                    </asp:TextBox>
                </div>

                <div class="col-12 col-md-6">
                    <label for="ddlRol" class="form-label">Rol Asignado</label>
                    <asp:DropDownList ID="ddlRol"
                        runat="server"
                        CssClass="form-select">
                        <asp:ListItem>Administrador</asp:ListItem>
                        <asp:ListItem>Odontólogo</asp:ListItem>
                        <asp:ListItem>Recepcionista</asp:ListItem>
                    </asp:DropDownList>
                </div>


                <%-- Estado --%>
                <div class="col-12 col-md-6">
                    <label for="ddlEstado" class="form-label">Estado</label>
                    <asp:DropDownList
                        CssClass="form-select"
                        ID="ddlEstado"
                        runat="server">
                        <asp:ListItem Selected="True">Activo</asp:ListItem>
                        <asp:ListItem>Inactivo</asp:ListItem>
                    </asp:DropDownList>
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
                    CssClass="btn btn-primary btn-sm" />
            </div>
        </div>
    </div>
</div>

