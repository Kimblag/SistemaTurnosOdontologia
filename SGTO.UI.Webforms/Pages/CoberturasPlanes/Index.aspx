<%@ Page Title="Coberturas y Planes" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.CoberturasPlanes.CoberturasPlanes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-generic justify-content-center">

        <div class="row g-4">

            <div class="col-12 col-md-6">
                <a href='<%= ResolveUrl("~/Pages/CoberturasPlanes/Coberturas/Index.aspx") %>'
                    class="card card-config bg-soft-primary shadow-sm border-0 text-decoration-none text-dark h-100 hover-scale">
                    <div class="card-body text-center d-flex flex-column justify-content-between p-5">
                        <div>
                            <div class="icon-wrapper bg-primary bg-opacity-10 text-primary mb-4 mx-auto rounded-circle d-flex align-items-center justify-content-center"
                                style="width: 80px; height: 80px;">
                                <i class="bi-shield-plus fs-1"></i>
                            </div>
                            <h3 class="card-title fw-bold mb-3">Coberturas</h3>
                            <p class="card-text text-muted fs-6">
                                Administre las Obras Sociales y Prepagas habilitadas en la clínica.
                            </p>
                        </div>
                        <div class="mt-4 fw-semibold text-primary">
                            Gestionar Coberturas <i class="bi bi-arrow-right ms-2"></i>
                        </div>
                    </div>
                </a>
            </div>

            <div class="col-12 col-md-6">
                <a href='<%= ResolveUrl("~/Pages/CoberturasPlanes/Planes/Index.aspx") %>'
                    class="card card-config bg-soft-success shadow-sm border-0 text-decoration-none text-dark h-100 hover-scale">
                    <div class="card-body text-center d-flex flex-column justify-content-between p-5">
                        <div>
                            <div class="icon-wrapper bg-success bg-opacity-10 text-success mb-4 mx-auto rounded-circle d-flex align-items-center justify-content-center"
                                style="width: 80px; height: 80px;">
                                <i class="bi bi-card-checklist fs-1"></i>
                            </div>
                            <h3 class="card-title fw-bold mb-3">Planes</h3>
                            <p class="card-text text-muted fs-6">
                                Configure los planes y porcentajes de cobertura asociados a cada entidad.
                            </p>
                        </div>
                        <div class="mt-4 fw-semibold text-success">
                            Gestionar Planes <i class="bi bi-arrow-right ms-2"></i>
                        </div>
                    </div>
                </a>
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
          <div class="modal-content border-0 shadow">
              <div class="modal-header bg-white border-bottom-0">
                  <h5 id="modalResultadoTitulo" class="modal-title fw-bold">Mensaje</h5>
                  <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
              </div>
              <div class="modal-body text-center py-4">
                  <div class="mb-3">
                      <i class="bi bi-info-circle text-primary" style="font-size: 3rem;"></i>
                  </div>
                  <p id="modalResultadoDesc" class="lead fs-6"></p>
              </div>
              <div class="modal-footer border-top-0 justify-content-center pb-4">
                  <button id="btnModalCerrar" type="button" class="btn btn-primary px-4" data-bs-dismiss="modal">Aceptar</button>
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
