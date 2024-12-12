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

        private void ActualizarRegistro(int userId)
        {
            // Lógica para actualizar el registro
        }
    }
}