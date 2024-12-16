<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MP.Master" AutoEventWireup="true" CodeBehind="AdminVacacionesSolicitud.aspx.cs" Inherits="WebAppVacaciones.Pages.AdminVacacionesSolicitud" %>


<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Solicitudes Pendientes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        .boton-estandar {
            width: 120px;
            height: 40px;
            font-size: 14px;
            text-align: center;
            padding: 10px;
            margin-right: 5px;
        }

        .custom-button:hover {
            background-color: #08a0de;
        }

        .centered-container {
            display: flex;
            justify-content: center;
        }

        .container-custom {
            max-width: 1200px;
            width: 100%;
        }

        .notification {
            position: fixed;
            top: 10px;
            right: 10px;
            z-index: 1000;
            max-width: 300px;
        }
    </style>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bulma@0.9.4/css/bulma.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
     <section class="hero custom-hero">
     <div class="hero-body">
         <div class="container">
             <h1 class="title">Solicitudes Pendientes</h1>
             <br />
             <h2 class="subtitle">Aquí puedes consultar las solicitudes de dias de vacaciones pendientes.</h2>
         </div>
     </div>
 </section>

</asp:Content>
