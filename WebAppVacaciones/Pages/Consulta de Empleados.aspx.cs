using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppVacaciones.Pages
{
    public partial class Consulta_de_Empleados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatos();
                CargarPDV();
                CargarPuestos(); // Cargar el DropDownList de Puestos desde la base de datos

                DropDownListDia.Items.Add(new ListItem("Seleccionar tipo de dia", ""));
                DropDownListDia.Items.Add(new ListItem("Mañana", "1"));
                DropDownListDia.Items.Add(new ListItem("Tarde", "2"));
                DropDownListDia.Items.Add(new ListItem("Dia completo", "3"));
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

            if (e.CommandName == "Agregar")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "abrirModalVadAdd", "abrirModalVadAdd();", true);
            }
            else if (e.CommandName == "Consultar")
            {
                CargarVacaciones(userId);
            }
            else if (e.CommandName == "Actualizar")
            {

                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gridDetallesEmpleado.Rows[rowIndex];

                // Obtén el ID del empleado y guárdalo en el HiddenField
                hfEmpleadoID.Value = row.Cells[0].Text; // ID_Empleado

                // Asigna valores a los controles del modal
                txtNombre.Text = row.Cells[6].Text;
                DropDownListPuesto.SelectedValue = ObtenerPuesto(row.Cells[7].Text);
                txtFechaIngreso.Text = DateTime.Parse(row.Cells[8].Text).ToString("yyyy-MM-dd");
                ddlPDV.SelectedValue = ObtenerIdPDV(row.Cells[5].Text);

                ScriptManager.RegisterStartupScript(this, GetType(), "abrirModalMod", "abrirModalMod();", true);
            }
            else if (e.CommandName == "Eliminar")
            {
                EliminarUsuario(userId);
            }

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener la conexión desde el archivo Web.config
                string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ModificarEmpleado", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros obtenidos desde los controles del modal
                        cmd.Parameters.AddWithValue("@ID_Empleado", Convert.ToInt32(hfEmpleadoID.Value)); // HiddenField para ID del empleado
                        cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                        cmd.Parameters.AddWithValue("@Puesto", DropDownListPuesto.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@Fecha_Ingreso", Convert.ToDateTime(txtFechaIngreso.Text.Trim()));
                        cmd.Parameters.AddWithValue("@ID_PDV", Convert.ToInt32(ddlPDV.SelectedValue));

                        con.Open();
                        cmd.ExecuteNonQuery();

                        MostrarMensaje("Empleado actualizado exitosamente.", false);
                    }
                }

                // Recargar los datos de la tabla después de guardar
                CargarDatos();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al actualizar empleado: " + ex.Message, true);
            }
        }




        private void MostrarMensaje(string mensaje, bool esError)
        {
            string tipoAlerta = esError ? "error" : "success";
            string script = $"Swal.fire({{ title: '{mensaje}', icon: '{tipoAlerta}' }});";
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", script, true);
        }

        private string ObtenerPuesto(string nombrePuesto)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id_Puesto FROM Puesto WHERE Puesto = @nombrePuesto", con))
                {
                    cmd.Parameters.AddWithValue("@nombrePuesto", nombrePuesto);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? "0";
                }
            }
        }


        private string ObtenerIdPDV(string nombrePDV)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT ID_PDV FROM PDV WHERE Nombre_PDV = @NombrePDV", con))
                {
                    cmd.Parameters.AddWithValue("@NombrePDV", nombrePDV);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? "0";
                }
            }
        }



        private void EliminarUsuario(int idEmpleado)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EliminarEmpleadoConReferencias", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Empleado", idEmpleado);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "Swal.fire('Registro anulado con éxito', '', 'success');", true);
            CargarDatos();
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

        private void CargarPuestos()
        {
            // Obtener la cadena de conexión desde el archivo Web.config
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            // Usar la conexión SQL para ejecutar un procedimiento almacenado
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Usar el procedimiento almacenado "sp_puesto" para cargar los puestos
                SqlCommand cmd = new SqlCommand("sp_puesto", con);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    // Abrir la conexión a la base de datos
                    con.Open();
                    // Ejecutar el procedimiento almacenado y obtener los datos
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Establecer los datos en el DropDownList de Puestos
                    DropDownListPuesto.DataSource = reader;
                    DropDownListPuesto.DataTextField = "Puesto";  // Mostrar el nombre del puesto
                    DropDownListPuesto.DataValueField = "Id_Puesto"; // Guardar el nombre del puesto como valor
                    DropDownListPuesto.DataBind();
                }
                catch (Exception ex)
                {
                    // En caso de error, mostrar un mensaje
                    MostrarMensaje("Error al cargar puestos: " + ex.Message, true);
                }
            }

            // Añadir un elemento predeterminado al inicio del DropDownList
            DropDownListPuesto.Items.Insert(0, new ListItem("Seleccione un Puesto", "0"));
        }
        private void CargarPDV()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_consultar_pdv", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlPDV.DataSource = reader;
                    ddlPDV.DataTextField = "Nombre_PDV";
                    ddlPDV.DataValueField = "ID_PDV";
                    ddlPDV.DataBind();
                }
            }

            ddlPDV.Items.Insert(0, new ListItem("Seleccione un PDV", "0"));
        }


    }
}