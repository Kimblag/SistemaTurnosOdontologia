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
                </div>

                <%-- Descripción --%>
                <div class="col-12">
                    <label for="txtDescripcion" class="form-label">Descripción</label>
                    <asp:TextBox
                        ID="txtDescripcion"
                        runat="server"
                        TextMode="MultiLine"
                        placeholder="Ingrese la descripción..."
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
                        <asp:ListItem Selected="True">Activo</asp:ListItem>
                        <asp:ListItem>Inactivo</asp:ListItem>
                    </asp:DropDownList>
                </div>

            </div>

            <%--Permisos del rol--%>
            <div class="row gy-4">
                <h5>Permisos del Rol</h5>
                <div class="m-0 border border-1"></div>

                <%--Tabla con tipos de permiso por módulos--%>
                <div>

                    <%-- Encabezado de columnas --%>
                    <div class="row fw-bold border-bottom py-2">
                        <div class="col text-start">Módulo</div>
                        <div class="col">Ver</div>
                        <div class="col">Crear</div>
                        <div class="col">Editar</div>
                        <div class="col">Eliminar</div>
                        <div class="col">Activar</div>
                        <div class="col">Desactivar</div>
                    </div>

                    <div class="row py-2 border-bottom">
                        <div class="col text-start fw-semibold">Inicio</div>
                        <div class="col">
                            <asp:CheckBox ID="chkInicioVer" runat="server" />
                        </div>
                        <div class="col">
                            <asp:CheckBox ID="chkInicioCrear" runat="server" />
                        </div>
                        <div class="col">
                            <asp:CheckBox ID="chkInicioEditar" runat="server" />
                        </div>
                        <div class="col">
                            <asp:CheckBox ID="chkInicioEliminar" runat="server" />
                        </div>
                        <div class="col">
                            <asp:CheckBox ID="chkInicioActivar" runat="server" />
                        </div>
                        <div class="col">
                            <asp:CheckBox ID="chkInicioDesactivar" runat="server" />
                        </div>
                    </div>

                     <div class="row py-2 border-bottom">
                        <div class="col fw-bold">
                            Turnos
                        </div>
                        <div class="col">
                            <asp:CheckBox ID="chkTurnosVer" runat="server" />
                        </div>
                        <div class="col">
                            <asp:CheckBox ID="chkTurnosCrear" runat="server" />
                        </div>
                        <div class="col">
                            <asp:CheckBox ID="chkTurnosEditar" runat="server" />
                        </div>
                        <div class="col">
                            <asp:CheckBox ID="chkTurnosEliminar" runat="server" />
                        </div>
                        <div class="col">
                            <asp:CheckBox ID="chkTurnosActivar" runat="server" />
                        </div>
                        <div class="col">
                            <asp:CheckBox ID="chkTurnosDesactivar" runat="server" />
                        </div>
                    </div>

                     <div class="row py-2 border-bottom">
                        <div class="col fw-bold">Pacientes</div>
                        <div class="col">
                            <asp:CheckBox ID="chkPacientesVer" runat="server" />
                        </div>
                        <div class="col">
                            <asp:CheckBox ID="chkPacientesCrear" runat="server" />
                        </div>
                        <div class="col">
                            <asp:CheckBox ID="chkPacientesEditar" runat="server" />
                        </div>
                        <div class="col">
                            <asp:CheckBox ID="chkPacientesEliminar" runat="server" />
                        </div>
                        <div class="col">
                            <asp:CheckBox ID="chkPacientesActivar" runat="server" />
                        </div>
                        <div class="col">
                            <asp:CheckBox ID="chkPacientesDesactivar" runat="server" />
                        </div>
                    </div>

                     <div class="row py-2 border-bottom">
                        <div class="col fw-bold">Médicos</div>
                        <div class="col">
                            <asp:CheckBox ID="chkMedicosVer" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkMedicosCrear" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkMedicosEditar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkMedicosEliminar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkMedicosActivar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkMedicosDesactivar" runat="server" /></div>
                    </div>


                     <div class="row py-2 border-bottom">
                        <div class="col fw-bold">Coberturas</div>
                        <div class="col">
                            <asp:CheckBox ID="chkCoberturasVer" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkCoberturasCrear" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkCoberturasEditar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkCoberturasEliminar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkCoberturasActivar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkCoberturasDesactivar" runat="server" /></div>
                    </div>


                     <div class="row py-2 border-bottom">
                        <div class="col fw-bold">Planes</div>
                        <div class="col">
                            <asp:CheckBox ID="chkPlanesVer" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkPlanesCrear" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkPlanesEditar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkPlanesEliminar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkPlanesActivar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkPlanesDesactivar" runat="server" /></div>
                    </div>


                     <div class="row py-2 border-bottom">
                        <div class="col fw-bold">Especialidades</div>
                        <div class="col">
                            <asp:CheckBox ID="chkEspecialidadesVer" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkEspecialidadesCrear" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkEspecialidadesEditar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkEspecialidadesEliminar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkEspecialidadesActivar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkEspecialidadesDesactivar" runat="server" /></div>
                    </div>


                     <div class="row py-2 border-bottom">
                        <div class="col fw-bold">Tratamientos</div>
                        <div class="col">
                            <asp:CheckBox ID="chkTratamientosVer" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkTratamientosCrear" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkTratamientosEditar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkTratamientosEliminar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkTratamientosActivar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkTratamientosDesactivar" runat="server" /></div>
                    </div>


                     <div class="row py-2 border-bottom">
                        <div class="col fw-bold">Reportes</div>
                        <div class="col">
                            <asp:CheckBox ID="chkReportesVer" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkReportesCrear" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkReportesEditar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkReportesEliminar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkReportesActivar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkReportesDesactivar" runat="server" /></div>
                    </div>


                     <div class="row py-2 border-bottom">
                        <div class="col fw-bold">Configuración</div>
                        <div class="col">
                            <asp:CheckBox ID="chkConfiguracionVer" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkConfiguracionCrear" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkConfiguracionEditar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkConfiguracionEliminar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkConfiguracionActivar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkConfiguracionDesactivar" runat="server" /></div>
                    </div>


                     <div class="row py-2 border-bottom">
                        <div class="col fw-bold">Usuarios</div>
                        <div class="col">
                            <asp:CheckBox ID="chkUsuariosVer" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkUsuariosCrear" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkUsuariosEditar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkUsuariosEliminar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkUsuariosActivar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkUsuariosDesactivar" runat="server" /></div>
                    </div>

                     <div class="row py-2 border-bottom">
                        <div class="col fw-bold">Roles</div>
                        <div class="col">
                            <asp:CheckBox ID="chkRolesVer" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkRolesCrear" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkRolesEditar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkRolesEliminar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkRolesActivar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkRolesDesactivar" runat="server" /></div>
                    </div>


                     <div class="row py-2 border-bottom">
                        <div class="col fw-bold">Parámetros del Sistema</div>
                        <div class="col">
                            <asp:CheckBox ID="chkParametrosVer" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkParametrosCrear" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkParametrosEditar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkParametrosEliminar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkParametrosActivar" runat="server" /></div>
                        <div class="col">
                            <asp:CheckBox ID="chkParametrosDesactivar" runat="server" /></div>
                    </div>


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
