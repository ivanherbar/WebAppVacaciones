using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppVacaciones.Pages
{
    public partial class Bienvenido : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AppendHeader("Cache-Control", "no-store");

            if (Session["usuario"] != null && Session["Id_rol"] != null)
            {
                string usuario = Session["usuario"].ToString();
                int idRol = Convert.ToInt32(Session["Id_rol"]);

                // Verificamos el rol y concatenamos el nombre adecuado
                string rol = idRol == 1 ? "Administrador" : idRol == 2 ? "Usuario" : "Desconocido";

                // Mostramos el texto en el label
                Label1.Text = $"{rol} - {usuario}";
            }
            else
            {
                Label1.Text = string.Empty;
            }
        }
    }
}
