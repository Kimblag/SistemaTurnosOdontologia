<%@ Page Title="Gestión de Coberturas" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.CoberturasPlanes.Coberturas.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic">

        <%--Filtros--%>
        <div class="container-fluid px-0 mb-4">
            <div class="d-flex flex-wrap align-items-center gap-2 bg-white p-3 rounded shadow-sm border w-100">

                <div class="flex-grow-1">
                    <div class="input-group">
                        <span class="input-group-text bg-white border-end-0 text-muted">
                            <i class="bi bi-search"></i>
                        </span>
                        <asp:TextBox ID="txtBuscarCobertura" runat="server"
                            CssClass="form-control border-start-0"
                            placeholder="Buscar cobertura por nombre..." />
                    </div>
                </div>


                <div>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select" Width="190px"
                        OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Text="Todos los estados" Value="todos" />
                        <asp:ListItem Text="Activo" Value="activo" />
                        <asp:ListItem Text="Inactivo" Value="inactivo" />
                    </asp:DropDownList>
                </div>

                <div class="d-flex gap-2 border-start ps-3 ms-1">
                    <asp:Button ID="btnBuscar" runat="server" Text="Aplicar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click" />
                </div>

                <%-- 4. Botón Nuevo --%>
                <div class="border-start ps-3 ms-1">
                    <asp:Button ID="btnNuevaCobertura" runat="server"
                        Text="+ Nueva"
                        OnClick="btnNuevaCobertura_Click"
                        CssClass="btn btn-success text-nowrap" />
                </div>

            </div>
        </div>

        <%--tabla listado--%>
        <div class="content-wrapper">
            <asp:GridView ID="gvCoberturas" runat="server"
                AutoGenerateColumns="false"
                OnRowDataBound="gvCoberturas_RowDataBound"
                OnPageIndexChanging="gvCoberturas_PageIndexChanging"
                OnRowCommand="gvCoberturas_RowCommand"
                DataKeyNames="IdCobertura"
                CssClass="table gridview mb-0"
                AllowPaging="True" PageSize="7"
                AllowSorting="true"
                OnSorting="gvCoberturas_Sorting"
                HeaderStyle-CssClass="gv-header">

                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="CantidadPlanes" HeaderText="Cantidad de Planes" />


                    <%--columna estado--%>
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <div id="lblEstado" runat="server" class="badge"><%# Eval("Estado") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <%--columna acciones--%>
                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="240px" ItemStyle-CssClass="text-end">
                        <ItemTemplate>

                            <div class="d-flex justify-content-end gap-2">
                                <button
                                    type="button"
                                    class="btn btn-success btn-sm shadow-sm d-flex align-items-center"
                                    data-nombre='<%# Eval("Nombre") %>'
                                    data-descripcion='<%# Eval("Descripcion") %>'
                                    data-planes='<%# string.Join("||", ((SGTO.Negocio.DTOs.CoberturaDto)Container.DataItem).NombrePlanes ?? new List<string>()) %>'
                                    onclick="abrirModalPlanes(this)">
                                    <i class="bi bi-link me-1"></i>Ver Planes
                                </button>

                                <div class="btn-group btn-group-sm" role="group">

                                    <%-- se cargan la lista de nombres en el atrbuto data para poder acceder a los planes actuales
                                sin necesidad de recargar porque la fila seleccionada ya tiene los datos en el DTO
                                    --%>

                                    <asp:LinkButton ID="btnEditar"
                                        runat="server"
                                        CssClass="btn btn-outline-secondary"
                                        CommandName="Editar"
                                        ToolTip="Editar Datos"
                                        CommandArgument='<%# Eval("IdCobertura") %>'>
                             <i class="bi bi-pencil"></i>
                                    </asp:LinkButton>

                                    <button type="button"
                                        class="btn btn-outline-danger"
                                        data-id='<%# Eval("IdCobertura") %>'
                                        title="Dar de baja"
                                        onclick="abrirModalConfirmacion('<%# Eval("IdCobertura") %>', 'cobertura')">
                                        <i class="bi bi-trash"></i>
                                    </button>
                                </div>

                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="bi bi-shield-check fs-5"></i>
                        No hay coberturas para mostrar.
                    </div>
                </EmptyDataTemplate>

            </asp:GridView>
        </div>




        <%--Modal planes--%>
        <div class="modal" tabindex="-1" id="modalPlanes" aria-labelledby="modalPlanesLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <div class="d-flex flex-column">
                            <p id="modalTitulo" class="modal-title mb-1 fs-5 fw-bold">Planes</p>
                            <p id="modalDesc" class="text-muted small mb-0"></p>
                        </div>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">

                        <%--lista para planes--%>
                        <ul id="listadoPlanes" class="list-group">
                        </ul>

                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>

    </div>



    <%--modal de confirmación--%>
    <div class="modal fade" id="modalConfirmar" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 id="modalConfirmarTitulo" class="modal-title">Confirmar acción</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <p id="modalConfirmarTexto"></p>
                </div>
                <div class="modal-footer">
                    <asp:HiddenField ID="hdnIdEliminar" runat="server" />
                    <asp:HiddenField ID="hdnTipoEliminar" runat="server" />
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnConfirmarEliminar" runat="server"
                        CssClass="btn btn-danger"
                        Text="Confirmar"
                        OnClick="btnConfirmarEliminar_Click" />
                </div>
            </div>
        </div>
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

            window.abrirModalPlanes = function (btn) {
                try {
                    const nombre = btn.getAttribute('data-nombre');
                    const desc = btn.getAttribute('data-descripcion');
                    const planesRaw = btn.getAttribute('data-planes') || '';
                    const planes = planesRaw ? planesRaw.split('||') : [];

                    document.getElementById('modalTitulo').textContent = nombre || 'Planes';
                    document.getElementById('modalDesc').textContent = desc || '';

                    const ul = document.getElementById('listadoPlanes');
                    ul.innerHTML = '';

                    if (planes.length > 0 && planes[0] !== '') {
                        planes.forEach(p => {
                            const li = document.createElement('li');
                            li.className = 'list-group-item';
                            li.textContent = p;
                            ul.appendChild(li);
                        });
                    } else {
                        const li = document.createElement('li');
                        li.className = 'list-group-item text-muted';
                        li.textContent = 'No hay planes registrados.';
                        ul.appendChild(li);
                    }

                    const modal = new bootstrap.Modal(document.getElementById('modalPlanes'));
                    modal.show();
                } catch (err) {
                    console.error('Error en abrirModalPlanes:', err);
                }
            };

            // modal de confirmación    
            window.abrirModalConfirmacion = function (id, tipo) {
                try {
                    document.getElementById('<%= hdnIdEliminar.ClientID %>').value = id;
                    document.getElementById('<%= hdnTipoEliminar.ClientID %>').value = tipo;

                    let titulo = "Confirmar acción";
                    let texto = "¿Está seguro que desea continuar?";

                    switch (tipo) {
                        case "paciente":
                            titulo = "Confirmar baja de paciente";
                            texto = "¿Está seguro de que desea dar de baja este paciente?";
                            break;
                        case "plan":
                            titulo = "Confirmar baja de plan";
                            texto = "¿Está seguro de que desea dar de baja este plan?";
                            break;
                        case "cobertura":
                            titulo = "Confirmar baja de cobertura";
                            texto = "¿Está seguro de que desea dar de baja esta cobertura?";
                            break;
                    }

                    document.getElementById('modalConfirmarTitulo').textContent = titulo;
                    document.getElementById('modalConfirmarTexto').textContent = texto;

                    new bootstrap.Modal(document.getElementById('modalConfirmar')).show();
                } catch (err) {
                    console.error("Error al abrir modal de confirmación:", err);
                }
            };


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
