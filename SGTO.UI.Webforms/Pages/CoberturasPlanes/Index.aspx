<%@ Page Title="Coberturas y Planes" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.CoberturasPlanes.CoberturasPlanes" %>

<%--registro de controles--%>
<%@ Register Src="~/Controles/Coberturas/CoberturasListado.ascx" TagPrefix="uc1" TagName="CoberturasListado" %>
<%@ Register Src="~/Controles/Coberturas/PlanesListado.ascx" TagPrefix="uc1" TagName="PlanesListado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic">

        <%--Tabs de navegación--%>
        <ul class="nav nav-tabs" id="coberturasPlanesTabs" role="tablist">
            <li class="nav-item" role="presentation">
                <button class="nav-link active"
                    id="tab-coberturas"
                    data-bs-toggle="tab"
                    data-bs-target="#pane-coberturas"
                    type="button"
                    role="tab"
                    aria-controls="pane-coberturas"
                    aria-selected="true">
                    Coberturas
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link"
                    id="tab-planes"
                    data-bs-toggle="tab"
                    data-bs-target="#pane-planes"
                    type="button"
                    role="tab"
                    aria-controls="pane-planes"
                    aria-selected="false">
                    Planes
                </button>
            </li>
        </ul>

        <%--Contenido de las pestañas--%>
        <div class="tab-content" id="coberturasPlanesTabsContent">
            <div class="tab-pane fade show active"
                id="pane-coberturas"
                role="tabpanel"
                aria-labelledby="tab-coberturas">
                <uc1:CoberturasListado ID="CoberturasListadoControl" runat="server" />
            </div>

            <div class="tab-pane fade"
                id="pane-planes"
                role="tabpanel"
                aria-labelledby="tab-planes">
                <uc1:PlanesListado ID="PlanesListadoControl" runat="server" />
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
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>


    <script>

        document.addEventListener("DOMContentLoaded", () => {
            // Guardar tab activa en la sesion storage del navegador
            const tabLocalStorage = "activeTabCoberturasPlanes";

            const ultimaTab = sessionStorage.getItem(tabLocalStorage);
            //console.log(ultimaTab)
            if (ultimaTab) {
                const tab = document.querySelector(`[data-bs-target="${ultimaTab}"]`);
                //console.log("busqueda", tab)
                if (tab) {
                    const bootstrabTab = new bootstrap.Tab(tab);
                    bootstrabTab.show();
                }
            }

            /// guardar la tab cuandos e cambia 
            const botonTab = document.querySelectorAll('button[data-bs-toggle="tab"]');
            botonTab.forEach(tab => {
                tab.addEventListener('shown.bs.tab', function (event) {
                    const target = event.target.getAttribute('data-bs-target');
                    sessionStorage.setItem(tabLocalStorage, target);
                });
            });




            // modal de confirmación    
            window.abrirModalConfirmacion = function (id, tipo) {
                try {
                    document.getElementById('<%= hdnIdEliminar.ClientID %>').value = id;
                    document.getElementById('<%= hdnTipoEliminar.ClientID %>').value = tipo;

                    const titulo = tipo === "plan"
                        ? "Confirmar baja de plan"
                        : "Confirmar baja de cobertura";

                    const texto = tipo === "plan"
                        ? "¿Está seguro de que desea dar de baja este plan?"
                        : "¿Está seguro de que desea dar de baja esta cobertura?";

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
