using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.UI;

namespace WebAppVacaciones.Pages
{
    public partial class Registro_de_Usuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Inicialización al cargar la página
                ddlRol_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        protected void ddlRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtiene el valor seleccionado del DropDownList
            string selectedValue = ddlRol.SelectedValue;

            // Habilita o deshabilita el campo ID de Empleado basado en la selección
            if (selectedValue == "1") // Administrador
            {
                txtIDEmpleado.Enabled = false;
                txtIDEmpleado.Text = string.Empty; // Limpiar el valor si es necesario
            }
            else if (selectedValue == "2") // Empleado
            {
                txtIDEmpleado.Enabled = true;
            }
        }
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_registrar", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@Usuario", txtUsuario.Text);
                        cmd.Parameters.AddWithValue("@Clave", txtClave.Text);
                        cmd.Parameters.AddWithValue("@Id_rol", ddlRol.SelectedValue);
                        cmd.Parameters.AddWithValue("@Patron", "VacacionesGNTTel");

                        if (ddlRol.SelectedValue == "2" && !string.IsNullOrEmpty(txtIDEmpleado.Text))
                        {
                            cmd.Parameters.AddWithValue("@ID_Empleado", txtIDEmpleado.Text);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@ID_Empleado", DBNull.Value);
                        }

                        // Parámetro de salida para obtener el resultado del procedimiento almacenado
                        SqlParameter resultadoParam = new SqlParameter("@Resultado", SqlDbType.Int);
                        resultadoParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(resultadoParam);

                        cmd.ExecuteNonQuery();

                        int resultado = Convert.ToInt32(resultadoParam.Value);

                        if (resultado == 1)
                        {
                            // Usuario ya existe
                            ScriptManager.RegisterStartupScript(this, GetType(), "usuarioExiste", "alert('El nombre de usuario ya existe.');", true);
                        }
                        else
                        {
                            // Usuario registrado con éxito
                            ScriptManager.RegisterStartupScript(this, GetType(), "registroExitoso", "alert('Usuario registrado con éxito');", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Mensaje de error
                    ScriptManager.RegisterStartupScript(this, GetType(), "registroError", $"alert('Error al registrar el usuario: {ex.Message}');", true);
                }
            }
        }

    }
}
 