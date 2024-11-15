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
                using (SqlCommand cmd = new SqlCommand("SELECT Nombre, Usuario, CONVERT(VARCHAR(50), DECRYPTBYPASSPHRASE('VacacionesGNTTel', Clave)) AS Clave, Id_Rol, ID_Empleado FROM Usuarios WHERE Id_Usuario = @Id_Usuario", conn))
                {
                    cmd.Parameters.AddWithValue("@Id_Usuario", idUsuario);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtNombre.Text = reader["Nombre"].ToString();
                        txtUsuario.Text = reader["Usuario"].ToString();
                        txtClave.Text = reader["Clave"].ToString();
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
                using (SqlCommand cmd = new SqlCommand("sp_editarUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agrega los parámetros necesarios para el procedimiento almacenado
                    cmd.Parameters.AddWithValue("@Id_Usuario", Request.QueryString["Id_Usuario"]);
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Usuario", txtUsuario.Text);
                    cmd.Parameters.AddWithValue("@Clave", txtClave.Text);
                    cmd.Parameters.AddWithValue("@Patron", "VacacionesGNTTel");

                    // Parámetro de salida para capturar el resultado
                    SqlParameter resultadoParam = new SqlParameter("@Resultado", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(resultadoParam);

                    // Ejecuta el procedimiento almacenado
                    cmd.ExecuteNonQuery();

                    // Obtén el valor del parámetro de salida
                    int resultado = (int)cmd.Parameters["@Resultado"].Value;

                    // Maneja los diferentes resultados
                    switch (resultado)
                    {
                        case 0:
                            // Éxito: mostrar alerta y redirigir
                            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                                "alert('Usuario actualizado correctamente.'); window.location.href='Bienvenido.aspx';", true);
                            break;
                        case 1:
                            lblMensaje.Text = "El usuario no existe.";
                            break;
                        case 2:
                            lblMensaje.Text = "El nombre de usuario ya está en uso.";
                            break;
                        case 3:
                            lblMensaje.Text = "Un administrador no puede tener un ID de empleado.";
                            break;
                        case 4:
                            lblMensaje.Text = "Un empleado debe tener un ID de empleado.";
                            break;
                        default:
                            lblMensaje.Text = "Ocurrió un error desconocido.";
                            break;
                    }
                }
            }
        }



    }
}