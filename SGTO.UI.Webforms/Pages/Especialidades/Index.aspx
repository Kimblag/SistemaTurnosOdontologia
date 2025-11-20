<%@ Page Title="Especialidades Clínicas" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="Index.aspx.cs"
    Inherits="SGTO.UI.Webforms.Pages.Especialidades.Especialidades" %>

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
                            placeholder="Buscar especialidades..." />
                    </div>
                </div>

                <div>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select" Width="190px">
                        <asp:ListItem Selected="True" Text="Todos los estados" Value="todos" />
                        <asp:ListItem Text="Activo" Value="activo" />
                        <asp:ListItem Text="Inactivo" Value="inactivo" />
                    </asp:DropDownList>
                </div>

                <div class="d-flex gap-2 border-start ps-3 ms-1">
                    <asp:Button ID="btnBuscar" runat="server" Text="Aplicar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click" />
                </div>

                <div class="border-start ps-3 ms-1">
                    <asp:Button ID="btnNuevaEspecialidad" runat="server"
                        Text="+ Nueva"
                        OnClick="btnNuevaEspecialidad_Click"
                        CssClass="btn btn-success text-nowrap" />
                </div>

            </div>
        </div>

        <%-- Tabla --%>
        <div class="content-wrapper">

            <asp:GridView ID="gvEspecialidades" runat="server"
                AutoGenerateColumns="false"
                CssClass="table gridview mb-0"
                DataKeyNames="IdEspecialidad"
                AllowPaging="True" PageSize="7"
                OnRowDataBound="gvEspecialidades_RowDataBound"
                OnPageIndexChanging="gvEspecialidades_PageIndexChanging"
                OnRowCommand="gvEspecialidades_RowCommand">

                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre de Especialidad" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />

                    <%--columna estado--%>

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
                                    ToolTip="Editar Datos"
                                    CommandName="Editar"
                                    CommandArgument='<%# Eval("IdEspecialidad") %>'>
                                    <i class="bi bi-pencil"></i>
                                </asp:LinkButton>


                                <button type="button"
                                    class="btn btn-outline-danger"
                                    title="Dar de baja"
                                    data-id='<%# Eval("IdEspecialidad") %>'
                                    onclick="abrirModalConfirmacion('<%# Eval("IdEspecialidad") %>')">
                                    <i class="bi bi-trash"></i>
                                </button>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="bi bi-x-octagon"></i>
                        No hay especialidades para mostrar.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>

        </div>
    </div>


    <!-- Hidden fields para pasar datos al servidor -->
    <asp:HiddenField ID="hdnIdEliminar" runat="server" />
    <asp:HiddenField ID="hdnTipoEliminar" runat="server" />

    <!-- Modal Confirmar -->
    <div class="modal fade" id="modalConfirmar" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 id="modalConfirmarTitulo" class="modal-title">Confirmar baja</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <p id="modalConfirmarTexto">¿Está seguro de que desea dar de baja esta especialidad?</p>
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

    <!-- Modal Resultado  -->
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

    <script>
        document.addEventListener("DOMContentLoaded", () => {
            window.abrirModalConfirmacion = function (id) {
                try {
                    document.getElementById('<%= hdnIdEliminar.ClientID %>').value = id;
                    document.getElementById('<%= hdnTipoEliminar.ClientID %>').value = "especialidad";

                    document.getElementById('modalConfirmarTitulo').textContent = "Confirmar baja de especialidad";
                    document.getElementById('modalConfirmarTexto').textContent = "¿Está seguro de que desea dar de baja esta especialidad?";

                    new bootstrap.Modal(document.getElementById('modalConfirmar')).show();
                } catch (err) {
                    console.error("Error al abrir modal de confirmación:", err);
                }
            };

            window.abrirModalResultado = function (titulo, descripcion, href) {

                if (titulo) document.getElementById('modalResultadoTitulo').textContent = titulo;
                if (descripcion) document.getElementById('modalResultadoDescripcion').textContent = descripcion;
                if (href) document.getElementById('modalResultadoLink').setAttribute('href', href);
                new bootstrap.Modal(document.getElementById('modalResultado')).show();
            };
        });
    </script>







</asp:Content>
