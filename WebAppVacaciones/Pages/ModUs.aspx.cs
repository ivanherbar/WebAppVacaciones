using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppVacaciones
{
    public partial class ModUs : System.Web.UI.Page
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
                using (SqlCommand command = new SqlCommand("sp_datos", con))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agrega el parámetro al comando
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



        // Método para manejar el evento de cambio de texto en el TextBox de búsqueda
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtSearch.Text.Trim();
            CargarDatos(filtro);
        }

        protected void gridDetallesEmpleado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int userId = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "Consultar":
                    // Lógica para consultar un registro
                    ConsultarRegistro(userId);
                    break;

                case "Actualizar":
                    // Lógica para actualizar un registro
                    ActualizarRegistro(userId);
                    break;

                case "Eliminar":
                    // Lógica para eliminar un registro
                    EliminarRegistro(userId);
                    break;
            }
        }

        private void ConsultarRegistro(int userId)
        {
            // Lógica para consultar el registro
        }

        private void ActualizarRegistro(int userId)
        {
            // Lógica para actualizar el registro
        }

        private void EliminarRegistro(int userId)
        {
            // Lógica para eliminar el registro
        }

    }
}