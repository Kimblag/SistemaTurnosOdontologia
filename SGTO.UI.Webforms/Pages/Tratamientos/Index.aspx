<%@ Page Title="Nomenclador de Tratamientos" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="SGTO.UI.Webforms.Pages.Tratamientos.Tratamientos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic">

        <%-- Filtros --%>
        <div class="container-fluid px-0 mb-4">
            <div class="d-flex flex-wrap align-items-center gap-2 bg-white p-3 rounded shadow-sm border w-100">

                <div class="flex-grow-1">
                    <div class="input-group">
                        <span class="input-group-text bg-white border-end-0 text-muted">
                            <i class="bi bi-search"></i>
                        </span>
                        <asp:TextBox ID="txtBuscar" runat="server"
                            CssClass="form-control border-start-0"
                            placeholder="Buscar tratamientos..."
                            />
                    </div>
                </div>
                <div>
                    <asp:DropDownList ID="ddlCampo" runat="server" CssClass="form-select" Width="160px"
                        OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Filtrar por..." Value="" />
                        <asp:ListItem Text="Especialidad" Value="Especialidad" />
                        <asp:ListItem Text="Estado" Value="Estado" />
                    </asp:DropDownList>
                </div>

                <div>
                    <asp:DropDownList ID="ddlCriterio" runat="server" CssClass="form-select" Width="260px" Enabled="false">
                        <asp:ListItem Text="Seleccione un criterio" Value="" />
                    </asp:DropDownList>
                </div>

                <div class="d-flex gap-2 border-start ps-3 ms-1">
                    <asp:Button ID="btnBuscar" runat="server" Text="Aplicar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click" />
                </div>

                <div class="border-start ps-3 ms-1">
                    <asp:Button ID="btnNuevoTratamiento" runat="server"
                        Text="+ Nuevo"
                        OnClick="btnNuevoTratamiento_Click"
                        CssClass="btn btn-success text-nowrap" />
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



                    <asp:TemplateField HeaderText="Acciones"
                        ItemStyle-Width="240px"
                        ItemStyle-CssClass="text-start">
                        <ItemTemplate>

                            <div class="btn-group btn-group-sm" role="group">

                                <asp:LinkButton ID="btnEditar"
                                    runat="server"
                                    CssClass="btn btn-outline-secondary"
                                    CommandName="Editar"
                                    ToolTip="Editar Datos"
                                    CommandArgument='<%# Eval("IdTratamiento") %>'> 
                                <i class="bi bi-pencil"></i>
                                </asp:LinkButton>

                                <button type="button"
                                    class="btn btn-outline-danger"
                                    title="Dar de baja"
                                    data-id='<%# Eval("IdTratamiento") %>'
                                    onclick="abrirModalConfirmacion('<%# Eval("IdTratamiento") %>')">
                                    <i class="bi bi-trash"></i>
                                </button>

                            </div>
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
