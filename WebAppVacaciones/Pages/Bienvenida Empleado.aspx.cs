﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppVacaciones.Pages
{
    public partial class Bienvenida_Empleado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AppendHeader("Cache-Control", "no-store");


            if (Session["usuario"] != null)
            {
                Label1.Text = Session["Nombre"].ToString() + " - " + Session["Usuario"].ToString();
            }
            else
            {
                Label1.Text = string.Empty;
            }
        }
    }
}