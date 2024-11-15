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
                        txtClave.Text = reader["Clave"].ToString()+55;
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
                    cmd.Parameters.AddWithValue("@Id_Usuario", Request.QueryString["Id_Usuario"]);

                    cmd.ExecuteNonQuery();
                }
            }

            lblMensaje.Text = "Usuario modificado exitosamente.";
            lblMensaje.CssClass = "has-text-success";
        }

    }
}