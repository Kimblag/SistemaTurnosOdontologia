<%@ Page Title="Configuración - Editar Usuarios" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Editar.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Configuracion.Usuarios.Editar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="card shadow-sm p-5 gap-4">


        <div class="d-flex justify-content-end">
            <asp:Button ID="btnVolver" runat="server"
                Text="Volver"
                CssClass="btn btn-outline-secondary btn-sm"
                OnClick="btnVolver_Click" />
        </div>


        <asp:Panel ID="panelCamposGenerales" runat="server">
            <div class="row gy-4">

                <div class="col-12 col-md-6">
                    <label for="txtNombre" class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="txtApellido" class="form-label">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="txtEmail" class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="txtNombreUsuario" class="form-label">Nombre de usuario</label>
                    <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="txtPassword" class="form-label">Nueva contraseña (opcional)</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Dejar en blanco para no cambiar" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="txtConfirmarPassword" class="form-label">Confirmar contraseña</label>
                    <asp:TextBox ID="txtConfirmarPassword" runat="server" TextMode="Password" CssClass="form-control" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="ddlRol" class="form-label">Rol</label>
                    <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select" Enabled="false">
                        <asp:ListItem Value="1">Administrador</asp:ListItem>
                        <asp:ListItem Value="2">Recepcionista</asp:ListItem>
                        <asp:ListItem Value="3">Médico</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-12 col-md-6">
                    <label for="ddlEstado" class="form-label">Estado</label>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                        <asp:ListItem Value="A">Activo</asp:ListItem>
                        <asp:ListItem Value="I">Inactivo</asp:ListItem>
                    </asp:DropDownList>
                </div>

            </div>
        </asp:Panel>

        <%--  por si es medico--%>
        <asp:Panel ID="panelCamposMedico" runat="server" Visible="false" CssClass="mt-5">
            <h6 class="fw-semibold mb-3">Datos del Médico</h6>

            <div class="row g-3 mb-3">
                <div class="col-12 col-md-6">
                    <label for="txtDni" class="form-label">DNI</label>
                    <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="txtFechaNacimiento" class="form-label">Fecha de Nacimiento</label>
                    <asp:TextBox ID="txtFechaNacimiento" runat="server" TextMode="Date" CssClass="form-control" />
                </div>
            </div>

            <div class="row g-3 mb-3">
                <div class="col-12 col-md-6">
                    <label for="ddlGenero" class="form-label">Género</label>
                    <asp:DropDownList ID="ddlGenero" runat="server" CssClass="form-select">
                        <asp:ListItem Value="">Seleccione...</asp:ListItem>
                        <asp:ListItem Value="M">Masculino</asp:ListItem>
                        <asp:ListItem Value="F">Femenino</asp:ListItem>
                        <asp:ListItem Value="O">Otro</asp:ListItem>
                        <asp:ListItem Value="N">Prefiere no decir</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-12 col-md-6">
                    <label for="txtTelefono" class="form-label">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
                </div>
            </div>

            <div class="row g-3 mb-3">
                <div class="col-12 col-md-6">
                    <label for="txtMatricula" class="form-label">Matrícula Profesional (solo números)</label>
                    <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="ddlEspecialidad" class="form-label">Especialidad Principal</label>
                    <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select" />
                </div>
            </div>


            <div class="mt-4">
    <label class="fw-semibold">Horarios de atención</label>
    <p class="small text-muted">Seleccione los días y franjas horarias de atención del médico.</p>
    <asp:Label ID="lblHorarioClinica" runat="server" CssClass="text-muted small d-block mb-2"></asp:Label>

    <div class="table-responsive mb-3">
        <table class="table table-bordered align-middle">
            <thead class="table-light">
                <tr>
                    <th>Día</th>
                    <th>Atiende</th>
                    <th>Desde</th>
                    <th>Hasta</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Lunes</td>
                    <td><asp:CheckBox ID="chkLunes" runat="server" /></td>
                    <td><asp:DropDownList ID="ddlHoraInicioLunes" runat="server" CssClass="form-select form-select-sm" /></td>
                    <td><asp:DropDownList ID="ddlHoraFinLunes" runat="server" CssClass="form-select form-select-sm" /></td>
                </tr>
                <tr>
                    <td>Martes</td>
                    <td><asp:CheckBox ID="chkMartes" runat="server" /></td>
                    <td><asp:DropDownList ID="ddlHoraInicioMartes" runat="server" CssClass="form-select form-select-sm" /></td>
                    <td><asp:DropDownList ID="ddlHoraFinMartes" runat="server" CssClass="form-select form-select-sm" /></td>
                </tr>
                <tr>
                    <td>Miércoles</td>
                    <td><asp:CheckBox ID="chkMiercoles" runat="server" /></td>
                    <td><asp:DropDownList ID="ddlHoraInicioMiercoles" runat="server" CssClass="form-select form-select-sm" /></td>
                    <td><asp:DropDownList ID="ddlHoraFinMiercoles" runat="server" CssClass="form-select form-select-sm" /></td>
                </tr>
                <tr>
                    <td>Jueves</td>
                    <td><asp:CheckBox ID="chkJueves" runat="server" /></td>
                    <td><asp:DropDownList ID="ddlHoraInicioJueves" runat="server" CssClass="form-select form-select-sm" /></td>
                    <td><asp:DropDownList ID="ddlHoraFinJueves" runat="server" CssClass="form-select form-select-sm" /></td>
                </tr>
                <tr>
                    <td>Viernes</td>
                    <td><asp:CheckBox ID="chkViernes" runat="server" /></td>
                    <td><asp:DropDownList ID="ddlHoraInicioViernes" runat="server" CssClass="form-select form-select-sm" /></td>
                    <td><asp:DropDownList ID="ddlHoraFinViernes" runat="server" CssClass="form-select form-select-sm" /></td>
                </tr>
                <tr>
                    <td>Sábado</td>
                    <td><asp:CheckBox ID="chkSabado" runat="server" /></td>
                    <td><asp:DropDownList ID="ddlHoraInicioSabado" runat="server" CssClass="form-select form-select-sm" /></td>
                    <td><asp:DropDownList ID="ddlHoraFinSabado" runat="server" CssClass="form-select form-select-sm" /></td>
                </tr>
                <tr>
                    <td>Domingo</td>
                    <td><asp:CheckBox ID="chkDomingo" runat="server" /></td>
                    <td><asp:DropDownList ID="ddlHoraInicioDomingo" runat="server" CssClass="form-select form-select-sm" /></td>
                    <td><asp:DropDownList ID="ddlHoraFinDomingo" runat="server" CssClass="form-select form-select-sm" /></td>
                </tr>
            </tbody>
        </table>
    </div>
</div>



        </asp:Panel>

        <asp:Panel ID="panelAcciones" runat="server" CssClass="mt-5">
            <div class="row justify-content-end gx-2 gy-2">
                <div class="col-6 col-sm-4 col-md-2 d-grid">
                    <asp:Button ID="btnCancelar" runat="server"
                        Text="Cancelar"
                        CssClass="btn btn-outline-secondary btn-sm"
                        CausesValidation="false"
                        OnClick="btnCancelar_Click" />
                </div>
                <div class="col-6 col-sm-4 col-md-2 d-grid">
                    <asp:Button ID="btnGuardar" runat="server"
                        Text="Guardar cambios"
                        CssClass="btn btn-primary btn-sm"
                        OnClick="btnGuardar_Click"
                        ValidationGroup="EditarUsuario" />
                </div>
            </div>
        </asp:Panel>

    </div>


    <%-- modal resultado --%>
    <div class="modal fade" id="modalResultado" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 id="modalResultadoTitulo" class="modal-title">Resultado</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <p id="modalResultadoDesc"></p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" id="btnModalCerrar" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>


    <script>

        document.addEventListener("DOMContentLoaded", () => {

            // modal resultado
            window.abrirModalResultado = function (titulo, descripcion) {
                try {
                    document.getElementById('modalResultadoTitulo').textContent = titulo || "Resultado";
                    document.getElementById('modalResultadoDesc').textContent = descripcion || "";
                    new bootstrap.Modal(document.getElementById('modalResultado')).show();
                } catch (err) {
                    console.error("Error al abrir modal de resultado:", err);
                }
            };
        });
    </script>

</asp:Content>
