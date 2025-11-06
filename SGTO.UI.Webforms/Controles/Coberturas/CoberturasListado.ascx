<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoberturasListado.ascx.cs" Inherits="SGTO.UI.Webforms.Controles.Coberturas.CoberturasListado" %>


<div class="d-flex flex-column gap-3 pt-2">

    <%--Filtros--%>
    <div class="d-flex gap-2 align-items-center my-3 mb-3 justify-content-between">

        <div class="d-flex gap-2 align-items-center w-50">
            <asp:TextBox ID="txtBuscarCobertura" runat="server" CssClass="form-control" placeholder="Buscar cobertura..."></asp:TextBox>

            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select" AutoPostBack="True" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                <asp:ListItem Selected="True" Text="Todos" Value="todos" />
                <asp:ListItem Text="Activo" Value="activo" />
                <asp:ListItem Text="Inactivo" Value="inactivo" />
            </asp:DropDownList>

            <asp:Button
                ID="btnBuscar"
                runat="server"
                Text="Aplicar Filtro"
                CssClass="btn btn-outline-primary"
                OnClick="btnBuscar_Click" />

            <asp:Button
                ID="btnLimpiar"
                runat="server"
                Text="Limpiar"
                CssClass="btn btn-outline-secondary"
                OnClick="btnLimpiar_Click" />
        </div>

        <asp:Button ID="btnNuevaCobertura" runat="server" Text="+ Nueva Cobertura"
            CssClass="btn btn-primary btn-sm me-1" OnClick="btnNuevaCobertura_Click" />
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
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>

                        <%-- se cargan la lista de nombres en el atrbuto data para poder acceder a los planes actuales
                        sin necesidad de recargar porque la fila seleccionada ya tiene los datos en el DTO
                        --%>
                        <button
                            type="button"
                            class="btn btn-outline-primary btn-sm me-1"
                            data-nombre='<%# Eval("Nombre") %>'
                            data-descripcion='<%# Eval("Descripcion") %>'
                            data-planes='<%# string.Join("||", ((SGTO.Negocio.DTOs.CoberturaDto)Container.DataItem).NombrePlanes ?? new List<string>()) %>'
                            onclick="abrirModalPlanes(this)">
                            <i class="bi bi-link me-1"></i>Ver Planes
                        </button>

                        <asp:LinkButton ID="btnEditar"
                            runat="server"
                            ToolTip="Editar"
                            CssClass="btn btn-outline-secondary btn-sm me-1"
                            CommandName="Editar"
                            CommandArgument='<%# Eval("IdCobertura") %>'>
                             <i class="bi bi-pencil"></i>
                        </asp:LinkButton>

                        <button type="button"
                            class="btn btn-outline-danger btn-sm me-1"
                            data-id='<%# Eval("IdCobertura") %>'
                            onclick="abrirModalConfirmacion('<%# Eval("IdCobertura") %>', 'cobertura')">
                            <i class="bi bi-x"></i>
                        </button>
                        </button>
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


        <%--window.abrirModalConfirmacion = function (btn) {
            try {
                const idCobertura = btn.getAttribute('data-id');
                console.log("CONFIRMACION");
                console.log(idCobertura);

                const hidden = document.getElementById('<%= hdnIdCoberturaEliminar.ClientID %>');
                if (hidden) {
                    hidden.value = idCobertura;
                    console.log("hidden seteado con:", hidden.value);
                } else {
                    console.error("No se encontró el hidden field.");
                }

                const modal = new bootstrap.Modal(document.getElementById('modalConfirmarCobertura'));
                modal.show();
            } catch (err) {
                console.error("Error en abrirModalConfirmacion:", err);
            }
        };


        window.abrirModalResultado = function (titulo, descripcion) {
            try {
                document.getElementById('modalResultadoTitulo').textContent = titulo || 'Acción completada';
                document.getElementById('modalResultadoDesc').textContent = descripcion || '';
                const modal = new bootstrap.Modal(document.getElementById('modalResultadoCobertura'));
                modal.show();
            } catch (err) {
                console.error("Error en abrirModalResultado:", err);
            }
        };--%>

    });
</script>
