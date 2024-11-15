<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MP.Master" AutoEventWireup="true" CodeBehind="Modificar Usuario.aspx.cs" Inherits="WebAppVacaciones.Pages.Modificar_Usuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Modificar Usuarios
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
     <!-- Añadir CSS personalizado aquí -->
   <style>
       .custom-button:hover {
           background-color: #08a0de; /* Color de fondo al pasar el mouse */
       }
   </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
     <section class="hero custom-hero">
     <div class="hero-body">
         <div class="container">
             <h1 class="title">Modificar Usuario</h1>
             <br />
             <h2 class="subtitle">Edita el formulario para modificar al usuario.</h2>
         </div>
     </div>
 </section>

 <section class="section">
     <div class="container">
         <div class="field">
             <label class="label">Nombre Completo</label>
             <div class="control">
                 <asp:TextBox CssClass="input" ID="txtNombre" runat="server" Placeholder="Nombre completo" Required="true"></asp:TextBox>
             </div>
         </div>

         <div class="field">
             <label class="label">Nombre de Usuario</label>
             <div class="control">
                 <asp:TextBox CssClass="input" ID="txtUsuario" runat="server" Placeholder="Nombre de usuario" Required="true"></asp:TextBox>
             </div>
         </div>

         <div class="field">
             <label class="label">Contraseña</label>
             <div class="control">
                 <asp:TextBox CssClass="input" ID="txtClave" runat="server"  Placeholder="Contraseña" Required="true"></asp:TextBox>
             </div>
         </div>

         <div class="field">
             <div class="control">
                 <asp:Button CssClass="button custom-button" ID="btnModificar" runat="server" Text="Modificar" OnClick="btnModificar_Click" />
             </div>
         </div>

         <div class="field">
             <div class="control">
                 <asp:Label ID="lblMensaje" runat="server" CssClass="has-text-danger"></asp:Label>
             </div>
         </div>
     </div>
 </section>
</asp:Content>
