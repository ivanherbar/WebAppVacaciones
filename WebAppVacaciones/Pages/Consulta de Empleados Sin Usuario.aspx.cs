using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppVacaciones.Pages
{
    public partial class Consulta_Empleado_Sin_Usuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatos();
            }
        }

        private void CargarDatos(string filtro = "")
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_EmpleadosSinUsuario", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Filtro", filtro);

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    gridDetallesEmpleado.DataSource = dt;
                    gridDetallesEmpleado.DataBind();
                }
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtSearch.Text.Trim();
            CargarDatos(filtro);
        }

        // Manejo del comando para las acciones de actualizar y eliminar
        protected void gridDetallesEmpleado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                // Obtener el ID del empleado desde CommandArgument
                int idEmpleado = Convert.ToInt32(e.CommandArgument);

                // Llamar al método para eliminar
                EliminarUsuario(idEmpleado);
            }
            if (e.CommandName == "Actualizar")
            {

                CargarPDV();

                // Abrir el modal para mostrar los registros de vacaciones
                ScriptManager.RegisterStartupScript(this, GetType(), "abrirModal", "abrirModal();", true);
            }

        }


        // Método para cargar el DropDownList de PDV (Punto de Venta) desde la base de datos
        private void CargarPDV()
        {
            // Obtener la cadena de conexión desde el archivo Web.config
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            // Se utiliza una conexión a la base de datos SQL Server
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Usar un procedimiento almacenado llamado "sp_consultar_pdv"
                SqlCommand cmd = new SqlCommand("sp_consultar_pdv", con);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    // Abre la conexión a la base de datos
                    con.Open();
                    // Ejecuta el procedimiento y obtiene los resultados
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Establece los datos del DropDownList
                    ddlPDV.DataSource = reader;
                    ddlPDV.DataTextField = "Nombre_PDV";  // El nombre que se mostrará en el DropDownList
                    ddlPDV.DataValueField = "ID_PDV";     // El valor que se almacenará internamente
                    ddlPDV.DataBind();
                }
                catch (Exception ex)
                {
                    // En caso de error, muestra un mensaje
                    MostrarMensaje("Error al cargar PDV: " + ex.Message, true);
                }
            }

            // Añade un elemento predeterminado al inicio del DropDownList
            ddlPDV.Items.Insert(0, new ListItem("Seleccione un PDV", "0"));
        }

        private void MostrarMensaje(string mensaje, bool esError)
        {
            // Determina el tipo de alerta según si es error o éxito
            string tipoAlerta = esError ? "error" : "success";
            // Mostrar una alerta en pantalla con el mensaje
            string script = $"alert('{mensaje}');"; // Aquí podrías usar toastr, alert, u otra librería de notificaciones

            // Registrar el script para ejecutarlo en la página web
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", script, true);
        }


        private void EliminarUsuario(int idEmpleado)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_EliminarEmpleado", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Empleado", idEmpleado);

                    // Parámetro para capturar el valor de retorno
                    SqlParameter returnValue = new SqlParameter();
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(returnValue);

                    connection.Open();
                    cmd.ExecuteNonQuery();

                    int resultadoOperacion = (int)returnValue.Value;


                    CargarDatos(); // Recargar la lista de usuarios
                    // Mostrar notificaciones basadas en el código de retorno
                    if (resultadoOperacion == 1)
                    {
                        MostrarNotificacion("El empleado fue eliminado correctamente.", "success");
                    }
                    else if (resultadoOperacion == -1)
                    {
                        MostrarNotificacion("El empleado no existe.", "warning");
                    }
                    else
                    {
                        MostrarNotificacion("Ocurrió un error al intentar eliminar el empleado.", "error");
                    }
                } // SqlCommand se libera aquí
            } // SqlConnection se cierra automáticamente aquí

        }
        // Método para mostrar notificaciones
        private void MostrarNotificacion(string mensaje, string tipo)
        {
            string script = $"Swal.fire({{ text: '{mensaje}', icon: '{tipo}', timer: 3000, showConfirmButton: false }});";
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", script, true);
        }


    }
}