<%@ Page Title="Tratamientos" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="SGTO.UI.Webforms.Pages.Tratamientos.Tratamientos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic">

        <%-- Filtros --%>
        <div class="container-fluid px-0 mb-3">
            <div class="row g-2 align-items-center">

                <%-- Izquierda: buscador + filtros --%>
                <div class="col-12 col-lg-9 d-flex flex-wrap align-items-center gap-2">

                    <%-- Buscador --%>
                    <div class="col-auto flex-grow-1" style="min-width: 260px; max-width: 400px;">
                        <div class="input-group">
                            <span class="input-group-text bg-white border-end-0">
                                <i class="bi bi-search text-muted"></i>
                            </span>
                            <asp:TextBox
                                ID="txtBuscar"
                                runat="server"
                                CssClass="form-control border-start-0"
                                placeholder="Buscar tratamientos..."
                                AutoPostBack="true"
                                OnTextChanged="txtBuscar_TextChanged" />
                        </div>
                    </div>

                    <%-- Filtro Especialidad --%>
                    <div class="col-auto">
                        <asp:DropDownList
                            ID="ddlEspecialidad"
                            runat="server"
                            CssClass="form-select"
                            Width="200"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlEspecialidad_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Text="Todas las especialidades" Value="0" />
                        </asp:DropDownList>
                    </div>

                    <%-- Filtro por Estado --%>
                    <div class="col-auto">
                        <asp:DropDownList
                            ID="ddlEstado"
                            runat="server"
                            CssClass="form-select"
                            Width="170"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">

                            <asp:ListItem Selected="True" Text="Todos" Value="todos" />
                            <asp:ListItem Text="Activo" Value="activo" />
                            <asp:ListItem Text="Inactivo" Value="inactivo" />
                        </asp:DropDownList>
                    </div>

                    <%-- BOTONES DE FILTRO  --%>
                    <div class="col-auto">
                        <asp:Button
                            ID="btnBuscar"
                            runat="server"
                            Text="Aplicar Filtro"
                            CssClass="btn btn-primary"
                            OnClick="btnBuscar_Click" />
                    </div>
                    <div class="col-auto">
                        <asp:Button
                            ID="btnLimpiar"
                            runat="server"
                            Text="Limpiar"
                            CssClass="btn btn-outline-secondary"
                            OnClick="btnLimpiar_Click" />
                    </div>

                </div>

                <%-- Derecha: botón nuevo tratamiento --%>
                <div class="col-12 col-lg-3 text-lg-end">
                    <asp:Button
                        ID="btnNuevoTratamiento"
                        runat="server"
                        Text="+ Nuevo Tratamiento"
                        OnClick="btnNuevoTratamiento_Click"
                        CssClass="btn btn-sm btn-primary fw-semibold px-3 py-2 d-flex d-lg-inline-flex align-items-center gap-1 mx-auto mx-lg-0" />
                </div>

            </div>
        </div>

        <%-- Tabla --%>
        <div class="content-wrapper">

            <asp:GridView ID="gvTratamientos" runat="server"
                AutoGenerateColumns="false"
                CssClass="table gridview mb-0"
                DataKeyNames="IdTratamiento"
                AllowPaging="True" PageSize="7"
                OnRowDataBound="gvTratamientos_RowDataBound"
                OnPageIndexChanging="gvTratamientos_PageIndexChanging"
                OnRowCommand="gvTratamientos_RowCommand">

                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre de Tratamiento" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="CostoBase" HeaderText="Costo" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="NombreEspecialidad" HeaderText="Especialidad" />

                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <div id="lblEstado" runat="server" class="badge"><%# Eval("Estado") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar"
                                runat="server"
                                CssClass="btn btn-outline-secondary btn-sm me-1"
                                CommandName="Editar"
                                CommandArgument='<%# Eval("IdTratamiento") %>'> 
                                <i class="bi bi-pencil"></i>
                            </asp:LinkButton>

                            <button type="button"
                                class="btn btn-outline-danger btn-sm"
                                data-id='<%# Eval("IdTratamiento") %>'
                                onclick="abrirModalConfirmacion('<%# Eval("IdTratamiento") %>')">
                                <i class="bi bi-x"></i>
                            </button>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="bi bi-x-octagon"></i>
                        No hay tratamientos para mostrar. 
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>

        </div>
    </div>

    <asp:HiddenField ID="hdnIdEliminar" runat="server" />

    <div class="modal fade" id="modalConfirmar" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 id="modalConfirmarTitulo" class="modal-title">Confirmar baja</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <p id="modalConfirmarTexto">¿Está seguro de que desea dar de baja este tratamiento?</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnConfirmarEliminar"
                        runat="server"
                        CssClass="btn btn-danger"
                        Text="Sí, dar de baja"
                        UseSubmitBehavior="false"
                        OnClick="btnConfirmarEliminar_Click" />
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modalResultado" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 id="modalResultadoTitulo" class="modal-title">Resultado</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <p id="modalResultadoDescripcion">Operación realizada.</p>
                </div>
                <div class="modal-footer">
                    <a id="modalResultadoLink" class="btn btn-primary" href="#" data-bs-dismiss="modal">Aceptar</a>
                </div>
            </div>
        </div>
    </div>

    <%-- Script para el modal --%>
    <script>
        document.addEventListener("DOMContentLoaded", () => {
            window.abrirModalConfirmacion = function (id) {
                try {
                    document.getElementById('<%= hdnIdEliminar.ClientID %>').value = id;
                    document.getElementById('modalConfirmarTitulo').textContent = "Confirmar baja de tratamiento";
                    document.getElementById('modalConfirmarTexto').textContent = "¿Está seguro de que desea dar de baja este tratamiento?";
                    new bootstrap.Modal(document.getElementById('modalConfirmar')).show();
                } catch (err) {
                    console.error("Error al abrir modal de confirmación:", err);
                }
            };

            // Script para el modal de resultado
            window.abrirModalResultado = function (titulo, descripcion, href) {
                if (titulo) document.getElementById('modalResultadoTitulo').textContent = titulo;
                if (descripcion) document.getElementById('modalResultadoDescripcion').textContent = descripcion;
                if (href) document.getElementById('modalResultadoLink').setAttribute('href', href);
                new bootstrap.Modal(document.getElementById('modalResultado')).show();
            };
        });
    </script>

</asp:Content>
