<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanesForm.ascx.cs" Inherits="SGTO.UI.Webforms.Controles.Coberturas.PlanesForm" %>

<div class="card shadow-sm p-5 gap-5 cobertura-form">

    <div class="mb-3">
        <div class="row gy-4">

            <%-- cobertura asociada --%>
            <div class="col-12">
                <label for="ddlCobertura" class="form-label">Cobertura Asociada</label>
                <asp:DropDownList
                    CssClass="form-select"
                    ID="ddlCobertura"
                    runat="server">
                    <asp:ListItem Selected="True">OSDE</asp:ListItem>
                    <asp:ListItem>OSPIN</asp:ListItem>
                    <asp:ListItem>OSECAC</asp:ListItem>
                </asp:DropDownList>
            </div>

            <%-- Nombre --%>
            <div class="col-12">
                <label for="txtNombreCobertura" class="form-label">Nombre del Plan</label>
                <asp:TextBox
                    ID="txtNombrePlan"
                    runat="server"
                    placeholder="Ingrese el nombre..."
                    CssClass="form-control">
                </asp:TextBox>
            </div>

            <%-- Descripción --%>
            <div class="col-12">
                <label for="txtDescripcionPlan" class="form-label">Descripción</label>
                <asp:TextBox
                    ID="txtDescripcionPlan"
                    runat="server"
                    placeholder="Ingrese la descripción..."
                    TextMode="MultiLine"
                    CssClass="form-control descripcion-cobertura">
                </asp:TextBox>
            </div>


            <%-- porcentaje cobertura --%>
            <div class="col-12">
                <label for="txtPorcentajeCobertura" class="form-label">Porcentaje de cobertura</label>
                <div class="input-group mb-3">
                    <asp:TextBox
                        ID="txtPorcentajeCobertura"
                        runat="server"
                        placeholder="Ej.: 40..."
                        TextMode="Number"
                        MaxLength="3"
                        CssClass="form-control">
                    </asp:TextBox>
                    <span class="input-group-text">%</span>
                </div>
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
