<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MP.Master" AutoEventWireup="true" CodeBehind="ModUs.aspx.cs" Inherits="WebAppVacaciones.ModUs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
      <style>
      .custom-button:hover {
          background-color: #08a0de; /* Color de fondo al pasar el mouse */
      }

      .centered-container {
          display: flex;
          justify-content: center; /* Centrar horizontalmente */
      }

      .container-custom {
          max-width: 1200px; /* Ajustar el tamaño máximo del contenedor */
          width: 100%; /* Asegurar que el contenedor ocupe el 100% del ancho disponible */
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
  <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script> <!-- Biblioteca para notificaciones -->

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">

    <section class="hero custom-hero">
        <div class="hero-body">
            <div class="container">
                <h1 class="title">Gestion de Empleados</h1>
                <br />
                <h2 class="subtitle">Aquí puedes consultar, modificar o eliminar empleados.</h2>
            </div>
        </div>
    </section>


    <!-- GridView para mostrar resultados -->

    <section class="section">
        <div class="centered-container">
            <div class="container container-custom">
                <div class="field">
                    <label class="label">Nombre del Empleado</label>
                    <div class="control">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="input" Placeholder="Buscar..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
                    </div>
                </div>


                <asp:GridView ID="gridDetallesEmpleado" runat="server" CssClass="table is-striped is-bordered is-hoverable" AutoGenerateColumns="false" >
                    <Columns>
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:Button ID="btnConsultar" runat="server" Text="Consultar" CommandName="Consultar" CommandArgument='<%# Eval("Id_Usuario") %>' CssClass="button is-info" />
                                <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" CommandName="Actualizar" CommandArgument='<%# Eval("Id_Usuario") %>' CssClass="button is-warning" />
                                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CommandName="Eliminar" CommandArgument='<%# Eval("Id_Usuario") %>' CssClass="button is-danger" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Nombre_Region" HeaderText="Región" />
                        <asp:BoundField DataField="Nombre_Plaza" HeaderText="Plaza" />
                        <asp:BoundField DataField="Nombre_PDV" HeaderText="PDV" />
                        <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                        <asp:BoundField DataField="Nombre_Rol" HeaderText="Rol" />
                        <asp:BoundField DataField="Nombre_Empleado" HeaderText="Nombre" />
                        <asp:BoundField DataField="Puesto" HeaderText="Puesto" />
                        <asp:BoundField DataField="Fecha_Ingreso" HeaderText="Ingreso" DataFormatString="{0:yyyy-MM-dd}" />
                        <asp:BoundField DataField="Antigüedad" HeaderText="Antigüedad" />
                        <asp:BoundField DataField="Dias_por_Año" HeaderText="Días por Año" />
                        <asp:BoundField DataField="Dias_Disponibles" HeaderText="Días Disponibles" />
                        <asp:BoundField DataField="Dias_Disfrutados" HeaderText="Días Disfrutados" />
                    </Columns>
                </asp:GridView>

            </div>
        </div>
    </section>


</asp:Content>
