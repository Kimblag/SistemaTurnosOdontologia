<%@ Page Language="C#" Title="Login" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SGTO.UI.Webforms.Pages.Login.Login" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Iniciar Sesión</title>

    <%-- Fuentes Google --%>
    <link href="https://fonts.googleapis.com" rel="preconnect" />
    <link crossorigin="" href="https://fonts.gstatic.com" rel="preconnect" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@600&family=Open+Sans:wght@400;600&display=swap" rel="stylesheet" />

    <link href="<%= ResolveUrl("~/Content/bootstrap.min.css") %>" rel="stylesheet" />
    <link href="<%= ResolveUrl("~/Content/site.css") %>" rel="stylesheet" />
    <script src="<%= ResolveUrl("~/Scripts/jquery-3.7.1.min.js") %>"></script>
    <script src="<%= ResolveUrl("~/Scripts/bootstrap.min.js") %>"></script>


    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css">

    <style>
        body {
            font-family: 'Open Sans', sans-serif;
            background-color: #F5F5F5;
            color: #1A1A1A;
        }

        h1, h2, h3 {
            font-family: 'Poppins', sans-serif;
        }

        .text-primary-custom {
            color: #007C91;
        }

        .bg-primary-custom {
            background-color: #007C91;
            border-color: #007C91;
        }

            .bg-primary-custom:hover {
                background-color: #006070;
                border-color: #006070;
            }

        .login-card {
            border-radius: 1rem;
            overflow: hidden;
            box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
            border: none;
        }

        .form-control-lg {
            font-size: 0.95rem;
            padding: 0.8rem 1rem;
            border-radius: 0.5rem;
            background-color: #F5F5F5;
            border: 1px solid #E5E7EB;
        }

            .form-control-lg:focus {
                border-color: #007C91;
                box-shadow: 0 0 0 0.2rem rgba(0, 124, 145, 0.25);
                background-color: #fff;
            }

        .side-image {
            object-fit: cover;
            height: 100%;
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container d-flex align-items-center justify-content-center min-vh-100 py-5">

            <div class="card login-card w-100" style="max-width: 900px;">
                <div class="row g-0">

                    <div class="col-md-6 p-5 d-flex flex-column justify-content-center bg-white">
                        <div class="mb-4">
                            <h1 class="h4 fw-bold text-primary-custom mb-1">Sistema de Turnos</h1>
                            <h2 class="h2 fw-bold text-dark">Iniciar Sesión</h2>
                        </div>

                        <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger d-flex align-items-center mb-4" role="alert">
                            <i class="bi bi-exclamation-triangle-fill me-2"></i>
                            <div>
                                <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                            </div>
                        </asp:Panel>

                        <div class="mb-3">
                            <label for="txtUsuario" class="form-label text-secondary small fw-bold">Usuario o Email</label>
                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control form-control-lg" placeholder="ej. usuario@dental.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario"
                                ErrorMessage="El usuario es requerido." CssClass="text-danger small mt-1" Display="Dynamic" ValidationGroup="LoginGroup"></asp:RequiredFieldValidator>
                        </div>

                        <div class="mb-4">
                            <label for="txtPassword" class="form-label text-secondary small fw-bold">Contraseña</label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control form-control-lg" TextMode="Password" placeholder="******"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                ErrorMessage="La contraseña es requerida." CssClass="text-danger small mt-1" Display="Dynamic" ValidationGroup="LoginGroup"></asp:RequiredFieldValidator>
                        </div>



                        <div class="d-grid gap-2">
                            <asp:Button ID="btnLogin" runat="server" Text="Iniciar sesión" CssClass="btn bg-primary-custom text-white fw-bold py-3 rounded-3"
                                OnClick="btnLogin_Click" ValidationGroup="LoginGroup" />
                        </div>
                    </div>

                    <div class="col-md-6 d-none d-md-block position-relative bg-primary-custom">
                        <div class="position-absolute top-0 start-0 w-100 h-100 bg-dark opacity-25"></div>

                        <img src="https://images.unsplash.com/photo-1606811841689-23dfddce3e95?q=80&w=1000&auto=format&fit=crop" alt="Consultorio Dental" class="side-image" />
                    </div>

                </div>
            </div>

        </div>
    </form>
</body>
</html>
