<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="TratamientoForm.ascx.cs"
    Inherits="SGTO.UI.Webforms.Controles.Tratamientos.TratamientoForm" %>

<div class="card shadow-sm p-5 w-100 gap-2">

    <%-- Encabezado con ID (opcional) --%>
    <div class="d-flex justify-content-end mb-2">
        <small class="text-muted">
            ID Tratamiento: <asp:Literal ID="litIdTratamiento" runat="server" />
        </small>
    </div>

    <%-- Campos --%>
    <div>

        <%-- Nombre y Costo Base --%>
        <div class="row g-3 mb-3">

            <%-- Nombre --%>
            <div class="col-12 col-md-6">
                <asp:Label runat="server"
                           AssociatedControlID="txtNombre"
                           CssClass="form-label"
                           Text="Nombre" />
                <asp:TextBox ID="txtNombre" runat="server"
                    CssClass="form-control"
                    placeholder="Ej.: Blanqueamiento Dental"></asp:TextBox>
            </div>

            <%-- Costo Base --%>
            <div class="col-12 col-md-6">
                <asp:Label runat="server"
                           AssociatedControlID="txtCostoBase"
                           CssClass="form-label"
                           Text="Costo Base" />
                <div class="input-group">
                    <span class="input-group-text">$</span>
                    <asp:TextBox ID="txtCostoBase" runat="server"
                        CssClass="form-control"
                        placeholder="Ej.: 15000"
                        TextMode="Number"></asp:TextBox>
                </div>
                <small class="text-muted">Ingresá el costo en moneda local.</small>
            </div>
        </div>

        <%-- Descripción --%>
        <div class="row g-3 mb-3">
            <div class="col-12">
                <asp:Label runat="server"
                           AssociatedControlID="txtDescripcion"
                           CssClass="form-label"
                           Text="Descripción" />
                <asp:TextBox ID="txtDescripcion" runat="server"
                    CssClass="form-control"
                    TextMode="MultiLine"
                    Rows="3"
                    placeholder="Ej.: Procedimiento para aclarar el color de los dientes, eliminando manchas y decoloraciones."></asp:TextBox>
            </div>
        </div>

        <%-- Especialidad y Estado --%>
        <div class="row g-3 mb-3">

            <%-- Especialidad Asociada --%>
            <div class="col-12 col-md-6">
                <asp:Label runat="server"
                           AssociatedControlID="ddlEspecialidad"
                           CssClass="form-label"
                           Text="Especialidad Asociada" />
                <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select">
                    <%-- Cargar dinámicamente en el .cs --%>
                    <asp:ListItem Text="Seleccione una especialidad..." Value="" />
                </asp:DropDownList>
            </div>

            <%-- Estado --%>
            <div class="col-12 col-md-6">
                <asp:Label runat="server"
                           AssociatedControlID="ddlEstado"
                           CssClass="form-label"
                           Text="Estado" />
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                    <asp:ListItem Value="1" Selected="True">Activo</asp:ListItem>
                    <asp:ListItem Value="0">Inactivo</asp:ListItem>
                </asp:DropDownList>
            </div>

        </div>

    </div>

    <%-- Botones --%>
    <div class="h-100 w-100 align-content-end">
        <div class="row justify-content-end gx-2 gy-2">

            <%-- Cancelar --%>
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
                <%-- OnClick="btnGuardar_Click" /> --%>
            </div>
        </div>
    </div>

</div>