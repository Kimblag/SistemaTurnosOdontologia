<%@ Page Title="Gestión de Planes" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.CoberturasPlanes.Planes.Index" %>

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
                        <asp:TextBox ID="txtBuscarPlanes" runat="server"
                            CssClass="form-control border-start-0"
                            placeholder="Buscar planes por nombre..." />
                    </div>
                </div>

                <div>
                    <asp:DropDownList ID="ddlCampo" runat="server" CssClass="form-select" Width="170px"
                        OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Filtrar por..." Value="" />
                        <asp:ListItem Text="Cobertura" Value="Cobertura" />
                        <asp:ListItem Text="Estado" Value="Estado" />
                    </asp:DropDownList>
                </div>

                <div>
                    <asp:DropDownList ID="ddlCriterio" runat="server" CssClass="form-select" Width="250px" Enabled="false">
                        <asp:ListItem Text="Seleccione un criterio" Value="" />
                    </asp:DropDownList>
                </div>

                <div class="d-flex gap-2 border-start ps-3 ms-1">
                    <asp:Button ID="btnBuscar" runat="server" Text="Aplicar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click" />
                </div>

                <div class="border-start ps-3 ms-1">
                    <asp:Button ID="btnNuevaPlanes" runat="server"
                        Text="+ Nuevo"
                        OnClick="btnNuevoPlan_Click"
                        CssClass="btn btn-success text-nowrap" />
                </div>

            </div>
        </div>

        <%--tabla listado--%>
        <div class="content-wrapper">
            <asp:GridView ID="gvPlanes" runat="server"
                AutoGenerateColumns="false"
                OnRowDataBound="gvPlanes_RowDataBound"
                OnPageIndexChanging="gvPlanes_PageIndexChanging"
                OnRowCommand="gvPlanes_RowCommand"
                DataKeyNames="IdPlan"
                CssClass="table gridview mb-0"
                AllowPaging="True" PageSize="7">

                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre del Plan" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="PorcentajeCobertura" HeaderText="% de Cobertura" />
                    <asp:BoundField DataField="NombreCobertura" HeaderText="Cobertura Asociada" />

                    <%--columna estado--%>
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <div id="lblEstado" runat="server" class="badge"><%# Eval("Estado") %></div>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <%--columna acciones--%>
                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="240px" ItemStyle-CssClass="text-start">
                        <ItemTemplate>

                            <div class="btn-group btn-group-sm" role="group">

                                <asp:LinkButton ID="btnEditar"
                                    runat="server"
                                    ToolTip="Editar Datos"
                                    CssClass="btn btn-outline-secondary"
                                    CommandName="Editar"
                                    CommandArgument='<%# Eval("IdPlan") %>'>
                             <i class="bi bi-pencil"></i>
                                </asp:LinkButton>

                                <button type="button"
                                    class="btn btn-outline-danger"
                                    data-id='<%# Eval("IdPlan") %>'
                                    onclick="abrirModalConfirmacion('<%# Eval("IdPlan") %>', 'plan')">
                                    <i class="bi bi-trash"></i>
                                </button>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

                <EmptyDataTemplate>
                    <div class="empty-state">
                        <i class="bi bi-shield-check fs-5"></i>
                        No hay Planes para mostrar.
                    </div>
                </EmptyDataTemplate>

            </asp:GridView>
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
