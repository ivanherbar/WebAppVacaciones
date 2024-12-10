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
            if (e.CommandName == "Actualizar")
            {
                // Obtener el índice de la fila desde el CommandSource
                int rowIndex = ((GridViewRow)((Button)e.CommandSource).NamingContainer).RowIndex;

                // Obtener la fila desde el GridView
                GridViewRow row = gridDetallesEmpleado.Rows[rowIndex];

                // Extraer los datos de las celdas
                string nombre = row.Cells[1].Text;
                string puesto = row.Cells[2].Text;
                string fechaIngreso = row.Cells[3].Text;
                string pdv = row.Cells[4].Text;

                // Llenar los campos del modal con los datos extraídos
                txtNombre.Text = nombre;
                txtPuesto.Text = puesto;
                txtFechaIngreso.Text = fechaIngreso;
                txtPDV.Text = pdv;

                // Abrir el modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AbrirModal", "abrirModal();", true);
            }
            else if (e.CommandName == "Eliminar")
            {
                // Obtener el índice de la fila desde el CommandSource
                int rowIndex = ((GridViewRow)((Button)e.CommandSource).NamingContainer).RowIndex;

                // Obtener el ID del empleado desde el CommandArgument
                int idEmpleado = Convert.ToInt32(gridDetallesEmpleado.DataKeys[rowIndex].Value);

                // Lógica para eliminar el registro
                EliminarRegistro(idEmpleado);
            }
        }

        private void EliminarRegistro(int userId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_EliminarEmpleado", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID_Empleado", userId);

                    con.Open();
                    command.ExecuteNonQuery();
                }
            }

            // Recargar los datos después de la eliminación
            CargarDatos();
        }

        private void ActualizarRegistro(int userId)
        {
            // Lógica para actualizar el registro
        }
    }
}
