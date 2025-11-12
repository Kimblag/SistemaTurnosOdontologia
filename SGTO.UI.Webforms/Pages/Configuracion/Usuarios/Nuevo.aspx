<%@ Page Title="Configuración - Nuevo Usuario" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Nuevo.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Configuracion.Usuarios.Nuevo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="card shadow-sm p-5 gap-4">


        <div class="d-flex justify-content-between align-items-center mb-3">
            <div>
                <h5 class="fw-semibold mb-1">Nuevo Usuario</h5>
                <p class="text-muted mb-0" id="lblPaso" runat="server">
                    Paso 1 de 2 · Seleccione el rol del usuario.
                </p>
            </div>
            <asp:Button ID="btnVolver" runat="server"
                Text="Volver"
                CssClass="btn btn-outline-secondary btn-sm"
                OnClick="btnVolver_Click" />
        </div>


        <div class="row gy-3 mb-3">
            <div class="col-12 col-md-5">
                <label for="ddlRol" class="form-label fw-semibold">Rol</label>
                <asp:DropDownList
                    ID="ddlRol"
                    runat="server"
                    CssClass="form-select"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlRol_SelectedIndexChanged">
                    <asp:ListItem Value="">Seleccione un rol...</asp:ListItem>
                    <asp:ListItem Value="Administrador">Administrador</asp:ListItem>
                    <asp:ListItem Value="Recepcionista">Recepcionista</asp:ListItem>
                    <asp:ListItem Value="Médico">Médico</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <hr class="my-3" />


        <asp:Panel ID="panelCamposGenerales" runat="server" Visible="false">
            <div class="row gy-4">

                <div class="col-12 col-md-6">
                    <label for="txtNombre" class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ingrese el nombre..." />
                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                        ControlToValidate="txtNombre"
                        ErrorMessage="El nombre es obligatorio."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                    <asp:RegularExpressionValidator ID="revNombre" runat="server"
                        ControlToValidate="txtNombre"
                        ValidationExpression="^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$"
                        ErrorMessage="El nombre solo puede contener letras y espacios."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="txtApellido" class="form-label">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Ingrese el apellido..." />
                    <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                        ControlToValidate="txtApellido"
                        ErrorMessage="El apellido es obligatorio."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                    <asp:RegularExpressionValidator ID="revApellido" runat="server"
                        ControlToValidate="txtApellido"
                        ValidationExpression="^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$"
                        ErrorMessage="El apellido solo puede contener letras y espacios."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="txtEmail" class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" placeholder="Ingrese el email..." />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                        ControlToValidate="txtEmail"
                        ErrorMessage="El email es obligatorio."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server"
                        ControlToValidate="txtEmail"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                        ErrorMessage="Ingrese un email válido."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="txtNombreUsuario" class="form-label">Nombre de usuario</label>
                    <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" placeholder="Ingrese el nombre de usuario..." />
                    <asp:RequiredFieldValidator ID="rfvNombreUsuario" runat="server"
                        ControlToValidate="txtNombreUsuario"
                        ErrorMessage="El nombre de usuario es obligatorio."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="txtPassword" class="form-label">Contraseña</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Ingrese la contraseña..." />
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                        ControlToValidate="txtPassword"
                        ErrorMessage="La contraseña es obligatoria."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                    <asp:RegularExpressionValidator ID="revPassword" runat="server"
                        ControlToValidate="txtPassword"
                        ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$"
                        ErrorMessage="Debe tener al menos 6 caracteres y contener letras y números."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="txtConfirmarPassword" class="form-label">Confirmar Contraseña</label>
                    <asp:TextBox ID="txtConfirmarPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Confirme la contraseña..." />
                    <asp:CompareValidator ID="cvPassword" runat="server"
                        ControlToValidate="txtConfirmarPassword"
                        ControlToCompare="txtPassword"
                        ErrorMessage="Las contraseñas no coinciden."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="ddlEstado" class="form-label">Estado</label>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select" Enabled="false">
                        <asp:ListItem Selected="True" Value="A">Activo</asp:ListItem>
                        <asp:ListItem Value="I">Inactivo</asp:ListItem>
                    </asp:DropDownList>
                </div>

            </div>
        </asp:Panel>

        <asp:Panel ID="panelCamposMedico" runat="server" Visible="false" CssClass="mt-5">

            <h6 class="fw-semibold mb-3">Datos del Médico</h6>

            <div class="row g-3 mb-3">
                <div class="col-12 col-md-6">
                    <label for="txtDni" class="form-label">DNI</label>
                    <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" placeholder="Ej.: 30123456" />
                    <asp:RequiredFieldValidator ID="rfvDni" runat="server"
                        ControlToValidate="txtDni"
                        ErrorMessage="El DNI es obligatorio."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                    <asp:RegularExpressionValidator ID="revDni" runat="server"
                        ControlToValidate="txtDni"
                        ValidationExpression="^\d{7,8}$"
                        ErrorMessage="Ingrese un DNI válido (7 u 8 dígitos)."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="txtFechaNacimiento" class="form-label">Fecha de Nacimiento</label>
                    <asp:TextBox ID="txtFechaNacimiento" runat="server" TextMode="Date" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvFechaNac" runat="server"
                        ControlToValidate="txtFechaNacimiento"
                        ErrorMessage="La fecha de nacimiento es obligatoria."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                    <asp:RangeValidator ID="rvFechaNac" runat="server"
                        ControlToValidate="txtFechaNacimiento"
                        Type="Date"
                        MinimumValue="1900-01-01"
                        ErrorMessage="La fecha de nacimiento debe ser válida y corresponder a un adulto."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
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
                    <asp:RequiredFieldValidator ID="rfvGenero" runat="server"
                        ControlToValidate="ddlGenero"
                        InitialValue=""
                        ErrorMessage="Debe seleccionar un género."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="txtTelefono" class="form-label">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" TextMode="Phone" placeholder="Ej.: 1141234567" />
                    <asp:RequiredFieldValidator ID="rfvTelefono" runat="server"
                        ControlToValidate="txtTelefono"
                        ErrorMessage="El teléfono es obligatorio."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                    <asp:RegularExpressionValidator ID="revTelefono" runat="server"
                        ControlToValidate="txtTelefono"
                        ValidationExpression="^\d{7,15}$"
                        ErrorMessage="Ingrese un número de teléfono válido (solo dígitos)."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                </div>
            </div>

            <div class="row g-3 mb-3">
                <div class="col-12 col-md-6">
                    <label for="txtMatricula" class="form-label">Matrícula</label>
                    <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control" placeholder="Ej.: 12345" />
                    <asp:RequiredFieldValidator ID="rfvMatricula" runat="server"
                        ControlToValidate="txtMatricula"
                        ErrorMessage="La matrícula es obligatoria."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                    <asp:RegularExpressionValidator ID="revMatricula" runat="server"
                        ControlToValidate="txtMatricula"
                        ValidationExpression="^\d{3,6}$"
                        ErrorMessage="Ingrese una matrícula válida (solo números, 3 a 6 dígitos)."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                </div>

                <div class="col-12 col-md-6">
                    <label for="ddlEspecialidad" class="form-label">Especialidad Principal</label>
                    <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select" />
                    <asp:RequiredFieldValidator ID="rfvEspecialidad" runat="server"
                        ControlToValidate="ddlEspecialidad"
                        InitialValue=""
                        ErrorMessage="Debe seleccionar una especialidad."
                        CssClass="text-danger small"
                        Display="Dynamic"
                        ValidationGroup="NuevoUsuario" />
                </div>
            </div>

            <hr class="my-4" />

            <h6 class="fw-semibold mb-3">Disponibilidad Semanal</h6>

            <p class="text-muted small mb-3">
                Seleccionar horarios dentro del rango de atención de la clínica: 
            <strong>
                <asp:Label ID="lblHorarioClinica" runat="server" Text=""></asp:Label></strong>
            </p>

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
                            <td>
                                <asp:CheckBox ID="chkLunes" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlHoraInicioLunes" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                            <td>
                                <asp:DropDownList ID="ddlHoraFinLunes" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Martes</td>
                            <td>
                                <asp:CheckBox ID="chkMartes" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlHoraInicioMartes" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                            <td>
                                <asp:DropDownList ID="ddlHoraFinMartes" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Miércoles</td>
                            <td>
                                <asp:CheckBox ID="chkMiercoles" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlHoraInicioMiercoles" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                            <td>
                                <asp:DropDownList ID="ddlHoraFinMiercoles" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Jueves</td>
                            <td>
                                <asp:CheckBox ID="chkJueves" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlHoraInicioJueves" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                            <td>
                                <asp:DropDownList ID="ddlHoraFinJueves" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Viernes</td>
                            <td>
                                <asp:CheckBox ID="chkViernes" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlHoraInicioViernes" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                            <td>
                                <asp:DropDownList ID="ddlHoraFinViernes" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Sábado</td>
                            <td>
                                <asp:CheckBox ID="chkSabado" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlHoraInicioSabado" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                            <td>
                                <asp:DropDownList ID="ddlHoraFinSabado" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Domingo</td>
                            <td>
                                <asp:CheckBox ID="chkDomingo" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlHoraInicioDomingo" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                            <td>
                                <asp:DropDownList ID="ddlHoraFinDomingo" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                        </tr>
                    </tbody>
                </table>
            </div>


        </asp:Panel>


        <asp:Panel ID="panelAcciones" runat="server" Visible="false" CssClass="mt-5">
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
                        Text="Guardar"
                        CssClass="btn btn-primary btn-sm"
                        CausesValidation="true"
                        OnClick="btnGuardar_Click" ValidationGroup="NuevoUsuario" />
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
