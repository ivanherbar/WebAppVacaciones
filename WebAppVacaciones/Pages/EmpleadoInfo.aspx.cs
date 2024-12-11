using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppVacaciones.Pages
{
    public partial class EmpleadoInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int EmpleadoLogeado = int.Parse(Session["ID_Empleado"].ToString());

            if (!IsPostBack)
            {
                CargarDatos(EmpleadoLogeado);
            }
        }

        private void CargarDatos(int EmpleadoLogeado)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_datos_UsuarioLogueado", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    // Agregar el parámetro @ID_Empleado al comando
                    command.Parameters.AddWithValue("@ID_Empleado", EmpleadoLogeado);

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    // Asignar los datos al control GridView o equivalente
                    gridDetallesEmpleado.DataSource = dt;
                    gridDetallesEmpleado.DataBind();
                }
            }
        }




        protected void gridDetallesEmpleado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int userId = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "Consultar":
                    CargarVacaciones(userId);
                    break;
                case "Actualizar":
                    ActualizarRegistro(userId);
                    break;
                case "Eliminar":
                    EliminarRegistro(userId);
                    break;
            }
        }

        private void CargarVacaciones(int userId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ConsultarDiasVacaciones", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID_Empleado", userId);

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    // Verificar si la tabla tiene registros
                    if (dt.Rows.Count > 0)
                    {
                        // Mostrar los datos de vacaciones en el grid
                        gridVacaciones.DataSource = dt;
                        gridVacaciones.DataBind();

                        // Abrir el modal para mostrar los registros de vacaciones
                        ScriptManager.RegisterStartupScript(this, GetType(), "abrirModal", "abrirModal();", true);
                    }
                    else
                    {
                        // Si no hay registros, mostrar alerta con SweetAlert
                        ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "Swal.fire('Sin registro de vacaciones', '', 'warning');", true);
                    }
                }
            }
        }



        private void ActualizarRegistro(int userId)
        {
            // Lógica para actualizar el registro
        }

        private void EliminarRegistro(int userId)
        {
            // Lógica para eliminar el registro
        }

        protected void gridVacaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Anular")
            {
                // Extraer los valores concatenados en el CommandArgument
                string[] argumentos = e.CommandArgument.ToString().Split(',');
                int idEmpleado = Convert.ToInt32(argumentos[0]); // ID_Empleado
                DateTime fecha = Convert.ToDateTime(argumentos[1]); // Fecha
                string medioDia = argumentos[2]; // MedioDia

                // Llamar al método que anula la vacación
                AnularVacacion(idEmpleado, fecha, medioDia);
            }
        }


        private void AnularVacacion(int idEmpleado, DateTime fecha, string medioDia)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("AnularDiaVacacionesPorFecha", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros
                        cmd.Parameters.AddWithValue("@ID_Empleado", idEmpleado);
                        cmd.Parameters.AddWithValue("@Fecha", fecha);
                        cmd.Parameters.AddWithValue("@MedioDia", medioDia);

                        // Abrir conexión
                        con.Open();

                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();

                        // Mostrar mensaje de éxito
                        ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "Swal.fire('Registro anulado con éxito', '', 'success');", true);
                    }
                }
                catch (SqlException ex)
                {
                    // Capturar el mensaje de error generado por RAISERROR en el procedimiento almacenado
                    ScriptManager.RegisterStartupScript(this, GetType(), "alerta", $"Swal.fire('Error', '{ex.Message}', 'error');", true);
                }
            }
        }



    }
}