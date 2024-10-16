using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

                    gridVacaciones.DataSource = dt;
                    gridVacaciones.DataBind();
                }
            }

            // Abre el modal desde el lado del servidor
            ScriptManager.RegisterStartupScript(this, GetType(), "abrirModal", "abrirModal();", true);
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
            int vacationId = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "Anular":
                    AnularVacacion(vacationId);
                    break;
            }
        }

        private void AnularVacacion(int vacationId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("AnularVacacion", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID_Empleado", vacationId);

                    con.Open();
                    command.ExecuteNonQuery();
                }
            }

            // Recargar las vacaciones después de anular
            ScriptManager.RegisterStartupScript(this, GetType(), "cerrarModal", "cerrarModal();", true);
            CargarVacaciones(Convert.ToInt32(Session["EmpleadoID"])); // Asegúrate de que tienes el ID del empleado
        }

    }
}
