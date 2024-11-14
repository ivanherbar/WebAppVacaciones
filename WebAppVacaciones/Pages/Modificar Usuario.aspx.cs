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
    public partial class Modificar_Usuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlRol_SelectedIndexChanged(null, EventArgs.Empty);
                CargarEmpleadosSinUsuario();

                if (Request.QueryString["Id_Usuario"] != null)
                {
                    int idUsuario = int.Parse(Request.QueryString["Id_Usuario"]);
                    CargarUsuario(idUsuario);
                }
            }
        }


        private void CargarUsuario(int idUsuario)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Nombre, Usuario, Clave, Id_Rol, ID_Empleado FROM Usuarios WHERE Id_Usuario = @Id_Usuario", conn))
                {
                    cmd.Parameters.AddWithValue("@Id_Usuario", idUsuario);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtNombre.Text = reader["Nombre"].ToString();
                        txtUsuario.Text = reader["Usuario"].ToString();
                        txtClave.Text = reader["Clave"].ToString();
                        ddlRol.SelectedValue = reader["Id_Rol"].ToString();
                        ddlIDEmpleado.SelectedValue = reader["ID_Empleado"] != DBNull.Value ? reader["ID_Empleado"].ToString() : "";
                    }
                }
            }
        }


        protected void ddlRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Habilita o deshabilita el campo ID de Empleado basado en la selección
            if (ddlRol.SelectedValue == "1") // Administrador
            {
                ddlIDEmpleado.Enabled = false;
                ddlIDEmpleado.Items.Clear(); // Limpiar si es necesario
                txtNombre.Text = string.Empty; // Limpiar el campo Nombre Completo
                CargarEmpleadosSinUsuario(); // Cargar empleados al iniciar
            }
            else if (ddlRol.SelectedValue == "2") // Empleado
            {
                ddlIDEmpleado.Enabled = true;
                CargarEmpleadosSinUsuario(); // Cargar empleados cuando se selecciona "Empleado"
            }
        }

        private void CargarEmpleadosSinUsuario()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_EmpleadosSinUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        ddlIDEmpleado.DataSource = reader;
                        ddlIDEmpleado.DataTextField = "Nombre";  // Nombre del empleado
                        ddlIDEmpleado.DataValueField = "ID_Empleado"; // ID del empleado
                        ddlIDEmpleado.DataBind();

                        // Añadir un elemento predeterminado
                        ddlIDEmpleado.Items.Insert(0, new ListItem("Seleccione un Empleado", "0"));
                    }
                    catch (Exception ex)
                    {
                        // Manejar el error
                        ScriptManager.RegisterStartupScript(this, GetType(), "errorCargaEmpleados", $"alert('Error al cargar empleados: {ex.Message}');", true);
                    }
                }
            }
        }

        protected void ddlIDEmpleado_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Transcribir el nombre del empleado al TextBox de Nombre Completo
            if (ddlIDEmpleado.SelectedValue != "0")
            {
                string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Nombre FROM Empleado WHERE ID_Empleado = @ID_Empleado", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID_Empleado", ddlIDEmpleado.SelectedValue);

                        try
                        {
                            conn.Open();
                            string nombre = cmd.ExecuteScalar()?.ToString();

                            if (!string.IsNullOrEmpty(nombre))
                            {
                                txtNombre.Text = nombre; // Asigna el nombre al TextBox
                            }
                        }
                        catch (Exception ex)
                        {
                            // Manejar el error
                            ScriptManager.RegisterStartupScript(this, GetType(), "errorCargarNombre", $"alert('Error al cargar el nombre: {ex.Message}');", true);
                        }
                    }
                }
            }
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Usuarios SET Nombre = @Nombre, Usuario = @Usuario, Clave = @Clave, Id_Rol = @Id_Rol, ID_Empleado = @ID_Empleado WHERE Id_Usuario = @Id_Usuario", conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Usuario", txtUsuario.Text);
                    cmd.Parameters.AddWithValue("@Clave", txtClave.Text);
                    cmd.Parameters.AddWithValue("@Id_Rol", ddlRol.SelectedValue);
                    cmd.Parameters.AddWithValue("@ID_Empleado", ddlIDEmpleado.SelectedValue);
                    cmd.Parameters.AddWithValue("@Id_Usuario", Request.QueryString["Id_Usuario"]);

                    cmd.ExecuteNonQuery();
                }
            }

            lblMensaje.Text = "Usuario modificado exitosamente.";
            lblMensaje.CssClass = "has-text-success";
        }

    }
}