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
                ResetControls();
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
                case "Solicitados":
                    CargarSolicitados(userId);
                break;

            }
        }

        private void CargarSolicitados(int userId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ConsultarDiasSolicitados", con))
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
                        GridViewSolicitados.DataSource = dt;
                        GridViewSolicitados.DataBind();

                        // Abrir el modal para mostrar los registros de vacaciones
                        ScriptManager.RegisterStartupScript(this, GetType(), "abrirModalSolicitados", "abrirModalSolicitados();", true);
                    }
                    else
                    {
                        // Si no hay registros, mostrar alerta con SweetAlert
                        ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "Swal.fire('Sin registro de vacaciones', '', 'warning');", true);
                    }
                }
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





        protected void GuardarVacacion(object sender, EventArgs e)
        {
            // Obtener el ID del empleado desde la sesión
            int empleadoId = int.Parse(Session["ID_Empleado"].ToString());

            // Obtener la fecha seleccionada
            DateTime fechaSolicitud;
            if (DateTime.TryParse(TextBox2.Text, out fechaSolicitud))
            {
                // Obtener el medio día seleccionado
                string medioDia = DropDownListDia.SelectedValue; // Asegúrate de que este Dropdown tenga valores 'N', 'M', 'T'

                // Llamar al procedimiento almacenado
                EjecutarProcedimientoAlmacenado(empleadoId, fechaSolicitud, medioDia);
                ResetControls();
            }
            else
            {
                // Manejar error en la fecha
                ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "Swal.fire('Fecha inválida', '', 'error');", true);
                ResetControls();
            }

        }

        private void EjecutarProcedimientoAlmacenado(int empleadoId, DateTime fecha, string medioDia)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            // Validar el valor de MedioDia
            if (medioDia != "N" && medioDia != "M" && medioDia != "T")
            {
                // Si no es un valor válido, mostrar un mensaje de error
                ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "Swal.fire('El valor de MedioDia debe ser N, M o T.', '', 'error');", true);
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_RegistrarSolicitudVacaciones", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agregar los parámetros al procedimiento almacenado
                        cmd.Parameters.AddWithValue("@ID_Empleado", empleadoId);
                        cmd.Parameters.AddWithValue("@Fecha", fecha);
                        cmd.Parameters.AddWithValue("@MedioDia", medioDia);

                        // Abrir la conexión
                        con.Open();

                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();

                        // Mostrar mensaje de éxito
                        ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "Swal.fire('Solicitud registrada con éxito', '', 'success');", true);
                    }
                }
                catch (SqlException ex)
                {
                    // Manejar posibles errores
                    ScriptManager.RegisterStartupScript(this, GetType(), "alerta", $"Swal.fire('Error', '{ex.Message}', 'error');", true);
                }
            }
        }

        private void ResetControls()
        {
            DropDownListDia.SelectedIndex = 0; // Seleccionar la primera opción
            TextBox2.Text = ""; // Limpiar el campo de texto
        }


    }
}