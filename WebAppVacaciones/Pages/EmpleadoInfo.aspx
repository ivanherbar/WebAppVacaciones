<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Empleado.Master" AutoEventWireup="true" CodeBehind="EmpleadoInfo.aspx.cs" Inherits="WebAppVacaciones.Pages.EmpleadoInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Tu Información
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
                <h1 class="title">Tu Información</h1>
                <br />
                <h2 class="subtitle">Aquí puedes consultar, tu información y solicitar dias de vacaciones.</h2>
            </div>
        </div>
    </section>

    <section class="section">
        <div class="centered-container">
            <div class="container container-custom">
                

                <asp:GridView ID="gridDetallesEmpleado" runat="server" CssClass="table is-striped is-bordered is-hoverable" AutoGenerateColumns="false" OnRowCommand="gridDetallesEmpleado_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="ID_Empleado" HeaderText="ID" />
                        <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                        <asp:BoundField DataField="Nombre_Rol" HeaderText="Rol" />
                        <asp:BoundField DataField="Nombre_Region" HeaderText="Región" />
                        <asp:BoundField DataField="Nombre_Plaza" HeaderText="Plaza" />
                        <asp:BoundField DataField="Nombre_PDV" HeaderText="PDV" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Puesto" HeaderText="Puesto" />
                        <asp:BoundField DataField="Fecha_Ingreso" HeaderText="Ingreso" DataFormatString="{0:yyyy-MM-dd}" />
                        <asp:BoundField DataField="Mes_Aniversario" HeaderText="Mes Aniversario" />
                        <asp:BoundField DataField="Año_Aniversario" HeaderText="Año Aniversario" />
                        <asp:BoundField DataField="Antigüedad" HeaderText="Antigüedad" />
                        <asp:BoundField DataField="Dias_por_Año" HeaderText="Días por Año" />
                        <asp:BoundField DataField="Dias_Disponibles" HeaderText="Días Disponibles" />
                        <asp:BoundField DataField="Dias_Disfrutados" HeaderText="Días Disfrutados" />
                        <asp:TemplateField HeaderText="Vacaciones">
                            <ItemTemplate>
                                <asp:Button ID="btnVacaciones" runat="server" Text="Consultar" CommandName="Consultar" CommandArgument='<%# Eval("ID_Empleado") %>' CssClass="button is-info boton-estandar" />
                                <asp:Button ID="btnActualizar" runat="server" Text="Solicitar" CommandName="Actualizar" CommandArgument='<%# Eval("ID_Empleado") %>' CssClass="button is-warning boton-estandar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </section>

    <!-- Modal para mostrar las vacaciones -->
    <div class="modal" id="modalVacaciones">
        <div class="modal-background"></div>
        <div class="modal-card">
            <header class="modal-card-head">
                <p class="modal-card-title">Días de Vacaciones</p>

            </header>
            <section class="modal-card-body">
                <asp:GridView ID="gridVacaciones" runat="server" CssClass="table is-striped is-bordered" AutoGenerateColumns="false" OnRowCommand="gridVacaciones_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="ID_Empleado" HeaderText="ID Empleado" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                        <asp:BoundField DataField="TipoDia" HeaderText="TipoDia" />
                    </Columns>
                </asp:GridView>

            </section>
            <footer class="modal-card-foot">
                <button class="button" onclick="cerrarModal()">Cerrar</button>
            </footer>
        </div>
    </div>

    <script>
        function abrirModal() {
            document.getElementById('modalVacaciones').classList.add('is-active');
        }

        function cerrarModal() {
            document.getElementById('modalVacacione    s').classList.remove('is-active');
        }
    </script>

</asp:Content>
